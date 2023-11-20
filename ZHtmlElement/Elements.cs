using Global.Zennolab.HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZennoLab.CommandCenter;

namespace ZHtmlElement
{
    public class Elements
    {

        public SiteDataModel SiteDataModel { get; private set; }
        private List<string> LstXPathText { get; set; } = new List<string>();

        private readonly Tab _tab;
        private readonly HtmlDocument _doc;

        public Elements(Tab tab)
        {
            _tab = tab;
            _doc = new HtmlDocument();
            _doc.LoadHtml(_tab.DomText);

            SiteDataModel = new SiteDataModel();
        }

        /// <summary>
        /// LINQ фильтрация через ZP.
        /// </summary>
        /// <returns></returns>
        public List<HtmlElement> GetTextElements()
        {
            var xPathElements = GetXPathText();

            var lstHtmlElements = new List<HtmlElement>();
            foreach (var xPath in xPathElements)
            {
                lstHtmlElements.Add(_tab.FindElementByXPath(xPath, 0));
            }

            var result = lstHtmlElements
                .AsParallel()
                .Where(el => IsHidden(el) != true)
                .Where(el => el.InnerText.Length != 0)
                .ToList();

            return result;
        }

        /// <summary>
        /// Собираем пути на нужные элементы. Фильтруем по тегу div, span и длине текста.
        /// </summary>
        /// <returns></returns>
        public List<string> GetXPathText()
        {
            var xPath = "//body//*[text()]";
            var texts = _doc.DocumentNode.SelectNodes(xPath).ToList();

            var result = texts
                .Where(el => el.Name == "div" || el.Name == "span")
                .Where(el => el.InnerText.Length > 30)
                .ToList();

            return result
                .Select(el => el.XPath)
                .ToList();
        }

        /// <summary>
        /// Пример для просомтра работы HtmlAgilityPack.
        /// </summary>
        /// <returns></returns>
        public List<HtmlNode> GetElements()
        {
            var xPath = "//body//*[text()]";
            var texts = _doc.DocumentNode.SelectNodes(xPath).ToList();

            return texts
                .Where(el => el.Name == "div" || el.Name == "span")
                .Where(el => el.InnerText.Length > 30)
                .ToList();
        }

        /// <summary>
        /// Собираем пути на нужные элементы.
        /// </summary>
        public void ParseXPathElements_v2()
        {
            var xPath = "//body//*[text()]";
            var elements = _doc.DocumentNode.SelectNodes(xPath).ToList();

            //Text
            var xPathText = elements
                .Where(el => el.Name == "div" || el.Name == "span")
                .Where(el => el.InnerText.Length > 30)
                .Select(el => el.XPath);
            LstXPathText.AddRange(xPathText);
        }

        /// <summary>
        /// Асинхронная фильтрация элементов ZP.
        /// </summary>
        public async void FilterElementsTextAsync()
        {
            SiteDataModel.LstText = new List<HtmlElement>();
            var lstHtmlElements = new List<HtmlElement>();

            await Task.Run(() =>
            {
                foreach (var xPath in LstXPathText)
                {
                    lstHtmlElements.Add(_tab.FindElementByXPath(xPath, 0));
                }
            });

            await Task.Run(() =>
            {
                foreach (var element in lstHtmlElements)
                {
                    if (!IsHidden(element) && element.InnerText.Length > 0)
                    {
                        SiteDataModel.LstText.Add(element);
                    }
                }
            });
        }

        /// <summary>
        /// Проверяет скрыт ли элемент.
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsHidden(HtmlElement he)
        {
            string style = he.GetAttribute("style").Replace(" ", "").ToLower();
            bool isHidden =
                he.IsVoid ||
                he.Width <= 0 ||
                he.Height <= 0 ||
                style.Contains("display:none") ||
                style.Contains("visibility:hidden") ||
                style.Contains("visibility:collapse");
            return isHidden;
        }
    }
}

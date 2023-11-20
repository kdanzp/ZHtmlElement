using static Global.Threads.ThreadHell;
using System.Collections.Generic;
using System.Linq;
using ZennoLab.CommandCenter;

namespace ZHtmlElement
{
    public class SiteHandler
    {
        public SiteDataModel SiteData { get; set; }

        private readonly Tab _tab;

        public SiteHandler(Tab tab)
        {
            _tab = tab;
            SiteData = new SiteDataModel();
        }

        /// <summary>
        /// Парсить HtmlElements согласно настроек.
        /// </summary>
        /// <returns></returns>
        public void ParseElements()
        {
            //Для дебага
            List<string> texts;

            var elements = new Elements(_tab);
            SiteData.LstText = elements.GetTextElements();
            texts = SiteData.LstText.Select(t => t.InnerText).ToList();
        }

        public void ParseElements_v2()
        {
            //Для дебага
            List<string> texts;

            var elements = new Elements(_tab);
            elements.ParseXPathElements_v2();
            elements.FilterElementsTextAsync();
            SiteData.LstText = elements.SiteDataModel.LstText;
            texts = SiteData.LstText.Select(x => x.InnerText).ToList();
        }
    }
}

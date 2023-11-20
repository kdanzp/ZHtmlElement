using System.Collections.Generic;
using ZennoLab.CommandCenter;

namespace ZHtmlElement
{
    public class SiteDataModel
    {
        public string Text { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string СurrentUrl { get; set; }
        public List<HtmlElement> LstText { get; set; }
        public List<HtmlElement> LstLink { get; set; }
        public List<HtmlElement> LstImage { get; set; }
    }
}

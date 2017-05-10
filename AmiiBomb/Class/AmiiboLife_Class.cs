using AngleSharp.Parser.Html;
using System;
using System.Drawing;
using System.Linq;

namespace AmiiBomb
{
    class AmiiboLife_Class
    {
        public static string BaseAdress = "http://amiibo.life/nfc/";

        public static string[] Get_Amiibo_Info(string Amiibo_NFC_ID)
        {
            var HTML_Dom = new HtmlParser().Parse(Helper_Class.Get_Source_From_Url(BaseAdress + Amiibo_NFC_ID));

            try
            {
                return new string[]
                {
                    Helper_Class.Clean_NewLine_Spaces(HTML_Dom.QuerySelectorAll("div.figure-info h1.name").First().TextContent), //Name
                    Helper_Class.Clean_NewLine_Spaces(HTML_Dom.QuerySelectorAll("div.figure-info p.series-name").First().TextContent), //Series
                    HTML_Dom.QuerySelectorAll("div.figure.show.row img").First().GetAttribute("src") //Picture Link
                };
            }
            catch
            {
                return new string[] { "", "", "" };
            }
        }
    }

    [Serializable]
    class AmiiboLife_Cache_Class
    {
        public string SHA1 { get; set; }
        public string Name { get; set; }
        public string Serie { get; set; }
        public string NFC_ID { get; set; }
        public Image Picture { get; set; }
    }
}

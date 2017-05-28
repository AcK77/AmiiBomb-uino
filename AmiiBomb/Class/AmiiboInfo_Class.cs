using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Drawing;
using System.Linq;

namespace AmiiBomb
{
    class AmiiboInfo_Class
    {
        public static string[] Get_AmiiboInfo(string Amiibo_NFC_ID)
        {
            switch (Main_Form.Config.Database)
            {
                case 1:
                    return Get_AmiiboAPI(Amiibo_NFC_ID);
                default:
                    return Get_AmiiboLife(Amiibo_NFC_ID);
            }
        }

        public static string AmiiboLife_BaseAdress = "http://amiibo.life/nfc/";

        public static string[] Get_AmiiboLife(string Amiibo_NFC_ID)
        {
            var HTML_Dom = new HtmlParser().Parse(Helper_Class.Get_Source_From_Url(AmiiboLife_BaseAdress + Amiibo_NFC_ID));

            try
            {
                return new string[]
                {
                    Helper_Class.Clean_NewLine_Spaces(HTML_Dom.QuerySelectorAll("div.figure-info h1.name").First().TextContent), //Name
                    Helper_Class.Clean_NewLine_Spaces(HTML_Dom.QuerySelectorAll("div.figure-info p.series-name").First().TextContent), //Series
                    AmiiboLife_BaseAdress + Amiibo_NFC_ID + "/image" //Picture Link
                };
            }
            catch
            {
                return new string[] { "", "", "" };
            }
        }

        public static string AmiiboAPI_BaseAdress = "http://amiiboapi.herokuapp.com/api/amiibo/";

        public static string[] Get_AmiiboAPI(string Amiibo_NFC_ID)
        {
            var JSON = Helper_Class.Get_Source_From_Url(AmiiboAPI_BaseAdress + Amiibo_NFC_ID.Replace("-", string.Empty));
            DataRow Amiibo = JsonConvert.DeserializeObject<DataSet>(JSON).Tables["amiibo"].Rows[0];

            try
            {
                return new string[]
                {
                    Amiibo["name"].ToString(), //Name
                    Amiibo["amiiboSeries"].ToString(), //Series
                    Amiibo["image"].ToString() //Picture Link
                };
            }
            catch
            {
                return new string[] { "", "", "" };
            }
        }
    }

    [Serializable]
    class AmiiboInfo_Cache_Class
    {
        public string SHA1 { get; set; }
        public string Name { get; set; }
        public string Serie { get; set; }
        public string NFC_ID { get; set; }
        public Image Picture { get; set; }
    }
}

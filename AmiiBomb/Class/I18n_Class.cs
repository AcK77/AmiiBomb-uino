using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AmiiBomb
{
    //https://github.com/MoonGateLabs/i18n-unity-csharp

    public sealed class I18n
    {
        private static JSONNode config = null;
        private static JSONNode config_base = null;

        private static readonly I18n instance = new I18n();

        //private static string[] locales = new string[] { "en-US", "fr-FR" };

        private static string _currentLocale = "en-US";

        private static string _localePath = "lang/";

        private static bool _isLoggingMissing = true;

        static I18n()
        {
           
        }

        private I18n()
        {
        }

        public static I18n Instance
        {
            get
            {
                return instance;
            }
        }

        static void InitConfig()
        {
            //if (locales.Contains(_currentLocale))
            //{
                string localConfigPath = _localePath + _currentLocale;
                config = JSON.Parse(File.ReadAllText(localConfigPath + ".lang"));
                config_base = JSON.Parse(File.ReadAllText(_localePath + "en-US.lang"));
            //}
            //else if (_isLoggingMissing)
            //{
            //Console.WriteLine("Missing: locale [" + _currentLocale + "] not found in supported list");
            //}
        }

        public static string GetLocale()
        {
            return _currentLocale;
        }

        public static void SetLocale(string newLocale = null)
        {
            if (newLocale != null)
            {
                _currentLocale = newLocale;
                InitConfig();
            }
        }

        public static void SetPath(string localePath = null)
        {
            if (localePath != null)
            {
                _localePath = localePath;
                InitConfig();
            }
        }

        public static void Configure(string localePath = null, string newLocale = null, bool logMissing = true)
        {
            _isLoggingMissing = logMissing;
            SetPath(localePath);
            SetLocale(newLocale);
            InitConfig();
        }

        public string __(string key, params object[] args)
        {
            if (config == null)
            {
                InitConfig();
            }
            string translation = key;
            if (config[key] != null)
            {
                // if this key is a direct string
                if (config[key].Count == 0)
                {
                    translation = config[key];
                }
                else
                {
                    translation = FindSingularOrPlural(key, args);
                }
                // check if we have embeddable data
                if (args.Length > 0)
                {
                    translation = string.Format(translation, args);
                }
            }
            else if (_isLoggingMissing)
            {
                Console.WriteLine("Missing translation for:" + key);
                if (config_base[key].Count == 0)
                {
                    translation = config_base[key];
                }
                else
                {
                    translation = FindSingularOrPlural(key, args);
                }
                // check if we have embeddable data
                if (args.Length > 0)
                {
                    translation = string.Format(translation, args);
                }
            }
            return translation;
        }

        string FindSingularOrPlural(string key, object[] args)
        {
            JSONClass translationOptions = config[key].AsObject;
            string translation = key;
            string singPlurKey;
            // find format to try to use
            switch (GetCountAmount(args))
            {
                case 0:
                    singPlurKey = "zero";
                    break;
                case 1:
                    singPlurKey = "one";
                    break;
                default:
                    singPlurKey = "other";
                    break;
            }
            // try to use this plural/singular key
            if (translationOptions[singPlurKey] != null)
            {
                translation = translationOptions[singPlurKey];
            }
            else if (_isLoggingMissing)
            {
                Console.WriteLine("Missing singPlurKey:" + singPlurKey + " for:" + key);
            }
            return translation;
        }

        int GetCountAmount(object[] args)
        {
            int argOne = 0;
            // if arguments passed, try to parse first one to use as count
            if (args.Length > 0 && IsNumeric(args[0]))
            {
                argOne = Math.Abs(Convert.ToInt32(args[0]));
                if (argOne == 1 && Math.Abs(Convert.ToDouble(args[0])) != 1)
                {
                    // check if arg actually equals one
                    argOne = 2;
                }
                else if (argOne == 0 && Math.Abs(Convert.ToDouble(args[0])) != 0)
                {
                    // check if arg actually equals one
                    argOne = 2;
                }
            }
            return argOne;
        }

        bool IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            return false;
        }

    }
}

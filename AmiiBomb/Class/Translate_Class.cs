using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmiiBomb
{
    class Translate_Class
    {
        private static I18n i18n = I18n.Instance;

        public static string GetLocale(string Locale)
        {
            foreach (CultureInfo info in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                if (CultureInfo.CreateSpecificCulture(info.Name).Name == Locale)
                    return Helper_Class.FirstLetterToUpperCase(info.NativeName);
            }

            return null;
        }

        public static void Translate(Main_Form Form)
        {
            //Main Menu
            //---------

            //File Menu
            Form.fileToolStripMenuItem.Text = i18n.__("Menu_File");
                Form.selectbinFolderToolStripMenuItem.Text = i18n.__("Menu_Select_Amiibo_Folder");
                Form.dumpAmiiboToolStripMenuItem.Text = i18n.__("Menu_Dump_Amiibo");
                Form.flashAmiiBombuinoToolStripMenuItem.Text = i18n.__("Menu_Flash_AmiiBombuino");
                    Form.internalFlasherToolStripMenuItem.Text = i18n.__("Menu_Internal_Flasher");
                    Form.withXLoaderToolStripMenuItem.Text = i18n.__("Menu_XLoader");
                Form.quitToolStripMenuItem.Text = i18n.__("Menu_Quit");

            //Options Menu
            Form.optionsToolStripMenuItem.Text = i18n.__("Menu_Options");
                Form.keyToolStripMenuItem.Text = i18n.__("Menu_Keys");
                    Form.selectKeybinFileToolStripMenuItem.Text = i18n.__("Menu_Select_Amiibo_Keys");
                    Form.whereFindAmiiboKeyToolStripMenuItem.Text = i18n.__("Menu_Where_Find_Keys");
                    Form.registerAmiiboKeyToolStripMenuItem.Text = i18n.__("Menu_Check_Keys_Clipboard");
                Form.filesCacheToolStripMenuItem.Text = i18n.__("Menu_Files_Cache");
                    Form.activeFilesCachingToolStripMenuItem.Text = i18n.__("Menu_Active_Cache");
                    Form.deleteCacheToolStripMenuItem.Text = i18n.__("Menu_Delete_Cache");
                Form.languagesToolStripMenuItem.Text = i18n.__("Menu_Languages");

            //Help Menu
            Form.helpToolStripMenuItem.Text = i18n.__("Menu_Help");
                Form.howConnectToolStripMenuItem.Text = i18n.__("Menu_How_Connect");
                Form.aboutToolStripMenuItem.Text = i18n.__("Menu_About");

            Form.donateToolStripMenuItem.Text = i18n.__("Menu_Donate");

            //Amiibo Menu
            Form.amiiboToolStripMenuItem.Text = i18n.__("Menu_Amiibo_Menu");
                Form.moreInformationsToolStripMenuItem.Text = "..." + i18n.__("Menu_Amiibo_More_Informations");
                Form.actionsToolStripMenuItem.Text = "..." + i18n.__("Menu_Amiibo_Action");
                    Form.decryptToolStripMenuItem.Text = i18n.__("Menu_Amiibo_Decrypt");
                    Form.createTagToolStripMenuItem.Text = i18n.__("Menu_Amiibo_CreateTag");
                    Form.dumpAppDataToolStripMenuItem.Text = i18n.__("Menu_Amiibo_Dump_AppData");
                    Form.writeAppDataToolStripMenuItem.Text = i18n.__("Menu_Amiibo_Write_AppData");

            Form.deleteFileToolStripMenuItem.Text = i18n.__("Menu_Delete_File");

            //ListView
            //--------
            ((ColumnHeader)Form.listView1.Columns[0]).Text = i18n.__("Table_Column_File");
        }

        public static void Translate(Flash_Form Form)
        {
            Form.groupBox1.Text = i18n.__("NFC_Arduino");
            Form.label1.Text = i18n.__("NFC_Port");
            Form.label2.Text = i18n.__("NFC_Baudrate");
            Form.checkBox1.Text = i18n.__("NFC_Write_Lockbytes");
            Form.toolStripStatusLabel1.Text = i18n.__("NFC_No_Connected");
            Form.toolStripStatusLabel4.Text = "|  " + i18n.__("NFC_AmiiBombuino_Install") + ":";
            Form.button2.Text = i18n.__("NFC_Dump_Amiibo_Tag");
        }

        public static void Translate(About_Form Form)
        {
            Form.Text = i18n.__("About_Windows_Title") + " AmiiBomb";
            Form.label3.Text = i18n.__("About_Library");
            Form.label1.Text = i18n.__("About_Thanks");
            Form.label10.Text = i18n.__("About_Translation_By");
            Form.label9.Text = "● " + i18n.__("About_Donate");
        }

        public static void Translate(Arduino_Form Form)
        {
            Form.groupBox1.Text = i18n.__("AmiiBombuino_Arduino");
            Form.label1.Text = i18n.__("AmiiBombuino_Port");
            Form.label2.Text = i18n.__("AmiiBombuino_Model");
            Form.button1.Text = i18n.__("AmiiBombuino_Flash");
        }
    }
}

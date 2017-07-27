using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading;
using System.Text.RegularExpressions;
using AmiiBomb.Properties;
using System.Drawing;
using System.Collections.Generic;

namespace AmiiBomb
{
    public partial class Main_Form : Form
    {
        public static Config_Class Config = new Config_Class();
        FileSystemWatcher File_Watcher = new FileSystemWatcher();
        FileSystemWatcher Folder_Watcher = new FileSystemWatcher();
        public static AmiiboKeys AmiiKeys;
        Thread thCheck_Clipboard;
        string Last_File_Watched = "",
        Last_File_Action_Watched = "",
        Last_Folder_Watched = "",
        Last_Folder_Action_Watched = "",
        AmiiBomb_Config_File = "lib\\AmiiBomb.conf",
        Amiibo_Keys_Hash = "BBDBB49A917D14F7A997D327BA40D40C39E606CE",
        Current_Folder = "",
        Search_Backup_Current_Folder = "";
        Button Exit_Search_Button = new Button();
        TreeNode Last_Node;
        string Current_NFC_ID = "";
        private I18n i18n = I18n.Instance;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        public Main_Form()
        {
            AppDomain.CurrentDomain.AssemblyResolve += LoadAssembly;
            InitializeComponent();
            Helper_Class.DoubleBuffered(listView1, true);
            splitContainer1.Panel1MinSize = 0;
            splitContainer1.Panel2MinSize = 0;

            splitContainer3.Panel1Collapsed = true;
            splitContainer3.Panel1.Hide();

            AddSearchButton();
        }

        private Assembly LoadAssembly(object sender, ResolveEventArgs args)
        {
            Assembly result = null;
            if (args != null && !string.IsNullOrEmpty(args.Name))
            {
                FileInfo info = new FileInfo(Assembly.GetExecutingAssembly().Location);
                var assemblyName = args.Name.Split(new string[] { "," }, StringSplitOptions.None)[0];
                var assemblyPath = Path.Combine(Path.Combine(info.Directory.FullName, "lib"), string.Format("{0}.{1}", assemblyName, "dll"));

                if (File.Exists(assemblyPath)) result = Assembly.Load(File.ReadAllBytes(assemblyPath));
                else return args.RequestingAssembly;
            }

            return result;
        }

        public void FileWatcher()
        {
            File_Watcher.Path = Current_Folder;
            File_Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            File_Watcher.Filter = "*.bin";

            File_Watcher.Renamed += new RenamedEventHandler(OnFileChanged);
            File_Watcher.Changed += new FileSystemEventHandler(OnFileChanged);
            File_Watcher.Created += new FileSystemEventHandler(OnFileChanged);
            File_Watcher.Deleted += new FileSystemEventHandler(OnFileChanged);

            File_Watcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(object source, FileSystemEventArgs e)
        {
            if (Last_File_Watched != e.FullPath || Last_File_Action_Watched != e.ChangeType.ToString())
            {
                ListFiles(Current_Folder);
                treeView1.Invoke(new Action(() => treeView1.SelectedNode = Last_Node));
            }

            Last_File_Watched = e.FullPath;
            Last_File_Action_Watched = e.ChangeType.ToString();
        }

        public void FolderWatcher()
        {
            Folder_Watcher.Path = (Current_Folder == "")? Config.Bin_Folder_Path : Current_Folder;
            Folder_Watcher.NotifyFilter = NotifyFilters.DirectoryName;
            Folder_Watcher.IncludeSubdirectories = true;

            Folder_Watcher.Renamed += new RenamedEventHandler(OnFolderChanged);
            Folder_Watcher.Changed += new FileSystemEventHandler(OnFolderChanged);
            Folder_Watcher.Created += new FileSystemEventHandler(OnFolderChanged);
            Folder_Watcher.Deleted += new FileSystemEventHandler(OnFolderChanged);

            Folder_Watcher.EnableRaisingEvents = true;
        }

        private void OnFolderChanged(object source, FileSystemEventArgs e)
        {
            if (Last_Folder_Watched != e.FullPath || Last_Folder_Action_Watched != e.ChangeType.ToString())
            {
                List<string> savedExpansionState = null;  
                treeView1.Invoke(new Action(() => savedExpansionState = treeView1.Nodes.GetExpansionState()));
                LoadAmiiboBinFolder();
                treeView1.Invoke(new Action(() => treeView1.Nodes.SetExpansionState(savedExpansionState)));
            }

            Last_Folder_Watched = e.FullPath;
            Last_Folder_Action_Watched = e.ChangeType.ToString();
        }

        private void LoadAmiiboKey()
        {
            if (Helper_Class.ValidSHA1(File.ReadAllBytes(Config.KeyFile_Path), Amiibo_Keys_Hash))
            {
                AmiiKeys = AmiiboKeys.LoadKeys(Config.KeyFile_Path);
                splitContainer1.Visible = true;
            }
            else
            {
                Config.KeyFile_Path = null;
                File.WriteAllText(AmiiBomb_Config_File, JsonConvert.SerializeObject(Config));

                AskAmiiboKey();
            }
        }

        private void LoadAmiiboBinFolder(string Folder = "", TreeNode ParentNode = null)
        {
            treeView1.Invoke(new Action(() => treeView1.BeginUpdate()));
            if (Folder == "")
            {
                treeView1.Invoke(new Action(() => treeView1.Nodes.Clear()));
                treeView1.Invoke(new Action(() => treeView1.Nodes.Add("\\")));
                Folder = Config.Bin_Folder_Path;
                ParentNode = treeView1.Nodes[0];
            }

            string[] Directories = Directory.GetDirectories(Folder);
            foreach (string DirName in Directories)
            {
                treeView1.Invoke(new Action(() => ParentNode.Nodes.Add(new DirectoryInfo(DirName).Name)));
                LoadAmiiboBinFolder(DirName, ParentNode.Nodes[ParentNode.Nodes.Count - 1]);
            }

            treeView1.Invoke(new Action(() => treeView1.EndUpdate()));

            treeView1.Invoke(new Action(() => { treeView1.SelectedNode = treeView1.Nodes[0]; }));
        }

        private void ListFiles(string Dir_Path)
        {
            listView1.Invoke(new Action(() => listView1.Items.Clear()));
            listView1.Invoke(new Action(() => listView1.BeginUpdate()));

            string[] Files = Directory.GetFiles(Dir_Path, "*.bin");

            foreach (string BinFile in Files)
            {
                if (new FileInfo(BinFile).Length == 572)
                {
                    DialogResult DgResult = MessageBox.Show(i18n.__("Message_Amiibo_Hash_Detected", Environment.NewLine), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DgResult == DialogResult.Yes)
                    {
                        byte[] Hashed_File = File.ReadAllBytes(Path.Combine(Current_Folder, BinFile));
                        Array.Resize(ref Hashed_File, 540);
                        File.Move(Path.Combine(Current_Folder, BinFile), Path.Combine(Current_Folder, Path.GetFileNameWithoutExtension(BinFile) + ".bin.bak"));
                        File.WriteAllBytes(Path.Combine(Current_Folder, BinFile), Hashed_File);
                    }
                }

                if (new FileInfo(BinFile).Length == 540)
                {
                    ListViewItem lvItem = new ListViewItem(" " + Path.GetFileName(BinFile), 0);
                    listView1.Invoke(new Action(() => listView1.Items.Add(lvItem)));
                }
            }

            listView1.Invoke(new Action(() => listView1.EndUpdate()));
            listView1.Invoke(new Action(() => listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)));
            listView1.Invoke(new Action(() => listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)));
        }

        private void Get_FileInfo(string BinFile)
        {
            byte[] Decrypted_Amiibo;

            if (File_IsEncrypted(BinFile))
            {
                Decrypted_Amiibo = Amiibo_Class.Decrypt(File.ReadAllBytes(Path.Combine(Current_Folder, BinFile)), AmiiKeys);
                spoofRandomIDToolStripMenuItem.Enabled = true;
            }
            else
            {
                Decrypted_Amiibo = File.ReadAllBytes(Path.Combine(Current_Folder, BinFile));
                spoofRandomIDToolStripMenuItem.Enabled = false;
            }

            string Char_ID = Amiibo_Class.Get_Character_ID(Decrypted_Amiibo);
            string GameSeries_ID = Amiibo_Class.Get_GameSeries_ID(Decrypted_Amiibo);
            string NFC_ID = Amiibo_Class.Get_NFC_ID(Decrypted_Amiibo);

            currentAmiiboToolStripMenuItem.DropDownItems.Clear();
            int Init = Amiibo_Class.Get_Amiibo_Initialize_UserData(Decrypted_Amiibo);

            if (Init != 0)
            {
                if (Init == 16 || Init == 48)
                {
                    currentAmiiboToolStripMenuItem.DropDownItems.Add(i18n.__("Menu_Amiibo_Owner") + ": " + Amiibo_Class.Get_Amiibo_Mii_Nickname(Decrypted_Amiibo));
                    currentAmiiboToolStripMenuItem.DropDownItems.Add(i18n.__("Menu_Amiibo_Nickname") + ": " + Amiibo_Class.Get_Amiibo_Nickname(Decrypted_Amiibo));
                }

                if (Init == 48)
                {
                    currentAmiiboToolStripMenuItem.DropDownItems.Add(i18n.__("Menu_Amiibo_LastModDate") + ": " + Amiibo_Class.Get_Amiibo_LastModifiedDate(Decrypted_Amiibo));
                    currentAmiiboToolStripMenuItem.DropDownItems.Add(i18n.__("Menu_Amiibo_WriteCounter") + ": " + Amiibo_Class.Get_Amiibo_Write_Counter(Decrypted_Amiibo));
                    currentAmiiboToolStripMenuItem.DropDownItems.Add(i18n.__("Menu_Amiibo_AppID") + ": " + Amiibo_Class.Get_Amiibo_AppID(Decrypted_Amiibo));
                    currentAmiiboToolStripMenuItem.DropDownItems.Add(i18n.__("Menu_Amiibo_InitAppID") + ": " + Amiibo_Class.Get_Amiibo_Initialized_AppID(Decrypted_Amiibo));
                    currentAmiiboToolStripMenuItem.DropDownItems.Add(i18n.__("Menu_Amiibo_Country") + ": " + Amiibo_Class.Get_Amiibo_Country(Decrypted_Amiibo));
                }
            }
            else currentAmiiboToolStripMenuItem.DropDownItems.Add(i18n.__("Menu_Amiibo_Sorry"));


            foreach (ToolStripMenuItem Item in currentAmiiboToolStripMenuItem.DropDownItems)
            {
                Item.RightToLeft = RightToLeft.No;
                Item.Enabled = false;
            }

            string Cache_File_Path = Path.Combine("cache", Path.GetFileNameWithoutExtension(BinFile) + ".amiibomb");

            if (File.Exists(Cache_File_Path) && Config.Cache)
            {
                Stream stream = File.Open(Cache_File_Path, FileMode.Open);
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                AmiiboInfo_Cache_Class Cache_File = (AmiiboInfo_Cache_Class)binaryFormatter.Deserialize(stream);
                stream.Close();

                if (Helper_Class.ValidSHA1(Decrypted_Amiibo, Cache_File.SHA1))
                {
                    Current_NFC_ID = Cache_File.NFC_ID;
                    pictureBox1.Image = Cache_File.Picture;
                    nameToolStripMenuItem.Text = label3.Text = Cache_File.Name;
                    seriesToolStripMenuItem.Text = label4.Text = Cache_File.Serie;
                }
                else
                {
                    File.Delete(Cache_File_Path);
                    Get_FileInfo(BinFile);
                }
            }
            else
            {
                Current_NFC_ID = NFC_ID;
                string[] AmiiboLife_Info = AmiiboInfo_Class.Get_AmiiboInfo(NFC_ID);
                if (AmiiboLife_Info[2] != "")
                {
                    pictureBox1.Load(AmiiboLife_Info[2]);
                    pictureBox1.Image = Helper_Class.DropShadow(pictureBox1.Image);
                }
                else pictureBox1.Image = null;
                nameToolStripMenuItem.Text = AmiiboLife_Info[0].Trim();
                label3.Text = AmiiboLife_Info[0].Trim();
                seriesToolStripMenuItem.Text = AmiiboLife_Info[1].Trim();
                label4.Text = AmiiboLife_Info[1].Trim();

                AmiiboInfo_Cache_Class Cache_File = new AmiiboInfo_Cache_Class();
                Cache_File.SHA1 = Helper_Class.SHA1_File(Decrypted_Amiibo);
                Cache_File.Name = AmiiboLife_Info[0].Trim();
                Cache_File.Serie = AmiiboLife_Info[1].Trim();
                Cache_File.NFC_ID = Current_NFC_ID;
                Cache_File.Picture = pictureBox1.Image;

                if (Config.Cache && AmiiboLife_Info[0] != "" && AmiiboLife_Info[1] != "" && AmiiboLife_Info[2] != "")
                {
                    Stream stream = File.Open(Cache_File_Path, FileMode.Create);
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    binaryFormatter.Serialize(stream, Cache_File);
                    stream.Close();
                }

                if (AmiiboLife_Info[0] == "" || AmiiboLife_Info[1] == "" || AmiiboLife_Info[2] == "")
                    moreInformationsToolStripMenuItem.Visible = false;
                else moreInformationsToolStripMenuItem.Visible = true;
            }
        }

        private bool File_IsEncrypted(string BinFile)
        {
            if (Amiibo_Class.IsEncrypted(File.ReadAllBytes(Path.Combine(Current_Folder, BinFile))))
                return true;
            else return false;
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems.Count <= 1)
            {
                splitContainer1.Panel2.Visible = false;
                amiiboToolStripMenuItem.Visible = false;

                if (File_IsEncrypted(listView1.SelectedItems[0].Text.Trim()))
                {
                    encryptedToolStripMenuItem.Text = i18n.__("Menu_Encrypted");
                    encryptedToolStripMenuItem.Image = Resources.locked;
                    decryptToolStripMenuItem.Text = i18n.__("Menu_Amiibo_Decrypt");
                }
                else
                {
                    encryptedToolStripMenuItem.Text = i18n.__("Menu_Decrypted");
                    encryptedToolStripMenuItem.Image = Resources.lock_open;
                    decryptToolStripMenuItem.Text = i18n.__("Menu_Amiibo_Encrypt");
                }

                Get_FileInfo(listView1.SelectedItems[0].Text.Trim());

                currentAmiiboToolStripMenuItem.Text = listView1.SelectedItems[0].Text.Trim();

                splitContainer1.Panel2.Visible = true;
                amiiboToolStripMenuItem.Visible = true;

                amiiboToolStripMenuItem.ShowDropDown();
            }
        }

        private void whereFindAmiiboKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(i18n.__("Message_Where_Find_Keys1") + Environment.NewLine +
                            i18n.__("Message_Where_Find_Keys2") + Environment.NewLine +
                            i18n.__("Message_Where_Find_Keys3") + Environment.NewLine +
                            i18n.__("Message_Where_Find_Keys4") + Environment.NewLine +
                            i18n.__("Message_Where_Find_Keys5") + Environment.NewLine +
                            i18n.__("Message_Where_Find_Keys6"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Process.Start("http://www.google.com/search?q=amiibo+retail+encryption+key+pastebin");
        }

        private void dumpAmiiboToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Flash_Form Form = new Flash_Form();
            Form.Bin_Folder = Current_Folder;
            Form.Action_Write = false;
            Form.ShowDialog();
        }

        private void selectbinFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = i18n.__("Folder_Bin_Selection");
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Config.Bin_Folder_Path = folderBrowserDialog1.SelectedPath;
                File.WriteAllText(AmiiBomb_Config_File, JsonConvert.SerializeObject(Config));

                LoadAmiiboBinFolder();
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void selectKeybinFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = i18n.__("Select_Key_Title");
            openFileDialog1.Filter = i18n.__("Select_Key_Filter", "|*.bin|", "|*.*");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Helper_Class.ValidSHA1(File.ReadAllBytes(openFileDialog1.FileName), Amiibo_Keys_Hash))
                {
                    Config.KeyFile_Path = openFileDialog1.FileName;
                    File.WriteAllText(AmiiBomb_Config_File, JsonConvert.SerializeObject(Config));
                }
                else
                {
                    DialogResult DgResult = MessageBox.Show(i18n.__("Select_Key_Message", Environment.NewLine), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DgResult == DialogResult.Yes) selectKeybinFileToolStripMenuItem.PerformClick();
                }
            }
        }

        private void Check_Clipboard()
        {
            while (true)
            {
                if (registerAmiiboKeyToolStripMenuItem.Checked)
                {
                    try
                    {
                        IDataObject ClipData = Clipboard.GetDataObject();
                        if (ClipData.GetData(DataFormats.Text) != null)
                        {
                            if (ClipData.GetDataPresent(DataFormats.Text))
                            {
                                string Clipboard_Text = ClipData.GetData(DataFormats.Text).ToString();
                                Clipboard_Text = Regex.Replace(Clipboard_Text, "[^a-zA-Z0-9-]", string.Empty);

                                if (Clipboard_Text.Length == 320)
                                {
                                    if (Helper_Class.ValidSHA1(Helper_Class.String_To_Byte_Array(Clipboard_Text), Amiibo_Keys_Hash))
                                    {
                                        this.Invoke(new Action(() =>
                                        {
                                            Helper_Class.FlashWindowEx(this);
                                            DialogResult DgResult = MessageBox.Show(this, i18n.__("Clipboard_Key_Message1"), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                            if (DgResult == DialogResult.Yes)
                                            {
                                                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                                                saveFileDialog1.Filter = i18n.__("Clipboard_Key_Filter", "|*.bin");
                                                saveFileDialog1.Title = i18n.__("Clipboard_Key_Title");

                                                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK && saveFileDialog1.FileName != "")
                                                {
                                                    byte[] clipboardStringBytes = Helper_Class.String_To_Byte_Array(Clipboard_Text);
                                                    Helper_Class.ByteArrayToFile(saveFileDialog1.FileName, clipboardStringBytes);
                                                    MessageBox.Show(this, i18n.__("Clipboard_Key_Message2"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                                else Clipboard.SetDataObject("");
                                            }
                                            else Clipboard.SetDataObject("");
                                        }));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception) {}
                }

                Application.DoEvents();
            }
        }

        private void AskBinFolder()
        {
            MessageBox.Show(i18n.__("Message_Ask_Bin_Folder"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            selectbinFolderToolStripMenuItem.PerformClick();
        }

        private void AskAmiiboKey()
        {
            DialogResult DgResult = MessageBox.Show(i18n.__("Message_Ask_Amiibo_Keys1"), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (DgResult == DialogResult.Yes)
            {
                selectKeybinFileToolStripMenuItem.PerformClick();
            }
            else
            {
                DgResult = MessageBox.Show(i18n.__("Message_Ask_Amiibo_Keys2"), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DgResult == DialogResult.Yes)
                {
                    thCheck_Clipboard.Start();
                    whereFindAmiiboKeyToolStripMenuItem.PerformClick();
                }
            }
        }

        private void Load_Languages()
        {
            string[] Lang_Files = Directory.GetFiles("lang", "*.lang");
            
            foreach (string LangFile in Lang_Files)
            {
                string FileLocale = Path.GetFileNameWithoutExtension(LangFile);
                ToolStripMenuItem items = new ToolStripMenuItem();
                items.Tag = FileLocale;
                items.Text = Translate_Class.GetLocale(FileLocale);
                items.Image = Image.FromFile(@"lang\" + FileLocale + ".png");
                items.ImageScaling = ToolStripItemImageScaling.None;
                items.Click += new EventHandler(LanguagesToolStripMenuItem_Click);
                languagesToolStripMenuItem.DropDownItems.Add(items);
            }
        }

        private void LanguagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage(((ToolStripMenuItem)sender).Tag.ToString());
        }

        private void Main_Form_Shown(object sender, EventArgs e)
        {
            thCheck_Clipboard = new Thread(Check_Clipboard);
            thCheck_Clipboard.SetApartmentState(ApartmentState.STA);

            Load_Languages();

            if (File.Exists(AmiiBomb_Config_File))
            {
                Config = JsonConvert.DeserializeObject<Config_Class>(File.ReadAllText(AmiiBomb_Config_File));
                if (Config.Bin_Folder_Path == null) AskBinFolder();
                if (Config.KeyFile_Path == null) AskAmiiboKey();
                if (Config.Locale == null) Config.Locale = I18n.GetLocale();

                switch (Config.Database)
                {
                    case 1:
                        amiibolifeToolStripMenuItem.Checked = false;
                        amiiboAPIToolStripMenuItem.Checked = true;
                        break;
                    default:
                        amiibolifeToolStripMenuItem.Checked = true;
                        amiiboAPIToolStripMenuItem.Checked = false;
                        break;
                }

                activeFilesCachingToolStripMenuItem.Checked = Config.Cache;
            }
            else
            {
                AskBinFolder();
                AskAmiiboKey();
            }

            if (Config.Locale != null)
            {
                I18n.SetLocale(Config.Locale);
                Translate_Class.Translate(this);
            }

            if (!thCheck_Clipboard.IsAlive) thCheck_Clipboard.Start();

            if (Config.Bin_Folder_Path != null)
            {
                LoadAmiiboBinFolder();
                treeView1.Nodes[0].Expand();
                treeView1.SelectedNode = treeView1.Nodes[0];
                FolderWatcher();
            }

            if (Config.KeyFile_Path != null) LoadAmiiboKey();

            Controls_Size();
        }
        

        private void Main_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thCheck_Clipboard.IsAlive) thCheck_Clipboard.Abort();
        }

        private void registerAmiiboKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registerAmiiboKeyToolStripMenuItem.Checked = !registerAmiiboKeyToolStripMenuItem.Checked;
        }

        private void Main_Form_Resize(object sender, EventArgs e)
        {
            Controls_Size();
        }

        private void Controls_Size()
        {
            int ListView_Width = 30;
            foreach (ColumnHeader column in listView1.Columns)
            {
                ListView_Width += column.Width;
            }

            if ((this.Width / 2) > ListView_Width) splitContainer1.SplitterDistance = ListView_Width;
            else if (this.Width > 160) splitContainer1.SplitterDistance = this.Width / 2;

            splitContainer3.SplitterDistance = 20;
        }

        private void moreInformationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://amiibo.life/nfc/" + Current_NFC_ID);
        }

        private void decryptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FileName = listView1.SelectedItems[0].Text.Trim();

            if (File_IsEncrypted(FileName))
            {
                File.WriteAllBytes(Path.Combine(Current_Folder, Path.GetFileNameWithoutExtension(FileName) + ".dec.bin"), Amiibo_Class.Decrypt(File.ReadAllBytes(Path.Combine(Current_Folder, FileName)), AmiiKeys));
            }
            else
            {
                File.WriteAllBytes(Path.Combine(Current_Folder, Path.GetFileNameWithoutExtension(FileName) + ".enc.bin"), Amiibo_Class.Encrypt(File.ReadAllBytes(Path.Combine(Current_Folder, FileName)), AmiiKeys));
            }
        }

        private void createTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Flash_Form Form = new Flash_Form();
            Form.Current_File_Bin = Path.Combine(Current_Folder, listView1.SelectedItems[0].Text.Trim());
            Form.Action_Write = true;
            Form.ShowDialog();
        }

        private void writeAppDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = i18n.__("Write_AppData_Title");
            openFileDialog1.Filter = i18n.__("Write_AppData_Filter", "|*.AppData|", "|*.*");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                byte[] AppData_Patched_File = Amiibo_Class.WriteAppData(File.ReadAllBytes(Path.Combine(Current_Folder, listView1.SelectedItems[0].Text.Trim())), File.ReadAllBytes(openFileDialog1.FileName));
                File.WriteAllBytes(Path.ChangeExtension(Path.Combine(Current_Folder, listView1.SelectedItems[0].Text.Trim()), ".AppData_Patched.bin"), AppData_Patched_File);
                MessageBox.Show(this, i18n.__("Write_AppData_Message1", Path.GetFileName(openFileDialog1.FileName)), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dumpAppDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = i18n.__("Dump_AppData_Filter", "|*.AppData");
            saveFileDialog1.Title = i18n.__("Dump_AppData_Title");
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(listView1.SelectedItems[0].Text.Trim()) + ".AppData";

            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK && saveFileDialog1.FileName != "")
            {
                byte[] AppData = Amiibo_Class.Dump_AppData(File.ReadAllBytes(Path.Combine(Current_Folder, listView1.SelectedItems[0].Text.Trim())));
                File.WriteAllBytes(saveFileDialog1.FileName, AppData);
                MessageBox.Show(this, i18n.__("Dump_AppData_Message1"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ChangeLanguage(string Locale)
        {
            DialogResult DgResult = MessageBox.Show(i18n.__("Message_Restart_AmiiBomb"), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DgResult == DialogResult.Yes)
            {
                Config.Locale = Locale;
                File.WriteAllText(AmiiBomb_Config_File, JsonConvert.SerializeObject(Config));

                Process.Start(Application.ExecutablePath);
                Application.Exit();
            }
        }

        private void deleteCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] Files = Directory.GetFiles("cache", "*.amiibomb");

            foreach (string CacheFile in Files)
            {
                File.Delete(CacheFile);
            }

            MessageBox.Show(this, i18n.__("Message_Cache_Deleted"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void activeFilesCachingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            activeFilesCachingToolStripMenuItem.Checked = !activeFilesCachingToolStripMenuItem.Checked;
            Config.Cache = activeFilesCachingToolStripMenuItem.Checked;
            File.WriteAllText(AmiiBomb_Config_File, JsonConvert.SerializeObject(Config));
        }

        private void howConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("lib\\Arduino-RC522.png");
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = (listView1.SelectedItems.Count <= 0);
        }

        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FilePath = Path.Combine(Current_Folder, listView1.SelectedItems[0].Text.Trim());
            DialogResult DgResult = MessageBox.Show(i18n.__("Message_Confirm_Delete"), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DgResult == DialogResult.Yes)
            {
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
        }

        private void internalFlasherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Arduino_Form().ShowDialog();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == i18n.__("Search"))
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;

                treeView1.Enabled = false;
                Search_Backup_Current_Folder = Current_Folder;
                File_Watcher.EnableRaisingEvents = false;
                Folder_Watcher.EnableRaisingEvents = false;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = i18n.__("Search");
                textBox1.ForeColor = Color.DimGray;

                Current_Folder = Search_Backup_Current_Folder;
                treeView1.Enabled = true;
                File_Watcher.EnableRaisingEvents = true;
                Folder_Watcher.EnableRaisingEvents = true;
                ListFiles(Current_Folder);
                splitContainer1.Panel2.Visible = false;
                amiiboToolStripMenuItem.Visible = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != i18n.__("Search"))
            {
                Exit_Search_Button.Visible = true;
                if (!String.IsNullOrWhiteSpace(textBox1.Text))
                {
                    
                    listView1.Items.Clear();
                    listView1.BeginUpdate();

                    foreach (string Search_File in Directory.EnumerateFiles(Config.Bin_Folder_Path, "*.bin", SearchOption.AllDirectories))
                    {
                        if (Path.GetFileName(Search_File.ToLower()).Contains(textBox1.Text.ToLower()))
                        {
                            Current_Folder = Config.Bin_Folder_Path;

                            ListViewItem lvItem = new ListViewItem(" " + Search_File.Replace(Config.Bin_Folder_Path, "").Substring(1), 0);
                            listView1.Items.Add(lvItem);
                        }
                    }

                    listView1.EndUpdate();
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
            else Exit_Search_Button.Visible = false;
        }

        private void AddSearchButton()
        {
            Exit_Search_Button.Size = new Size(25, textBox1.Height);
            Exit_Search_Button.Dock = DockStyle.Right;
            Exit_Search_Button.Cursor = Cursors.Default;
            Exit_Search_Button.BackgroundImage = new Bitmap(Properties.Resources.cancel, new Size(12, 12));
            Exit_Search_Button.BackgroundImageLayout = ImageLayout.Center;
            Exit_Search_Button.FlatStyle = FlatStyle.Flat;
            Exit_Search_Button.ForeColor = Color.White;
            Exit_Search_Button.FlatAppearance.BorderSize = 0;
            Exit_Search_Button.Click += Exit_Search_Button_Click;
            textBox1.Controls.Add(Exit_Search_Button);
            this.AcceptButton = Exit_Search_Button;
            SendMessage(textBox1.Handle, 0xd3, (IntPtr)2, (IntPtr)(Exit_Search_Button.Width << 16));
        }

        private void Exit_Search_Button_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            splitContainer3.Panel2.Focus();
        }

        private void Show_Search_Box()
        {
            Exit_Search_Button.PerformClick();

            if (splitContainer3.Panel1Collapsed)
            {
                splitContainer3.Panel1Collapsed = false;
                splitContainer3.Panel1.Show();
                textBox1.Focus();
            }
            else
            {
                splitContainer3.Panel1Collapsed = true;
                splitContainer3.Panel1.Hide();
            }
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show_Search_Box();
        }

        private void spoofRandomIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] Original_File = File.ReadAllBytes(Path.Combine(Current_Folder, listView1.SelectedItems[0].Text.Trim()));
            byte[] UID = Amiibo_Class.Generate_Random_UID();
            Array.Copy(UID, 0x000, Original_File, 0x000, UID.Length);
            File.WriteAllBytes(Path.ChangeExtension(Path.Combine(Current_Folder, listView1.SelectedItems[0].Text.Trim()), ".spoof.bin"), Original_File);

            MessageBox.Show(i18n.__("Message_Spoofed_Bin"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void amiibolifeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            amiibolifeToolStripMenuItem.Checked = true;
            amiiboAPIToolStripMenuItem.Checked = false;

            Config.Database = 0;
            File.WriteAllText(AmiiBomb_Config_File, JsonConvert.SerializeObject(Config));
        }

        private void amiiboAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            amiibolifeToolStripMenuItem.Checked = false;
            amiiboAPIToolStripMenuItem.Checked = true;

            Config.Database = 1;
            File.WriteAllText(AmiiBomb_Config_File, JsonConvert.SerializeObject(Config));
        }

        private void appDataEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch(Current_NFC_ID)
            {
                case "01030000-024F0902":
                    Editor_TPWolf_Form Form = new Editor_TPWolf_Form();
                    Form.Current_File_Bin = Path.Combine(Current_Folder, listView1.SelectedItems[0].Text.Trim());
                    Form.ShowDialog();
                    break;
                default:
                    MessageBox.Show(i18n.__("Message_No_AppData_Editor"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }            
        }

        private void powerSavesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PowerSaves_Form Form = new PowerSaves_Form();
            Form.Bin_Folder = Current_Folder;
            //Form.Action_Write = false;
            Form.ShowDialog();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Current_Folder = Config.Bin_Folder_Path + e.Node.FullPath.Substring(1);
            Last_Node = e.Node;
            e.Node.Expand();
            ListFiles(Current_Folder);
            FileWatcher();
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                Show_Search_Box();

                return true;
            }
            else if (keyData == (Keys.Control | Keys.N))
            {
                if (!amiiboToolStripMenuItem.Visible) return true;
                else base.ProcessCmdKey(ref msg, keyData);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void withXLoaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var executablePath = Application.StartupPath + "\\lib\\XLoader\\XLoader.exe";
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(executablePath);
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(executablePath);
            p.Start();
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(i18n.__("Message_Support_Me"), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                if (pictureBox1.Image.Width < pictureBox1.Width && pictureBox1.Image.Height < pictureBox1.Height)
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                else
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About_Form().ShowDialog();
        }
    }
}

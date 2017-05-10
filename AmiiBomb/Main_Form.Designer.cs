namespace AmiiBomb
{
    partial class Main_Form
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_Form));
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.deleteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectbinFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpAmiiboToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.flashAmiiBombuinoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.internalFlasherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.withXLoaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectKeybinFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.whereFindAmiiboKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerAmiiboKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activeFilesCachingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.languagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.amiiboToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moreInformationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.currentAmiiboToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encryptedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decryptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createTagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.dumpAppDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeAppDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(343, 399);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Amiibo Filenames";
            this.columnHeader1.Width = 250;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "amiibo-logo.png");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.amiiboToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.donateToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(854, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(854, 399);
            this.splitContainer1.SplitterDistance = 343;
            this.splitContainer1.TabIndex = 13;
            this.splitContainer1.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "    ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(3, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "      ";
            // 
            // deleteFileToolStripMenuItem
            // 
            this.deleteFileToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.delete;
            this.deleteFileToolStripMenuItem.Name = "deleteFileToolStripMenuItem";
            this.deleteFileToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.deleteFileToolStripMenuItem.Text = "Delete File...";
            this.deleteFileToolStripMenuItem.Click += new System.EventHandler(this.deleteFileToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(507, 399);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectbinFolderToolStripMenuItem,
            this.dumpAmiiboToolStripMenuItem,
            this.toolStripSeparator8,
            this.flashAmiiBombuinoToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.page;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectbinFolderToolStripMenuItem
            // 
            this.selectbinFolderToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.folder_vertical_open;
            this.selectbinFolderToolStripMenuItem.Name = "selectbinFolderToolStripMenuItem";
            this.selectbinFolderToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.selectbinFolderToolStripMenuItem.Text = "Select Amiibo Dump Folder";
            this.selectbinFolderToolStripMenuItem.Click += new System.EventHandler(this.selectbinFolderToolStripMenuItem_Click);
            // 
            // dumpAmiiboToolStripMenuItem
            // 
            this.dumpAmiiboToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.database_go;
            this.dumpAmiiboToolStripMenuItem.Name = "dumpAmiiboToolStripMenuItem";
            this.dumpAmiiboToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.dumpAmiiboToolStripMenuItem.Text = "Dump Amiibo...";
            this.dumpAmiiboToolStripMenuItem.Click += new System.EventHandler(this.dumpAmiiboToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(216, 6);
            // 
            // flashAmiiBombuinoToolStripMenuItem
            // 
            this.flashAmiiBombuinoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.internalFlasherToolStripMenuItem,
            this.withXLoaderToolStripMenuItem});
            this.flashAmiiBombuinoToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.package_go;
            this.flashAmiiBombuinoToolStripMenuItem.Name = "flashAmiiBombuinoToolStripMenuItem";
            this.flashAmiiBombuinoToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.flashAmiiBombuinoToolStripMenuItem.Text = "Flash AmiiBombuino...";
            // 
            // internalFlasherToolStripMenuItem
            // 
            this.internalFlasherToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.plugin;
            this.internalFlasherToolStripMenuItem.Name = "internalFlasherToolStripMenuItem";
            this.internalFlasherToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.internalFlasherToolStripMenuItem.Text = "with Internal Flasher";
            this.internalFlasherToolStripMenuItem.Click += new System.EventHandler(this.internalFlasherToolStripMenuItem_Click);
            // 
            // withXLoaderToolStripMenuItem
            // 
            this.withXLoaderToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.application_get;
            this.withXLoaderToolStripMenuItem.Name = "withXLoaderToolStripMenuItem";
            this.withXLoaderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.withXLoaderToolStripMenuItem.Text = "with XLoader";
            this.withXLoaderToolStripMenuItem.Click += new System.EventHandler(this.withXLoaderToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(216, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.cancel;
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyToolStripMenuItem,
            this.filesCacheToolStripMenuItem,
            this.toolStripSeparator7,
            this.languagesToolStripMenuItem});
            this.optionsToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.cog;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // keyToolStripMenuItem
            // 
            this.keyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectKeybinFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.whereFindAmiiboKeyToolStripMenuItem,
            this.registerAmiiboKeyToolStripMenuItem});
            this.keyToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.key;
            this.keyToolStripMenuItem.Name = "keyToolStripMenuItem";
            this.keyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.keyToolStripMenuItem.Text = "Keys";
            // 
            // selectKeybinFileToolStripMenuItem
            // 
            this.selectKeybinFileToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.key_add;
            this.selectKeybinFileToolStripMenuItem.Name = "selectKeybinFileToolStripMenuItem";
            this.selectKeybinFileToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.selectKeybinFileToolStripMenuItem.Text = "Select Amiibo Keys file";
            this.selectKeybinFileToolStripMenuItem.Click += new System.EventHandler(this.selectKeybinFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(297, 6);
            // 
            // whereFindAmiiboKeyToolStripMenuItem
            // 
            this.whereFindAmiiboKeyToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.service_status;
            this.whereFindAmiiboKeyToolStripMenuItem.Name = "whereFindAmiiboKeyToolStripMenuItem";
            this.whereFindAmiiboKeyToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.whereFindAmiiboKeyToolStripMenuItem.Text = "Where find Amiibo Keys?";
            this.whereFindAmiiboKeyToolStripMenuItem.Click += new System.EventHandler(this.whereFindAmiiboKeyToolStripMenuItem_Click);
            // 
            // registerAmiiboKeyToolStripMenuItem
            // 
            this.registerAmiiboKeyToolStripMenuItem.Checked = true;
            this.registerAmiiboKeyToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.registerAmiiboKeyToolStripMenuItem.Name = "registerAmiiboKeyToolStripMenuItem";
            this.registerAmiiboKeyToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.registerAmiiboKeyToolStripMenuItem.Text = "Check Amiibo Keys Hex Chars in Clipboard";
            this.registerAmiiboKeyToolStripMenuItem.Click += new System.EventHandler(this.registerAmiiboKeyToolStripMenuItem_Click);
            // 
            // filesCacheToolStripMenuItem
            // 
            this.filesCacheToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.activeFilesCachingToolStripMenuItem,
            this.deleteCacheToolStripMenuItem});
            this.filesCacheToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.script;
            this.filesCacheToolStripMenuItem.Name = "filesCacheToolStripMenuItem";
            this.filesCacheToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.filesCacheToolStripMenuItem.Text = "Files Cache";
            // 
            // activeFilesCachingToolStripMenuItem
            // 
            this.activeFilesCachingToolStripMenuItem.Checked = true;
            this.activeFilesCachingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.activeFilesCachingToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.script_lightning;
            this.activeFilesCachingToolStripMenuItem.Name = "activeFilesCachingToolStripMenuItem";
            this.activeFilesCachingToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.activeFilesCachingToolStripMenuItem.Text = "Active Files Caching";
            this.activeFilesCachingToolStripMenuItem.Click += new System.EventHandler(this.activeFilesCachingToolStripMenuItem_Click);
            // 
            // deleteCacheToolStripMenuItem
            // 
            this.deleteCacheToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.script_delete;
            this.deleteCacheToolStripMenuItem.Name = "deleteCacheToolStripMenuItem";
            this.deleteCacheToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteCacheToolStripMenuItem.Text = "Delete Cache";
            this.deleteCacheToolStripMenuItem.Click += new System.EventHandler(this.deleteCacheToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(149, 6);
            // 
            // languagesToolStripMenuItem
            // 
            this.languagesToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.world;
            this.languagesToolStripMenuItem.Name = "languagesToolStripMenuItem";
            this.languagesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.languagesToolStripMenuItem.Text = "Languages";
            // 
            // amiiboToolStripMenuItem
            // 
            this.amiiboToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.amiiboToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nameToolStripMenuItem,
            this.seriesToolStripMenuItem,
            this.moreInformationsToolStripMenuItem,
            this.toolStripSeparator3,
            this.currentAmiiboToolStripMenuItem,
            this.encryptedToolStripMenuItem,
            this.toolStripSeparator5,
            this.actionsToolStripMenuItem});
            this.amiiboToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("amiiboToolStripMenuItem.Image")));
            this.amiiboToolStripMenuItem.Name = "amiiboToolStripMenuItem";
            this.amiiboToolStripMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.amiiboToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.amiiboToolStripMenuItem.Text = "Amiibo";
            this.amiiboToolStripMenuItem.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.amiiboToolStripMenuItem.Visible = false;
            // 
            // nameToolStripMenuItem
            // 
            this.nameToolStripMenuItem.Enabled = false;
            this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
            this.nameToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.nameToolStripMenuItem.Text = "Name";
            // 
            // seriesToolStripMenuItem
            // 
            this.seriesToolStripMenuItem.Enabled = false;
            this.seriesToolStripMenuItem.Name = "seriesToolStripMenuItem";
            this.seriesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.seriesToolStripMenuItem.Text = "Series";
            // 
            // moreInformationsToolStripMenuItem
            // 
            this.moreInformationsToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.information;
            this.moreInformationsToolStripMenuItem.Name = "moreInformationsToolStripMenuItem";
            this.moreInformationsToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.moreInformationsToolStripMenuItem.Text = "...More informations";
            this.moreInformationsToolStripMenuItem.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.moreInformationsToolStripMenuItem.Click += new System.EventHandler(this.moreInformationsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(179, 6);
            // 
            // currentAmiiboToolStripMenuItem
            // 
            this.currentAmiiboToolStripMenuItem.Name = "currentAmiiboToolStripMenuItem";
            this.currentAmiiboToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.currentAmiiboToolStripMenuItem.Text = "Current Amiibo";
            // 
            // encryptedToolStripMenuItem
            // 
            this.encryptedToolStripMenuItem.Name = "encryptedToolStripMenuItem";
            this.encryptedToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.encryptedToolStripMenuItem.Text = "?Encrypted";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(179, 6);
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decryptToolStripMenuItem,
            this.createTagToolStripMenuItem,
            this.toolStripSeparator4,
            this.dumpAppDataToolStripMenuItem,
            this.writeAppDataToolStripMenuItem});
            this.actionsToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.database_go;
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.actionsToolStripMenuItem.Text = "...Actions";
            // 
            // decryptToolStripMenuItem
            // 
            this.decryptToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.lock_edit;
            this.decryptToolStripMenuItem.Name = "decryptToolStripMenuItem";
            this.decryptToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.decryptToolStripMenuItem.Text = "Decrypt";
            this.decryptToolStripMenuItem.Click += new System.EventHandler(this.decryptToolStripMenuItem_Click);
            // 
            // createTagToolStripMenuItem
            // 
            this.createTagToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.tag_blue_add;
            this.createTagToolStripMenuItem.Name = "createTagToolStripMenuItem";
            this.createTagToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.createTagToolStripMenuItem.Text = "Create NTAG";
            this.createTagToolStripMenuItem.Click += new System.EventHandler(this.createTagToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(153, 6);
            // 
            // dumpAppDataToolStripMenuItem
            // 
            this.dumpAppDataToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.application_side_contract;
            this.dumpAppDataToolStripMenuItem.Name = "dumpAppDataToolStripMenuItem";
            this.dumpAppDataToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.dumpAppDataToolStripMenuItem.Text = "Dump AppData";
            this.dumpAppDataToolStripMenuItem.Click += new System.EventHandler(this.dumpAppDataToolStripMenuItem_Click);
            // 
            // writeAppDataToolStripMenuItem
            // 
            this.writeAppDataToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.application_side_expand;
            this.writeAppDataToolStripMenuItem.Name = "writeAppDataToolStripMenuItem";
            this.writeAppDataToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.writeAppDataToolStripMenuItem.Text = "Write AppData";
            this.writeAppDataToolStripMenuItem.Click += new System.EventHandler(this.writeAppDataToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howConnectToolStripMenuItem,
            this.toolStripSeparator6,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.help;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // howConnectToolStripMenuItem
            // 
            this.howConnectToolStripMenuItem.Name = "howConnectToolStripMenuItem";
            this.howConnectToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.howConnectToolStripMenuItem.Text = "How connect RC522 to Arduino";
            this.howConnectToolStripMenuItem.Click += new System.EventHandler(this.howConnectToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(238, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.information;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.Image = global::AmiiBomb.Properties.Resources.controller;
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.donateToolStripMenuItem.Text = "Donate";
            this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 423);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main_Form";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AmiiBomb - 0.2 Alpha";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_Form_FormClosed);
            this.Shown += new System.EventHandler(this.Main_Form_Shown);
            this.Resize += new System.EventHandler(this.Main_Form_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem currentAmiiboToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seriesToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem encryptedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.MenuStrip menuStrip1;
        public System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem selectbinFolderToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem dumpAmiiboToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem keyToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem languagesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem selectKeybinFileToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem whereFindAmiiboKeyToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem registerAmiiboKeyToolStripMenuItem;
        public System.Windows.Forms.ListView listView1;
        public System.Windows.Forms.ToolStripMenuItem amiiboToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem filesCacheToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem activeFilesCachingToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem deleteCacheToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        public System.Windows.Forms.ToolStripMenuItem moreInformationsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem decryptToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem createTagToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem dumpAppDataToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem writeAppDataToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        public System.Windows.Forms.ToolStripMenuItem howConnectToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem flashAmiiBombuinoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.ToolStripMenuItem deleteFileToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem internalFlasherToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem withXLoaderToolStripMenuItem;
    }
}


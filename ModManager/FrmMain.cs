using Common;
using Filetypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ModManager
{
    public partial class FrmMain : Form
    {

        [DllImport("shell32.dll", ExactSpelling = true)]
        private static extern void ILFree(IntPtr pidlList);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern IntPtr ILCreateFromPathW(string pszPath);

        [DllImport("shell32.dll", ExactSpelling = true)]
        private static extern int SHOpenFolderAndSelectItems(IntPtr pidlList, uint cild, IntPtr children, uint dwFlags);

        private static string launchDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\The Creative Assembly\\Launcher";

        private static string workShopConfigFile;

        public FrmMain()
        {
            InitializeComponent();
        }

        //当前Data文件夹
        private string currentDir;
        //创意工坊文件夹
        private string workshopDir;
        //PFM文件位置
        private string pfmFile;
        //RPFM文件位置
        private string rpfmFile;

        FileSystemWatcher watcher = new FileSystemWatcher();
        FileSystemWatcher workshopWatcher = new FileSystemWatcher();
        FileSystemWatcher workshopConfigWatcher = new FileSystemWatcher();

        //记录所有表对应文件 <表,表所在文件列表>
        Dictionary<string, List<string>> tableFile = new Dictionary<string, List<string>>();
        //记录所有文件下的表 <文件,文件下的表>
        Dictionary<string, List<string>> fileTables = new Dictionary<string, List<string>>();
        //记录所有文件下的词条
        Dictionary<string, List<ModLine>> fileFields = new Dictionary<string, List<ModLine>>();
        //记录词条所在的文件 <目录.词条Key,所在文件>
        Dictionary<string, List<string>> fieldFiles = new Dictionary<string, List<string>>();

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //加载创意工坊Mod配置
            if (Directory.Exists(launchDir))
            {
                workShopConfigFile = Directory.GetFiles(launchDir, "*-moddata.dat").FirstOrDefault();
            }

            DBTypeMap.Instance.InitializeTypeMap(Path.GetDirectoryName(Application.ExecutablePath));

            watcher.Filter = "*.pack";
            watcher.Renamed += Watcher_Event;
            watcher.Changed += Watcher_Event;
            watcher.Created += Watcher_Event;
            watcher.Deleted += Watcher_Event;
            watcher.IncludeSubdirectories = false;

            workshopWatcher.Filter = "*.pack";
            workshopWatcher.Renamed += Watcher_Event;
            workshopWatcher.Changed += Watcher_Event;
            workshopWatcher.Created += Watcher_Event;
            workshopWatcher.Deleted += Watcher_Event;
            workshopWatcher.IncludeSubdirectories = true;

            workshopConfigWatcher.Filter = "*-moddata.dat";
            workshopConfigWatcher.IncludeSubdirectories = false;

            if (workShopConfigFile != null)
            {
                workshopConfigWatcher.Renamed += Watcher_Event;
                workshopConfigWatcher.Changed += Watcher_Event;
                workshopConfigWatcher.Created += Watcher_Event;
                workshopConfigWatcher.Deleted += Watcher_Event;
                workshopConfigWatcher.IncludeSubdirectories = false;
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["pfm"] != null)
            {
                this.pfmFile = config.AppSettings.Settings["pfm"].Value;
            }
            if (config.AppSettings.Settings["rpfm"] != null)
            {
                this.rpfmFile = config.AppSettings.Settings["rpfm"].Value;
            }
            if (config.AppSettings.Settings["checkField"] != null)
            {
                tsmiCheckLine.Checked = config.AppSettings.Settings["checkField"].Value != "0";
            }
            else
            {
                tsmiCheckLine.Checked = true;
                config.AppSettings.Settings.Add("checkField", "1");
                config.Save();
            }
            if (config.AppSettings.Settings["ignoreUnactive"] != null)
            {
                tsmiIgnoreUnActive.Checked = config.AppSettings.Settings["ignoreUnactive"].Value != "0";
            }
            else
            {
                tsmiIgnoreUnActive.Checked = true;
                config.AppSettings.Settings.Add("ignoreUnactive", "1");
                config.Save();
            }
            if (config.AppSettings.Settings["path"] != null)
            {
                this.currentDir = config.AppSettings.Settings["path"].Value;

                if (config.AppSettings.Settings["workshop"] != null)
                {
                    this.workshopDir = config.AppSettings.Settings["workshop"].Value;
                }
                else
                {
                    this.workshopDir = null;
                }
            }

            if (workShopConfigFile != null && File.Exists(workShopConfigFile))
            {
                string root = null;
                using (StreamReader sr = new StreamReader(workShopConfigFile))
                {
                    List<WorkshopInfo> workShopInfos = JsonConvert.DeserializeObject<List<WorkshopInfo>>(sr.ReadToEnd());
                    if (workShopInfos.Count > 0)
                    {
                        string packageFile = workShopInfos[0].Packfile;
                        if (packageFile != null)
                        {
                            packageFile = Path.GetFullPath(packageFile);
                            int idx = Path.GetFullPath(workShopInfos[0].Packfile).ToLower().IndexOf("steamapps");
                            root = packageFile.Substring(0, idx + 9);
                        }
                    }
                }
                if (root != null)
                {
                    //尝试自动检测创意工坊目录
                    if (workshopDir == null)
                    {
                        workshopDir = root;
                        if (config.AppSettings.Settings["workshop"] == null)
                        {
                            config.AppSettings.Settings.Add("workshop", null);
                        }
                        config.AppSettings.Settings["workshop"].Value = workshopDir;
                        config.Save();
                    }
                    //尝试自动检测游戏目录
                    if (currentDir == null)
                    {
                        currentDir = root + @"\common\Total War THREE KINGDOMS\data";
                        if (config.AppSettings.Settings["path"] == null)
                        {
                            config.AppSettings.Settings.Add("path", null);
                        }
                        config.AppSettings.Settings["path"].Value = currentDir;
                        config.Save();
                    }
                }
            }
            loadDir();
        }

        private void Watcher_Event(object sender, FileSystemEventArgs e)
        {
            Thread td = new Thread(rl);

            td.IsBackground = true;
            td.Start();
        }

        public void rl()
        {
            Thread.Sleep(500);
            Action d = () =>
            {
                loadDir();
            };
            this.Invoke(d);
        }

        private void loadRoot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["path"] != null)
            {
                dialog.SelectedPath = config.AppSettings.Settings["path"].Value;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (config.AppSettings.Settings["path"] == null)
                {
                    config.AppSettings.Settings.Add("path", null);
                }
                config.AppSettings.Settings["path"].Value = dialog.SelectedPath;
                config.Save();

                this.currentDir = dialog.SelectedPath;

                loadDir();
            }
        }


        private void tsmiWorkshop_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "设置到steamapps目录即可";
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["workshop"] != null)
            {
                dialog.SelectedPath = config.AppSettings.Settings["workshop"].Value;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.SelectedPath.ToLower().EndsWith("steamapps"))
                {

                    if (config.AppSettings.Settings["workshop"] == null)
                    {
                        config.AppSettings.Settings.Add("workshop", null);
                    }
                    config.AppSettings.Settings["workshop"].Value = dialog.SelectedPath;
                    config.Save();

                    this.workshopDir = dialog.SelectedPath;

                    loadDir();
                }
                else
                {
                    MessageBox.Show("您必须选择steamapps目录", "目录错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void loadDir()
        {
            lock (this)
            {

                tableFile.Clear();
                fileTables.Clear();
                listMods.Items.Clear();
                listWorkShop.Items.Clear();
                listTables.Items.Clear();
                fileFields.Clear();
                fieldFiles.Clear();

                if (currentDir == null || currentDir.Length == 0 || !Directory.Exists(currentDir))
                {
                    return;
                }

                //读取所有pack文件
                List<FileInfo> files = new DirectoryInfo(currentDir).GetFiles("*.pack").ToList();

                List<WorkshopInfo> workShopInfos = new List<WorkshopInfo>();

                //加载创意工坊配置
                if (workShopConfigFile != null && workShopConfigFile.Length > 0 && File.Exists(workShopConfigFile))
                {
                    using (StreamReader sr = new StreamReader(workShopConfigFile))
                    {
                        workShopInfos = JsonConvert.DeserializeObject<List<WorkshopInfo>>(sr.ReadToEnd());
                    }
                }

                //加载创意工坊pack文件
                if (workshopDir != null && workshopDir.Length > 0)
                {
                    string tmp = workshopDir + @"\workshop\content\779340";
                    foreach (DirectoryInfo dir in new DirectoryInfo(tmp).GetDirectories())
                    {
                        files.AddRange(dir.GetFiles("*.pack").ToList()); ;
                    }
                }

                //文件排除列表
                List<string> exp = new List<string>();

                //读取manifest.txt文件筛选系统mod
                string[] mainfests = Directory.GetFiles(currentDir, "manifest.txt");
                if (mainfests.Length > 0)
                {
                    using (StreamReader sr = new StreamReader(mainfests[0]))
                    {
                        string line = null;
                        while ((line = sr.ReadLine()) != null)
                        {
                            int idx = line.ToLower().IndexOf(".pack");
                            if (idx > 0)
                            {
                                exp.Add(line.Substring(0, idx + 5));
                            }
                        }
                    }
                }

                files = files.Where(p => !exp.Contains(p.Name)).OrderBy(p => p.Name).ToList();
                files.ForEach(file =>
                {
                    WorkshopInfo wk = workShopInfos.Find(p => Path.GetFullPath(p.Packfile) == Path.GetFullPath(file.FullName));
                    if (wk == null || wk.Active || !tsmiIgnoreUnActive.Checked)
                    {

                        List<PackedFile> packageFile = ReadPackageFile(file.FullName);
                        //填充文件-目录数据
                        fileTables.Add(file.FullName, packageFile.Select(pkg => pkg.FullPath).ToList());
                        fileFields[file.FullName] = new List<ModLine>();


                        packageFile.ForEach(p =>
                            {
                                //填充目录-文件数据
                                if (!tableFile.ContainsKey(p.FullPath))
                                {
                                    tableFile[p.FullPath] = new List<string>();
                                }

                                List<string> list = tableFile[p.FullPath];
                                list.Add(file.FullName);

                                if (tsmiCheckLine.Checked)
                                {
                                    //填充文件-词条数据
                                    List<ModLine> lines = ReadPackedField(p, file.FullName);

                                    List<ModLine> fields = fileFields[file.FullName];
                                    fields.AddRange(lines);

                                    //填充词条-文件数据
                                    lines.ForEach(line =>
                                                {
                                                    string key = line.TableName + "." + string.Join(".", line.FieldKeyValue);
                                                    if (!fieldFiles.ContainsKey(key))
                                                    {
                                                        fieldFiles[key] = new List<string>();
                                                    }
                                                    List<string> fs = fieldFiles[key];
                                                    fs.Add(file.FullName);
                                                });
                                }
                            });
                    }
                    else
                    {
                        //填充文件-目录数据
                        fileTables.Add(file.FullName, new List<string>());
                        fileFields[file.FullName] = new List<ModLine>();
                    }
                });

                //填充左侧Mod目录
                foreach (FileInfo file in files)
                {
                    ModFile mod = new ModFile();
                    mod.FileName = file.FullName;
                    mod.Name = Path.GetFileNameWithoutExtension(file.FullName);

                    //判断是否为创意工坊
                    mod.WorkshopInfo = workShopInfos.Find(p => Path.GetFullPath(p.Packfile) == Path.GetFullPath(file.FullName));

                    if (mod.WorkshopInfo == null || mod.WorkshopInfo.Active || !tsmiIgnoreUnActive.Checked)
                    {
                        //判断是否冲突
                        foreach (string table in fileTables[file.FullName])
                        {
                            if (tableFile[table].Count(p => p != file.FullName) > 0)
                            {
                                mod.ConflictTable = true;
                                break;
                            }
                        }
                        if (tsmiCheckLine.Checked)
                        {
                            //如果目录无冲突,则判断词条冲突情况
                            List<ModLine> lines = fileFields[mod.FileName];
                            foreach (ModLine l in lines)
                            {
                                if (fieldFiles[l.TableName + "." + string.Join(".", l.FieldKeyValue)].Count(p => p != file.FullName) > 0)
                                {
                                    mod.ConflictField = true;
                                    break;
                                }
                            }
                        }
                    }


                    if (mod.WorkshopInfo != null)
                    {
                        listWorkShop.Items.Add(mod);
                    }
                    else
                    {
                        listMods.Items.Add(mod);
                    }
                }

                watcher.Path = currentDir;
                watcher.EnableRaisingEvents = true;

                if (workshopDir != null && workshopDir.Length > 0)
                {
                    string tmp = workshopDir + @"\workshop\content\779340";
                    workshopWatcher.Path = tmp;
                    workshopWatcher.EnableRaisingEvents = true;
                }

                if (workShopConfigFile != null && workShopConfigFile.Length > 0)
                {
                    workshopConfigWatcher.Path = launchDir;
                    workshopConfigWatcher.EnableRaisingEvents = true;
                }
            }
        }

        private List<PackedFile> ReadPackageFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    PackFileCodec codec = new PackFileCodec();
                    return codec.Open(fileName).Files;
                }
                else
                {
                    return new List<PackedFile>();
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.ToString(), "读取文件[" + fileName + "]出错了", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return new List<PackedFile>();
            }
        }

        private List<ModLine> ReadPackedField(PackedFile file, string fileName)
        {
            if (File.Exists(fileName))
            {
                string key = DBFile.Typename(file.FullPath);
                DBFileHeader header = PackedFileDbCodec.readHeader(file);

                string exp = null;
                if (PackedFileDbCodec.CanDecode(file, out exp))
                {
                    PackedFileDbCodec codec = PackedFileDbCodec.GetCodec(file);

                    DBFile f = codec.Decode(file.Data);

                    if (f != null)
                    {
                        if (f.Entries.Count > 0)
                        {
                            //寻找主键字段
                            List<string> keys = f.CurrentType.Fields.Where(p => p.PrimaryKey).Select(p => p.Name).ToList();
                            if (keys.Count > 0)
                            {
                                return f.Entries.Select(line =>
                                 {
                                     ModLine modLine = new ModLine();
                                     modLine.FileName = fileName;
                                     modLine.TableName = f.CurrentType.Name;
                                     modLine.FieldKeyName = new string[keys.Count];
                                     modLine.FieldKeyValue = new string[keys.Count];
                                     for (int i = 0; i < keys.Count; i++)
                                     {
                                         modLine.FieldKeyName[i] = keys[i];
                                         modLine.FieldKeyValue[i] = line[keys[i]].Value;
                                     }
                                     return modLine;
                                 }).ToList();
                            }
                        }
                    }
                }
            }
            return new List<ModLine>();
        }

        private void listMods_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();

                ModFile file = (sender as ListBox).Items[e.Index] as ModFile;

                Brush mybsh = Brushes.Black;
                // 焦点框
                e.DrawFocusRectangle();
                if (file.ConflictTable)
                {
                    mybsh = Brushes.Red;
                }
                else if (file.ConflictField)
                {
                    mybsh = Brushes.Orange;
                }

                //文本 
                e.Graphics.DrawString(file.Name, e.Font, mybsh, e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void listWorkShop_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();

                ModFile file = (sender as ListBox).Items[e.Index] as ModFile;

                Brush mybsh = Brushes.Black;
                // 焦点框
                e.DrawFocusRectangle();
                if (file.ConflictTable)
                {
                    mybsh = Brushes.Red;
                }
                else if (file.ConflictField)
                {
                    mybsh = Brushes.Orange;
                }

                //文本 
                e.Graphics.DrawString((file.WorkshopInfo.Active ? "" : "(未激活)") + file.WorkshopInfo.Name, e.Font, mybsh, e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void listMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            listTables.Items.Clear();
            ListBox list = sender as ListBox;
            if (list.SelectedIndex >= 0)
            {
                ModFile mod = list.Items[list.SelectedIndex] as ModFile;

                listTables.Items.Add("文件:   " + mod.FileName);
                listTables.Items.Add("");

                //展示所有冲突的目录
                fileTables[mod.FileName].ForEach(table =>
                {
                    listTables.Items.Add("目录:   " + table);

                    bool conf = false;

                    List<string> allFiles = tableFile[table];
                    allFiles = allFiles.Where(p => p != mod.FileName).ToList();
                    if (allFiles.Count > 0)
                    {
                        allFiles.ForEach(o =>
                        {
                            ConflictInfo c = new ConflictInfo();
                            c.File = mod.FileName;
                            c.Table = table;
                            c.ConflictType = ConflictType.TABLE;
                            c.ConflictFile = o;
                            listTables.Items.Add(c);
                        });
                        conf = true;
                    }
                    //查看词条冲突情况
                    fileFields[mod.FileName].Where(p => p.TableName == DBFile.Typename(table)).ToList().ForEach(p =>
                        {
                            List<string> files = fieldFiles[p.TableName + "." + string.Join(".", p.FieldKeyValue)]
                                .Where(q => q != mod.FileName).ToList();
                            if (files.Count > 0)
                            {
                                files.ForEach(q =>
                                    {
                                        ConflictInfo c = new ConflictInfo();
                                        c.File = mod.FileName;
                                        c.Table = table;
                                        c.ConflictType = ConflictType.KEY;
                                        c.ConflictFile = q;
                                        c.FieldValue = p.FieldKeyValue;
                                        c.FieldName = p.FieldKeyName;
                                        listTables.Items.Add(c);
                                    });
                                conf = true;
                            }
                        });
                    if (!conf)
                    {
                        ConflictInfo con = new ConflictInfo();
                        con.File = mod.FileName;
                        con.Table = table;
                        con.ConflictType = ConflictType.NONE;
                        listTables.Items.Add(con);
                    }

                    listTables.Items.Add("");
                });

            }
        }

        private void listTables_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                e.DrawBackground();
                e.DrawFocusRectangle();

                ConflictInfo con = (sender as ListBox).Items[e.Index] as ConflictInfo;
                if (con != null && con.ConflictType != ConflictType.NONE)
                {
                    if (con.ConflictType == ConflictType.TABLE)
                    {
                        e.Graphics.DrawString("与文件 " + Path.GetFileName(con.ConflictFile) + " 冲突目录 " + con.Table, e.Font, Brushes.Red, e.Bounds, StringFormat.GenericDefault);
                    }
                    else if (con.ConflictType == ConflictType.KEY)
                    {
                        e.Graphics.DrawString("与文件 " + Path.GetFileName(con.ConflictFile) + " 重复词条 " + string.Join(" | ", con.FieldValue), e.Font, Brushes.Orange, e.Bounds, StringFormat.GenericDefault);
                    }
                }
                else if (con != null)
                {
                    e.Graphics.DrawString("该目录正常", e.Font, Brushes.Green, e.Bounds, StringFormat.GenericDefault);
                }
                else if ((sender as ListBox).Items[e.Index] != null)
                {
                    e.Graphics.DrawString((sender as ListBox).Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
                }
            }
        }

        private void tsmiReload_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["path"] != null)
            {
                loadDir();
            }
        }

        private void tsmiCheckLine_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void listMods_DoubleClick(object sender, EventArgs e)
        {
            ListBox list = sender as ListBox;

            if (list.SelectedIndex >= 0)
            {
                ModFile mod = list.Items[list.SelectedIndex] as ModFile;
                ExplorerFile(mod.FileName);
            }
        }

        private void listMods_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListBox list = sender as ListBox;
                int index = list.IndexFromPoint(e.Location);
                if (index >= 0 && index != 65535)
                {
                    list.SelectedIndex = index;
                    ctxMenu.Tag = (list.Items[list.SelectedIndex] as ModFile).FileName;
                    ctxMenu.Show(list, e.Location);
                }
            }
        }

        private void pFMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pfmFile != null && pfmFile.Length > 0)
            {
                Process.Start(pfmFile, "\"" + (sender as ToolStripMenuItem).GetCurrentParent().Tag + "\"");
            }
            else
            {
                MessageBox.Show("请设置PFM的路径");
            }

        }

        private void 用RPFM打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rpfmFile != null && rpfmFile.Length > 0)
            {
                Process.Start(rpfmFile, "\"" + (sender as ToolStripMenuItem).GetCurrentParent().Tag + "\"");
            }
            else
            {
                MessageBox.Show("请设置RPFM的路径");
            }
        }

        private void 打开文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExplorerFile((sender as ToolStripMenuItem).GetCurrentParent().Tag.ToString());
        }


        private void 设置PFM路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "可执行文件|*.exe";

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["pfm"] != null)
            {
                dialog.FileName = config.AppSettings.Settings["pfm"].Value;
            }
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {

                if (config.AppSettings.Settings["pfm"] == null)
                {
                    config.AppSettings.Settings.Add("pfm", null);
                }
                config.AppSettings.Settings["pfm"].Value = dialog.FileName;
                config.Save();

                this.pfmFile = dialog.FileName;
            }
        }

        private void 设置RPFM路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "可执行文件|*.exe";

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["rpfm"] != null)
            {
                dialog.FileName = config.AppSettings.Settings["rpfm"].Value;
            }
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {

                if (config.AppSettings.Settings["rpfm"] == null)
                {
                    config.AppSettings.Settings.Add("rpfm", null);
                }
                config.AppSettings.Settings["rpfm"].Value = dialog.FileName;
                config.Save();

                this.rpfmFile = dialog.FileName;
            }
        }







        public static void ExplorerFile(string filePath)
        {
            if (!File.Exists(filePath) && !Directory.Exists(filePath))
                return;

            if (Directory.Exists(filePath))
            {
                Process.Start(@"explorer.exe", "/select,\"" + filePath + "\"");
            }
            else
            {
                IntPtr pidlList = ILCreateFromPathW(filePath);
                if (pidlList != IntPtr.Zero)
                {
                    try
                    {
                        Marshal.ThrowExceptionForHR(SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0));
                    }
                    finally
                    {
                        ILFree(pidlList);
                    }
                }
            }
        }

        private void listTables_DoubleClick(object sender, EventArgs e)
        {
            if (listTables.SelectedIndex >= 0)
            {
                ConflictInfo con = listTables.Items[listTables.SelectedIndex] as ConflictInfo;
                if (con != null && con.ConflictFile != null)
                {
                    ExplorerFile(con.ConflictFile);
                }
                else if (con != null && con.File != null)
                {
                    ExplorerFile(con.File);
                }
            }
        }

        private void listTables_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = listTables.IndexFromPoint(e.Location);
                if (index >= 0 && index != 65535)
                {
                    listTables.SelectedIndex = index;
                    ConflictInfo mod = listTables.Items[listTables.SelectedIndex] as ConflictInfo;
                    if (mod != null)
                    {
                        ctxMenu.Tag = mod.ConflictFile ?? mod.File;
                        ctxMenu.Show(listTables, e.Location);
                    }
                }
            }
        }

        private void 用PFM打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listTables.SelectedIndex >= 0)
            {
                if (pfmFile != null && pfmFile.Length > 0)
                {
                    ConflictInfo mod = listTables.Items[listTables.SelectedIndex] as ConflictInfo;
                    if (mod != null && mod.ConflictFile != null)
                    {
                        Process.Start(pfmFile, "\"" + mod.ConflictFile + "\"");
                    }
                    else if (mod != null && mod.File != null)
                    {
                        Process.Start(pfmFile, "\"" + mod.File + "\"");
                    }
                }
                else
                {
                    MessageBox.Show("请设置PFM的路径");
                }
            }
        }

        private void 用RPFM打开ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listTables.SelectedIndex >= 0)
            {
                if (rpfmFile != null && rpfmFile.Length > 0)
                {
                    ConflictInfo mod = listTables.Items[listTables.SelectedIndex] as ConflictInfo;
                    if (mod != null && mod.ConflictFile != null)
                    {
                        Process.Start(rpfmFile, "\"" + mod.ConflictFile + "\"");
                    }
                    else if (mod != null && mod.File != null)
                    {
                        Process.Start(rpfmFile, "\"" + mod.ConflictFile + "\"");
                    }
                }
                else
                {
                    MessageBox.Show("请设置RPFM的路径");
                }
            }
        }

        private void 打开文件夹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listTables.SelectedIndex >= 0)
            {
                ConflictInfo mod = listTables.Items[listTables.SelectedIndex] as ConflictInfo;
                if (mod != null && mod.ConflictFile != null)
                {
                    ExplorerFile(mod.ConflictFile);
                }
                else if (mod != null && mod.File != null)
                {
                    ExplorerFile(mod.File);
                }
            }
        }

        private void tsmiIrgnoreUnActive_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tsmiCheckLine_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["checkField"] != null)
            {
                config.AppSettings.Settings["checkField"].Value = (tsmiCheckLine.Checked ? "1" : "0");
            }
            else
            {
                config.AppSettings.Settings.Add("checkField", tsmiCheckLine.Checked ? "1" : "0");
            }
            config.Save();

            loadDir();
        }

        private void tsmiIgnoreUnActive_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["ignoreUnactive"] != null)
            {
                config.AppSettings.Settings["ignoreUnactive"].Value = (tsmiIgnoreUnActive.Checked ? "1" : "0");
            }
            else
            {
                config.AppSettings.Settings.Add("ignoreUnactive", tsmiIgnoreUnActive.Checked ? "1" : "0");
            }
            config.Save();

            loadDir();
        }
    }
}
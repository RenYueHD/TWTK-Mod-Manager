using Common;
using Filetypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModManager
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private string currentDir;
        FileSystemWatcher watcher = new FileSystemWatcher();

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
            DBTypeMap.Instance.InitializeTypeMap(Path.GetDirectoryName(Application.ExecutablePath));

            watcher.Filter = "*.pack";
            watcher.Renamed += Watcher_Event;
            watcher.Changed += Watcher_Event;
            watcher.Created += Watcher_Event;
            watcher.Deleted += Watcher_Event;
            watcher.IncludeSubdirectories = false;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["checkField"] != null)
            {
                tsmiCheckLine.Checked = config.AppSettings.Settings["checkField"].Value != "0";
            }
            else
            {
                tsmiCheckLine.Checked = true;
            }
            if (config.AppSettings.Settings["path"] != null)
            {
                loadDir(config.AppSettings.Settings["path"].Value);
            }
        }

        private void Watcher_Event(object sender, FileSystemEventArgs e)
        {
            Action d = () =>
            {
                loadDir(currentDir);
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

                loadDir(dialog.SelectedPath);
            }
        }

        private void loadDir(string dir)
        {
            lock (this)
            {
                tableFile.Clear();
                fileTables.Clear();
                listMods.Items.Clear();
                listTables.Items.Clear();
                fileFields.Clear();
                fieldFiles.Clear();

                //读取所有pack文件
                List<FileInfo> files = new DirectoryInfo(dir).GetFiles("*.pack").ToList();

                //文件排除列表
                List<string> exp = new List<string>();

                //读取manifest.txt文件筛选系统mod
                string[] mainfests = Directory.GetFiles(dir, "manifest.txt");
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
                });


                //填充左侧Mod目录
                foreach (FileInfo file in files)
                {
                    ModFile mod = new ModFile();
                    mod.FileName = file.FullName;
                    mod.Name = Path.GetFileNameWithoutExtension(file.FullName);
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

                    listMods.Items.Add(mod);
                }

                currentDir = dir;

                watcher.Path = dir;
                watcher.EnableRaisingEvents = true;
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


        private void listMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            listTables.Items.Clear();
            if (listMods.SelectedIndex >= 0)
            {
                ModFile mod = listMods.Items[listMods.SelectedIndex] as ModFile;
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
                loadDir(config.AppSettings.Settings["path"].Value);
            }
        }

        private void tsmiCheckLine_CheckedChanged(object sender, EventArgs e)
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
            if (currentDir != null)
            {
                loadDir(currentDir);
            }
        }
    }
}

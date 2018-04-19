using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TagLibrary.NetworkHelper;
using TagLibrary.Models;

namespace TagLibrary.Windows {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {

        private string version;
        private int versionMajor = 0;
        private int versionMinor = 1;
        private int versionBuild = 0;

        SqlSugarClient db;

        List<TagLibrary.Models.FileInfo> files;
        List<TagInfo> tags;
        List<FileTagMapper> mappers;
        BindingList<HostInfo> hosts = new BindingList<HostInfo>();


        /// <summary>
        /// 构造方法
        /// </summary>
        public MainWindow() {

            InitializeComponent();
            //初始化存储目录
            Directory.CreateDirectory("library");

            #region TEST
            //File.Delete("data.db");
            #endregion

            #region 初始化数据库

            #region sqlite文件检查 (暂时未实现)
            //try {
            //    bool isTableExists;
            //    using (var sqlite = new SQLiteConnection("Data Source=data.db;Version=3;")) {
            //        sqlite.Open();
            //        var tableExists = new SQLiteCommand("SELECT count(*) FROM sqlite_master WHERE type='table' AND name='TagLibrary';", sqlite);
            //        var result = tableExists.ExecuteReader();
            //        result.Read();
            //        if (result.GetInt32(0) == 0) {
            //            isTableExists = false;
            //        } else {
            //            isTableExists = true;
            //        }
            //        sqlite.Close();
            //    }

            //    if (!isTableExists) {
            //        GC.Collect();
            //        GC.WaitForPendingFinalizers();
            //        File.Delete("data.db");
            //        using (var sqlite = new SQLiteConnection("Data Source=data.db;Version=3;")) {
            //            sqlite.Open();
            //            var createTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS TagLibrary (Major INT, Minor INT, Build INT);", sqlite);
            //            createTable.ExecuteNonQuery();
            //            sqlite.Close();
            //        }
            //    }
            //} catch {
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    File.Delete("data.db");
            //    using (var sqlite = new SQLiteConnection("Data Source=data.db;Version=3;")) {
            //        sqlite.Open();
            //        var createTable = new SQLiteCommand("CREATE TABLE IF NOT EXISTS TagLibrary (Major INT, Minor INT, Build INT);", sqlite);
            //        createTable.ExecuteNonQuery();
            //        sqlite.Close();
            //    }
            //}
            #endregion

            //初始化SqlSugar
            db = new SqlSugarClient(new ConnectionConfig() {
                DbType = DbType.Sqlite,
                ConnectionString = "Data Source=data.db;Version=3;",
                InitKeyType = InitKeyType.Attribute
            });
            //检查版本并进行版本迁移工作
            CheckVersion();
            CodeFirst();
            #endregion

            #region TEST
            //TestData();
            #endregion

            #region 初始化字段
            version = string.Format("{0}.{1}.{2:000000}", versionMajor, versionMinor, versionBuild);
            #endregion

            #region 从数据库中读取数据



            //初始化文件列表
            files = db.Queryable<TagLibrary.Models.FileInfo>().ToList();
            fileList.ItemsSource = new BindingList<TagLibrary.Models.FileInfo>();
            foreach (var item in files) {
                (fileList.ItemsSource as BindingList<TagLibrary.Models.FileInfo>).Add(item);
            }

            //初始化文件格式列表
            //var fileFormats = new BindingList<TreeViewItem>();
            //fileList.GroupBy(fileInfo => fileInfo.Format)
            //    .Select(fileInfo => fileInfo.Key)
            //    .ToList()
            //    .ForEach(format => fileFormats.Add(new TreeViewItem() {
            //        Header = format
            //    }));
            //tagTree.ItemsSource = fileFormats;

            //初始化Tag列表
            tags = db.Queryable<TagInfo>().ToList();
            //var groups = new BindingList<TreeViewItem>();
            //foreach (var item in tags.GroupBy(item => item.Group).Select(item => item.First())) {
            //    var group = new TreeViewItem() {
            //        DataContext = item,
            //        ItemsSource = new BindingList<CheckBox>()
            //    };
            //    group.SetBinding(TreeViewItem.HeaderProperty, new Binding("Group"));
            //    groups.Add(group);
            //}
            //foreach (var item in tags) {
            //    var tag = new CheckBox() { DataContext = item };
            //    tag.SetBinding(ContentProperty, new Binding("Name"));
            //    tag.Checked += Tag_Checked;
            //    tag.Unchecked += Tag_Checked;
            //    (groups.Where(group => (group.DataContext as TagInfo).Group == item.Group).First().ItemsSource as BindingList<CheckBox>)
            //        .Add(tag);
            //}
            tagTree.Tags = tags;
            //foreach (var item in tags) {
            //    var tag = new CheckBox() { DataContext = item };
            //    tag.SetBinding(ContentProperty, new Binding("Name"));
            //    tag.Checked += Tag_Checked;
            //    tag.Unchecked += Tag_Checked;
            //    if ((tagTree.ItemsSource as BindingList<TreeViewItem>).Where(group => (group.DataContext as TagInfo).Group == item.Group).Count() == 0) {
            //        var group = new TreeViewItem() {
            //            DataContext = item,
            //            ItemsSource = new BindingList<CheckBox>() { tag }
            //        };
            //        group.SetBinding(HeaderedItemsControl.HeaderProperty, new Binding("Group"));
            //        (tagTree.ItemsSource as BindingList<TreeViewItem>).Add(group);
            //    } else {
            //        ((tagTree.ItemsSource as BindingList<TreeViewItem>).Where(group => (group.DataContext as TagInfo).Group == item.Group).First().ItemsSource as BindingList<CheckBox>)
            //            .Add(tag);
            //    }
            //}

            //初始化映射列表
            mappers = db.Queryable<FileTagMapper>().ToList();

            hostSelect.ItemsSource = hosts;
            hostList.ItemsSource = hosts;
            hosts.Add(new HostInfo("localhost"));
            hosts.Add(new HostInfo("127.0.0.1"));
            hosts.Add(new HostInfo("127.0.0.2"));
            hosts.Add(new HostInfo("127.0.0.3"));
            hosts.Add(new HostInfo("127.0.0.4"));
            hosts.Add(new HostInfo("127.0.0.5"));
            hosts.Add(new HostInfo("127.0.0.6"));
            hosts.Add(new HostInfo("127.0.0.7"));
            hosts.Add(new HostInfo("127.0.0.8"));
            hosts.Add(new HostInfo("127.0.0.9"));
            hosts.Add(new HostInfo("127.0.0.10"));
            hosts.Add(new HostInfo("127.0.0.11"));
            hosts.Add(new HostInfo("127.0.0.12"));
            hostSelect.SelectedIndex = 0;

            #endregion

            #region 关联事件
            #endregion

        }

        #region Click Event

        private void Test_Click(object sender, RoutedEventArgs e) {
            new TextRange(test1.Document.ContentStart, test1.Document.ContentEnd).Text = "";
            if (hosts[0].Status == "online") {
                hosts[0].Status = "offline";
            } else {
                hosts[0].Status = "online";
            }

        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFile_Click(object sender, RoutedEventArgs e) {
            var addFile = new AddFile() {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                Tags = tags ?? new List<TagInfo>()
            };
            addFile.ShowDialog();
        }

        /// <summary>
        /// 添加标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTag_Click(object sender, RoutedEventArgs e) {
            var addTag = new AddTag() {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            addTag.ShowDialog();
        }

        /// <summary>
        /// 菜单栏->文件->关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("真的要退出么！", "TagLibrary", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// 菜单栏->关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show($"TagLibrary\nVersion {version}\nCopyright © 2018 by LiRui");
        }

        /// <summary>
        /// 菜单栏->设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, RoutedEventArgs e) {
            var settings = new Setting {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            settings.ShowDialog();
        }

        #endregion

        /// <summary>
        /// 文件列表选择项变化时的处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            new TextRange(test1.Document.ContentStart, test1.Document.ContentEnd).Text = "";
            test1.AppendText("FileList_SelectionChanged");
        }

        /// <summary>
        /// 检查数据库版本
        /// </summary>
        private void CheckVersion() {
            if (db.DbMaintenance.IsAnyTable("TagLibrary")) {
                var result = db.Ado.SqlQueryDynamic("SELECT * FROM TagLibrary ORDER BY Major, Minor , Build DESC LIMIT 0, 1;");
                if (result[0].Major != versionMajor || result[0].Minor != versionMinor || result[0].Build != versionBuild) {
                    db.Ado.ExecuteCommand("INSERT INTO TagLibrary (Major, Minor, Build) VALUES (@Major, @Minor, @Build);"
                    , new List<SugarParameter>() {
                        new SugarParameter("Major", versionMajor),
                        new SugarParameter("Minor", versionMinor),
                        new SugarParameter("Build", versionBuild),
                    });
                }
                // 进行版本迁移操作
            } else {
                db.Ado.ExecuteCommand("CREATE TABLE IF NOT EXISTS TagLibrary (Major INT, Minor INT, Build INT);");
                db.Ado.ExecuteCommand("INSERT INTO TagLibrary (Major, Minor, Build) VALUES (@Major, @Minor, @Build);"
                    , new List<SugarParameter>() {
                        new SugarParameter("Major", versionMajor),
                        new SugarParameter("Minor", versionMinor),
                        new SugarParameter("Build", versionBuild),
                    });
            }
        }

        /// <summary>
        /// 根据实体生成数据库表
        /// </summary>
        private void CodeFirst() {
            if (!db.DbMaintenance.IsAnyTable("FileInfo")) {
                db.CodeFirst.InitTables(typeof(TagLibrary.Models.FileInfo));
            }
            if (!db.DbMaintenance.IsAnyTable("TagInfo")) {
                db.CodeFirst.InitTables(typeof(TagInfo));
                db.Insertable(new TagInfo() {
                    Name = "共享",
                    Group = "Default"
                });

            }
            if (!db.DbMaintenance.IsAnyTable("FileTagMapper")) {
                db.CodeFirst.InitTables(typeof(FileTagMapper));
            }
        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="fullFileName"></param>
        private void AddFile(string fullFileName) {
            var fileInfo = new TagLibrary.Models.FileInfo() {
                Name = Path.GetFileName(fullFileName),
                UUID = Guid.NewGuid().ToString().Replace("-", ""),
                OriginalPath = Path.GetDirectoryName(fullFileName),
                Format = Path.GetExtension(fullFileName).TrimStart('.').ToUpper()
            };
            fileInfo = db.Insertable(fileInfo).ExecuteReturnEntity();
            //(fileList.ItemsSource as BindingList<TagLibrary.Models.FileInfo>).Add(fileInfo);
            files.Add(fileInfo);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId"></param>
        private void RemoveFile(int fileId) {
            db.Deleteable<TagLibrary.Models.FileInfo>(fileId).ExecuteCommand();
            (fileList.ItemsSource as BindingList<TagLibrary.Models.FileInfo>)
                .Remove((fileList.ItemsSource as BindingList<TagLibrary.Models.FileInfo>).Where(fileInfo => fileInfo.Id == fileId).First());

        }

        /// <summary>
        /// 添加主机
        /// </summary>
        /// <param name="host"></param>
        private void AddHost(string host, string status = "offline") {
            int count = hosts.Where(item => item.Host == host).Count();
            if (count == 0) {
                hosts.Add(new HostInfo(host, status));
            } else {
                hosts.Where(item => item.Host == host).First().Status = status;
            }
        }

        /// <summary>
        /// 移除主机
        /// </summary>
        /// <param name="host"></param>
        private void RemoveHost(string host) {
            hosts.Remove(hosts.Where(item => item.Host == host).First());
        }

        /// <summary>
        /// 添加Tag
        /// </summary>
        /// <param name="tagInfo"></param>
        //private void AddTag(TagInfo tagInfo) {
        //    // 判断是否有重复TagInfo
        //    if (tags.Where(item => item.Name == tagInfo.Name && item.Group == tagInfo.Group).Count() == 0) {
        //        tagInfo = db.Insertable(tagInfo).ExecuteReturnEntity();
        //        tags.Add(tagInfo);
        //        var tag = new CheckBox() { DataContext = tagInfo };
        //        tag.SetBinding(ContentProperty, new Binding("Name"));
        //        tag.Checked += Tag_Checked;
        //        tag.Unchecked += Tag_Checked;
        //        var groups = tagTree.ItemsSource as BindingList<TreeViewItem>;
        //        if (groups.Where(item => (item.DataContext as TagInfo).Group == tagInfo.Group).Count() == 0) {
        //            var group = new TreeViewItem() {
        //                DataContext = tagInfo,
        //                ItemsSource = new BindingList<CheckBox>() { tag }
        //            };
        //            group.SetBinding(HeaderedItemsControl.HeaderProperty, new Binding("Group"));
        //            groups.Add(group);
        //        } else {
        //            (groups.Where(group => (group.DataContext as TagInfo).Group == tagInfo.Group).First().ItemsSource as BindingList<CheckBox>)
        //                .Add(tag);
        //        }
        //    }
        //}

        /// <summary>
        /// 删除Tag
        /// </summary>
        /// <param name="tagInfo"></param>
        //private void RemoveTag(TagInfo tagInfo) {
        //}

        private void TestData() {
            foreach (var file in Directory.EnumerateFiles("library")) {
                File.Delete(file);
            }

            #region Add File
            foreach (var file in Directory.EnumerateFiles(@"D:\test")) {
                var fileInfo = new TagLibrary.Models.FileInfo() {
                    Name = Path.GetFileName(file),
                    UUID = Guid.NewGuid().ToString().Replace("-", ""),
                    OriginalPath = Path.GetDirectoryName(file),
                    Format = Path.GetExtension(file).TrimStart('.').ToUpper(),
                    Size = File.OpenRead(file).Length
                };
                db.Insertable(fileInfo).ExecuteCommand();
                File.Copy(file, "library\\" + fileInfo.UUID + Path.GetExtension(file));
            }
            #endregion
            #region Add Tag

            var groups = new List<string>();
            for (int i = 0; i < 3; i++) {
                groups.Add(Guid.NewGuid().ToString().Split('-')[1]);
            }

            for (int i = 0; i < 5; i++) {
                var tagInfo = new TagInfo() {
                    Name = Guid.NewGuid().ToString().Split('-')[1],
                    Group = groups[Guid.NewGuid().ToByteArray()[0] % 3]
                };
                db.Insertable(tagInfo).ExecuteCommand();
            }

            #endregion
            #region Add Mapper

            for (int i = 0; i < 10; i++) {
                db.Insertable(new FileTagMapper() {
                    FileId = Guid.NewGuid().ToByteArray()[8] % 15,
                    TagId = Guid.NewGuid().ToByteArray()[8] % 5
                }).ExecuteCommand();
            }

            #endregion
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: 释放托管状态(托管对象)。
                    db?.Dispose();
                    db = null;
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~MainWindow() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose() {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

        private void MenuItemSetTag_Click(object sender, RoutedEventArgs e) {
            var setTagWindow = new SetTag() {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Tags = tags,
                File = fileList.SelectedItem as Models.FileInfo
            };
            setTagWindow.ShowDialog();
        }
    }
}

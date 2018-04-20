using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        List<FileInfo> files;
        List<TagInfo> tags;
        List<FileTagMapper> mappers;
        BindingList<HostInfo> hosts = new BindingList<HostInfo>();


        /// <summary>
        /// 构造方法
        /// </summary>
        public MainWindow() {

            InitializeComponent();
            //初始化存储目录
            System.IO.Directory.CreateDirectory("library");

            #region TEST
            //System.IO.File.Delete("data.db");
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
            files = db.Queryable<FileInfo>().ToList();
            fileList.ItemsSource = new BindingList<FileInfo>();
            foreach (var item in files) {
                (fileList.ItemsSource as BindingList<FileInfo>).Add(item);
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
            hostSelect.SelectedIndex = 0;

            #endregion

            #region 关联事件
            #endregion

        }

        #region 菜单栏事件处理方法

        private void Test_Click(object sender, RoutedEventArgs e) {
            new TextRange(test1.Document.ContentStart, test1.Document.ContentEnd).Text = "";
            int[] i = { 1, 2, 3, 4, 5 };
            int[] j = { 3, 4, 5, 6, 7 };
            var t =
            i.Select(x => x as int?).ToList();
            t.Add(null);
            var result1 = i.Except(j);
            var result2 = j.Except(i);
        }

        /// <summary>
        /// 菜单栏->文件->添加文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_File_AddFile_Click(object sender, RoutedEventArgs e) {
            var addFileWindow = new AddFile() {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                Tags = tags.Where(x=> !x.Group.StartsWith("Default - ")).ToList() ?? new List<TagInfo>()
            };
            addFileWindow.ShowDialog();
            if (addFileWindow.IsOK) {
                foreach (var filename in addFileWindow.FileNames) {
                    AddFile(filename);
                    var oldTags = addFileWindow.SelectedTags.Where(x => x.Id != null).ToList();
                    var newTags = addFileWindow.SelectedTags.Where(x => x.Id == null).ToList();
                    newTags.ForEach(x => AddTag(x));
                    var newTagsWithId = tags.Join(newTags, x => new { x.Name, x.Group }, y => new { y.Name, y.Group }, (x, y) => x);
                    AddMapper(files.Where(x => x.Name == System.IO.Path.GetFileName(filename) && x.OriginalPath == System.IO.Path.GetDirectoryName(filename)).First()
                        , oldTags.Concat(newTagsWithId).ToList());
                    TagTree_TagCheckChanged(tagTree.SelectedTag);
                }
            }
        }

        /// <summary>
        /// 菜单栏->标签->添加标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_File_AddTag_Click(object sender, RoutedEventArgs e) {
            var addTagWindow = new AddTag() {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            addTagWindow.ShowDialog();
            if (addTagWindow.IsOK) {
                AddTag(new TagInfo() {
                    Name = addTagWindow.TagName,
                    Group = addTagWindow.TagGroup
                });
            }
        }

        /// <summary>
        /// 菜单栏->文件->关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_File_Close_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("真的要退出么！", "TagLibrary", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// 菜单栏->关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_File_About_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show($"TagLibrary\nVersion {version}\nCopyright © 2018 by LiRui");
        }

        /// <summary>
        /// 菜单栏->设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_File_Settings_Click(object sender, RoutedEventArgs e) {
            var settings = new Setting {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            settings.ShowDialog();
        }

        #endregion

        #region 文件列表事件处理方法

        /// <summary>
        /// 文件列表->右键->设置标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileList_ContextMenu_SetTag_Click(object sender, RoutedEventArgs e) {
            var setTagWindow = new SetTag() {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Tags = tags.Where(x => !x.Group.StartsWith("Default - ")).ToList() ??new List<TagInfo>(),
                File = fileList.SelectedItem as FileInfo,
                SelectedTags = mappers
                               .Where(x => x.FileId == (fileList.SelectedItem as FileInfo).Id)
                               .Join(tags, x => x.TagId, y => y.Id, (x, y) => y)
                               .ToList(),
            };
            setTagWindow.AddingTag += (sender1, tagInfo) => {
                AddTag(tagInfo);
            };
            setTagWindow.ShowDialog();
        }

        /// <summary>
        /// 文件列表选择项变化时的处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            new TextRange(test1.Document.ContentStart, test1.Document.ContentEnd).Text = "";
            test1.AppendText("FileList_SelectionChanged");
        }

        #endregion

        #region 其他事件处理方法

        /// <summary>
        /// 事件->TagTree->选择Tag变化
        /// </summary>
        /// <param name="selectedTags"></param>
        private void TagTree_TagCheckChanged(List<TagInfo> selectedTags) {
            if (selectedTags.Count() == 0) {
                (fileList.ItemsSource as BindingList<FileInfo>).Clear();
                foreach (var item in files) {
                    (fileList.ItemsSource as BindingList<FileInfo>).Add(item);
                }
            } else {
                var selectFiles = selectedTags
                .Join(mappers, x => x.Id, y => y.TagId, (x, y) => y.FileId)
                .Join(files, x => x, y => y.Id, (x, y) => y)
                .ToList();
                (fileList.ItemsSource as BindingList<FileInfo>).Clear();
                foreach (var item in selectFiles) {
                    (fileList.ItemsSource as BindingList<FileInfo>).Add(item);
                }
            }
        }

        #endregion

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
                db.CodeFirst.InitTables(typeof(FileInfo));
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
            FileInfo fileInfo;
            using (var fileStream = System.IO.File.OpenRead(fullFileName)) {
                fileInfo = new FileInfo() {
                    Name = System.IO.Path.GetFileName(fullFileName),
                    UUID = Guid.NewGuid().ToString().Replace("-", ""),
                    OriginalPath = System.IO.Path.GetDirectoryName(fullFileName),
                    Format = System.IO.Path.GetExtension(fullFileName).TrimStart('.').ToUpper(),
                    Size = fileStream.Length
                };
            }
            System.IO.File.Copy(fullFileName, "library\\" + fileInfo.UUID + "." + fileInfo.Format);
            fileInfo = db.Insertable(fileInfo).ExecuteReturnEntity();
            //(fileList.ItemsSource as BindingList<FileInfo>).Add(fileInfo);
            files.Add(fileInfo);

            //添加默认标签
            AddTag(new TagInfo() { Group = "Default - Format", Name = fileInfo.Format });
            AddMapper(fileInfo, tags.Where(x => x.Group == "Default - Format" && x.Name == fileInfo.Format).ToList());
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileId"></param>
        private void RemoveFile(int fileId) {
            db.Deleteable<FileInfo>(fileId).ExecuteCommand();
            (fileList.ItemsSource as BindingList<FileInfo>)
                .Remove((fileList.ItemsSource as BindingList<FileInfo>).Where(fileInfo => fileInfo.Id == fileId).First());

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
        private void AddTag(TagInfo tagInfo) {
            // 判断是否有重复TagInfo
            if (tags.Where(item => item.Name == tagInfo.Name && item.Group == tagInfo.Group).Count() == 0) {
                tagInfo = db.Insertable(tagInfo).ExecuteReturnEntity();
                tags.Add(tagInfo);
                tagTree.AddTag(new List<TagInfo>() { tagInfo });
            }
        }

        /// <summary>
        /// 删除Tag
        /// </summary>
        /// <param name="tagInfo"></param>
        //private void RemoveTag(TagInfo tagInfo) {
        //}

        private void AddMapper(FileInfo fileInfo, List<TagInfo> tagInfos) {
            var tagIds = mappers
                .Where(x => x.FileId == fileInfo.Id)
                .Select(x => x.TagId);
            var notNeedAdd = tagInfos.Join(tagIds, x => x.Id, y => y, (x, y) => x);
            tagInfos = tagInfos.Except(notNeedAdd).ToList();
            tagInfos
                .Select(x => new FileTagMapper() { FileId = fileInfo.Id ?? 0, TagId = x.Id ?? 0 })
                .ToList()
                .ForEach(x => mappers.Add(db.Insertable(x).ExecuteReturnEntity()));
        }

        private void TestData() {
            foreach (var file in System.IO.Directory.EnumerateFiles("library")) {
                System.IO.File.Delete(file);
            }

            #region Add File
            foreach (var file in System.IO.Directory.EnumerateFiles(@"D:\test")) {
                var fileInfo = new FileInfo() {
                    Name = System.IO.Path.GetFileName(file),
                    UUID = Guid.NewGuid().ToString().Replace("-", ""),
                    OriginalPath = System.IO.Path.GetDirectoryName(file),
                    Format = System.IO.Path.GetExtension(file).TrimStart('.').ToUpper(),
                    Size = System.IO.File.OpenRead(file).Length
                };
                db.Insertable(fileInfo).ExecuteCommand();
                System.IO.File.Copy(file, "library\\" + fileInfo.UUID + System.IO.Path.GetExtension(file));
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

    }
}

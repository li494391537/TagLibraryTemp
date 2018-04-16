using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SqlSugar;
using Lirui.TagCommon;
using System.Windows.Data;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;

namespace TagLibrary {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {

        private class FileListItem : ListViewItem {
            public FileListItem() {
            }
        }

        private class TagCheckBox: CheckBox {
            public new Tag Tag { get; set; }
            public TagCheckBox(Tag tag) {
                Tag = tag;
            }
        }

        private List<TreeViewItem> fileFormats;
        private List<TreeViewItem> tagGroups;
        private List<Tag> tags;

        /// <summary>
        /// 构造方法
        /// </summary>
        public MainWindow() {
            InitializeComponent();

            #region 初始化字段
            fileFormats = new List<TreeViewItem>() {
                new TreeViewItem() {
                    Header = "mp3"
                },
                new TreeViewItem() {
                    Header = "png"
                },
                new TreeViewItem() {
                    Header = "mp4"
                }
            };
            tagGroups = new List<TreeViewItem>();
            tags = new List<Tag>() {
                new Tag() {
                    Id = 0,
                    Name = "Tag1",
                    Group = "Group1"
                },
                new Tag() {
                    Id = 1,
                    Name = "Tag2",
                    Group = "Group1"
                },
                new Tag() {
                    Id = 2,
                    Name = "Tag3",
                    Group = "Group2"
                }
            };

            tags.ForEach(tag => {
                var result = tagGroups.Where(tagGroup => tagGroup.Header as string == tag.Group);
                if (result.Count() != 0) {
                    result.First().Items.Add(tag);
                } else {
                    var newGroup = new TreeViewItem() {
                        Header = tag.Group,
                    };
                    newGroup.Items.Add(tag);
                    fileFormats[0].Items.Add(newGroup);
                    tagGroups.Add(newGroup);
                }
            });
            #endregion

            #region 关联事件
            #endregion

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
        /// 文件列表选择项变化时的处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            new TextRange(test1.Document.ContentStart, test1.Document.ContentEnd).Text = "";
            test1.AppendText("FileList_SelectionChanged");
        }

        private void Test_Click(object sender, RoutedEventArgs e) {
            new TextRange(test1.Document.ContentStart, test1.Document.ContentEnd).Text = "";
            test1.AppendText("Test_Click");
        }
        /// <summary>
        /// 菜单栏->关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Copyright © 2018 by LiRui");
        }
        /// <summary>
        /// 菜单栏->设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, RoutedEventArgs e) {
            new TextRange(test1.Document.ContentStart, test1.Document.ContentEnd).Text = "";
            test1.AppendText("Settings_Click");
        }
    }
}

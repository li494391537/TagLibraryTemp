using Lirui.TagCommon;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace TagLibrary.Windows {
    /// <summary>
    /// AddFile.xaml 的交互逻辑
    /// </summary>
    public partial class AddFile : Window {
        private List<TagInfo> tags = new List<TagInfo>();
        public List<TagInfo> Tags {
            set {
                tags = value;
                if (tagTreeView != null) {
                    tagTreeView.Tags = tags;
                }
            }
        }

        public AddFile() {
            InitializeComponent();
            tagSelectedTreeView.HasContextMenu = false;
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e) {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog() {
                Multiselect = false
            };
            openFileDialog.ShowDialog();
            fileName.Text = openFileDialog.FileName;
        }

        private void SelectTag_Click(object sender, RoutedEventArgs e) {
            var selectTag = tagTreeView.SelectedTag();
            var tags = selectTag.Join(this.tags, x => x, y => y.Id, (x, y) => y).ToList();
            tagSelectedTreeView.Tags = tags;
            tagSelectedTreeView.ExpandAll();
        }

        private void CancelSelectTag_Click(object sender, RoutedEventArgs e) {
            var tags = tagSelectedTreeView.SelectedTag();
            tagTreeView.RemoveSelectedTag(tags);
            tagSelectedTreeView.RemoveTag(tags, true);
        }

        private void AddTag_Click(object sender, RoutedEventArgs e) {
            var addTagWindow = new AddTag();
            addTagWindow.ShowDialog();
            if (addTagWindow.Value) {
                var tagInfo = new TagInfo() {
                    Name = addTagWindow.TagName,
                    Group = addTagWindow.TagGroup
                };
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e) {

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}

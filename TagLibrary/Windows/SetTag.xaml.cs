using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TagLibrary.Models;

namespace TagLibrary.Windows {
    /// <summary>
    /// SetTag.xaml 的交互逻辑
    /// </summary>
    public partial class SetTag : Window {
        private List<TagInfo> tags = new List<TagInfo>();
        public List<TagInfo> Tags {
            set {
                tags = value;
                if (tagTreeView != null) {
                    tagTreeView.Tags = tags;
                }
            }
        }

        private List<TagInfo> selectedTags = new List<TagInfo>();
        public List<TagInfo> SelectedTags {
            set {
                selectedTags = value;
                if (tagSelectedTreeView != null) {
                    tagSelectedTreeView.Tags = selectedTags;
                }
            }
        }

        public FileInfo File { get; set; }

        public SetTag() {
            InitializeComponent();
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

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            tagSelectedTreeView.HasContextMenu = false;
            fileName.Text = File.Name;
        }
    }
}

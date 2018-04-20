using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TagLibrary.Models;

namespace TagLibrary.Windows {
    /// <summary>
    /// AddFile.xaml 的交互逻辑
    /// </summary>
    public partial class AddFile : Window {

        public event Action<object, TagInfo> AddingTag;
        
        private List<TagInfo> tags = new List<TagInfo>();
        public List<TagInfo> Tags {
            get => tags;
            set {
                tags = value;
                if (tagTreeView != null) {
                    tagTreeView.Tags = tags;
                }
            }
        }

        public bool IsOK { get; private set; } = false;
        public string[] FileNames { get; private set; }
        public List<TagInfo> SelectedTags { get; private set; }

        public AddFile() {
            InitializeComponent();
            tagTreeView.HasContextMenu = true;
            tagSelectedTreeView.HasContextMenu = false;
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e) {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog() {
                Multiselect = true
            };
            openFileDialog.ShowDialog();
            fileName.Text = "";
            openFileDialog.FileNames.ToList().ForEach(x => fileName.Text += "\"" + System.IO.Path.GetFileName(x) + "\" ");
            FileNames = openFileDialog.FileNames;
        }

        private void SelectTag_Click(object sender, RoutedEventArgs e) {
            var selectTag = tagTreeView.SelectedTag;
            var selectedTag = tagSelectedTreeView.Tags;
            tagSelectedTreeView.AddTag(selectTag.Except(selectedTag).ToList());
            tagSelectedTreeView.ExpandAll();
        }

        private void CancelSelectTag_Click(object sender, RoutedEventArgs e) {
            var tags = tagSelectedTreeView.SelectedTag;
            tagTreeView.RemoveSelectedTag(tags);
            tagSelectedTreeView.RemoveTag(tags, true);
        }

        private void AddTag_Click(object sender, RoutedEventArgs e) {
            var addTagWindow = new AddTag();
            addTagWindow.ShowDialog();
            if (addTagWindow.IsOK) {
                var tagInfo = new TagInfo() {
                    Name = addTagWindow.TagName,
                    Group = addTagWindow.TagGroup
                };
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e) {
            IsOK = true;
            SelectedTags = tagSelectedTreeView.Tags;
            if (!FileNames.All(x => System.IO.File.Exists(x))) {
                MessageBox.Show("文件不存在！", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void TagTreeView_AddingTag(object arg1, TagInfo arg2) {
            AddingTag?.Invoke(arg1, arg2);
        }
    }
}

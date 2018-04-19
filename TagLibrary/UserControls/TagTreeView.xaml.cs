using Lirui.TagCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TagLibrary.Windows;

namespace TagLibrary.UserControls {
    /// <summary>
    /// TagTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class TagTreeView : UserControl {
        public event Action<List<int>> TagCheckChanged;
        public event Action<object, TagInfo> AddingTag;
        public bool HasContextMenu { get; set; } = true;
        private List<TagInfo> tags = new List<TagInfo>();
        public List<TagInfo> Tags {
            set {
                tags = value;
                var groups = new BindingList<TreeViewItem>();
                foreach (var item in tags.GroupBy(item => item.Group).Select(item => item.First())) {
                    var group = new TreeViewItem() {
                        DataContext = item,
                        ItemsSource = new BindingList<CheckBox>()
                    };
                    group.SetBinding(TreeViewItem.HeaderProperty, new Binding("Group"));
                    groups.Add(group);
                }
                foreach (var item in tags) {
                    var tag = new CheckBox() { DataContext = item };
                    tag.SetBinding(ContentProperty, new Binding("Name"));
                    tag.Checked += Tag_Checked;
                    tag.Unchecked += Tag_Checked;
                    (groups.Where(group => (group.DataContext as TagInfo).Group == item.Group).First().ItemsSource as BindingList<CheckBox>)
                        .Add(tag);
                }
                tagTree.ItemsSource = groups;
            }
        }

        public TagTreeView() {
            InitializeComponent();
        }

        #region Click Event
        private void MenuItemAddTag_Click(object sender, RoutedEventArgs e) {
            var addTagWindow = new AddTag();
            addTagWindow.ShowDialog();
            if (addTagWindow.Value) {
                AddingTag?.Invoke(this, new TagInfo {
                    Group = addTagWindow.TagGroup,
                    Name = addTagWindow.TagName
                });
            }
        }

        private void MenuItemCollapseAll_Click(object sender, RoutedEventArgs e) {
            CollapseAll();
        }

        private void MenuItemExpandAll_Click(object sender, RoutedEventArgs e) {
            ExpandAll();
        }
        #endregion

        private void Tag_Checked(object sender, RoutedEventArgs e) {
            var selectTag = new List<int>();
            foreach (var group in (tagTree.ItemsSource as BindingList<TreeViewItem>)) {
                foreach (var tag in (group.ItemsSource as BindingList<CheckBox>).Where(item => item.IsChecked == true)) {
                    selectTag.Add((tag.DataContext as TagInfo).Id);
                }
            }
            TagCheckChanged?.Invoke(selectTag);
        }

        private void TagTree_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            if (!HasContextMenu) {
                tagTreeContextMenu.IsEnabled = false;
                tagTreeContextMenu.Visibility = Visibility.Collapsed;
            }
        }

        public void AddTag(List<TagInfo> tagInfos) {
            //唯一性约束检查
            foreach (var tagInfo in tagInfos.Join(tags, x => new { x.Name, x.Group }, y => new { y.Name, y.Group }, (x, y) => x)) {
                tagInfos.Remove(tagInfo);
            }

            foreach (var tagInfo in tagInfos) {
                tags.Add(tagInfo);
                var tag = new CheckBox() { DataContext = tagInfo };
                tag.SetBinding(ContentProperty, new Binding("Name"));
                tag.Checked += Tag_Checked;
                tag.Unchecked += Tag_Checked;
                var groups = tagTree.ItemsSource as BindingList<TreeViewItem>;
                if (groups.Where(item => (item.DataContext as TagInfo).Group == tagInfo.Group).Count() == 0) {
                    var group = new TreeViewItem() {
                        DataContext = tagInfo,
                        ItemsSource = new BindingList<CheckBox>() { tag }
                    };
                    group.SetBinding(HeaderedItemsControl.HeaderProperty, new Binding("Group"));
                    groups.Add(group);
                } else {
                    (groups.Where(group => (group.DataContext as TagInfo).Group == tagInfo.Group).First().ItemsSource as BindingList<CheckBox>)
                        .Add(tag);
                }
            }
        }

        public void RemoveTag(List<int> tagIds, bool isRemoveGroup = false) {
            foreach (var tag in tagIds.Join(tags, x => x, y => y.Id, (x, y) => y)) {
                tags.Remove(tag);
                var groupBox = (tagTree.ItemsSource as BindingList<TreeViewItem>).Where(item => (item.DataContext as TagInfo).Group == tag.Group).First();
                var tagBox = (groupBox.ItemsSource as BindingList<CheckBox>).Where(item => (item.DataContext as TagInfo).Name == tag.Name).First();
                (groupBox.ItemsSource as BindingList<CheckBox>).Remove(tagBox);
            }
            if (isRemoveGroup) {
                foreach (var item in (tagTree.ItemsSource as BindingList<TreeViewItem>).Where(item => (item.ItemsSource as BindingList<CheckBox>).Count == 0).ToList()) {
                    (tagTree.ItemsSource as BindingList<TreeViewItem>).Remove(item);
                }
            }
        }

        public bool UpdateTag(TagInfo tagInfo) {
            var tag = tags.Where(item => item.Id == tagInfo.Id).First();
            var tempTag = new TagInfo() {
                Id = tagInfo.Id,
                Name = tagInfo.Name ?? tag.Name,
                Group = tagInfo.Group ?? tag.Group
            };
            //唯一性约束检查
            if (tags.Where(item => item.Group == tempTag.Group && item.Name == tempTag.Name).Count() == 0) {
                tag.Name = tempTag.Name;
                tag.Group = tempTag.Group;
                return true;
            }
            return false;
        }

        public void AddSelectedTag(List<int> tagIds) {
            List<CheckBox> allTags = new List<CheckBox>();
            foreach (var group in tagTree.ItemsSource as BindingList<TreeViewItem>) {
                foreach (var tag in group.ItemsSource as BindingList<CheckBox>) {
                    allTags.Add(tag);
                }
            }
            foreach (var tag in allTags.Join(tagIds, x => (x.DataContext as TagInfo).Id, y => y, (x, y) => x)) {
                tag.IsChecked = true;
            }
        }

        public void RemoveSelectedTag(List<int> tagIds) {
            List<CheckBox> allTags = new List<CheckBox>();
            foreach (var group in tagTree.ItemsSource as BindingList<TreeViewItem>) {
                foreach (var tag in group.ItemsSource as BindingList<CheckBox>) {
                    allTags.Add(tag);
                }
            }
            foreach (var tag in allTags.Join(tagIds, x => (x.DataContext as TagInfo).Id, y => y, (x, y) => x)) {
                tag.IsChecked = false;
            }
        }

        public List<int> SelectedTag() {
            var selectTag = new List<int>();
            foreach (var group in (tagTree.ItemsSource as BindingList<TreeViewItem>)) {
                foreach (var tag in (group.ItemsSource as BindingList<CheckBox>).Where(item => item.IsChecked == true)) {
                    selectTag.Add((tag.DataContext as TagInfo).Id);
                }
            }
            return selectTag;
        }

        #region 折叠、展开

        public void CollapseGroup(string group) {
            (tagTree.ItemsSource as BindingList<TreeViewItem>)
                .Where(item => (item.DataContext as TagInfo).Group == group)
                .First()
                .IsExpanded = false;
        }

        public void CollapseAll() {
            foreach (var group in tags.Select(item => item.Group).Distinct()) {
                CollapseGroup(group);
            }
        }

        public void ExpandGroup(string group) {
            (tagTree.ItemsSource as BindingList<TreeViewItem>)
                .Where(item => (item.DataContext as TagInfo).Group == group)
                .First()
                .IsExpanded = true;
        }

        public void ExpandAll() {
            foreach (var group in tags.Select(item => item.Group).Distinct()) {
                ExpandGroup(group);
            }
        }
        #endregion
    }
}

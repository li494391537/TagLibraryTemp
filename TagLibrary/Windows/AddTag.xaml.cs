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

namespace TagLibrary.Windows {
    /// <summary>
    /// AddTag.xaml 的交互逻辑
    /// </summary>
    public partial class AddTag : Window {

        public AddTag() {
            InitializeComponent();
        }

        public string TagGroup { get; private set; } = string.Empty;
        public string TagName { get; private set; } = string.Empty;
        public bool Value { get; private set; } = false;

        private void ButtonOK_Click(object sender, RoutedEventArgs e) {
            TagGroup = tagGroup.Text;
            TagName = tagName.Text;
            Value = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}

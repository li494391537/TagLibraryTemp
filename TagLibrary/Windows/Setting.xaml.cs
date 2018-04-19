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
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window {
        public Setting() {
            InitializeComponent(); setting1.IsSelected = true;
            setting1.Focus();
        }

        private void OK_Click(object sender, RoutedEventArgs e) {
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void Apply_Click(object sender, RoutedEventArgs e) {

        }
    }
}

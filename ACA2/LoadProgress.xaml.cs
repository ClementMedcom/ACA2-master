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

namespace ACA2
{
    /// <summary>
    /// Interaction logic for LoadProgress.xaml
    /// </summary>
    public partial class LoadProgress : Window
    {
        public LoadProgress()
        {
            InitializeComponent();
        }

        private void loadProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (loadProgress.Maximum == loadProgress.Value)
            {
                LoadProgress prog = this;
                prog.Close();
            }
            else
            {
                progresslbl.Content = Convert.ToInt32(((loadProgress.Value / loadProgress.Maximum) * 100)) + "%";
            }
        }
    }
}

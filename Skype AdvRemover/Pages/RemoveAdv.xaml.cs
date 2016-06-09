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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Skype_AdvRemover.Pages
{
    /// <summary>
    /// Interaction logic for RemoveAdv.xaml
    /// </summary>
    public partial class RemoveAdv : UserControl
    {
        public RemoveAdv()
        {
            InitializeComponent();

            for (int i = 0; i < 4; i++)
            {
                CheckBox rb = new CheckBox() { Content = "Profile " + i, IsChecked = true };
                rb.Checked += (sender, args) =>
                {
                    Console.WriteLine("Pressed " + (sender as CheckBox).Tag);
                };
                rb.Unchecked += (sender, args) => { /* Do stuff */ };
                rb.Tag = i;

                MyStackPanel.Children.Add(rb);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SARemover Skype = new SARemover();
            Result r = new Result();

            r.txtResult.Text = "";
            r.Show();

            foreach (var item in Skype.Restart())
            {
                r.txtResult.Text += item;
            }
        }
    }
}
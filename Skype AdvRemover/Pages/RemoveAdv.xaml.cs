using Skype_AdvRemover;
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
        SARemover Skype = new SARemover();
        public RemoveAdv()
        {
            InitializeComponent();

            //SARemover Skype = new SARemover();

            foreach (SkypeProfile profile in Skype.SkypeProfiles)
            {
                CheckBox rb = new CheckBox() { Content = profile, IsChecked = true };
                rb.Checked += (sender, args) =>
                {
                    //Console.WriteLine("Pressed " + (sender as CheckBox).Tag);
                    // profile.RemoveAdv();
                };
                rb.Unchecked += (sender, args) => { /* Do stuff */ };
                rb.Tag = profile;

                MyStackPanel.Children.Add(rb);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in MyStackPanel.Children)
            {
                if (item.IsChecked == true)
                {

                    Console.WriteLine("Removing Skype Advertisements from profile {0}.", Skype.GetSkypeProfile(item.Tag.ToString()).Name);
                    Skype.GetSkypeProfile(item.Tag.ToString()).RemoveAdv();
                }
            }

            if (RestartSkype.IsChecked == true)
            {
                Console.WriteLine("Skype will be restarted.");
                Skype.Restart();
            }
        }
    }
}
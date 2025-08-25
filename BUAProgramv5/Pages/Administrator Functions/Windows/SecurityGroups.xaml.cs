using BUAProgramv5.Models.AD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BUAProgramv5.Pages.Administrator_Functions.Windows
{
    /// <summary>
    /// Interaction logic for SecurityGroups.xaml
    /// </summary>
    public partial class SecurityGroups : Window
    {
        public ObservableCollection<ADObjectCheckList> Groups { get; set; }
        private List<ADObjectCheckList> currentState = new List<ADObjectCheckList>();
        public SecurityGroups(ObservableCollection<ADObjectCheckList> list)
        {
            InitializeComponent();
            Groups = list;
            this.DataContext = this;
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (!currentState.Any(x => x.Name == checkBox.Content.ToString()))
            {
                currentState.Add(new ADObjectCheckList
                {
                    Name = checkBox.Content.ToString(),
                    DistinguishedName = checkBox.Tag.ToString(),
                    Checked = false
                });
            }
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (!currentState.Any(x => x.Name == checkBox.Content.ToString()))
            {
                currentState.Add(new ADObjectCheckList
                {
                    Name = checkBox.Content.ToString(),
                    DistinguishedName = checkBox.Tag.ToString(),
                    Checked = true
                });
            }
        }

        /// <summary>
        /// Exit window listing groups.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (ADObjectCheckList item in currentState)
            {
                ADObjectCheckList entity = Groups.Where(x => x.Name == item.Name).Single();
                entity.Checked = item.Checked;
            }

            securityGroupWindow.Close();

        }

        /// <summary>
        /// Adds groups to list, then closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            securityGroupWindow.Close();
        }
    }
}

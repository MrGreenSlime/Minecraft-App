using Globals;
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

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for Items.xaml
    /// </summary>
    public partial class Items : Window
    {
        public LogicInterface.LogicInterface Logic { get; set; }
        public Items(LogicInterface.LogicInterface logic)
        {
            InitializeComponent();
            Logic = logic;
            foreach (Colonie item in Logic.World.colonies)
            {
                ColonySelection.Items.Add(item);
            }
        }

        private void ColonySelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            colonyItems.Items.Clear();
            PlayerItems.Items.Clear();
            patterns.Items.Clear();
            Colonie storage = (Colonie)ColonySelection.SelectedItem;
            if (storage.items != null)
            {
                storage.items.patterns.Sort();
                foreach (StorageItem item in storage.items.patterns)
                {
                    patterns.Items.Add(item.displayName);
                }
                storage.items.items.playerSide.Sort();
                foreach (StorageItem item in storage.items.items.playerSide)
                {
                    PlayerItems.Items.Add(item);
                }
                storage.items.items.colonySide.Sort();
                foreach (StorageItem item in storage.items.items.colonySide)
                {
                    colonyItems.Items.Add(item);
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow(Logic);
            Close();
            window.Show();
        }
    }
}

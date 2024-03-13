using Globals;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer
{
    public partial class MainWindow : Window
    {
        public LogicInterface.LogicInterface Logic {  get; set; }
        public MainWindow()
        {
            DataInterface.DataInterface Data = new DataImplementation.DataImplement();
            Logic = new LogicImplementation.LogicImplementation(Data);
            InitializeComponent();
            Logic.setColonie();
            foreach (Colonie item in Logic.World.colonies)
            {
                colonySelection.Items.Add(item);
            }
        }
        private void builderTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (builderTasks.SelectedIndex == -1)
                return;
            itemLabel.Content = "items in builder request";
            itemsOfRequest.Items.Clear();
            var test = builderTasks.SelectedItem;
            
            if (((BuilderRequests)builderTasks.SelectedItem).requests != null)
            {
                foreach (SpecifiedRequest item in ((BuilderRequests)builderTasks.SelectedItem).requests)
                {
                    itemsOfRequest.Items.Add(item);
                }
            }
            
        }

        private void regularTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (regularTasks.SelectedIndex == -1)
                return;
            itemLabel.Content = "items possible in request";
            itemsOfRequest.Items.Clear();
            foreach (Item item in ((Requests)regularTasks.SelectedItem).items)
            {
                itemsOfRequest.Items.Add(item);
            }
        }

        private void builderTasks_GotFocus(object sender, RoutedEventArgs e)
        {
            if (builderTasks.SelectedIndex == -1)
                return;
            itemLabel.Content = "items in builder request";
            itemsOfRequest.Items.Clear();
            var test = builderTasks.SelectedItem;

            if (((BuilderRequests)builderTasks.SelectedItem).requests != null)
            {
                foreach (SpecifiedRequest item in ((BuilderRequests)builderTasks.SelectedItem).requests)
                {
                    itemsOfRequest.Items.Add(item);
                }
            }
        }

        private void regularTasks_GotFocus(object sender, RoutedEventArgs e)
        {
            if (regularTasks.SelectedIndex == -1)
                return;
            itemLabel.Content = "items possible in request";
            itemsOfRequest.Items.Clear();
            foreach (Item item in ((Requests)regularTasks.SelectedItem).items)
            {
                itemsOfRequest.Items.Add(item);
            }
        }

        private void colonySelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            builderTasks.Items.Clear();
            regularTasks.Items.Clear();
            itemsOfRequest.Items.Clear();
            foreach (BuilderRequests item in ((Colonie)colonySelection.SelectedItem).BuilderRequests)
            {
                builderTasks.Items.Add(item);
            }
            foreach (Requests item in ((Colonie)colonySelection.SelectedItem).Requests)
            {
                regularTasks.Items.Add(item);
            }
        }
        private void update()
        {
            Logic.setColonie();
            colonySelection.Items.Clear();
            foreach (Colonie item in Logic.World.colonies)
            {
                colonySelection.Items.Add(item);
            }
        }
    }
}
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LogicInterface.LogicInterface Logic {  get; set; }
        public MainWindow()
        {
            DataInterface.DataInterface Data = new DataImplementation.DataImplement();
            Logic = new LogicImplementation.LogicImplementation(Data);
            InitializeComponent();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            Test.IsEnabled = false;
            builderTasks.Items.Clear();
            regularTasks.Items.Clear();
            Logic.setColonie();
            if (Logic.Colonie != null)
            {
                foreach (Requests item in Logic.Colonie.Requests)
                {
                    regularTasks.Items.Add(item);
                }
                foreach (BuilderRequests item in Logic.Colonie.builderRequests)
                {
                    builderTasks.Items.Add(item);
                }
                
                
            }
        }

        private void builderTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
    }
}
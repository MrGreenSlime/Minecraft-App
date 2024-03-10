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
            Logic.setColonie();
        }
    }
}
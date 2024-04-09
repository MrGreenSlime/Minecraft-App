using Globals;
using Ookii.Dialogs.Wpf;
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
            Logic.setInstance(ShowFolderBrowserDialog());
            Logic.setColonie();
            Logic.setStorage();
            Logic.setPaths();
            foreach (string pa in Logic.paths)
            {
                worldSelection.Items.Add(pa);
            }
        }
        public MainWindow(LogicInterface.LogicInterface logic)
        {
            Logic = logic;
            InitializeComponent();
            Logic.setColonie();
            Logic.setStorage();
            Logic.setPaths();
            foreach (string pa in Logic.paths)
            {
                worldSelection.Items.Add(pa);
            }
        }

        private string ShowFolderBrowserDialog()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Please select your minecraft source folder.";
            dialog.UseDescriptionForTitle = true;

            if (!VistaFolderBrowserDialog.IsVistaFolderDialogSupported)
            {
                MessageBox.Show(this, "Because you are not using Windows Vista or later, the regular folder browser dialog will be used. Please use Windows Vista to see the new dialog.", "Sample folder browser dialog");
            }

            if (dialog.ShowDialog(this) ?? false)
            {
                //MessageBox.Show(this, $"The selected folder was: {Environment.NewLine}{dialog.SelectedPath}", "Sample folder browser dialog");
                return dialog.SelectedPath;
            }
            return "";
        }

        private void worldSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (worldSelection.SelectedIndex == -1)
                return;
            Logic.setPath(worldSelection.SelectedItem.ToString());
            Items window = new Items(Logic);
            Close();
            window.Show();
        }
    }
}
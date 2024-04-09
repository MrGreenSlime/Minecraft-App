using LogicInterface;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DataInterface.DataInterface DataInterface { get; set; }
        private LogicInterface.LogicInterface LogicInterface { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DataInterface = new DataImplementation.DataImplement();
            LogicInterface = new LogicImplementation.LogicImplementation(DataInterface);
            MainWindow window = new MainWindow(LogicInterface);
            window.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            LogicInterface.Close();
        }
    }

}

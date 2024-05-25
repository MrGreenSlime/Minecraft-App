﻿using Globals;
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
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
        private CancellationTokenSource cancellationTokenSource;
        public MainWindow(LogicInterface.LogicInterface logic)
        {
            Logic = logic;
            cancellationTokenSource = new CancellationTokenSource();
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/logotest.png"));
            startButton.IsEnabled = false;
            stopButton.IsEnabled = false;
            if (!Logic.instanceSelected) Logic.setInstance(ShowFolderBrowserDialog());
            Logic.setPaths();
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
                Logic.instanceSelected = true;
                return dialog.SelectedPath;
            }
            return "";
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            Logic.setInstance(ShowFolderBrowserDialog());
        }

        private async void login_Click(object sender, RoutedEventArgs e)
        {
            await Logic.Login(emailtextbox.Text, passwordtextbox.Password);
            passwordtextbox.Password = "";
            if (Logic.IsLoggedIn())
            {
                login.Content = "switch account";
                loginError.Content = "";
                startButton.IsEnabled = true;
            } else
            {
                login.Content = "login";
                loginError.Content = "Invalid credentials";
                startButton.IsEnabled= false;
            }
            
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://j-plot.lucavandenweghe.ikdoeict.be/") { UseShellExecute = true });
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.stop();
            startButton.IsEnabled = true;
            stopButton.IsEnabled = false;
            login.IsEnabled = true;
            Reload.IsEnabled = true;
        }
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            startButton.IsEnabled = false;
            stopButton.IsEnabled = true;
            login.IsEnabled = false;
            Reload.IsEnabled = false;
            Logic.start();
            //Items window = new Items(Logic);
            //Close();
            //window.Show();
        }
        

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Logic.stop();
        }

        private void ColonieInstall_Click(object sender, RoutedEventArgs e)
        {
            InstallNewColonieWindow window = new InstallNewColonieWindow(Logic);
            Close();
            window.Show();
        }
    }
}
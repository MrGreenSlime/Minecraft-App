using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Globals;
using LogicInterface;
using Microsoft.VisualBasic.Logging;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for InstallNewColonieWindow.xaml
    /// </summary>
    public partial class InstallNewColonieWindow : Window
    {
        private LogicInterface.LogicInterface _logicInterface;
        public InstallNewColonieWindow(LogicInterface.LogicInterface logic)
        {
            _logicInterface = logic;
            InitializeComponent();
            RefreshWorldCombo();
        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            if (ComputerList.SelectedIndex == -1) ErrorLbl.Content = "Please select a computer Path";            
            else
            {
                _logicInterface.InstallLuaFiles(ComputerList.SelectedItem.ToString()!);
                RefreshWorldCombo();
                RefreshComputerList();
            }
        }

        private void WorldSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshComputerList();
        }

        private void RefreshWorldCombo()
        {
            int currentIndex = 0;
            if (WorldSelection.SelectedIndex != -1) currentIndex = WorldSelection.SelectedIndex;
            WorldSelection.Items.Clear();
            foreach (WorldPath path in _logicInterface.paths)
            {
                WorldSelection.Items.Add(path);
            }
            WorldSelection.SelectedIndex = currentIndex;
        }

        private void RefreshComputerList()
        {
            if (WorldSelection.SelectedIndex == -1) return;
            ComputerList.Items.Clear();
            foreach (string ComputerPath in ((WorldPath)WorldSelection.SelectedItem).ComputerPaths)
            {
                ComputerList.Items.Add(ComputerPath);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow(_logicInterface);
            Close();
            window.Show();
        }
    }
}

using Globals;
using Newtonsoft.Json.Linq;
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
        private readonly SynchronizationContext synchronizationContext;
        private CancellationTokenSource cancellationTokenSource;
        public LogicInterface.LogicInterface Logic { get; set; }

        public Items(LogicInterface.LogicInterface logic)
        {
            synchronizationContext = SynchronizationContext.Current!;
            cancellationTokenSource = new CancellationTokenSource();
            InitializeComponent();
            Logic = logic;

            foreach (World item in Logic.World)
            {
                foreach (Colonie item2 in item.colonies)
                {
                    ColonySelection.Items.Add(item2);
                }
            }
            var token = cancellationTokenSource.Token;
            Task task = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Logic.setColonie();
                    int storage = -1;
                    synchronizationContext.Post((c) => storage = ColonySelection.SelectedIndex, null);
                    synchronizationContext.Post((c) => ColonySelection.Items.Clear(), null);
                    foreach (World item in Logic.World)
                    {
                        foreach (Colonie item2 in item.colonies)
                        {
                            synchronizationContext.Post((c) => ColonySelection.Items.Add(item2), null);
                        }
                    }
                    synchronizationContext.Post((c) =>
                    {
                        if (storage != -1)
                        {
                            ColonySelection.SelectedIndex = storage;
                        }
                    }, null);
                    Thread.Sleep(10000);
                }

            });

        }

        private void ColonySelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            colonyItems.Items.Clear();
            PlayerItems.Items.Clear();
            patterns.Items.Clear();
            Colonie storage = (Colonie)ColonySelection.SelectedItem;
            builderTasks.Items.Clear();
            regularTasks.Items.Clear();
            itemsOfRequest.Items.Clear();
            if ((Colonie)ColonySelection.SelectedItem == null)
            {
                return;
            }
            foreach (BuilderRequests item in ((Colonie)ColonySelection.SelectedItem).BuilderRequests)
            {
                builderTasks.Items.Add(item);
            }
            foreach (Requests item in ((Colonie)ColonySelection.SelectedItem).Requests)
            {
                regularTasks.Items.Add(item);
            }
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
            cancellationTokenSource.Cancel();
            MainWindow window = new MainWindow(Logic);
            Close();
            window.Show();
        }
        private void builderTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (builderTasks.SelectedIndex == -1)
                return;
            itemLabel.Content = "items in builder request";
            itemsOfRequest.Items.Clear();
            var test = builderTasks.SelectedItem;

            if (((BuilderRequests)builderTasks.SelectedItem).Requests != null)
            {
                foreach (SpecifiedRequest item in ((BuilderRequests)builderTasks.SelectedItem).Requests)
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
            foreach (RequestItem item in ((Requests)regularTasks.SelectedItem).items)
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

            if (((BuilderRequests)builderTasks.SelectedItem).Requests != null)
            {
                foreach (SpecifiedRequest item in ((BuilderRequests)builderTasks.SelectedItem).Requests)
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
            foreach (RequestItem item in ((Requests)regularTasks.SelectedItem).items)
            {
                itemsOfRequest.Items.Add(item);
            }
        }
    }
}

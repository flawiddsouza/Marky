using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace Marky
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {
        string pathToFlavors = AppDomain.CurrentDomain.BaseDirectory + "flavors";
        string lastSavedSelection;
        bool save;

        public Preferences()
        {
            InitializeComponent();

            bool directoryExists = Directory.Exists(pathToFlavors);
            bool directoryNotEmpty = directoryExists ? !Helpers.IsDirectoryEmpty(pathToFlavors) : false;

            if (directoryExists && directoryNotEmpty)
            {
                string[] files = Directory.GetFiles(pathToFlavors, "*.css");
                List<string> filenames = new List<string>();
                foreach (var file in files)
                {
                    filenames.Add(Path.GetFileName(file));
                }

                FlavorSelector.ItemsSource = filenames;

                foreach(var filename in filenames)
                {
                    // this not only selects the filename but also enables the condition
                    // to ignore a null value or a non-existent filename in the
                    // Settings->Flavor entry
                    if (filename == Properties.Settings.Default.Flavor) 
                    {
                        FlavorSelector.SelectedValue = filename;
                    }
                }
            }

            lastSavedSelection = Properties.Settings.Default.Flavor;
            FlavorSelector.SelectionChanged += FlavorSelector_SelectionChanged;
        }

        private void FlavorSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.Flavor = (string)FlavorSelector.SelectedValue;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Flavor = (string)FlavorSelector.SelectedValue;
            Properties.Settings.Default.Save();
            save = true;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Restore lastSavedFlavor if it has been changed so that the view is refreshed to show the saved entry
            // We need to do this because we have a auto-refreshing view that keeps updating itself in relation to FlavorSelector.SelectionChange
            if (Properties.Settings.Default.Flavor != lastSavedSelection && !save)
            {
                Properties.Settings.Default.Flavor = lastSavedSelection;
            }
        }
    }
}

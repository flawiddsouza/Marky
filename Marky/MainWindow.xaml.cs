using CefSharp;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Marky
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedUICommand Preferences = new RoutedUICommand("Preferences", "Preferences", typeof(MainWindow));
        public static RoutedUICommand ExportHTML = new RoutedUICommand("Export to HTML", "ExportHTML", typeof(MainWindow));
        public string MarkdownFilePath { get; set; }
        private string CSS;
        private string renderedMarkdown;
        private string html;
        private bool pageLoaded; // check if any item has been loaded into the browser component

        public MainWindow()
        {
            InitializeComponent();

            CSS = new MarkdownFlavorSelector().GetCSS();

            Properties.Settings.Default.SettingChanging += Default_SettingChanging;
        }

        private void Viewer_Loaded(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).RenderView += delegate 
            {
                RenderMarkdown();
            };
        }

        private async void RenderMarkdown()
        {
            if (!string.IsNullOrEmpty(MarkdownFilePath)) // check if MarkdownFilePath is set or not
            {
                var m = new MarkdownDeep.Markdown(); // alternative: MarkdownSharp.Markdown(); rest of the syntax is the same
                string markdown = await Helpers.ReadAllLinesAsync(MarkdownFilePath);
                renderedMarkdown = m.Transform(markdown);
                html = "<html>\n<head>\n<style>\n"+ CSS + "</style>\n</head>\n<body oncontextmenu='return true'>\n" + renderedMarkdown + "</body>\n</html>";
                Viewer.LoadHtml(html, "http://renderpage/");
                pageLoaded = true;
                ExportToHTML.IsEnabled = true;
            }
        }

        private void RefreshView()
        {
            html = "<html>\n<head>\n<style>\n" + CSS + "</style>\n</head>\n<body oncontextmenu='return true'>\n" + renderedMarkdown + "</body>\n</html>";
            Viewer.LoadHtml(html, "http://renderpage/");
        }

        private void Copy_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Viewer.Copy();
        }

        private void Copy_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SelectAll_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Viewer.SelectAll();
        }

        private void SelectAll_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExportHTML_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.FileName = Path.GetFileNameWithoutExtension(MarkdownFilePath);
            dlg.DefaultExt = ".html";
            dlg.Filter = "HTML document (.html)|*.html";

            var result = dlg.ShowDialog();

            if (result == true)
            {
                File.WriteAllText(dlg.FileName, html);
            }
        }

        private void ExportHTML_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(pageLoaded)
            {
                e.CanExecute = true;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Open_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                bool sizeExceeds = new FileInfo(openFileDialog.FileName).Length > 10000000; // sizeExceeds becomes true when the file size is greated than 10MB
                if (sizeExceeds)
                {
                    if (MessageBox.Show("This file is a bit too large for a markdown file. Are you sure you want to view it? The viewer may become unresponsive during the process!", "Warning", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                Viewer.Title = Path.GetFileName(openFileDialog.FileName);
                MarkdownFilePath = openFileDialog.FileName;
                RenderMarkdown();
            }
        }

        private void Preferences_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var preferences = new Preferences();
            preferences.ShowDialog();
        }

        private void Default_SettingChanging(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            CSS = new MarkdownFlavorSelector().GetCSS();
            RefreshView();
        }
    }
}
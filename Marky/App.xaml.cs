using CefSharp;
using System;
using System.IO;
using System.Windows;

namespace Marky
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public event EventHandler RenderView;

        public App()
        {
            var settings = new CefSettings
            {
                LogSeverity = LogSeverity.Disable,
            };

            //settings.RegisterScheme(new CefCustomScheme()
            //{
            //    SchemeName = LocalSchemeHandlerFactory.SchemeName,
            //    SchemeHandlerFactory = new LocalSchemeHandlerFactory()
            //});

            Cef.Initialize(settings);
        }

        void App_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            foreach (var markdownFile in e.Args)
            {
                if (File.Exists(markdownFile))
                {
                    bool sizeExceeds = new FileInfo(markdownFile).Length > 10000000; // sizeExceeds becomes true when the file size is greated than 10MB
                    if(sizeExceeds)
                    {
                        if (MessageBox.Show("This file is a bit too large for a markdown file. Are you sure you want to view it? The viewer may become unresponsive during the process!", "Warning", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                        {
                            break;
                        }
                        continue;
                    }
                    mainWindow.Title = Path.GetFileName(markdownFile);
                    mainWindow.MarkdownFilePath = markdownFile;
                    OnRenderView(EventArgs.Empty); // raise event to render view
                }
                else
                {
                    MessageBox.Show("Specified file was not found", "Error");
                }
            }
        }

        protected virtual void OnRenderView(EventArgs e)
        {
            EventHandler handler = RenderView;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}

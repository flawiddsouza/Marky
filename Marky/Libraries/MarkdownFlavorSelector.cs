using System;
using System.IO;

namespace Marky
{
    class MarkdownFlavorSelector
    {
        string pathToFlavors = AppDomain.CurrentDomain.BaseDirectory + "flavors";

        private bool directoryExists, directoryNotEmpty;

        public string GetCSS()
        {
            directoryExists = Directory.Exists(pathToFlavors);
            directoryNotEmpty = directoryExists ? !Helpers.IsDirectoryEmpty(pathToFlavors) : false;

            string CSS;

            string pathToPreferredFlavor = pathToFlavors + "\\" + Properties.Settings.Default.Flavor;

            if (Properties.Settings.Default.Flavor != null && File.Exists(pathToPreferredFlavor))
            {
                CSS = File.ReadAllText(pathToPreferredFlavor);
            }
            else
            {
                CSS = GetRandomCSS();
            }

            return CSS;
        }

        private string GetRandomCSS()
        {
            if (directoryExists && directoryNotEmpty)
            {
                var rand = new Random();
                var files = Directory.GetFiles(pathToFlavors, "*.css");
                return File.ReadAllText(files[rand.Next(files.Length)]);
            }
            else
            {
                return null;
            }   
        }
    }
}

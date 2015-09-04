using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Marky
{
    static class Helpers
    {
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static async Task<string> ReadAllLinesAsync(string markdownFilePath)
        {
            using (StreamReader reader = new StreamReader(markdownFilePath))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}

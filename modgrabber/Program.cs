using System;
using System.IO;

namespace modgrabber
{
    class modgrabber
    {
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
        static void Main(string[] args)
        {
            string path = Console.ReadLine();
            string[] subdirectoryEntries = Directory.GetDirectories(path);
            try
            {
                Directory.CreateDirectory(path + "\\mods");
            }
            catch
            {
                Console.WriteLine("Произошла ошибка при создании выходной папки. Рекомендуется удалить предыдущую папку.", "Ошибка");
            }
            string output = path + "\\mods";
            foreach (string child in subdirectoryEntries)
            {
                Console.WriteLine("Обрабатывается папка {0}", child);
                string[] file = Directory.GetFiles(child, "*.pak");
                string[] directory = Directory.GetDirectories(child);
                string[] name = child.Split("\\");
                if (file.Length > 0)
                {
                    Console.WriteLine("Мод {0} перемещён в папку с модами", file[0]);
                    File.Copy(file[0], output + "\\" + name[name.Length - 1]+".pak");
                }
                else if (directory.Length > 0)
                {
                    DirectoryCopy(directory[0], output + "\\" + name[name.Length - 1], true);
                }
            }
        }
    }
}

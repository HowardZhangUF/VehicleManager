using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitInfo
{
    internal class Program
    {
        /// <summary>
        /// 更改資訊設定中的 AssemblyProduct
        /// </summary>
        private static void ChangeAssemblyProduct(IEnumerable<FileInfo> files, string description)
        {
            foreach (var file in files)
            {
                var contents = File.ReadAllLines(file.FullName);
                for (int i = 0; i < contents.Count(); i++)
                {
                    if (contents[i].Contains("assembly: AssemblyProduct"))
                    {
                        contents[i] = $"[assembly: AssemblyProduct(\"{description}\")]";
                        File.WriteAllText(file.FullName, string.Join("\r\n", contents), System.Text.Encoding.UTF8);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 獲得所有 AssemblyInfo.cs 位置
        /// </summary>
        private static IEnumerable<FileInfo> GetAllAssemblyFiles()
        {
            string dircName = TryGetSolutionDirectoryInfo().FullName;
            DirectoryInfo dirc = new DirectoryInfo(dircName);
            return dirc.GetFiles("AssemblyInfo.cs", SearchOption.AllDirectories);
        }

        /// <summary>
        /// 從 Git 中獲得資訊
        /// </summary>
        private static string GetGitMessage()
        {
            string dircName = TryGetSolutionDirectoryInfo().FullName;
            using (var repository = new Repository(dircName))
            {
                var commit = repository.Commits.FirstOrDefault();
                var message = commit.MessageShort.Replace("\"", "'");
                return $"{commit.Author.When.ToString("yyyy/MM/dd hh:mm")},{commit.Author.Name}-{commit.Id.Sha.Remove(8)}-{message}";
            }
        }

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("自動生成 AssemblyProduct");
                Console.WriteLine("按 N 鍵清空，按 G 鍵則將 Git 資訊填入");
                var files = GetAllAssemblyFiles();
                var key = Console.ReadKey().Key;
                if (key == ConsoleKey.N) ChangeAssemblyProduct(files, "");
                else if (key == ConsoleKey.G) ChangeAssemblyProduct(files, GetGitMessage());
                Console.WriteLine("處理完成");
                Console.ReadKey();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 獲得方案檔所在資料夾
        /// </summary>
        private static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}

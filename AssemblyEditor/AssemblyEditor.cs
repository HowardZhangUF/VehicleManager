using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyEditor
{
    /// <summary>
    /// <para>組件資訊編輯器，此專案參考 NuGet LibGit2Sharp-SSH 套件，</para>
    /// <para>可讀取 Git 中的最新作者名稱、修改日期與描述，</para>
    /// <para>並將其寫入各專案的 AssemblyInfo.cs 對應的組件欄位中，</para>
    /// <para>方便作為版本控管的手段</para>
    /// </summary>
    class AssemblyEditor
    {
        /// <summary>
        /// 更改資訊設定中的 Assembly
        /// </summary>
        private static void ChangeAssembly(IEnumerable<FileInfo> files, string product, string version)
        {
            foreach (var file in files)
            {
                var contents = File.ReadAllLines(file.FullName);
                for (int i = 0; i < contents.Count(); i++)
                {
                    if (contents[i].StartsWith("[assembly: AssemblyProduct"))
                    {
                        contents[i] = $"[assembly: AssemblyProduct(\"{product}\")]";
                    }
                    if (contents[i].StartsWith("[assembly: AssemblyFileVersion"))
                    {
                        contents[i] = $"[assembly: AssemblyFileVersion(\"{version}\")]";
                    }
                }
                File.WriteAllText(file.FullName, string.Join("\r\n", contents), System.Text.Encoding.UTF8);
            }
        }

        /// <summary>
        /// 獲得此方案底下所有 AssemblyInfo.cs 檔案位置
        /// </summary>
        private static IEnumerable<FileInfo> GetAllAssemblyFiles()
        {
            string dircName = TryGetSolutionDirectoryInfo().FullName;
            DirectoryInfo dirc = new DirectoryInfo(dircName);
            return dirc.GetFiles("AssemblyInfo.cs", SearchOption.AllDirectories);
        }

        /// <summary>
        /// 從 Git 中獲得相關資訊資訊，其中修改日期 (<paramref name="when"/>) 格式為：年.月.日.小時*100+分
        /// </summary>
        private static void GetGitMessage(ref string when, ref string author, ref string sha, ref string message)
        {
            string dircName = TryGetSolutionDirectoryInfo().FullName;
            using (var repository = new Repository(dircName))
            {
                var commit = repository.Commits.FirstOrDefault();
                when = $"{ commit.Author.When.UtcDateTime.Year}.{ commit.Author.When.UtcDateTime.Month}.{ commit.Author.When.UtcDateTime.Day}.{commit.Author.When.UtcDateTime.Hour * 100 + commit.Author.When.UtcDateTime.Minute }";
                author = commit.Author.Name;
                sha = commit.Id.Sha.Remove(8);
                message = commit.MessageShort.Replace("\"", "'");
            }
        }

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("自動生成 Assembly");
                Console.WriteLine("按 N 鍵清空，按 G 鍵則將 Git 資訊填入");
                var files = GetAllAssemblyFiles();
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.N:
                        {
                            ChangeAssembly(files, "", "1.0.0.0");
                            Console.WriteLine("處理完成");
                        }
                        break;
                    case ConsoleKey.G:
                        {
                            string when = string.Empty, author = string.Empty, sha = string.Empty, message = string.Empty;
                            GetGitMessage(ref when, ref author, ref sha, ref message);
                            ChangeAssembly(files, $"{author}-{sha}-{message}", when);
                            Console.WriteLine("處理完成");
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("不處理");
                        }
                        break;
                }

                Console.ReadKey();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 獲得此方案檔所在資料夾
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

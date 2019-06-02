using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Notebook
{
    class Program
    {
        static string path = "note";
        static int totalPages;

        static void Main(string[] args)
        {
            if (args.Length != 0)
                path = args[0];
            else
            {
                Console.Write("Enter the folder path: ");
                path = Console.ReadLine();
            }

            CheckPathValidity();

            ConsoleKeyInfo nextKey;
            int currentPage = 0;

            Console.BufferHeight = Console.WindowHeight;

            do
            {
                Console.Clear();

                DrawHeader(currentPage);
                try
                {
                    DrawPage(currentPage);
                }
                catch (FileNotFoundException)
                {
                    CenterAlign("Error");
                    CenterAlign("Page " + currentPage + " does not exist");
                }

                Console.CursorVisible = false;
                nextKey = Console.ReadKey(true);

                switch (nextKey.Key)
                {
                    case ConsoleKey.M:
                        currentPage++;
                        break;
                    case ConsoleKey.N:
                        currentPage--;
                        break;
                    case ConsoleKey.P:
                        Console.SetCursorPosition(0, Console.WindowHeight - 2);
                        Console.Write("ENTER PAGE NUMBER: ");
                        Console.CursorVisible = true;
                        try
                        {
                            int page = Convert.ToInt32(Console.ReadLine());
                            currentPage = page;
                        }
                        catch (Exception) {}

                        break;
                }

                if (currentPage < 0)
                    currentPage = 0;
                else if (currentPage > totalPages)
                    currentPage = totalPages;
            } while (nextKey.Key != ConsoleKey.Q && nextKey.Key != ConsoleKey.Escape);
        }

        static void CheckPathValidity()
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine("This folder does not exists");
                Environment.Exit(1);
            }

            int maxFile = 0;
            try
            {
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    int fileNumber = Convert.ToInt32(fileName);

                    if (fileNumber > maxFile)
                        maxFile = fileNumber;
                }

                for (int i = 0; i <= maxFile; i++)
                {
                    if (!File.Exists(path + "/" + i))
                        throw new Exception();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("This folder is not a valid Notebook");
                Environment.Exit(1);
            }

            totalPages = maxFile;
        }

        static void DrawHeader(int currentPage)
        {
            Console.Write(path.ToUpper().Replace('_', ' ') + " [" + currentPage + "/" + totalPages + "] ");

            int leadingProgressBar = Console.WindowWidth - (path.ToUpper() + " [" + currentPage + "/" + totalPages + "] ").Length;

            for (int i = 1; i <= leadingProgressBar; i++)
            {
                if (i <= ((float)currentPage / totalPages) * leadingProgressBar)
                    Console.Write("█");
                else
                    Console.Write("░");
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        static void DrawPage(int page)
        {
            string[] lines = File.ReadAllLines(path + "/" + page.ToString());

            foreach (string line in lines)
            {
                if (line.StartsWith("!cent "))
                    CenterAlign(line.Substring(6));
                else
                    Console.WriteLine(line);
            }
        }

        static void CenterAlign(string str)
        {
            int leadingWhiteSpace = (Console.WindowWidth - str.Length) / 2;
            for (int i = 1; i <= leadingWhiteSpace; i++)
                Console.Write(" ");
            Console.WriteLine(str);
        }
    }
}

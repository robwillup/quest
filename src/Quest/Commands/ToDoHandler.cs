using Quest.IO;
using Quest.Models;
using Quest.ObjectParsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quest.Commands
{
    public static class ToDoHandler
    {
        public static int List(App app)
        {
            List<string> files = new List<string>();
            if (app == null)
                files = GetAllToDos();
            else
            {
                string path = Path.Combine(app.LocalPath, ".quest");
                if (app.Features != null && app.Features.Count() > 0)
                    path = Path.Combine(path, app.Features.FirstOrDefault(e => true).Name);

                files = Directory.GetFiles(path, "todo.md", SearchOption.AllDirectories).ToList();
            }
            foreach (string file in files)
            {
                List<string> content = File.ReadAllLines(file).ToList();
                if (!content.Any(l => l.Contains("*")))
                    continue;
                Console.WriteLine($"Feature: {new DirectoryInfo(file).Parent.Name}");
                foreach (string line in content)
                    Console.WriteLine(line);
            }

            return 0;
        }

        public static App Handle(string[] args)
        {
            try
            {
                if (args.Length == 1)
                    return null;

                return AppParser.GetAppFromCommandLineArguments(args);                                                
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static List<string> GetAllToDos()
        {
            Config config = Setup.GetConfig();
            if (config.Applications == null || config.Applications.Count == 0)
                return null;
            List<string> files = new List<string>();
            List<App> allApps = config.Applications;
            foreach (App app in allApps)
            {
                var todoFiles = Directory.GetFiles(Path.Combine(app.LocalPath, ".quest"), "todo.md", SearchOption.AllDirectories);
                foreach (string file in todoFiles)
                    files.Add(file);
            }
            return files;
        }
    }
}
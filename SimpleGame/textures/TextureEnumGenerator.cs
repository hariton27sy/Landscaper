using System;
using System.IO;

namespace SimpleGame.textures
{
    public static class TextureEnumGenerator
    {
        public static void Run()
        {
            var root = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
            var currentFolder = Path.Combine(root, "textures");
            var configPath = Path.Combine(currentFolder, ".conf");
            var enumPath = Path.Combine(currentFolder, "BlockType.cs");

            GenerateEnum(configPath, enumPath);
        }

        private static void GenerateEnum(string configPath, string enumPath)
        {
            Console.WriteLine(configPath);
            Console.WriteLine(enumPath);
            using (StreamWriter enumFile = new StreamWriter(enumPath, false))
            {
                enumFile.WriteLine("namespace SimpleGame.textures\n{\n\tpublic enum BlockType\n\t{");
                using (StreamReader config = new StreamReader(configPath))
                {
                    config.ReadLine();
                    config.ReadLine();
                    var count = Int32.Parse(config.ReadLine());
                    for (int i = 0; i < count; i++)
                    {
                        var info = config.ReadLine();
                        var split = info.Split();
                        var sheet = split[0];
                        var id = split[1];
                        var name = split[2];
                        
                        enumFile.WriteLine($"\t\t{name} = {id},");
                        
                        config.ReadLine();
                    }
                }
                enumFile.WriteLine("\t}\n}");
            }
        }
    }
}
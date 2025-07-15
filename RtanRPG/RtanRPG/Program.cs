using RtanRPG.Utils;
using System.Text.Json;
using System.IO;

namespace RtanRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameConfig config = new GameConfig();
            config.PlayerName = "나";
            config.Level = 1;
            config.Experience = 1;
            JsonHandler<GameConfig> jsonHandler = new JsonHandler<GameConfig>();
            string folderPath = Path.Combine(Environment.CurrentDirectory, "SaveData");
            Directory.CreateDirectory(folderPath); 
            string filePath = Path.Combine(folderPath, "config.json");

            jsonHandler.Save(config, filePath);
            config = jsonHandler.Load(filePath);
            Console.WriteLine($"{config.PlayerName}");
            Console.WriteLine($"{folderPath}");

        }
    }
}

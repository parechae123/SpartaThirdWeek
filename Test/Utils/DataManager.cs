using Test.Data;

namespace Test.Utils
{
    public class DataManager : Singleton<DataManager>
    {
        private static readonly string Path = Environment.CurrentDirectory;

        protected override void Initialize()
        {
            base.Initialize();

            PlayerData = JsonDataSerializer.Deserialize<PlayerData>(GetDataFilePath(nameof(PlayerData)));
            MapData = JsonDataSerializer.Deserialize<MapData[]>(GetDataFilePath(nameof(MapData)));
            SceneData = JsonDataSerializer.Deserialize<SceneData[]>(GetDataFilePath(nameof(SceneData)));
        }

        public static string GetDataFilePath(string filename)
        {
            return Path + @"\Asset\Data\" + filename + ".json";
        }
        
        public static string GetSpriteFilePath(string filename)
        {
            return Path + @"\Asset\Sprite\" + filename + ".png";
        }
        
        public static string GetVideoFilePath(string filename)
        {
            return Path + @"\Asset\Video\" + filename + ".mp4";
        }

        public PlayerData? PlayerData { get; set; }
        
        public MapData[] MapData { get; set; } = [];

        public SceneData[] SceneData { get; set; } = [];
    }
}
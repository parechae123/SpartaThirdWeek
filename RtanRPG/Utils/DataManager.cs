using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RtanRPG.Utils
{
    public class SceneData
    {
        public int[] Indexex { get; set; } = new int[0];
        public string[] Menus { get; set; }
        public string BackgroundMusic { get; set; }
    }

    public class SpawnData : SceneData
    {
        public int index { get; set; } 
        public int Left { get; set; }
        public int Right { get; set; }
    }

    public class StageSceneData : SpawnData
    {
        public int[][] map { get; set; }
        public SpawnData[] Positions { get; set; }
    }

    public class CharacterData
    {
        public string name { get; set; }
        public int Level { get; set; }
        public string Class { get; set; }
        public int HealthPoint { get; set; }
        public int AttackPoint { get; set; }
        public int DefensePoint { get; set; }
        public int Gold { get; set; }
    }

    public class BossEnemyData : CharacterData
    {
        public string[] Music { get; set; }
        public string[] Notes { get; set; }
    }

    public class PlayerData : CharacterData
    {
        public int[] Inventories { get; set; }
        public int[] Equipments { get; set; }
    }

    public class DataManager
    {
        public PlayerData PlayerData { get; set; } 
        public BossEnemyData[] BossEnemyData { get; set; }  
        public SceneData[] SceneData { get; set; } 
        public StageSceneData[] StageSceneData { get; set; }                   //필요 객체들 생성
        private readonly string filePath;
        private static DataManager instance;

        private DataManager()   //싱글톤화 시키면서 저장할 파일 생성
        {
            string folderPath = Path.Combine(Environment.CurrentDirectory, "SaveData");
            Directory.CreateDirectory(folderPath);
            filePath = Path.Combine(folderPath, "AllData.json");
            instance.Load();
        }

        public static DataManager Instance //싱글톤 실행 하나의 객체만 만듦
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataManager();
                }
                
                return instance;
            }
        }

        public void Save()      //json으로 번역해서 파일을 저장함
        {
            JsonSerializer<DataManager> serializer = new JsonSerializer<DataManager>();
            serializer.Save(this, filePath);
        }

        public void Load()    //저장을 통해 json으로 번역돼 저장한 파일을 다시 원상복구시킴
        {
            JsonSerializer<DataManager> serializer = new JsonSerializer<DataManager>();
            DataManager loadFile = serializer.Load(filePath);
            if (loadFile != null)
            {
                this.PlayerData = loadFile.PlayerData;
                this.BossEnemyData = loadFile.BossEnemyData;
                this.SceneData = loadFile.SceneData;
                this.StageSceneData = loadFile.StageSceneData;
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RtanRPG.Data;

namespace RtanRPG.Utils
{
    public class DataManager
    {
        public PlayerData PlayerData { get; set; } 
        public BossEnemyData[] BossEnemyData { get; set; }  
        public SceneData[] SceneData { get; set; } 
        public StageSceneData[] StageSceneData { get; set; }                   //필요 객체들 생성
        private readonly string filePath;
        private static DataManager instance;
        public static string Path = Environment.CurrentDirectory;

        public static string GetDataFilePath(string filename)
        {
            return Path + @"\Asset\Data\" + filename + ".json";
        }

        public static string GetVideoFilePath(string filename)
        {
            return Path + @"\Asset\Video\" + filename + ".mp4";
        }

        private DataManager()   //싱글톤화 시키면서 저장할 파일 생성
        {
            //string folderPath = Path.Combine(Environment.CurrentDirectory, "SaveData");
            //Directory.CreateDirectory(folderPath);
            //filePath = Path.Combine(folderPath, "AllData.json");
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

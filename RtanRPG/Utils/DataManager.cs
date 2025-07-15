using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RtanRPG.Utils
{
    public class player                     //예시용
    {
        public int level { get; set; }
        public string name { get; set; }
        public string job { get; set; }
        public int atk { get; set; }
    }
    public class DataManager
    {
        public player PlayerData { get; set; } = new player();
        public List<player> PlayersData { get; set; } = new List<player>();
        public List<player> ItemsData { get; set; } = new List<player>();
        public List<player> ShopItemData { get; set; } = new List<player>();
        public string AllData { get; set; }                     //필요 객체들 생성
        private readonly string filePath;
        private static DataManager instance;

        private DataManager()   //싱글톤화 시키면서 저장할 파일 생성
        {
            string folderPath = Path.Combine(Environment.CurrentDirectory, "SaveData");
            Directory.CreateDirectory(folderPath);
            filePath = Path.Combine(folderPath, "AllData.json");
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
                this.PlayersData = loadFile.PlayersData;
                this.ItemsData = loadFile.ItemsData;
                this.ShopItemData = loadFile.ShopItemData;
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RtanRPG.Utils
{
    public class DataManager
    {
        public string AllData { get; set; }
        private readonly string filePath;
        
        private static DataManager instance;
        private static readonly object lockObj = new();
        private DataManager() 
        {
            string folderPath = Path.Combine(Environment.CurrentDirectory, "SaveData");
            Directory.CreateDirectory(folderPath);
            filePath = Path.Combine(folderPath, "AllData.json");
        }

        public static DataManager Instance
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

        public void Load()
        {
            JsonSerializer<DataManager> serializer = new JsonSerializer<DataManager>();
            DataManager loadFile = serializer.Load(filePath);
            if (loadFile != null)
            {
                this.AllData = loadFile.AllData;
            }
        }


        public void Save()
        {
            JsonSerializer<DataManager> serializer = new JsonSerializer<DataManager>();
            serializer.Save(this, filePath);
        }
    }

}

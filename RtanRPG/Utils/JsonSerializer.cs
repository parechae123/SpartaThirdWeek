using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RtanRPG.Utils
{
    public class JsonSerializer<T>
    {
        public void Save(T data, string filePath)
        {
            string jason = JsonSerializer.Serialize(data);
            File.WriteAllText(filePath, jason);
        }
        public T Load(string filePath)
        {
            string json = File.ReadAllText(filePath);
            T data = JsonSerializer.Deserialize<T>(json);
            return data;
        }
    }
}

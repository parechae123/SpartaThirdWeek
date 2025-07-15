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

    //public static class JsonSerializer<T>
    //{
    //    private static readonly string Path = System.IO.Path.GetDirectoryName(
    //    System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;

    //    public static void Serialize(T data)
    //    {
    //        string json = JsonSerializer.Serialize(data);
    //    }

    //    public static T Deserialize(string json)
    //    {

    //        return JsonSerializer.Deserialize<T>(json);

    //    }
    //}


}

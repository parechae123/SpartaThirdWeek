using System.Text.Json;

namespace Test.Utils
{
    public static class JsonDataSerializer
    {
        private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

        public static T Deserialize<T>(string path)
        {
            var text = File.ReadAllText(path);

            var data = JsonSerializer.Deserialize<T>(text);
            if (data == null)
            {
                throw new NullReferenceException();
            }

            return data;
        }

        public static void Serialize<T>(string path, T data)
        {
            try
            {
                var text = JsonSerializer.Serialize(data, Options);

                File.WriteAllText(path, text);
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception.Message);
            }
        }
    }
}
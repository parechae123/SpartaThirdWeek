using System.Data;
using Test.Utils.Extension;

namespace Test
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                var application = new Application();
                
                application.Play();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
namespace RtanRPG
{
    internal class Program
    {
        static void Main(string[] args)
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
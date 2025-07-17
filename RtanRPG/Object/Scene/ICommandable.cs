namespace RtanRPG.Object.Scene
{
    public interface ICommandable
    {
        void Execute(ConsoleKey key);

        Dictionary<ConsoleKey, Action?> Commands { get; }
    }
}
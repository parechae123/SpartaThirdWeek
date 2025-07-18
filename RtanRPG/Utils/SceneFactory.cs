using RtanRPG.Object.Scene;

namespace RtanRPG.Utils
{
    public static class SceneFactory
    {
        public static BaseScene Create(int index)
        {
            switch (index)
            {
                case 0:  return new MainScene(index);
                case 1:  return new Stage01Scene(index);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
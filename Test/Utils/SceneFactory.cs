using Test.Object.Scene;

namespace Test.Utils
{
    public static class SceneFactory
    {
        public static BaseScene Create(int index)
        {
            switch (index)
            {
                case 0:  return new MainScene(index);
                case 1:  return new MenuScene(index);
                case 2:  return new Cut01Scene(index);
                case 3:  return new StageScene(index);
                case 4:  return new BattleScene(index);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
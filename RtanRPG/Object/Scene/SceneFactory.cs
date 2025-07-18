using RtanRPG.Object.Scene;

namespace RtanRPG.Object.Scene
{
    public static class SceneFactory
    {
        public static BaseScene Create(int index)
        {
            switch (index)
            {
                case 0: return new MainScene(index);
                case 3: return new Stage01Scene(index);
                case 6: return new BattleScene(index);
                default: throw new System.Exception("Unknown scene index: " + index);
            }
        }
    }
}
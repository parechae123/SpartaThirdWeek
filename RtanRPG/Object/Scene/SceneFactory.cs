namespace RtanRPG.Object.Scene
{
    public static class SceneFactory
    {
        public static BaseScene Create(int index)
        {
            switch (index)
            {
                case 0: return new MainScene();
                case 1: return new StatusScene();
                case 2: return new CutScene();
                case 3: return new Stage01Scene();
                case 4: return new Stage02Scene();
                case 5: return new Stage03Scene();
                case 6: return new BattleScene();
                default: throw new System.Exception("Unknown scene index: " + index);
            }
        }
    }
}
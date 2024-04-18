using System;
using Game.UI.Gameplay;

namespace Game.Configs.LevelData
{
    public interface ILevelContent
    {
        public ILevelElement[] GetLevelElements();
        public Extras[] Extras { get; }
        float Time { get; }
        float BoundDistance { get; }
    }

    [Serializable]
    public class Extras
    {
        public string extrasId;
        public int amount;
    }
}
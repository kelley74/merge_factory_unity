namespace Game.Configs.LevelData
{
    public interface ILoadable
    {
        public string GetAddress();
    }

    public interface ILevelElement
    {
        string Id { get; }
        ILoadable GetLoadable();
        int Amount { get; }
        bool IsTarget { get; }
    }
}
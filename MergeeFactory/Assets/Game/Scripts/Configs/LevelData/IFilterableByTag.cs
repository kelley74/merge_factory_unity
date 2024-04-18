namespace Game.Configs.LevelData
{
    public interface IFilterableByTag
    {
        bool ContainsTag(string tag);
        bool ContainsTags(string[] tags);
    }
}
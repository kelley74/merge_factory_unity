using System.Linq;
using UnityEngine;

namespace Game.Configs.LevelData
{
    [CreateAssetMenu(menuName = "Data/LevelRepository", fileName = "LevelRepository")]
    public class LevelObjectsRepository : ScriptableObject
    {
        [SerializeField] private LevelObject[] _levelObjects;

        public LevelObject[] GetLevelObjectsByTag(string[] tags)
        {
            return _levelObjects.Where(t => (t as IFilterableByTag).ContainsTags(tags)).ToArray();
        }
    }
}
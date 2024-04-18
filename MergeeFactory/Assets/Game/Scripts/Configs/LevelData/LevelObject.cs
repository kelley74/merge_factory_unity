using System.Linq;
using UnityEngine;

namespace Game.Configs.LevelData
{
    [CreateAssetMenu(menuName = "Data/LevelObject", fileName = "LevelObject")]
    public class LevelObject : ScriptableObject, ILoadable, IFilterableByTag
    {
        [SerializeField] private string[] _levelTags;
        [SerializeField] private string _objectId;

        string ILoadable.GetAddress()
        {
            return _objectId;
        }

        bool IFilterableByTag.ContainsTag(string tag)
        {
            return _levelTags.Contains(tag);
        }

        public bool ContainsTags(string[] tags)
        {
            foreach (var tag in tags)
            {
                if ((this as IFilterableByTag).ContainsTag(tag))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
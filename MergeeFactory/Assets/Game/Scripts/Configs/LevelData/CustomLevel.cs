using System.Linq;
using UnityEngine;

namespace Game.Configs.LevelData
{
    [CreateAssetMenu(menuName = "Data/Level/Custom", fileName = "CustomLevel")]
    public class CustomLevel : ScriptableObject, ILevelContent
    {
        [SerializeField] private LevelElement[] _elements;
        [SerializeField] private int _time = 90;
        [SerializeField] private float _boundDistance = 1.3f;
        [SerializeField] private Extras[] _extras;

        float ILevelContent.Time => _time;
        float ILevelContent.BoundDistance => _boundDistance;
        Extras[] ILevelContent.Extras => _extras;

        public ILevelElement[] GetLevelElements()
        {
            return _elements.Cast<ILevelElement>().ToArray();
        }
    }
}
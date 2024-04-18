using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Configs.LevelData
{
    [CreateAssetMenu(menuName = "Data/Level/GeneratedByTag", fileName = "GeneratedLevel")]
    public class GeneratedLevelByTags : ScriptableObject, ILevelContent
    {
        [SerializeField] private LevelObjectsRepository _repository;
        [SerializeField] private string[] _tags;
        [SerializeField] private int _uniqueObjectsAmount;
        [SerializeField] private int _targetObjectsAmount;
        [SerializeField] [Range(1,20)] private int _repetitionsMin;
        [SerializeField] [Range(1,20)] private int _repetitionsMax;
        [SerializeField] private int _time = 90;
        [SerializeField] private float _boundDistance = 1.3f;
        [SerializeField] private Extras[] _extras;

        float ILevelContent.Time => _time;
        float ILevelContent.BoundDistance => _boundDistance;
        Extras[] ILevelContent.Extras => _extras;

        // This method could be used for level config testing
        public ILevelElement[] GetLevelElements()
        {
            var allObjects = _repository.GetLevelObjectsByTag(_tags).Cast<ILoadable>().ToList();
            if (allObjects.Count < _uniqueObjectsAmount)
            {
                throw new Exception(
                    "There are not enough unique objects to build a level according the level data");
            }

            var finalObjects = new List<ILevelElement>();
            var targets = _targetObjectsAmount; 
            
            for (int i = 0; i < _uniqueObjectsAmount; i++)
            {
                var obj = allObjects[Random.Range(0, allObjects.Count)];
                allObjects.Remove(obj);
                var entries = Random.Range(_repetitionsMin, _repetitionsMax + 1) * 3;
                finalObjects.Add(new LevelGeneratedElement(obj, entries, targets>0));
                targets--;
            }

            return finalObjects.ToArray();
        }

#if UNITY_EDITOR
        [ContextMenu("Test Level")]
        private void CheckLevelObjects()
        {
            var items = GetLevelElements();
            int i = 0;
            foreach (var item in items)
            {
                Debug.Log($"[CHECK] Item {i} - {item.GetLoadable().GetAddress()}");
                i++;
            }
        }
        
#endif
    }
}
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Configs.LevelData
{
    [CreateAssetMenu(menuName = "Data/LevelsConfig", fileName = "LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField] private CustomLevel[] _customLevels;
        [SerializeField] private GeneratedLevelByTags[] _generatedLevels;

        public ILevelContent GetLevelContent(int levelNumber)
        {
            if (levelNumber < _customLevels.Length)
            {
                return _customLevels[levelNumber];
            }
            
            levelNumber -= _customLevels.Length;
            if (levelNumber < _generatedLevels.Length)
            {
                return _generatedLevels[levelNumber];
            }

            throw new Exception("Handle end of Content case");
        }
        
#if UNITY_EDITOR
        [ContextMenu("Random Test")]
        private void CheckLevelObjects()
        {
            var level = Random.Range(0, _customLevels.Length + _generatedLevels.Length + 2);
            Debug.Log($"TEST LEVEL {level}");
            var content = GetLevelContent(level);
            int i = 0;
            foreach (var item in content.GetLevelElements())
            {
                Debug.Log($"[CHECK] Item {i} - {item.GetLoadable().GetAddress()}");
                i++;
            }
        }
        
#endif
    }
}

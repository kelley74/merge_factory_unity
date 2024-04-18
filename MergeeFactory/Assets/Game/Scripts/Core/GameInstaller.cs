using Game.Configs.LevelData;
using UnityEngine;
using Zenject;

namespace Game.Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private GameProcessor _gameProcessor;

        [Inject] private DiContainer _container;

        public override void InstallBindings()
        {
            Container.Bind<GameMaster>().FromNew().AsSingle().NonLazy();
            Container.Bind<LevelsConfig>().FromInstance(_levelsConfig).AsSingle().NonLazy();
            Container.Bind<DeckHolder>().FromNew().AsSingle().NonLazy();
            
            Container.Bind<GameProcessor>().FromInstance(_gameProcessor).AsSingle().NonLazy();
        }
    }
}
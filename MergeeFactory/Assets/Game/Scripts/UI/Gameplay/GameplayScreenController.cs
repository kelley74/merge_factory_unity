using Game.UI.Base;
using UnityEngine;

namespace Game.UI.Gameplay
{
    public class GameplayScreenController : UiController<GameplayScreenView>
    {
        protected override string ViewPrefab => "GameplayScreen";
        protected override int SiblingIndex => 231;
        
        protected override void OnOpen()
        {
        }

        protected override void OnClose()
        {
        }

        public Vector3[] FetchDeckSpots()
        {
            return _view.SpotPositions;
        }
    }
}

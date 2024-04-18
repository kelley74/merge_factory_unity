using System.Collections.Generic;
using System.Linq;

namespace Game.UI.Base
{
    public class SiblingSorter
    {
        private List<ISiblingSettable> _controllers = new List<ISiblingSettable>();

        public void RegisterUiController(ISiblingSettable controller)
        {
            _controllers.Add(controller);

            _controllers = _controllers.OrderBy((t) => t.CurrentSiblingIndex).ToList();

            for (int i=0; i<_controllers.Count; i++)
            {
                _controllers[i].ResetSibling(i);
            }
        
        }
    }
}

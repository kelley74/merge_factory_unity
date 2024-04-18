using BigCity.Scripts.UI.Base;

namespace Game.UI.ResultScreen
{
    public class ResultModel : IUiModel
    {
        public bool Win { get; private set; }
        public LoseReason LoseReason {get; private set; }

        public ResultModel(bool win, LoseReason reason = LoseReason.None)
        {
            Win = win;
            LoseReason = reason;
        }
    }

    public enum LoseReason
    {
        None,
        Time,
        Deck,
        NotValid
    }
}
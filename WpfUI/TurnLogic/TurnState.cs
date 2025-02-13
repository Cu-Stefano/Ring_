namespace WpfUI.TurnLogic
{
    public interface ITurnState
    {
        void OnEnter();
        void OnExit();
    }

    public abstract class TurnState(MapLogic turnMapLogic) : ITurnState
    {
        protected MapLogic TurnMapLogic { get; } = turnMapLogic;

        public abstract void OnEnter();
        public abstract void OnExit();
    }
}
namespace Final_Task.Games
{
    public abstract class CasinoGameBase
    {
        public event Action? OnWin;
        public event Action? OnLoose;
        public event Action? OnDraw;

        public abstract void PlayGame();

        protected abstract void FactoryMethod();

        protected void OnWinInvoke() => OnWin?.Invoke();
        protected void OnLooseInvoke() => OnLoose?.Invoke();
        protected void OnDrawInvoke() => OnDraw?.Invoke();
    }
}

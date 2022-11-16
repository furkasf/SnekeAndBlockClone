using System;

namespace GameCore
{
    public static class CoreGameSignals
    {
        public static Action onReset;
        public static Action onPlay;
        public static Action<bool> setGamePlay;
        public static Func<bool> onGamePlay;
    }
}
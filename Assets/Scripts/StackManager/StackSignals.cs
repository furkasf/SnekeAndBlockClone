using Snake;
using System;

namespace StackManager
{
    public static class StackSignals
    {
        public static Action<AbstractSnake> onAddStack;
        public static Action onRemoveStack;
        public static Action onEnterPowerState;
        public static Action<bool> onReadyToPlay;
        public static Func<SnakePowerState> onInPowerStack;
    }
}
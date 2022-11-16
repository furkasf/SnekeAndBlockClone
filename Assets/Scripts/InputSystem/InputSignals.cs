using System;

namespace MyInput
{
    public static class InputSignals
    {
        public static Action onEnableInput;
        public static Action onDisableInput;
        public static Action onPlay;
        public static Action onPause;

        public static Action onFirstTimeTouchTaken;
        public static Action onInputTaken;
        public static Action<InputParams> onInputDragged;
        public static Action onInputReleased;
    }
}
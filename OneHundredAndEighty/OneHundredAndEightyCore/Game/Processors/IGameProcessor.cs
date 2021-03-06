﻿#region Usings

using OneHundredAndEightyCore.Recognition;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public interface IGameProcessor
    {
        void OnThrow(DetectedThrow thrw);
        event ProcessorBase.EndMatchDelegate OnMatchEnd;
    }
}
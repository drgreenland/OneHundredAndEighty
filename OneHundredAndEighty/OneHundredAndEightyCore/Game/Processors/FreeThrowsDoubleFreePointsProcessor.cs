﻿#region Usings

using System.Collections.Generic;
using OneHundredAndEightyCore.Common;
using OneHundredAndEightyCore.Recognition;
using OneHundredAndEightyCore.ScoreBoard;

#endregion

namespace OneHundredAndEightyCore.Game.Processors
{
    public class FreeThrowsDoubleFreePointsProcessor : ProcessorBase
    {
        public FreeThrowsDoubleFreePointsProcessor(Game game,
                                                   List<Player> players,
                                                   DBService dbService,
                                                   ScoreBoardService scoreBoard)
            : base(game, players, dbService, scoreBoard)
        {
        }

        public override void OnThrow(DetectedThrow thrw)
        {
            PlayerOnThrow.HandPoints += thrw.TotalPoints;

            scoreBoard.AddPointsToClassic(thrw.TotalPoints, PlayerOnThrow);

            var dbThrow = new Throw(PlayerOnThrow,
                                    Game,
                                    thrw.Sector,
                                    thrw.Type,
                                    ThrowResult.Ordinary,
                                    PlayerOnThrow.ThrowNumber,
                                    thrw.TotalPoints,
                                    thrw.Poi,
                                    thrw.ProjectionResolution);

            dbService.ThrowSaveNew(dbThrow);

            PlayerOnThrow.HandThrows.Push(dbThrow);

            if (IsHandOver())
            {
                Check180();

                ClearThrows();

                TogglePlayerOnThrow();
            }
            else
            {
                PlayerOnThrow.ThrowNumber += 1;
            }
        }
    }
}
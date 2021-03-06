﻿#region Usings

using System;

#endregion

namespace OneHundredAndEightyCore.Game
{
    public class Game
    {
        public int Id { get; private set; }
        public GameType Type { get; }
        public DateTime StartTimeStamp { get; }
        public DateTime EndTimeStamp { get; private set; }

        public Game(GameType type, int id = -1)
        {
            Id = id;
            Type = type;
            StartTimeStamp = DateTime.Now;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public void SetEndTimeStamp()
        {
            EndTimeStamp = DateTime.Now;
        }
    }
}
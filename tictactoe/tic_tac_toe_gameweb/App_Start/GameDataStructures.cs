using System;
using System.Collections.Generic;
using TicTacToe.Entities.Game;

namespace TicTacToe
{
    /// <summary>
    /// Class to maintain queues for games
    /// </summary>
    public class GameDataStructures
    {
        // queue to check which games are waiting for players to join. GameId (key), GameInfo (value)
        public static IList<GameInfo> WaitingGames;

        // dictionary to get the game based on the gameid (key), GameInfo (value)
        public static IDictionary<string, GameInfo> StartedGames;

        // dictionary to check which players are playing which game. PlayerId (key), GameId (value)
        public static IDictionary<string, string> PlayerOngoingGames;

        // dictionary to get the userid based on user email id (key), userid (value)
        public static IDictionary<string, string> Users;

        public GameDataStructures()
        {
            WaitingGames = new List<GameInfo>();
            PlayerOngoingGames = new Dictionary<string, string>();
            StartedGames = new Dictionary<string, GameInfo>();
            Users = new Dictionary<string, string>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacToe.Entities.Game
{
    /// <summary>
    /// Class to hold the user information
    /// </summary>
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// Player symbol. Can be used to assign it to individual players in game
    /// </summary>
    public enum Symbol
    {
        X = 1,
        O = 2
    }

    public class Move
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }

    /// <summary>
    /// class to store the information of the player
    /// </summary>
    public class Player
    {
        public string Id { get; set; }
        public Symbol Symbol { get; set; }
        public ICollection<string> Moves { get; set; }        
    }

    /// <summary>
    /// class to get the game information
    /// </summary>    
    public class GameInfo
    {
        public string Id { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Status Status { get; set; }
        public string WinnerId { get; set; }

        public int[,] Moves { get; set; }

        public int TotalMoves { get; set; }
    }

    /// <summary>
    /// enum to send or receive the game status
    /// </summary>
    public enum Status
    {
        Win = 1,        
        Draw = 2,
        GameInProgess = 3,
        WaitingToJoinGame = 4
    }    
    
} // namespace
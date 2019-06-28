using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicTacToe.Entities.Game;
using TicTacToe.BAL;

namespace TicTacToe.Service.Game
{
    /// <summary>
    /// API controller to handle all requests related to the game
    /// </summary>
    public class GameController : ApiController
    {
        /// <summary>
        /// Action method to create or join the game
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public string Start(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return string.Empty;
            }

            string gameId = string.Empty;

            BAL.Game objGame = new BAL.Game();
            gameId = objGame.JoinGame(email);

            return gameId;
        }

        /// <summary>
        /// Action method to make a move for particular game
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="playerId"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        // GET: api/Game/Move
        [HttpPost]
        public void Move(string gameId, string playerId, int row, int column)
        {            
            BAL.Game objGame = new BAL.Game();
            objGame.Move(gameId, playerId, new Move { Row = row, Column = column });
        }

        /// <summary>
        /// Service method to get the game information
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        [HttpGet]
        public GameInfo Status(string gameId)
        {
            if (string.IsNullOrEmpty(gameId))
            {
                return null;
            }

            GameInfo objInfo = null;

            BAL.Game objGame = new BAL.Game();
            objInfo = objGame.GetStatus(gameId);

            return objInfo;
        }
        
    } // class
} // namespace

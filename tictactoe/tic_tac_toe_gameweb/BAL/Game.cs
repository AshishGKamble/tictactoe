using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicTacToe.Entities.Game;

namespace TicTacToe.BAL
{
    /// <summary>
    /// BAL for playing the game. Have all functionality releate to game
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Function to join the game. This will also create the game if game is not created
        /// </summary>
        /// <param name="email"></param>
        public string JoinGame(string email)
        {
            string userId = string.Empty;

            if (string.IsNullOrEmpty(email))
            {
                // Here we should also raise an exception saying that values are null
                return string.Empty;
            }

            // Create user if not exists
            if (GameDataStructures.Users.ContainsKey(email))
            {
                userId = GameDataStructures.Users[email];
            }

            if (String.IsNullOrEmpty(userId))
            {
                Guid userGUId = Guid.NewGuid();
                userId = userGUId.ToString();

                // Save user id
                GameDataStructures.Users.Add(email, userId);
            }

            string gameId = string.Empty;

            // Check if is there any game user is playing
            if (GameDataStructures.PlayerOngoingGames.ContainsKey(userId))
                gameId = GameDataStructures.PlayerOngoingGames[userId];

            // If player has not joined any game yet get game which is free and return this gameid
            if (string.IsNullOrEmpty(gameId))
            {
                GameInfo objInfo = GameDataStructures.WaitingGames.FirstOrDefault();

                // no game is created yet
                if (objInfo != null)
                {
                    Player player = new Player();
                    player.Id = userId;
                    player.Symbol = Symbol.X;
                    player.Moves = new List<string>();

                    objInfo.Player2 = player;
                    objInfo.Status = Status.GameInProgess;
                    objInfo.StartTime = DateTime.Now;

                    GameDataStructures.WaitingGames.Remove(objInfo);

                    // assign gameid
                    gameId = objInfo.Id;

                    // assign game to the player
                    GameDataStructures.PlayerOngoingGames.Add(userId, gameId);

                    // Add game to started games
                    GameDataStructures.StartedGames.Add(gameId, objInfo);
                }
            }

            if (string.IsNullOrEmpty(gameId))
            {
                // Create game if game no game is free. Then create new game for the customer
                Guid gameGUId = Guid.NewGuid();
                gameId = gameGUId.ToString();

                // Add player game for waiting of another player
                GameInfo objInfo = new GameInfo();

                objInfo.Id = gameId;
                objInfo.Moves = new int[3, 3];

                Player player = new Player();
                player.Id = userId;
                player.Symbol = Symbol.O;
                player.Moves = new List<string>();

                objInfo.Player1 = player;

                objInfo.Status = Status.WaitingToJoinGame;

                // Add game to games waiting for player
                GameDataStructures.WaitingGames.Add(objInfo);

                // assign game to the player
                GameDataStructures.PlayerOngoingGames.Add(userId, gameId);
            }

            return gameId;
        }

        /// <summary>
        /// Function 
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="playerId"></param>
        /// <param name="move"></param>
        public void Move(string gameId, string playerId, Move move)
        {
            // Write validations
            if (string.IsNullOrEmpty(gameId) || string.IsNullOrEmpty(playerId))
            {
                throw new ArgumentException("GameId or PlayerId can not be blank");
            }

            if (move.Column < 0 || move.Column > 2 || move.Row < 0 || move.Row > 2)
            {
                throw new ArgumentException("Invalid Move. Move out of board size not allowed");
            }

            // Get game information
            GameInfo objGameinfo = GameDataStructures.StartedGames[gameId];

            // find out if player is player1 or player2 
            // Write logic to make a move
            int compareFactor = 0;

            if (objGameinfo.Player1.Id.Equals(playerId))
            {
                compareFactor = -1;
                objGameinfo.Moves[move.Row, move.Column] = compareFactor;
                objGameinfo.Player1.Moves.Add(String.Concat(move.Row, move.Column));
            }
            else if (objGameinfo.Player2.Id.Equals(playerId))
            {
                compareFactor = 1;
                objGameinfo.Moves[move.Row, move.Column] = compareFactor;
                objGameinfo.Player2.Moves.Add(String.Concat(move.Row, move.Column));
            }

            objGameinfo.TotalMoves += 1;

            // update games object in dictionary
            GameDataStructures.StartedGames[gameId] = objGameinfo;

            // Check for the Winner
            int rowSum = 0, colSum = 0, diagonalSum = 0, revDiagonalSum = 0;

            // check rows
            for (int colIndex = 0; colIndex < 3; colIndex++)
            {
                rowSum += objGameinfo.Moves[move.Row, colIndex];
            }

            // check columns
            for (int rowIndex = 0; rowIndex < 3; rowIndex++)
            {
                colSum += objGameinfo.Moves[rowIndex, move.Column];
            }

            // check diagonals
            for (int index = 0; index < 3; index++)
            {
                diagonalSum += objGameinfo.Moves[index, index];
            }

            // check reverse diagonals
            for (int rowIndex = 0, colIndex = 2; rowIndex < 3 && colIndex > 0; rowIndex++, colIndex--)
            {
                revDiagonalSum += objGameinfo.Moves[rowIndex, colIndex];
            }

            if (Math.Abs(rowSum) == 3 || Math.Abs(colSum) == 3 || Math.Abs(diagonalSum) == 3 || Math.Abs(revDiagonalSum) == 3)
            {
                objGameinfo.WinnerId = playerId;
                objGameinfo.Status = Status.Win;
            }

            if (objGameinfo.TotalMoves == 9 && objGameinfo.Status != Status.Win) // if all moves are played on board then its draw
            {
                objGameinfo.Status = Status.Draw;
            }
        }

        /// <summary>
        /// Function to get the game status based on the gameid
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public GameInfo GetStatus(string gameId)
        {
            if (string.IsNullOrEmpty(gameId))
            {
                return null;
            }

            GameInfo objGameInfo = null;

            // Get the game information based on the gameId
            objGameInfo = GameDataStructures.StartedGames[gameId];

            return objGameInfo;
        }

    } // class
} // namespace
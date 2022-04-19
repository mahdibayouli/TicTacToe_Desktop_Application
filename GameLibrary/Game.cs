using System;
using System.Windows;

namespace GameLibrary
{
    public enum GameMode
    {
        MULTPILAYER, HARD, EASY, NOTSELECTED
    }
    public enum XO //this is used to determine what the singleplayer chose to play, X or O 
    {
        X, O, NOTSELECTED
    }
    public class Game
    {
        public int[] board = new int[9]; // 0 => empty | 1 => X | 2 => O
        public int[] score = new int[3]; // score[0] : X's score | score[1] : O's score | score[2] : Draws

        public GameMode GameMode { get; set; }
        public XO SinglePlayerXorO { get; set; }
        public bool XIsPlaying { get; set; } // XIsPlaying == true : it's X's turn otherwise it's O's turn

        public Game()
        {
            this.SinglePlayerXorO = XO.NOTSELECTED;
            this.GameMode = GameMode.NOTSELECTED;
        }

        int Evaluate()
        {
            //returns 0, -1 or 1:
            if (LastPlayerWon(true))
            {
                //returns a positive value if there is a winner and it's the computer
                if (XIsPlaying && (SinglePlayerXorO == XO.O)) return 1;
                if (!XIsPlaying && (SinglePlayerXorO == XO.X)) return 1;

                //returns a negative value if there is a winner and it's the player
                else return -1;
            }

            //returns a neutral value if it is a draw or if the game is not finished yet
            return 0;
        }

        public int Minimax(int depth, bool maximizingTurn)
        {
            //evaluate the current state of the board
            int score = Evaluate();

            //BASE CASE : if the game is finished return the score
            if (score == 1 || score == -1 || BoardIsFull(true))
                return score;

            if (maximizingTurn) //it's the computer's turn
            {
                int best = -1000;  
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == 0)  //trying every available move on the board
                    {
                        board[i] = SinglePlayerXorO == XO.O ? 1 : 2;                //assuming the computer makes the move 
                        XIsPlaying = !XIsPlaying;                                   //moving to the next turn (the player's turn)
                        best = Math.Max(best, Minimax(depth + 1, !maximizingTurn)); //calling minimax on player's turn => minimizing
                        board[i] = 0;                                               //the move is undone
                        XIsPlaying = !XIsPlaying;                                   //moving back to the previous turn 
                    }
                }
                return best;
            }
            else //it's the player's turn (Minimizing)
            {
                int best = 1000;
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == 0)
                    {
                        board[i] = SinglePlayerXorO == XO.O ? 2 : 1;                //assuming the player makes the move 
                        XIsPlaying = !XIsPlaying;                                   //moving to the next turn (the computer's turn)
                        best = Math.Min(best, Minimax(depth + 1, !maximizingTurn)); //calling minimax on computer's turn => maximizing
                        board[i] = 0;                                               //the move is undone
                        XIsPlaying = !XIsPlaying;                                   //moving back to the previous turn 
                    }
                }
                return best;
            }
        }

        int BestChoice()
        {
            int bestVal = -1000, bestMove = -1;     //will be maximizing the bestVal for the computer
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == 0)  //trying every empty field on the board
                {
                    board[i] = SinglePlayerXorO == XO.O ? 1 : 2;
                    int moveVal = Minimax(0, false);
                    board[i] = 0;
                    if (moveVal > bestVal)  //is this move better than the current best move ? if so make it the current best move
                    {
                        bestMove = i;
                        bestVal = moveVal;
                    }
                }
            }
            return bestMove;
        }


        public int ComputerTurn()
        {
            
            int choice = GameMode == GameMode.HARD ? BestChoice() : RandomChoice();
            board[choice] = XIsPlaying ? 1 : 2;
            XIsPlaying = !XIsPlaying;       //moving to the next turn
            return choice;
        }
        public int RandomChoice()
        {
            //choosing a random empty field
            Random rand = new();
            int choice;
            do
                choice = rand.Next(9);
            while (board[choice] != 0); //chosen field must be unoccupied
            return choice;
        }

        public bool BoardIsFull(bool evaluating = false)
        {
            foreach (int i in board)
                if (i == 0) return false;
            if (!evaluating)    //preventing these MessageBoxes from showing during minimax evaluation (which uses BoardIsFull)
            {
                string message = string.Format("It's a draw!");
                MessageBox.Show(message);
                score[2]++; 
            }
            return true;
        }
        public bool LastPlayerWon(bool evaluating = false)
        {
            if (
               ((board[0] > 0) && (board[0] == board[1] && board[1] == board[2])) ||    //upper row
               ((board[3] > 0) && (board[3] == board[4] && board[4] == board[5])) ||    //middle row
               ((board[6] > 0) && (board[6] == board[7] && board[7] == board[8])) ||    //bottom row
               ((board[0] > 0) && (board[0] == board[3] && board[3] == board[6])) ||    //left column
               ((board[1] > 0) && (board[1] == board[4] && board[4] == board[7])) ||    //middle column
               ((board[2] > 0) && (board[2] == board[5] && board[5] == board[8])) ||    //right column
               ((board[0] > 0) && (board[0] == board[4] && board[4] == board[8])) ||    //first diagonal
               ((board[6] > 0) && (board[6] == board[4] && board[4] == board[2]))       //second diagonal
               )
            {
                if (!evaluating) //preventing these MessageBoxes from showing during minimax evaluation (which uses BoardIsFull)
                {
                    string message = string.Format("{0} wins!", XIsPlaying ? "O" : "X");
                    MessageBox.Show(message);
                    if (XIsPlaying) score[1]++; else score[0]++;
                }
                return true;
            }
            return false;
        }
    }
}

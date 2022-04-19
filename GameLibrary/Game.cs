using System;
using System.Windows;

namespace GameLibrary
{
    public enum GameMode { 
        MULTPILAYER, HARD, EASY, NOTSELECTED
    }
    public enum XO { 
        X , O , NOTSELECTED
    }
    public class Game
    {
        public int[] board = new int[9]; // 0: empty, 1: X, 2: O
        public int[] score = new int[3]; // each index corresponds to: {0 : X's score, 1 : O's score, 2: Draws}

        public GameMode GameMode { get; set; }
        public XO SinglePlayerXorO { get; set; }
        public bool XIsPlaying { get; set; }          // XIsPlaying == true ==> it's X's turn otherwise it's O's turn

        public Game() {
            this.SinglePlayerXorO = XO.NOTSELECTED;
            this.GameMode = GameMode.NOTSELECTED;
        }

        int Evaluate()
        {
            if (LastPlayerWon(true))
            {
                if (XIsPlaying && (SinglePlayerXorO == XO.O)) return 1;
                if (!XIsPlaying && (SinglePlayerXorO == XO.X)) return 1;
                else return -1;
            }
            return 0;
        }

        public int Minimax(int depth, int computer, bool maximizingTurn)
        {
            int score = Evaluate();
            if (score == 1 || score == -1 || BoardIsFull(true))
                return score;
            if (maximizingTurn)
            {
                int best = -1000;
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == 0)
                    {
                        board[i] = computer; //computer move, maximizing
                        XIsPlaying = !XIsPlaying;
                        best = Math.Max(best, Minimax(depth + 1, computer, !maximizingTurn));
                        board[i] = 0;
                        XIsPlaying = !XIsPlaying;
                    }
                }
                return best;
            }
            else
            {
                int best = 1000;
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == 0)
                    {
                        board[i] = computer == 1 ? 2 : 1;
                        XIsPlaying = !XIsPlaying;
                        best = Math.Min(best, Minimax(depth + 1, computer, !maximizingTurn));
                        board[i] = 0;
                        XIsPlaying = !XIsPlaying;
                    }
                }
                return best;
            }
        }

        int BestChoice(int computer)
        {
            int bestVal = -1000, bestMove = -1;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == 0)
                {
                    board[i] = computer;
                    int moveVal = Minimax(0, computer, false);
                    board[i] = 0;
                    if (moveVal > bestVal)
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
            int computer = SinglePlayerXorO == XO.X ? 2 : 1;
            int choice = GameMode == GameMode.HARD ? BestChoice(computer) : RandomChoice();
            board[choice] = XIsPlaying ? 1 : 2;
            XIsPlaying = !XIsPlaying;
            return choice;
        }
        public int RandomChoice()
        {
            Random rand = new();
            int choice;
            do
                choice = rand.Next(9);
            while (board[choice] != 0);
            return choice;
        }

        public bool BoardIsFull(bool evaluating = false)
        {
            foreach (int i in board)
                if (i == 0) return false;
            if (!evaluating)
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
                if (!evaluating)
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

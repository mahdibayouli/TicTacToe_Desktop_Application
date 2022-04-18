using System;
using System.Windows;

namespace GameLibrary
{
    public class Game
    {
        public int[] board = new int[9]; // 0: empty, 1: X, 2: O
        public int[] score = new int[3]; // each index corresponds to: {0 : X's score, 1 : O's score, 2: Draws}

        public bool singlePlayer = false;
        public bool singleX = false;        // if singleX == false then it's singleO 
        public bool turnX = true;           // if turnX == false then it's turnO
        public bool hardMode = false;       // if hardMode == false then it's easyMode

        int evaluate()
        {
            if (lastPlayerWon(true))
            {
                if (turnX && !singleX) return 1;
                if (!turnX && singleX) return 1;
                else return -1;
            }
            return 0;
        }

        public int minimax(int depth, int computer, bool maximizingTurn)
        {
            int score = evaluate();
            if (score == 1 || score == -1 || boardIsFull(true))
                return score;
            if (maximizingTurn)
            {
                int best = -1000;
                for (int i = 0; i < board.Length; i++)
                {
                    if (board[i] == 0)
                    {
                        board[i] = computer; //computer move, maximizing
                        turnX = !turnX;
                        best = Math.Max(best, minimax(depth + 1, computer, !maximizingTurn));
                        board[i] = 0;
                        turnX = !turnX;
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
                        turnX = !turnX;
                        best = Math.Min(best, minimax(depth + 1, computer, !maximizingTurn));
                        board[i] = 0;
                        turnX = !turnX;
                    }
                }
                return best;
            }
        }

        int bestChoice(int computer)
        {
            int bestVal = -1000, bestMove = -1;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == 0)
                {
                    board[i] = computer;
                    int moveVal = minimax(0, computer, false);
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


        public int computerTurn()
        {
            int computer = singleX ? 2 : 1;
            int choice = hardMode ? bestChoice(computer) : randomChoice();
            board[choice] = turnX ? 1 : 2;
            turnX = !turnX;
            return choice;
        }
        public int randomChoice()
        {
            Random rand = new Random();
            int choice;
            do
                choice = rand.Next(9);
            while (board[choice] != 0);
            return choice;
        }

        public bool boardIsFull(bool evaluating = false)
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
        public bool lastPlayerWon(bool evaluating = false)
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
                    string message = string.Format("{0} wins!", turnX ? "O" : "X");
                    MessageBox.Show(message);
                    if (turnX) score[1]++; else score[0]++;
                }
                return true;
            }
            return false;
        }
    }
}

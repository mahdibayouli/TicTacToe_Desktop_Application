using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameLibrary;

namespace TicTacToeWPF1
{
    public partial class MainWindow : Window
    {
        static readonly Game game = new();
        static Button[] boardButtons = new Button[9];
        static Button[] scoreButtons = new Button[2];
        public MainWindow()
        {
            InitializeComponent();
            game.GameMode = GameMode.NOTSELECTED;
            boardButtons = Board.Children.Cast<Button>().ToArray();
            scoreButtons = XO.Children.Cast<Button>().ToArray();
            NewGame();
        }
        static void NewGame()
        {
            for (int i = 0; i < game.board.Length; i++) game.board[i] = 0;      //emptying the board
            UpdateBoard();          
            game.XIsPlaying = true; //first player is always X         

            //if the mode is singlePlayer and the player chose O, then the computer makes the first move
            if ((game.SinglePlayerXorO == GameLibrary.XO.O) && ((game.GameMode == GameMode.EASY) || (game.GameMode == GameMode.HARD)))
            {
                game.ComputerTurn();
                UpdateBoard();
            }
        }

        static void UpdateBoard()
        {
            for (int i = 0; i < game.board.Length; i++)
                switch (game.board[i])
                {
                    case 1:
                        boardButtons[i].Content = "X";
                        break;
                    case 2:
                        boardButtons[i].Content = "O";
                        break;
                    case 0:
                        boardButtons[i].Content = "";
                        break;
                }
        }
        
        static void UpdateScore(bool restart = false)
        {
            if (restart) //this executes when the user clicks the new game button
                for (int i = 0; i < game.score.Length; i++)
                    game.score[i] = 0;

            scoreButtons[0].Content = string.Format("{0} : {1}", "X", game.score[0]);
            scoreButtons[1].Content = string.Format("{0} : {1}", "O", game.score[1]);
        }

        static bool CheckWinner()
        {
            if (game.LastPlayerWon() || game.BoardIsFull())
            {
                UpdateScore();
                NewGame();
                return true;
            }
            return false;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //***** Game Mode must be set before the game, if it's singleplayer the player must chose X or O
            if (game.GameMode == GameMode.NOTSELECTED)
            {
                MessageBox.Show("Please select the game mode to start playing");
                return;
            }
            if ((game.GameMode != GameMode.MULTPILAYER) && (game.SinglePlayerXorO == GameLibrary.XO.NOTSELECTED))
            {
                MessageBox.Show("Please select X or O to start playing");
                return;
            }

            
            int index = Grid.GetRow((Button)sender) * 3 + Grid.GetColumn((Button)sender);
            if (game.board[index] != 0) return; //the player can only chose an empty field, otherwise nothing happens
            game.board[index] = (game.XIsPlaying) ? 1 : 2;

            UpdateBoard();
            game.XIsPlaying = !game.XIsPlaying; //moving to the next turn
            if (CheckWinner()) return;          //if there's is a winner the game ends

            //Computer Turn if it's singleplayer
            if (game.GameMode != GameMode.MULTPILAYER)
            {
                game.ComputerTurn();
                UpdateBoard();
                if (CheckWinner()) return;
            }
        }

        private void RestartGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void ResetScore_Click(object sender, RoutedEventArgs e)
        {
            UpdateScore(true); //true: restart game => score[i] = 0;
        }

        private void X_Click(object sender, RoutedEventArgs e)
        {
            game.SinglePlayerXorO = GameLibrary.XO.X;           //singleplayer is now playing with X
            ((Button)sender).Background = Brushes.Turquoise;    //the choice's background is colored, the computer's one is grayed out 
            scoreButtons[1].Background = Brushes.Gray;
            NewGame();                                          //after the choice is made, a new game is started
        }

        private void O_Click(object sender, RoutedEventArgs e)
        {
            game.SinglePlayerXorO = GameLibrary.XO.O;           //singleplayer is now playing with O
            ((Button)sender).Background = Brushes.Turquoise;    //the choice's background is colored, the computer's one is grayed out 
            scoreButtons[0].Background = Brushes.Gray;
            NewGame();                                          //after the choice is made, a new game is started
        }

        private void Multiplayer_Selected(object sender, RoutedEventArgs e)
        {
            //game mode is set to multiplayer, and singleplayer's choice (X or O) is now NOTSELECTED
            game.SinglePlayerXorO = GameLibrary.XO.NOTSELECTED;
            game.GameMode = GameMode.MULTPILAYER;
            ((ComboBoxItem)sender).IsSelected = true;

            //**+ X and O buttons are disabled and grayed out
            scoreButtons[0].IsEnabled = false;
            scoreButtons[1].IsEnabled = false;
            scoreButtons[0].Background = Brushes.Gray;
            scoreButtons[1].Background = Brushes.Gray;

            //*** after choosing multiplayer mode, a new game is started
            NewGame();
        }

        private void Easy_Selected(object sender, RoutedEventArgs e)
        {
            game.GameMode = GameMode.EASY;              //game mode is set to easy
            ((ComboBoxItem)sender).IsSelected = true;   
            scoreButtons[0].IsEnabled = true;           //X and O buttons are now enabled (player has to choose)
            scoreButtons[1].IsEnabled = true;           
            // after choosing the easy mode, a new game is started
            NewGame();

        }

        private void Hard_Selected(object sender, RoutedEventArgs e)
        {
            game.GameMode = GameMode.HARD;              //game mode is set to hard
            ComboBoxItem box = (ComboBoxItem)sender;
            box.IsSelected = true;
            scoreButtons[0].IsEnabled = true;           //X and O buttons are now enabled (player has to choose)
            scoreButtons[1].IsEnabled = true;

            // after choosing the easy mode, a new game is started
            NewGame();
        }
    }

}

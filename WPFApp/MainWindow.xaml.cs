using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameLibrary;

namespace TicTacToeWPF1
{
    public partial class MainWindow : Window
    {
        static readonly Game game = new Game();
        static Button[] boardButtons = new Button[9];
        static Button[] scoreButtons = new Button[2];

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
            if (restart)
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

        static void NewGame()
        {
            for (int i = 0; i < game.board.Length; i++) game.board[i] = 0;
            game.XIsPlaying = true;
            foreach (var button in boardButtons) button.Content = "";

            if ((game.SinglePlayerXorO == GameLibrary.XO.O) && ((game.GameMode == GameMode.EASY) || (game.GameMode == GameMode.HARD)))
            {
                int choice = game.ComputerTurn();
                UpdateBoard();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            game.GameMode = GameMode.NOTSELECTED;
            boardButtons = Board.Children.Cast<Button>().ToArray();
            scoreButtons = XO.Children.Cast<Button>().ToArray();
            NewGame();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
            if (game.board[index] != 0) return;
            game.board[index] = (game.XIsPlaying) ? 1 : 2;

            UpdateBoard();
            game.XIsPlaying = !game.XIsPlaying;
            if (CheckWinner()) return;

            //Computer Turn
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
            game.SinglePlayerXorO = GameLibrary.XO.X;
            ((Button)sender).Background = Brushes.Turquoise;
            scoreButtons[1].Background = Brushes.Gray;
            NewGame();
        }

        private void O_Click(object sender, RoutedEventArgs e)
        {
            game.SinglePlayerXorO = GameLibrary.XO.O;
            Button button = (Button)sender;
            button.Background = Brushes.Turquoise;
            scoreButtons[0].Background = Brushes.Gray;
            NewGame();
        }

        private void Multiplayer_Selected(object sender, RoutedEventArgs e)
        {
            game.GameMode = GameMode.MULTPILAYER;
            game.SinglePlayerXorO = GameLibrary.XO.NOTSELECTED;
            ComboBoxItem box = (ComboBoxItem)sender;
            box.IsSelected = true;
            scoreButtons[0].IsEnabled = false;
            scoreButtons[1].IsEnabled = false;
            scoreButtons[0].Background = Brushes.Gray;
            scoreButtons[1].Background = Brushes.Gray;
            NewGame();
        }

        private void Easy_Selected(object sender, RoutedEventArgs e)
        {
            game.GameMode = GameMode.EASY;
            ComboBoxItem box = (ComboBoxItem)sender;
            box.IsSelected = true;
            scoreButtons[0].IsEnabled = true;
            scoreButtons[1].IsEnabled = true;
            NewGame();

        }

        private void Hard_Selected(object sender, RoutedEventArgs e)
        {
            game.GameMode = GameMode.HARD;
            ComboBoxItem box = (ComboBoxItem)sender;
            box.IsSelected = true;
            scoreButtons[0].IsEnabled = true;
            scoreButtons[1].IsEnabled = true;
            NewGame();
        }
    }

}

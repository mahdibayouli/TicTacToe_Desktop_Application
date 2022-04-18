using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameLibrary;

namespace TicTacToeWPF1
{
    public partial class MainWindow : Window
    {
        static Game game = new Game();
        static Button[] buttons = new Button[9];
        static Button[] XOscore = new Button[2];
        static bool gameModeSelected = false;
        static bool XOselected = false;
        static bool multiplayerSelected = false;

        void updateBoard()
        {
            for (int i = 0; i < game.board.Length; i++)
                switch (game.board[i])
                {
                    case 1:
                        buttons[i].Content = "X";
                        break;
                    case 2:
                        buttons[i].Content = "O";
                        break;
                    case 0:
                        buttons[i].Content = "";
                        break;
                }
        }
        void updateScore(bool restart = false)
        {
            if (restart)
                for (int i = 0; i < game.score.Length; i++)
                    game.score[i] = 0;

            XOscore[0].Content = string.Format("{0} : {1}", "X", game.score[0]);
            XOscore[1].Content = string.Format("{0} : {1}", "O", game.score[1]);
        }

        bool checkWinner()
        {
            if (game.lastPlayerWon() || game.boardIsFull())
            {
                updateScore();
                newGame();
                return true;
            }
            return false;
        }

        void newGame()
        {
            for (int i = 0; i < game.board.Length; i++) game.board[i] = 0;
            game.turnX = true;
            foreach (var button in buttons) button.Content = "";

            if ((!game.singleX) && gameModeSelected && (!multiplayerSelected) && XOselected)
            {
                int choice = game.computerTurn();
                updateBoard();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            gameModeSelected = false;
            buttons = Board.Children.Cast<Button>().ToArray();
            XOscore = XO.Children.Cast<Button>().ToArray();
            newGame();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!gameModeSelected)
            {
                MessageBox.Show("Please select the game mode to start playing");
                return;
            }
            if (!multiplayerSelected && !XOselected)
            {
                MessageBox.Show("Please select X or O to start playing");
                return;
            }

            int index = Grid.GetRow((Button)sender) * 3 + Grid.GetColumn((Button)sender);
            if (game.board[index] != 0) return;
            game.board[index] = (game.turnX) ? 1 : 2;

            updateBoard();
            game.turnX = !game.turnX;
            if (checkWinner()) return;

            //Computer Turn
            if (!multiplayerSelected)
            {
                game.computerTurn();
                updateBoard();
                if (checkWinner()) return;
            }
        }

        private void RestartGame_Click(object sender, RoutedEventArgs e)
        {
            newGame();
        }

        private void ResetScore_Click(object sender, RoutedEventArgs e)
        {
            updateScore(true); //true: restart game => score[i] = 0;
        }

        private void X_Click(object sender, RoutedEventArgs e)
        {
            XOselected = true;
            game.singleX = true;
            ((Button)sender).Background = Brushes.Turquoise;
            XOscore[1].Background = Brushes.Gray;
            newGame();
        }

        private void O_Click(object sender, RoutedEventArgs e)
        {
            XOselected = true;
            Button button = (Button)sender;
            game.singleX = false;
            button.Background = Brushes.Turquoise;
            XOscore[0].Background = Brushes.Gray;
            newGame();
        }

        private void Multiplayer_Selected(object sender, RoutedEventArgs e)
        {
            gameModeSelected = true;
            multiplayerSelected = true;
            XOselected = false;
            ComboBoxItem box = (ComboBoxItem)sender;
            box.IsSelected = true;
            game.singlePlayer = false;
            XOscore[0].IsEnabled = false;
            XOscore[1].IsEnabled = false;
            XOscore[0].Background = Brushes.Gray;
            XOscore[1].Background = Brushes.Gray;
            newGame();
        }

        private void Easy_Selected(object sender, RoutedEventArgs e)
        {
            gameModeSelected = true;
            multiplayerSelected = false;
            ComboBoxItem box = (ComboBoxItem)sender;
            box.IsSelected = true;
            game.hardMode = false;
            XOscore[0].IsEnabled = true;
            XOscore[1].IsEnabled = true;
            newGame();

        }

        private void Hard_Selected(object sender, RoutedEventArgs e)
        {
            gameModeSelected = true;
            multiplayerSelected = false;
            ComboBoxItem box = (ComboBoxItem)sender;
            box.IsSelected = true;
            game.hardMode = true;
            XOscore[0].IsEnabled = true;
            XOscore[1].IsEnabled = true;
            newGame();
        }
    }
}

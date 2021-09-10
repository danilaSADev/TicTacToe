using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;

namespace TicTacToe
{

    public partial class MainPage : ContentPage
    {
        private bool isGameRunning = true;
        private int[,] winningCombinations = new int[8, 3]
        {
            {0, 1, 2},
            {3, 4, 5},
            {6, 7, 8},
            {0, 3, 6},
            {1, 4, 7},
            {2, 5, 8},
            {0, 4, 8},
            {2, 4, 6},
        };
        private int[] gameboard = new int[9]; 
        private int step = 0;
        private char[] symbols = { 'X', 'O' };
        public ICommand SelectCell { get; }
        public ICommand ResetGame { get; }
        public MainPage()
        {
            SelectCell = new Command<Tuple<Button, int>>(OnSelectCell);
            ResetGame = new Command(OnResetGame);
            
            InitializeComponent();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    Button button = new Button()
                    {
                        Command = SelectCell,
                        //FontAttributes = FontAttributes.Bold,
                        FontSize = 24,
                    };
                    button.CommandParameter = Tuple.Create(button, x * 3 + y);
                    GameBoard.Children.Add(button, y, x);
                }
            }
        }
        private void AnnounceDraw()
        {
            isGameRunning = false;
            // outputs a draw text.
            string drawText = $"Draw! Restart your game!";
            // assign text to the label.
            winnerLabel.Text = drawText;

        }
        private void FinishGame()
        {
            isGameRunning = false;
            // Outputs a winner.
            string winnerText = $"And the winner is player '{symbols[step % 2]}'";
            // assign text to the label.
            winnerLabel.Text = winnerText;
        }
        private bool CheckWinningCombinations()
        {
            bool isWinningCombination = false;
            for (int i = 0; i < winningCombinations.GetLength(0); i++)
            {
                for (int j = 0; j < winningCombinations.GetLength(1); j++)
                {
                    isWinningCombination = gameboard[winningCombinations[i, j]] == symbols[step % 2];
                    if (!isWinningCombination) break;
                }
                if (isWinningCombination)
                {
                    return isWinningCombination;
                }
            }
            return false;
        }
        private void OnResetGame()
        {
            // reset game board
            gameboard = new int[9];
            // reset buttons 
            foreach (Button button in GameBoard.Children)
            {
                button.Text = "";
            }
            // reset label text;
            winnerLabel.Text = "";

            step = 0;
            isGameRunning = true;
        }
        private bool TryMakeStep(int cellIndex)
        {
            if (gameboard[cellIndex] != 0) return false;

            gameboard[cellIndex] = symbols[step % 2];

            bool isWinner = CheckWinningCombinations();

            if (isWinner)
            {
                FinishGame();
            }

            return true;
        }
        private void OnSelectCell(Tuple<Button, int> parametersTuple)
        {
            if (!isGameRunning) return;
            if (step >= 8)
            {
                AnnounceDraw();
            }
            bool isStepSuccesful = TryMakeStep(parametersTuple.Item2);

            if (!isStepSuccesful) return;
            //winnerLabel.Text = parametersTuple.Item2.ToString();
            parametersTuple.Item1.Text = symbols[step % 2].ToString();
            step++;
        }

        private void _OnResetGame(object sender, EventArgs e)
        {
            ResetGame.Execute(sender);
        }
    }
}

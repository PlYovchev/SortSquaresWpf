using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace SortSquares
{
    public partial class MainWindow : Window
    {
        enum Direction 
        {
            Up, Down, Left, Right, None
        }

        private readonly int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        private bool isGameOver = false;

        public Button[,] Buttons { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Buttons = new Button[4, 4];
            Buttons[0, 0] = this.btnOne;
            Buttons[0, 1] = this.btnTwo;
            Buttons[0, 2] = this.btnThree;
            Buttons[0, 3] = this.btnFour;
            Buttons[1, 0] = this.btnFive;
            Buttons[1, 1] = this.btnSix;
            Buttons[1, 2] = this.btnSeven;
            Buttons[1, 3] = this.btnEight;
            Buttons[2, 0] = this.btnNine;
            Buttons[2, 1] = this.btnTen;
            Buttons[2, 2] = this.btnEleven;
            Buttons[2, 3] = this.btnTwelve;
            Buttons[3, 0] = this.btnThirteen;
            Buttons[3, 1] = this.btnFourteen;
            Buttons[3, 2] = this.btnFifteen;
            Buttons[3, 3] = this.btnSixteen;

            Shuffle();

            SetClickListeners(Buttons);
        }

        private void Shuffle()
        {
            Random rnd = new Random();

            List<int> numbersTempList = new List<int>(numbers);
            for (int i = 0; i < Buttons.GetLength(0); i++)
            {
                for (int j = 0; j < Buttons.GetLength(1); j++)
                {
                    if (numbersTempList.Count != 0)
                    {
                        int randomIndex = rnd.Next(0, numbersTempList.Count);
                        Buttons[i, j].Content = numbersTempList[randomIndex];
                        Buttons[i, j].Visibility = Visibility.Visible;
                        numbersTempList.RemoveAt(randomIndex);
                    }
                    else
                    {
                        Buttons[i, j].Visibility = Visibility.Hidden;
                        Console.WriteLine(Buttons[i, j].Name + " " + numbersTempList.Count);
                    }
                }
            }

            if (CheckAreButtonsSorted())
            {
                Shuffle();
            }
        }

        private void SetClickListeners(Button[,] Buttons)
        {
            for (int i = 0; i < Buttons.GetLength(0); i++)
            {
                for (int j = 0; j < Buttons.GetLength(1); j++)
                {
                    Buttons[i, j].Click += OnButtonClick;
                }
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button btnSender = sender as Button;
            if (btnSender != null)
            {
                if (!isGameOver)
                {
                    int[] indexes = FindButtonIndexes(btnSender.Name);
                    Direction directionToMove = FindDirection(indexes);
                    if (directionToMove != Direction.None)
                    {
                        PerformTheMoveAction(indexes, directionToMove);
                        if (CheckAreButtonsSorted())
                        {
                            MessageBox.Show("Numbers are sorted!");
                            isGameOver = true;
                        }
                    }
                }
            }
        }

        public int[] FindButtonIndexes(string name)
        {
            for (int i = 0; i < Buttons.GetLength(0); i++)
            {
                for (int j = 0; j < Buttons.GetLength(1); j++)
                {
                    if (Buttons[i, j].Name == name)
                    {
                        int[] indexes = { i, j };
                        return indexes;
                    }
                }
            }

            return null;
        }

        private Direction FindDirection(int[] indexes)
        {
            if(indexes.Length < 2){
                throw new Exception("Something went wrong with finding the correct indexes of the clicked button!!!");
            }

            int row = indexes[0];
            int col = indexes[1];
            if (row > 0 && Buttons[row - 1, col].Visibility == Visibility.Hidden)
            {
                return Direction.Up;
            }
            else if (row < (Buttons.GetLength(0) - 1) && Buttons[row + 1, col].Visibility == Visibility.Hidden)
            {
                return Direction.Down;
            }
            else if (col > 0 && Buttons[row, col - 1].Visibility == Visibility.Hidden)
            {
                return Direction.Left;
            }
            else if (col < (Buttons.GetLength(1) - 1) && Buttons[row, col + 1].Visibility == Visibility.Hidden)
            {
                return Direction.Right;
            }

            return Direction.None;
        }

        private void PerformTheMoveAction(int[] indexes, Direction direction)
        {
            if(indexes.Length < 2){
                throw new Exception("Something went wrong with finding the correct indexes of the clicked button!!!");
            }
            int[] newIndexesForButton = new int[2];

            switch (direction)
            {
                case Direction.Up:
                    newIndexesForButton[0] = indexes[0] - 1;
                    newIndexesForButton[1] = indexes[1];
                    break;
                case Direction.Down:
                    newIndexesForButton[0] = indexes[0] + 1;
                    newIndexesForButton[1] = indexes[1];
                    break;
                case Direction.Left:
                    newIndexesForButton[0] = indexes[0];
                    newIndexesForButton[1] = indexes[1] - 1;
                    break;
                case Direction.Right:
                    newIndexesForButton[0] = indexes[0];
                    newIndexesForButton[1] = indexes[1] + 1;
                    break;
            }

            SwitchButtons(indexes, newIndexesForButton);
        }

        private void SwitchButtons(int[] initialIndexes, int[] newIndexes)
        {
            int initialRow = initialIndexes[0];
            int initialCol = initialIndexes[1];
            int toBeRow = newIndexes[0];
            int toBeCol = newIndexes[1];

            Buttons[toBeRow, toBeCol].Content = Buttons[initialRow, initialCol].Content;
            Buttons[toBeRow, toBeCol].Visibility = Visibility.Visible;
            Buttons[initialRow, initialCol].Visibility = Visibility.Hidden;
        }

        private bool CheckAreButtonsSorted()
        {
            int currentNumber = numbers.Min();
            for (int i = 0; i < Buttons.GetLength(0); i++)
            {
                for (int j = 0; j < Buttons.GetLength(1); j++)
                {
                    if (Buttons[i, j].Visibility == Visibility.Hidden)
                    {
                        continue;
                    }

                    if (Buttons[i, j].Content.Equals(currentNumber.ToString()))
                    {
                        currentNumber++;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void OnShuffleClick(object sender, RoutedEventArgs e)
        {
            Shuffle();
            isGameOver = false;
        }
    }
}
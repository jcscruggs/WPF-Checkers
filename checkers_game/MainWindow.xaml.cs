using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace checkers_game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private variables
        // holds the current values inside of each white tile on the board
        private CheckerType [,] Board_array;

        /// <summary>
        /// this variable holds a boolean value for player one.
        /// True if player one is currently playing 
        /// false other wise
        /// this variable is toggled within the button method 
        /// </summary>
        private bool player_one_turn;

        // variable will be toggled to true if the players first click is a button containing a checker belonging to them
        private bool players_second_click;

        // variable is toggled on if one of the players checkers are all gone
        private bool game_ended;

        // contains a string which is the name of the winner "player 1" or "player 2"
        private string winner;

        private bool playeronewin;

        // iterable type of buttons

        private List<Button> buttonList;

        // hold reference to previous button pressed so it can be updated if a valid move is made

        private Button prevButton;

        private int row, column, prevRow, prevCol;

        #endregion



        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame()
        {
            // get list of button for later accessing
            buttonList = Board.Children.Cast<Button>().ToList();
            
            // create blank board array with 64 tiles. this includes black and white tiles.

            Board_array = new CheckerType[8,8];

            // set the pieces for the board 
            for(int row = 0; row < 8; row++)
            {
                if (row == 0 || row == 2 || row == 6) // if the row is either 0, 2, or 6 
                {

                    for (int col = 0; col < 7; col += 2 ) { // place chips on even tiles starting with 0

                        if (row == 0 || row == 2) { Board_array[row, col] = CheckerType.P2_check; } // row 0 and 2 are filled with player 2 checkers

                        // row 6 is filled with player one checkers
                        else { Board_array[row, col] = CheckerType.P1_check; }

                    }
                   
                }

                if (row == 1 || row == 5 || row == 7) {

                    for (int col = 1; col < 8; col += 2)
                    { // place chips on odd tiles starting with 1

                        if (row == 5|| row == 7) { Board_array[row, col] = CheckerType.P1_check; } // row 5 and 7 are filled with player 1 checkers

                        // row 1 is filled with player 2 checkers
                        else { Board_array[row, col] = CheckerType.P2_check; }

                    }

                }
            } // end of building board

            // intialize each of the private variables
            game_ended = false;
            winner = "";
            player_one_turn = true; // player one is current player
            players_second_click = false;
            row = -1;
            column = 0;
            prevRow = 0;
            prevCol = 0;


            int counter = 0;

            // loop through each button and set to intial setup
            buttonList.ForEach(button =>

            // for player 2 buttons. fill with checkers images (top three rows of the board)
            { if (counter < 12) 
                {
                    button.Content = "•";
                    button.Foreground = Brushes.Violet;
                    counter++;
                }
                // for player 1 buttons. fill with checkers images (bottom three rows of the board)
                else if (counter >= 20 && counter < 32)
                {
                    button.Content = "•";
                    button.Foreground = Brushes.Gold;
                    counter++;
                }
                else {
                    button.Content = string.Empty;
                    counter++; }  // reset button to empty 
            }
            );

            

        } // end new game method


        /* helper functions
         * 
         * 
         * 
         */

        private void borderChangeOnCLick(Button button)
        {
            // when button is clicked the border is changed to help the user see which button they clicked
          button.BorderThickness = new Thickness(3,3,3,3);
            button.BorderBrush = Brushes.Snow;
        }

        private void borderChangeBack(Button button)
        {

            // reset button thickness and color
            button.BorderThickness = new Thickness(1, 1, 1, 1);
            button.BorderBrush = Brushes.SlateGray;
        }

        private void updateBoardGui()
        {

            /*
             * 
             * used to update game board gui when jumping checkers. 
             */
            buttonList.ForEach(button => {

                int row = Grid.GetRow(button);
                int col = Grid.GetColumn(button);

                if (Board_array[row,col] == CheckerType.P1_check)
                {
                    button.Content = "•";
                    button.Foreground = Brushes.Gold;
                }
                else if(Board_array[row,col] == CheckerType.P1_king)
                {
                    button.Content = "♛";
                    button.Foreground = Brushes.Gold;
                }
                else if(Board_array[row,col] == CheckerType.P2_check)
                {
                    button.Content = "•";
                    button.Foreground = Brushes.Violet;
                }
                else if(Board_array[row,col] == CheckerType.P2_king)
                {
                    button.Content = "♚";
                    button.Foreground = Brushes.Violet;
                }
                else
                {
                    button.Content = "";
                }
                

            });
        }

        /* create helper function here to check for further jumps in board array for player 
         * 
         * return will be boolean for if another jump is avaiablle
         * 
         * if return is true then perform actions within ccurrent conditional statements */


        private bool p1_jump_available( int row, int col )
        {
            if (row - 2 >= 0 && col - 2 >= 0 && Board_array[row - 2, col - 2] == CheckerType.Free && (Board_array[row - 1, col - 1] == CheckerType.P2_check || Board_array[row - 1, col - 1] == CheckerType.P2_king))
            {
                // check if another move can be made by moving left 2 and up 2 more spaces to allow for chain jumps
                return true;

            }
            else if (row - 2 >= 0 && col + 2 <= 7 && Board_array[row - 2, col + 2] == CheckerType.Free && (Board_array[row - 1, col + 1] == CheckerType.P2_check || Board_array[row - 1, col + 1] == CheckerType.P2_king))
            {

                // check if another move can be made by moving right 2 and up 2 more spaces to allow for chain jumps
                return true;
            }
            else
            {
                // no more valid jumps could be made so players turn is over
                return false;
            }
        }

        private bool p2_jump_available(int row, int col)
        {

            if (row + 2 <= 7 && col + 2 <= 7 && Board_array[row + 2, col + 2] == CheckerType.Free && (Board_array[row + 1, col + 1] == CheckerType.P1_check || Board_array[row + 1, col  +1] == CheckerType.P1_king))
            {
                // check if another move can be made by moving right 2 and down 2 more spaces
                return true;
            }
            else if (row + 2 <= 7 && col - 2 >= 0 && Board_array[row + 2, col - 2] == CheckerType.Free && (Board_array[row + 1, col -1] == CheckerType.P1_check || Board_array[row + 1, col - 1] == CheckerType.P1_king))
            {
                // check if another move can be made by moving left 2 and down 2 more spaces
                return true;
            }
            else
            {
                // no more valid jumps could be made
                return false;
            }


        }

        private bool Is_kinged(int row, int col, int prevRow, int prevCol)
        {
            // player one has reached the top and needs to be kinged
            if(row == 0)
            {
                Board_array[row, col] = CheckerType.P1_king;
                Board_array[prevRow,prevCol] = CheckerType.Free;
               
                updateBoardGui();
                return true;
                
            }
            else if(row == 7)
            {
                // player two has reached the bottom and needs to be kinged
                Board_array[row, col] = CheckerType.P2_king;
                Board_array[prevRow, prevCol] = CheckerType.Free;
                updateBoardGui();
                return true;
            } 
            else
            {
                // the checker is not kinged
                return false;
            }
        }

        private void End_turn()
        {
            Is_kinged(row, column, prevRow, prevCol);
            players_second_click = !players_second_click;
            player_one_turn = !player_one_turn;
        }

        private void invalid_choice()
        {
            players_second_click = false;
            borderChangeBack(prevButton);
        }


        /*
         *
         * event handler for button press
         */
        private void Button_Click(object sender, RoutedEventArgs e) {

           
            
            // if the game has ended, clicking any button will reset the board to play again
            if (game_ended)
            {
                playeronewin = true;
                return;
            }

           

            

            var button = (Button)sender;
           

           

            // get row and column of the button pressed so that it can be found within the Board_array and logic can be applied
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);
            
           
            

            // currently player ones turn
            if (player_one_turn)
            {
                // player has clicked on piece belonging to them and wants to move it
                if (players_second_click)
                {
                    System.Console.WriteLine(row + "," + column + "previous " + Grid.GetRow(prevButton) + "," + Grid.GetColumn(prevButton));


                    // check if piece is a valid movement from the first. also check if it is open
                    // if a piece is there then check if it is a enemies piece

                    // if invalid movement then flip boolean to go back to choosing first checker to move

                    // if a piece is jumped then a new check needs to be made to see if another jump can be made to allow for multijump moves
                     prevRow = Grid.GetRow(prevButton);
                     prevCol = Grid.GetColumn(prevButton);
                    if (Board_array[prevRow, prevCol] == CheckerType.P1_check)
                    {
                        // the piece is a normal check

                        if (Board_array[row, column] == CheckerType.Free && (row - prevRow == -1) && (column - prevCol == -1 || column - prevCol == 1))
                        {

                            if (!Is_kinged(row, column, prevRow, prevCol))
                            {


                                // the space is free and is a valid movement and not a kinged move
                                Board_array[row, column] = CheckerType.P1_check;
                                Board_array[prevRow, prevCol] = CheckerType.Free;
                                button.Content = "•";
                                button.Foreground = Brushes.Gold;
                                borderChangeBack(prevButton);
                                prevButton.Content = "";

                            }

                            // end the turn for player one after valid move
                            End_turn();
                            borderChangeBack(prevButton);

                        }
                        else if (Board_array[row, column] == CheckerType.Free && (row - prevRow == -2) && (column - prevCol == -2 ))
                        {
                            // if an enemy check is open to be jumped
                            if (Board_array[row +1,column +1] == CheckerType.P2_check || Board_array[row + 1, column + 1] == CheckerType.P2_king)
                            {

                                // jump enemy checker
                                Board_array[row, column] = CheckerType.P1_check;
                                Board_array[row + 1, column + 1] = CheckerType.Free;
                                
                                Board_array[prevRow, prevCol] = CheckerType.Free;

                                // reset the button border and update the game board
                                borderChangeBack(prevButton);
                                updateBoardGui(); 

                                
                                if(p1_jump_available(row,column))
                                {
                                    // check if another jump can be made and if so then make current button the previous button and let player go again
                                    prevButton = button;
                                    borderChangeOnCLick(button);

                                }
                                else
                                {
                                    // no more valid jumps could be made so players turn is over
                                  
                                    End_turn();
                                    borderChangeBack(prevButton);
                                }
                            }
                        }
                        else if (Board_array[row, column] == CheckerType.Free && (row - prevRow == -2) && (column - prevCol == 2))
                        {

                            if(Board_array[row + 1, column - 1] == CheckerType.P2_check || Board_array[row + 1, column - 1] == CheckerType.P2_king)
                            {
                                // jump enemy checker from the board
                                Board_array[row, column] = CheckerType.P1_check;
                                Board_array[row + 1, column - 1] = CheckerType.Free;
                                
                                Board_array[prevRow, prevCol] = CheckerType.Free;

                                // reset the button border and update the game board
                                borderChangeBack(prevButton);
                                updateBoardGui();

                                if (p1_jump_available(row,column))
                                {
                                    // check if another jump can be made and if so then make current button the previous button and let player go again
                                    prevButton = button;
                                    borderChangeOnCLick(button);

                                }
                                else
                                {
                                    // no more valid jumps could be made so players turn is over
                                    End_turn();
                                    borderChangeBack(prevButton);
                                }


                            }

                        }else
                        {
                            // no valid button was chosen so reset turn
                            invalid_choice();
                        }
                    }
                    else { 
                        // the piece is a king check
                    }
                    


                }
                // this is the players first click

                else {

                    // if the button clicked is owned by player 1 check or king then allow for movement
                    if (Board_array[row, column] == CheckerType.P1_check || Board_array[row, column] == CheckerType.P1_king)
                    {
                        prevButton = button; // save the current button so it can be accessed later
                        borderChangeOnCLick(button);
                        players_second_click = true;
                    }

                    
                }
            }
            // player twos turn
            else {



                // player has clicked on piece belonging to them and wants to move it
                if (players_second_click)
                {
                     prevRow = Grid.GetRow(prevButton);
                     prevCol = Grid.GetColumn(prevButton);
                    if (Board_array[prevRow, prevCol] == CheckerType.P2_check)
                    {
                        if (Board_array[row, column] == CheckerType.Free && (row - prevRow == 1) && (column - prevCol == -1 || column - prevCol == 1))
                        {
                            if (!Is_kinged(row, column, prevRow, prevCol))
                            {
                                Board_array[row, column] = CheckerType.P2_check;

                                Board_array[prevRow, prevCol] = CheckerType.Free;

                                button.Content = "•";
                                button.Foreground = Brushes.Violet;
                                borderChangeBack(prevButton);
                                prevButton.Content = "";
                            }
                            
                            // end player twos turn after valid move
                            End_turn();
                            borderChangeBack(prevButton);
                        }
                        else if (Board_array[row,column] == CheckerType.Free && (row - prevRow == 2) && column - prevCol == - 2)
                        {
                            // the check in between previous button and current button is a player 1 check or king 
                            if(Board_array[row - 1, column + 1] == CheckerType.P1_check || Board_array[row - 1, column + 1] == CheckerType.P1_king)
                            {

                                Board_array[row, column] = CheckerType.P2_check;
                                Board_array[row - 1, column + 1] = CheckerType.Free;
                                Board_array[prevRow, prevCol] = CheckerType.Free;


                                borderChangeBack(prevButton);
                                updateBoardGui();

                                if (p2_jump_available(row, column))
                                {
                                    borderChangeOnCLick(button);
                                    prevButton = button;
                                }
                                else
                                {
                                    End_turn();
                                    borderChangeBack(prevButton);
                                }

                            }
                        }
                        else if (Board_array[row, column] == CheckerType.Free && (row - prevRow == 2) && column - prevCol == 2)
                        {

                            Board_array[row, column] = CheckerType.P2_check;
                            Board_array[row - 1, column - 1] = CheckerType.Free;
                            Board_array[prevRow, prevCol] = CheckerType.Free;


                            borderChangeBack(prevButton);
                            updateBoardGui();

                            if (p2_jump_available(row, column))
                            {
                                borderChangeOnCLick(button);
                                prevButton = button;
                            }
                            else
                            {
                                End_turn();
                                borderChangeBack(prevButton);
                            }

                        }
                        else
                        {
                            // no valid button was chosen so reset turn
                            invalid_choice();
                        }
                    }
                    else
                    {
                        // piece chosen was a P2 king 
                    }
                }
               // this is the players first click
                else
                {


                  // if the button clicked is owned by player 2 check or king then allow for movement
                  if (Board_array[row, column] == CheckerType.P2_check || Board_array[row, column] == CheckerType.P2_king)
                  {
                      prevButton = button;  // save the current button so it can be accessed in the second click
                      players_second_click = true;
                      borderChangeOnCLick(button);
                  }
                }
                

            }

            
         

        } // end button_clicked method
    }
}

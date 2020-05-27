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
        // creates a 2D array that holds the current placement of all checks on the board. this is used to validate player moves
        private CheckerType [,] Board_array;

        // holds boolean variable to keep track of which player is currently playing
        private bool player_one_turn;

        // variable will be toggled to true if the players first click is a button containing a checker belonging to them
        private bool players_second_click;

        // iterable type of buttons

        private List<Button> buttonList;

        // hold reference to previous button pressed so it can be updated if a valid move is made

        private Button prevButton;

        // Grid row and column of current button and previous button pressed. 

        private int row, column, prevRow, prevCol;

        private int p1_check_count, p2_check_count;

        // variables for color of checkers
        private Brush p1_color;
        private Brush p2_color;

        #endregion



        public MainWindow(Brush p1_color, Brush p2_color)
        {
            InitializeComponent();
            this.p1_color = p1_color;
            this.p2_color = p2_color;

            
            p1_identifier.Foreground = p1_color;
            p2_identifier.Foreground = p2_color;

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
            
            player_one_turn = true; // player one is current player
            players_second_click = false;
            row = -1;
            column = 0;
            prevRow = 0;
            prevCol = 0;

            
            p1_check_count = 12;
            p2_check_count = 12;


            int counter = 0;

            // loop through each button and set to intial setup using lambda function
            buttonList.ForEach(button =>

            // for player 2 buttons. fill with checkers images (top three rows of the board)
            { if (counter < 12) 
                {
                    button.Content = "•";
                    button.Foreground = p2_color;
                    counter++;
                }
                // for player 1 buttons. fill with checkers images (bottom three rows of the board)
                else if (counter >= 20 && counter < 32)
                {
                    button.Content = "•";
                    button.Foreground = p1_color;
                    counter++;
                }
                else {
                    button.Content = string.Empty;
                    counter++; }  // reset button to empty 
            }
            );

            

        } // end new game method


        /*
         * 
         * ********************helper functions below*******************
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
                    button.Foreground = p1_color;
                }
                else if(Board_array[row,col] == CheckerType.P1_king)
                {
                    button.Content = "♛";
                    button.Foreground = p1_color;
                }
                else if(Board_array[row,col] == CheckerType.P2_check)
                {
                    button.Content = "•";
                    button.Foreground = p2_color;
                }
                else if(Board_array[row,col] == CheckerType.P2_king)
                {
                    button.Content = "♚";
                    button.Foreground = p2_color;
                }
                else
                {
                    button.Content = "";
                }
                

            });
        }

    
       
        // function that determines if more jumps are available after the intial jump. 
        // will return true if any are found and returns false otherwise
        private bool p1_jump_available(  )
        {
            if (row - 2 >= 0 && column - 2 >= 0 && Board_array[row - 2, column - 2] == CheckerType.Free && (Board_array[row - 1, column - 1] == CheckerType.P2_check || Board_array[row - 1, column - 1] == CheckerType.P2_king))
            {
                // check if another move can be made by moving left 2 and up 2 more spaces to allow for chain jumps
                return true;

            }
            else if (row - 2 >= 0 && column + 2 <= 7 && Board_array[row - 2, column + 2] == CheckerType.Free && (Board_array[row - 1, column + 1] == CheckerType.P2_check || Board_array[row - 1, column + 1] == CheckerType.P2_king))
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


        // function that determines if more jumps are available after the intial jump. 
        // will return true if any are found and returns false otherwise
        private bool p2_jump_available()
        {

            if (row + 2 <= 7 && column + 2 <= 7 && Board_array[row + 2, column + 2] == CheckerType.Free && (Board_array[row + 1, column + 1] == CheckerType.P1_check || Board_array[row + 1, column + 1] == CheckerType.P1_king))
            {
                // check if another move can be made by moving right 2 and down 2 more spaces
                return true;
            }
            else if (row + 2 <= 7 && column - 2 >= 0 && Board_array[row + 2, column - 2] == CheckerType.Free && (Board_array[row + 1, column - 1] == CheckerType.P1_check || Board_array[row + 1, column - 1] == CheckerType.P1_king))
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

        // function is similar to P1_jump_available() but checks if jumps are avaible in any of the four directions.
        // this function is used for both players since kings can move in any direction no matter the owner
        private bool more_king_jump_available()
        {
            if (player_one_turn)
            {
                if (row - 2 >= 0 && column - 2 >= 0 && Board_array[row - 2, column - 2] == CheckerType.Free && (Board_array[row - 1, column - 1] == CheckerType.P2_check || Board_array[row - 1, column - 1] == CheckerType.P2_king))
                {
                    // check if another move can be made by moving left 2 and up 2 more spaces to allow for chain jumps
                    return true;

                }
                else if (row - 2 >= 0 && column + 2 <= 7 && Board_array[row - 2, column + 2] == CheckerType.Free && (Board_array[row - 1, column + 1] == CheckerType.P2_check || Board_array[row - 1, column + 1] == CheckerType.P2_king))
                {

                    // check if another move can be made by moving right 2 and up 2 more spaces to allow for chain jumps
                    return true;
                }
                else if (row + 2 <= 7 && column - 2 >= 0 && Board_array[row + 2, column - 2] == CheckerType.Free && (Board_array[row + 1, column - 1] == CheckerType.P2_check || Board_array[row + 1, column - 1] == CheckerType.P2_king))
                {
                    // check if another move can be made by moving left 2 and down 2 more spaces to allow for chain jumps
                    return true;

                }
                else if (row + 2 <= 7 && column + 2 <= 7 && Board_array[row + 2, column + 2] == CheckerType.Free && (Board_array[row + 1, column + 1] == CheckerType.P2_check || Board_array[row + 1, column + 1] == CheckerType.P2_king))
                {

                    // check if another move can be made by moving right 2 and down 2 more spaces to allow for chain jumps
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (row - 2 >= 0 && column - 2 >= 0 && Board_array[row - 2, column - 2] == CheckerType.Free && (Board_array[row - 1, column - 1] == CheckerType.P1_check || Board_array[row - 1, column - 1] == CheckerType.P1_king))
                {
                    // check if another move can be made by moving left 2 and up 2 more spaces to allow for chain jumps
                    return true;

                }
                else if (row - 2 >= 0 && column + 2 <= 7 && Board_array[row - 2, column + 2] == CheckerType.Free && (Board_array[row - 1, column + 1] == CheckerType.P1_check || Board_array[row - 1, column + 1] == CheckerType.P1_king))
                {

                    // check if another move can be made by moving right 2 and up 2 more spaces to allow for chain jumps
                    return true;
                }
                else if (row - 2 >= 0 && column - 2 >= 0 && Board_array[row + 2, column - 2] == CheckerType.Free && (Board_array[row + 1, column - 1] == CheckerType.P1_check || Board_array[row + 1, column - 1] == CheckerType.P1_king))
                {
                    // check if another move can be made by moving left 2 and down 2 more spaces to allow for chain jumps
                    return true;

                }
                else if (row - 2 >= 0 && column + 2 <= 7 && Board_array[row + 2, column + 2] == CheckerType.Free && (Board_array[row + 1, column + 1] == CheckerType.P1_check || Board_array[row + 1, column + 1] == CheckerType.P1_king))
                {

                    // check if another move can be made by moving right 2 and down 2 more spaces to allow for chain jumps
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        // function to check if check has been moved to a part of the board that allows them to be kinged.
        private bool Is_kinged()
        {
            // player one has reached the top and needs to be kinged
            System.Console.WriteLine("is kinged row " + row);

            if (row == 0 && Board_array[prevRow, prevCol] == CheckerType.P1_check )
            {
                System.Console.WriteLine("should be kinged");
                Board_array[row, column] = CheckerType.P1_king;
                Board_array[prevRow,prevCol] = CheckerType.Free;
               
                updateBoardGui();
                return true;
                
            }
            else if(row == 7 && Board_array[prevRow, prevCol] == CheckerType.P2_check)
            {
                // player two has reached the bottom and needs to be kinged
                Board_array[row, column] = CheckerType.P2_king;
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

        // a function created to adhere to DRY principles since kings move in any direction no matter the owner

        private bool is_normal_king_move()
        {
            if(Board_array[row,column] == CheckerType.Free && (row - prevRow == 1 || row - prevRow == -1) && (column - prevCol == 1 || column - prevCol == -1))
            {
                
                Board_array[row, column] = Board_array[prevRow, prevCol];
                Board_array[prevRow, prevCol] = CheckerType.Free;
                return true;
            }
            else
            {
                return false;
            }
        }

        // function that checks if the attempted jump by the king is valid depending on the player

       private bool is_valid_king_jump()
       {
            if (player_one_turn)
            {
                // player ones move
                if (Board_array[row, column] == CheckerType.Free && row - prevRow == 2 && column - prevCol == 2 )
                {
                    if(Board_array[row - 1, column - 1] == CheckerType.P2_check || Board_array[row - 1, column - 1] == CheckerType.P2_king)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (Board_array[row, column] == CheckerType.Free && row - prevRow == 2 && column - prevCol == -2)
                {
                    if (Board_array[row - 1, column + 1] == CheckerType.P2_check || Board_array[row - 1, column + 1] == CheckerType.P2_king)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (Board_array[row, column] == CheckerType.Free && row - prevRow == -2 && column - prevCol == 2)
                {
                    if (Board_array[row + 1, column - 1] == CheckerType.P2_check || Board_array[row + 1, column - 1] == CheckerType.P2_king)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (Board_array[row, column] == CheckerType.Free && row - prevRow == -2 && column - prevCol == -2)
                {
                    if (Board_array[row + 1, column + 1] == CheckerType.P2_check || Board_array[row + 1, column + 1] == CheckerType.P2_king)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // player twos move
                if (Board_array[row, column] == CheckerType.Free && row - prevRow == 2 && column - prevCol == 2)
                {
                    if (Board_array[row - 1, column - 1] == CheckerType.P1_check || Board_array[row - 1, column - 1] == CheckerType.P1_king)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (Board_array[row, column] == CheckerType.Free && row - prevRow == 2 && column - prevCol == -2)
                {
                    if (Board_array[row - 1, column + 1] == CheckerType.P1_check || Board_array[row - 1, column + 1] == CheckerType.P1_king)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (Board_array[row, column] == CheckerType.Free && row - prevRow == -2 && column - prevCol == 2)
                {
                    if (Board_array[row + 1, column - 1] == CheckerType.P1_check || Board_array[row + 1, column - 1] == CheckerType.P1_king)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (Board_array[row, column] == CheckerType.Free && row - prevRow == -2 && column - prevCol == -2)
                {
                    if (Board_array[row + 1, column + 1] == CheckerType.P1_check || Board_array[row + 1, column + 1] == CheckerType.P1_king)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            
       }


        private void End_turn()
        {
            Is_kinged(); // check to make sure that the final move made by the player is a kinged move or not

            players_second_click = !players_second_click;
            player_one_turn = !player_one_turn;

            // after switching players determine current player and display to window
            if (player_one_turn)
            {
                current_player.Text = "Player 1 Turn";
            }
            else
            {
                current_player.Text = "Player 2 Turn";
            }
        }

        private void invalid_choice()
        {
            players_second_click = false;
            borderChangeBack(prevButton);
        }

        private bool game_over()
        {
            if(p1_check_count == 0 || p2_check_count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /*
         **************** End helper functions***************************
         * 
         * 
         *************** event handler for button press********************
         *    contains all the game logic for checkers  
         */
        private void Button_Click(object sender, RoutedEventArgs e) {

            

            // if the game has ended, a pop text box will appear to inform the winner
            // afterwards the game window is closed and the title window is intialized and displayed

            if (game_over())
            {
                if(p1_check_count > 0) // player 1 won 
                {
                    MessageBoxResult result = MessageBox.Show("PLAYER ONE WINS!", "GAME OVER");

                }
                else // player 2 won
                {
                    MessageBoxResult result = MessageBox.Show("PLAYER TWO WINS!", "GAME OVER");
                }

                Window1 window = new Window1();
                this.Visibility = Visibility.Collapsed; // hide current window
                window.Show(); // show the main window obj
                this.Close();
            }

           

            

            var button = (Button)sender;
           

           

            // get row and column of the button pressed so that it can be found within the Board_array and logic can be applied
             column = Grid.GetColumn(button);
             row = Grid.GetRow(button);
            
           
            

            // currently player ones turn
            if (player_one_turn)
            {
                // player has clicked on piece belonging to them and wants to move it
                if (players_second_click)
                { 

                     prevRow = Grid.GetRow(prevButton);
                     prevCol = Grid.GetColumn(prevButton);
                    if (Board_array[prevRow, prevCol] == CheckerType.P1_check)
                    {
                        // the piece is a normal check

                        // check if player made a normal move

                        if (Board_array[row, column] == CheckerType.Free && (row - prevRow == -1) && (column - prevCol == -1 || column - prevCol == 1))
                        {

                            if (!Is_kinged())
                            {


                                // the space is free and is a valid movement and not a kinged move
                                
                               Board_array[row, column] = CheckerType.P1_check;
                               Board_array[prevRow, prevCol] = CheckerType.Free;
                               button.Content = "•";
                               button.Foreground = p1_color;
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
                                p2_check_count--; // decrement the amount of player 2 checks

                                // jump enemy checker
                                Board_array[row + 1, column + 1] = CheckerType.Free;
                                

                                if (Is_kinged())
                                {
                                    End_turn();
                                    borderChangeBack(prevButton);
                                }
                                else
                                {
                                    Board_array[row, column] = CheckerType.P1_check;
                                    Board_array[prevRow, prevCol] = CheckerType.Free;

                                    // reset the button border and update the game board
                                    borderChangeBack(prevButton);
                                    updateBoardGui();


                                    if (p1_jump_available())
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
                        }
                        else if (Board_array[row, column] == CheckerType.Free && (row - prevRow == -2) && (column - prevCol == 2))
                        {

                            if(Board_array[row + 1, column - 1] == CheckerType.P2_check || Board_array[row + 1, column - 1] == CheckerType.P2_king)
                            {
                                p2_check_count--; // decrement the amount of player 2 checks
                                                  // jump enemy checker from the board

                                Board_array[row + 1, column - 1] = CheckerType.Free;

                                if (Is_kinged())
                                {
                                
                                    // check was kinged so it is end of players turn
                                    End_turn();
                                    borderChangeBack(prevButton);
                                }
                                else
                                { // checker was not kinged after jumping check
                                    Board_array[row, column] = CheckerType.P1_check;
                                    Board_array[prevRow, prevCol] = CheckerType.Free;


                                    // reset the button border and update the game board
                                    borderChangeBack(prevButton);
                                    updateBoardGui();

                                    if (p1_jump_available())
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

                        }else
                        {
                            // no valid button was chosen so reset turn
                            invalid_choice();
                        }
                    }
                    else
                    {
                        // the piece is a king check
                        if (is_normal_king_move())
                        {
                            button.Content = "♛";
                            button.Foreground = p1_color;

                            prevButton.Content = "";

                            borderChangeBack(prevButton);

                            End_turn();

                        }else if (is_valid_king_jump())
                        {
                            p2_check_count--; // decrement the amount of player 2 checks

                            // calculate the row and column of the jumped piece from any direction 
                            // this is because the king can move from any direction
                            // example: row = 5 prevRow = 7: then 5 + ((5 - 7) * -.5) = 6, which is the row of the jumped check
                            int jumped_row = (int)(row + ((row - prevRow) * -.5));
                            int jumped_col = (int)(column + ((column - prevCol) * -.5));


                            Board_array[row, column] = CheckerType.P1_king;

                            System.Console.WriteLine("value of jumped piece " + (jumped_row) + "  " + ( jumped_col));
                            Board_array[jumped_row,  jumped_col] = CheckerType.Free;

                            Board_array[prevRow, prevCol] = CheckerType.Free;

                            borderChangeBack(prevButton);
                            updateBoardGui();

                            if (more_king_jump_available())
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
                        else
                        {
                            invalid_choice();
                        }
                        
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
                            if (!Is_kinged())
                            {
                                Board_array[row, column] = CheckerType.P2_check;

                                Board_array[prevRow, prevCol] = CheckerType.Free;

                                button.Content = "•";
                                button.Foreground = p2_color;
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
                                p1_check_count--; // decrement the amount of player 1 checks

                                Board_array[row - 1, column + 1] = CheckerType.Free;
                                

                                if (Is_kinged())
                                {
                                    End_turn();
                                    borderChangeBack(prevButton);
                                }
                                else
                                {

                                    Board_array[row, column] = CheckerType.P2_check;
                                    Board_array[prevRow, prevCol] = CheckerType.Free;


                                    borderChangeBack(prevButton);
                                    updateBoardGui();

                                    if (p2_jump_available())
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
                        }
                        else if (Board_array[row, column] == CheckerType.Free && (row - prevRow == 2) && column - prevCol == 2)
                        {

                            p1_check_count--; // decrement the amount of player 1 checks

                            Board_array[row - 1, column - 1] = CheckerType.Free;
                            

                            if (Is_kinged())
                            {
                                End_turn();
                                borderChangeBack(prevButton);
                            }
                            else
                            {


                                Board_array[row, column] = CheckerType.P2_check;
                                Board_array[prevRow, prevCol] = CheckerType.Free;

                                borderChangeBack(prevButton);
                                updateBoardGui();

                                if (p2_jump_available())
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
                        else
                        {
                            // no valid button was chosen so reset turn
                            invalid_choice();
                        }
                    }
                    else
                    {
                        // piece chosen was a P2 king 
                        if (is_normal_king_move())
                        {
                            button.Content = "♚";
                            button.Foreground = p2_color;

                            prevButton.Content = "";

                            borderChangeBack(prevButton);

                            End_turn();

                        }
                        else if (is_valid_king_jump())
                        {
                            p1_check_count--; // decrement the amount of player 1 checks


                            // calculate the row and column of the jumped piece from any direction 
                            // this is because the king can move from any direction
                            // example: row = 5 prevRow = 7: then 5 + ((5 - 7) * -.5) = 6, which is the row of the jumped check

                            int jumped_row = (int)(row + ((row - prevRow) * -.5));
                            int jumped_col = (int)(column + ((column - prevCol) * -.5));


                            Board_array[row, column] = CheckerType.P2_king;

                            System.Console.WriteLine("value of jumped piece " + (row + jumped_row) + "  " + (column + jumped_col));
                            Board_array[jumped_row, jumped_col] = CheckerType.Free;

                            Board_array[prevRow, prevCol] = CheckerType.Free;

                            borderChangeBack(prevButton);
                            updateBoardGui();


                            if (more_king_jump_available())
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
                        else
                        {
                            invalid_choice();
                        }
                    }
                }
               // this is the players first click
                else
                {
                    current_player.Text = "Player 2 Turn";

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers_game
{

    // types of values that can be stored on each white till of board
    public enum CheckerType
    {
        Free, // no value
        P1_check, // player ones checker
        P1_king, // player ones king 
        P2_check, // player twos checker
        P2_king // player twos king

    }
}

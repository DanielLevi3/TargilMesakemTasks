using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargilMesakemMultiThreaded
{
    //  שאלת אתגר 5+6
    public class BankAccountMinusEventArgs : EventArgs
    {
        public int Amount { get; set; }
        public int Balance { get; set; }
    }
   
}

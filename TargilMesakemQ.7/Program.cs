using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TargilMesakemQ._7
{
    class Program
    {
        
        
        static void Main(string[] args)
        {
            Kitchen.dishReady += Waiter.OnDishReady;
            Kitchen.PrepareDish("Stake");
        }
    }
}

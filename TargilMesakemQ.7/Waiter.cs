using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargilMesakemQ._7
{
    class Waiter
    {
        public static void OnDishReady(object sender, DishEventArgs d)
        {
            Console.WriteLine($"Serving the dish {d.DishName} ,to the customers");
        }
    }
}

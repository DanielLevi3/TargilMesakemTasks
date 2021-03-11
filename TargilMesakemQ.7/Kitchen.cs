using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TargilMesakemQ._7
{
    class Kitchen
    {
        public static event EventHandler<DishEventArgs> dishReady;
        public static void PrepareDish(string dishName)
        {
            Console.WriteLine("Preparing dish...");
            Thread.Sleep(3000);

            if (dishReady != null)
            {
                Waiter.OnDishReady(null, new DishEventArgs { DishName = dishName });
            }
        }

    }
}

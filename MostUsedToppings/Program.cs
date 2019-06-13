using System;

namespace MostUsedToppings
{
    class Program
    {
        static void Main(string[] args)
        {
            var finder = new FindToppings("http://files.olo.com/pizzas.json", 20);
            var top20 = finder.RetrieveToppings();

            foreach (var topping in top20)
            {
                Console.WriteLine($"{topping.Rank}. {topping.Toppings} - {topping.NumRequests}");
            }
        }
    }
}

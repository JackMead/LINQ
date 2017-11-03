using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();

            var apples = p.PickApples().Take(10000);


            var numberPoisonedApples = apples.Count(i => i.Poisoned == true);
            Console.WriteLine("There are {0} poisoned apples", numberPoisonedApples);

            var secondLargestColourPoisonedApples =
                apples.Where(i => i.Poisoned).GroupBy(i => i.Colour).OrderByDescending(i => i.Count()).Skip(1).Take(1).ToList()[0].Key;
            Console.WriteLine("The second most commonly poisoned colour is {0}", secondLargestColourPoisonedApples);

            var maxNumberNonPoisonedRedApplesSuccessive = apples.Aggregate(new Tuple<int, int>(0, 0), (acc, apple) =>
                (apple.Colour == "Red" && !apple.Poisoned) 
                ? new Tuple<int, int>(acc.Item1 + 1, Math.Max(acc.Item1 + 1, acc.Item2)) 
                : new Tuple<int, int>(0, Math.Max(acc.Item1, acc.Item2))).Item2;
            Console.WriteLine("There are at most {0} red, non poisoned apples in a row", maxNumberNonPoisonedRedApplesSuccessive);

            var counter = 0;
            var question3UsingZip = apples.Zip(apples.Skip(1), (a1, a2) => (a1.Colour=="Red" && !a1.Poisoned && a2.Colour=="Red" && !a2.Poisoned)?(counter==0?counter=2:++counter):counter=0).Max();
            Console.WriteLine("There are {0} non poisoned red in a row according to zip", question3UsingZip);

            var numberOfGreenApplesFollowedByGreenApples = apples.Aggregate(
                new Tuple<int, bool>(0, false),
                (acc, apple) => (acc.Item2 == true && apple.Colour == "Green")
                    ? new Tuple<int, bool>(acc.Item1 + 1, apple.Colour == "Green")
                    : new Tuple<int, bool>(acc.Item1, apple.Colour == "Green")).Item1;

            Console.WriteLine("There are {0} Green apples that are followed by green apples", numberOfGreenApplesFollowedByGreenApples);

            var question4UsingZip = apples.Zip(apples.Skip(1), (a1, a2) => a1.Colour == "Green" && a2.Colour == "Green"  ).Count(i=>i);
            Console.WriteLine("There are {0} Green apples that are followed by green apples according to zip", question4UsingZip);

        }

        private IEnumerable<Apple> PickApples()
        {
            int colourIndex = 1;
            int poisonIndex = 7;

            while (true)
            {
                yield return new Apple
                {
                    Colour = GetColour(colourIndex),
                    Poisoned = poisonIndex % 41 == 0
                };

                colourIndex += 5;
                poisonIndex += 37;
            }
        }

        private string GetColour(int colourIndex)
        {
            if (colourIndex % 13 == 0 || colourIndex % 29 == 0)
            {
                return "Green";
            }

            if (colourIndex % 11 == 0 || colourIndex % 19 == 0)
            {
                return "Yellow";
            }

            return "Red";
        }

        private class Apple
        {
            public string Colour { get; set; }
            public bool Poisoned { get; set; }

            public override string ToString()
            {
                return $"{Colour} apple{(Poisoned ? " (poisoned!)" : "")}";
            }
        }

    }


}

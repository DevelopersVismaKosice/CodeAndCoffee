using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DidYouKnowCSharp
{
    public class Gotcha6
    {
        static void Main(string[] args)
        {
            var originalNumbers = new List<int> { 1, 2, 3, 4, 5, 6 };

            var list = new List<int>(originalNumbers);
            var collection = new Collection<int>(originalNumbers);

            originalNumbers.RemoveAt(0);

            DisplayItems(list, "List items: ");
            DisplayItems(collection, "Collection items: ");

            Console.ReadLine();
        }

        private static void DisplayItems(IEnumerable<int> items, string title)
        {
            Console.WriteLine(title);
            foreach (var item in items)
                Console.Write(item);
            Console.WriteLine();
        }
    }

    #region Comment

    //And output is:

    //List items: 123456
    //Collection items: 23456

    //Collection constructor that accepts IList creates a wrapper around original List, while List constructor creates a new List and copies all references from original to the new List.

    //See more here: http://blog.roboblob.com/2012/09/19/dot-net-gotcha-nr1-list-versus-collection-constructor/

    #endregion
}
using System;
using System.Linq;

namespace DidYouKnowCSharp2.ILView
{
    public class Foreach
    {
        public void TryIt()
        {
            int[] arrayOfNumbers = Enumerable.Range(1, 1000).ToArray();

            foreach(var number in arrayOfNumbers)
            {
                Console.WriteLine(number);
            }
        }
    }
}
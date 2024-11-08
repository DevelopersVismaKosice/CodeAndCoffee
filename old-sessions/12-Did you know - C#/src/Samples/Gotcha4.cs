using System;

namespace DidYouKnowCSharp
{
    public class Gotcha4
    {
        class SomeGeneric<T>
        {
            public static int i = 0;
        }

        class Test
        {
            public static void main(string[] args)
            {
                SomeGeneric<int>.i = 5;
                SomeGeneric<string>.i = 10;
                Console.WriteLine(SomeGeneric<int>.i);
                Console.WriteLine(SomeGeneric<string>.i);
                Console.WriteLine(SomeGeneric<int>.i);
            }
        }
    }

    #region Comment

    // Today I fixed a bug that eluded for long time.The bug was in a generic class that was used in multi threaded scenario and a static int field was used to provide lock free synchronisation using Interlocked. The bug was caused because each instantiation of the generic class for a type has its own static. So each thread got its own static field and it wasn't used a lock as intended.

    // This prints 5 10 5

    #endregion
}
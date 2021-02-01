using System;

namespace DidYouKnowCSharp
{
    public class Gotcha5
    {
        public static void main(string[] args)
        {
            int? i = null;
            // Nullable<int>
            i++;
        }
    }

    #region Comment
    // I would have expected an exception but runs fine and stays as null

    // Just found a weird one that had me stuck in debug for a while:

    // You can increment null for a nullable int without throwing an excecption and the value stays null.

    #endregion
}
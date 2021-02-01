using System;

namespace DidYouKnowCSharp
{
    public class Gotcha1
    {
        private int myVar;
        public int MyVar
        {
            get { return MyVar; }
        }
    }

    #region Comment

    // Blammo.Your app crashes with no stack trace.Happens all the time.

    // (Notice capital MyVar instead of lowercase myVar in the getter.)

    #endregion
}
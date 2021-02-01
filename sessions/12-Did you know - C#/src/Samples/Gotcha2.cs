using System;

namespace DidYouKnowCSharp
{
    public class Gotcha2
    {
        private string _myObj;
        public string MyObj
        {
            get
            {
                if (_myObj == null)
                    _myObj = "loaded"; // some other code to create my object
                return _myObj;
            }
        }

        public void Exection()
        {
            // blah
            // blah
            MyObj.Substring(5); // Line 3
            // blah
        }

        #region Comment 

        //Now you want to debug your CreateMyObj() method.So you put a breakpoint on Line 3 above, with intention to step into the code.Just for good measure, you also put a breakpoint on the line above that says _myObj = CreateMyObj();, and even a breakpoint inside CreateMyObj() itself.

        //The code hits your breakpoint on Line 3. You step into the code.You expect to enter the conditional code, because _myObj is obviously null, right? Uh... so...why did it skip the condition and go straight to return _myObj?! You hover your mouse over _myObj... and indeed, it does have a value! How did THAT happen?!

        //The answer is that your IDE caused it to get a value, because you have a "watch" window open - especially the "Autos" watch window, which displays the values of all variables/properties relevant to the current or previous line of execution.When you hit your breakpoint on Line 3, the watch window decided that you would be interested to know the value of MyObj - so behind the scenes, ignoring any of your breakpoints, it went and calculated the value of MyObj for you - including the call to CreateMyObj() that sets the value of _myObj!

        //That's why I call this the Heisenberg Watch Window - you cannot observe the value without affecting it... :)

        #endregion
    }
}
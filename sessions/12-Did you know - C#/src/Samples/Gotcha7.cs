using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DidYouKnowCSharp
{
    public class Gotcha7
    {
        static void Main(string[] args)
        {
            try
            {

            }
            // exception throwing
            catch (Exception e)
            {
                //Logger.Log(e.GetType().Name + Message);
                // Do stuff 
                throw e;
            }
        }
    }

    #region Comment

    // The problem is that it wipes the stack trace and makes diagnosing issues much harder, cause you can not track where the exception originated.

    // The correct code is either the throw statement with no args:

    // catch(Exception)
    // {
    //    throw;
    // }

    #endregion
}
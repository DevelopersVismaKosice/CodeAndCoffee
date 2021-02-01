using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace DidYouKnowCSharp
{
    
    public class Gotcha9
    {
        public void Method()
        {
            List<dynamic> table = new List<dynamic>();

            var result = from o in table
                            where o.column == null
                            select o;
            //Returns all rows where column is null

            int? myNullInt = null;
            result = from o in table
                            where o.column == myNullInt
                            select o;
        }
    }

    #region Comment

    // Never returns anything!

    // There's a bug-report for LINQ-to-Entites here, though they don't seem to check that forum often.Perhaps someone should file one for LINQ-to-SQL as well?

    #endregion
}
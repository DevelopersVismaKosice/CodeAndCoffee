using System;

namespace DidYouKnowCSharp
{
    public class Gotcha3
    {
        public void Exection()
        {
            // does this aways return same format date ?
            var date = DateTime.Now.ToString("dd/MM/yyyy");

            
        }

        #region Comment 

        // DateTime.ToString("dd/MM/yyyy"); This will actually not always give you dd/MM/yyyy but instead it will take into account the regional settings and replace your date separator depending on where you are. So you might get dd-MM-yyyy or something alike.

        //The right way to do this is to use DateTime.ToString("dd'/'MM'/'yyyy");

        //DateTime.ToString("r") is supposed to convert to RFC1123, which uses GMT.GMT is within a fraction of a second from UTC, and yet the "r" format specifier does not convert to UTC, even if the DateTime in question is specified as Local.

        //This results in the following gotcha(varies depending on how far your local time is from UTC) :

        //DateTime.Parse("Tue, 06 Sep 2011 16:35:12 GMT").ToString("r")
        //>              "Tue, 06 Sep 2011 17:35:12 GMT"
        //Whoops!

        #endregion
    }
}
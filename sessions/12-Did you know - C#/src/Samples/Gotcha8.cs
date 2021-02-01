using System.Diagnostics;

namespace DidYouKnowCSharp
{
    [DebuggerDisplay("Count = {count}")]
    //[DebuggerBrowsable(DebuggerBrowsableState.Never)] 
    public class Gotcha8
    {
        public int count = 4;
    }
}
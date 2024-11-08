using System;

namespace DidYouKnowCSharp2.Gotchas
{
    public class LambdaBasedGetter
    {
        public Guid Id => Guid.NewGuid();

        // try in https://sharplab.io/

        #region correct

        // public Guid Id { get; } = Guid.NewGuid();

        #endregion correct
    }
} 
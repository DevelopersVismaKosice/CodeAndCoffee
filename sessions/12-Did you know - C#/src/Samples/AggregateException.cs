using System;
using System.Linq;
using System.Threading.Tasks;

namespace DidYouKnowCSharp
{
    /// Source: https://docs.microsoft.com/en-us/dotnet/api/system.aggregateexception.flatten?view=net-5.0
    /// 
    public class AggregateExceptionSample
    {
        public static void Main()
        {
            var task1 = Task.Run(() => { throw new CustomException("This exception is expected!"); });

            >> var result = _repository.Load();

            // loads of code
            // Task.WaitAll()

            await result;

            try
            {
                task1.Wait();
            }
            catch (AggregateException ae)
            {
                // Call the Handle method to handle the custom exception,
                // otherwise rethrow the exception.
                ae.Handle(ex =>
                {
                    if (ex is CustomException)
                        Console.WriteLine(ex.Message);
                    return ex is CustomException;
                });

                

                ae.Flatten().InnerExceptions.ToList().ForEach(ex => logger.log(ex));
            }
        }
    }

    public class CustomException : Exception
    {
        public CustomException(String message) : base(message)
        { }
    }
    
    // The example displays the following output:
    //        This exception is expected!

}
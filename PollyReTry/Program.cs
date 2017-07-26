using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly.Retry;
using Polly;
using System.Net.Http;

namespace PollyReTry
{
    public class Program
    {
        
        static void Main(string[] args)
        {

            int count = 0;

            

            var policy = Policy.HandleResult<HttpResponseMessage>(x => x
                            .StatusCode == System.Net.HttpStatusCode.BadRequest)
                            .Retry(2, (response, retrycount)=> {
                                count = retrycount;
                            });
            

            var result = policy.Execute(() => { return CallBack(count); });
            Console.WriteLine($" Final result of the call .... with status code:  {result.StatusCode} and with Message : {result.ReasonPhrase}");
            Console.WriteLine($"Press any key to continue...");
            Console.ReadKey();
        }


        private static HttpResponseMessage CallBack(int numOfCalls)
        {
            int count = numOfCalls;
            
            Console.WriteLine($"Calling API {count}");
            if (count == 0)

                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    ReasonPhrase = "Email Send"
                };
            else
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ReasonPhrase = "Unable to fine the resource"
                };

            
        }
    }
}

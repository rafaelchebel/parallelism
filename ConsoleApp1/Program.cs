using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Silly counter: Press Z to Stop");
            var tokenSource = new CancellationTokenSource();
            var cancellationToken = tokenSource.Token;
            Task.Run(() =>
            {
                long n = 0;
                while (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine(n);
                    n = n + 1;
                }
            }, cancellationToken);

            while (true)
            {
                if (Console.Read() == 'z')
                {
                    tokenSource.Cancel();
                }
            }
        }
    }
}

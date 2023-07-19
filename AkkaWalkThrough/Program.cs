using Akka.Actor;

namespace AkkaWalkThrough
{
    public class Program
    {
        private static ActorSystem? ActorSystem;

        static void Main()
        {
            ActorSystem = ActorSystem.Create("NewActorSystem");

            PrintInstructions();

            IActorRef consoleWriterActor = ActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()), "consoleWriterActor");
            IActorRef consoleReaderActor = ActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)), "consoleReaderActor");

            consoleReaderActor.Tell("start");

            ActorSystem.WhenTerminated.Wait();  
        }

        private static void PrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.Write("Some lines will appear as");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" red ");
            Console.ResetColor();
            Console.Write(" and others will appear as");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" green! ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }
    }
}
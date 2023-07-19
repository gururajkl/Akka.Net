using Akka.Actor;

namespace AkkaWalkThrough
{
    public class Program
    {
        private static ActorSystem? ActorSystem;

        static void Main()
        {
            ActorSystem = ActorSystem.Create("NewActorSystem");

            IActorRef consoleWriterActor = ActorSystem.ActorOf(Props.Create(() => new ConsoleWriterActor()), "consoleWriterActor");
            IActorRef consoleReaderActor = ActorSystem.ActorOf(Props.Create(() => new ConsoleReaderActor(consoleWriterActor)), "consoleReaderActor");

            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            ActorSystem.WhenTerminated.Wait();  
        }
    }
}
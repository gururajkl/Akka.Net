using Akka.Actor;

namespace AkkaWalkThrough
{
    public class Program
    {
        private static ActorSystem? ActorSystem;

        static void Main()
        {
            ActorSystem = ActorSystem.Create("NewActorSystem");

            Props consoleWriterProps = Props.Create<ConsoleWriterActor>();
            IActorRef consoleWriterActor = ActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            Props validationActorProps = Props.Create(() => new ValidationActor(consoleWriterActor));
            IActorRef validationActor = ActorSystem.ActorOf(validationActorProps, "validationActor");

            Props consoleReaderActorProps = Props.Create<ConsoleReaderActor>(validationActor);
            IActorRef consoleReaderActor = ActorSystem.ActorOf(consoleReaderActorProps, "consoleReaderActor");

            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            ActorSystem.WhenTerminated.Wait();
        }
    }
}
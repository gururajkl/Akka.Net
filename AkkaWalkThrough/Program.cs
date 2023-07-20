using Akka.Actor;

namespace AkkaWalkThrough
{
    public class Program
    {
        private static ActorSystem? ActorSystem;

        static void Main()
        {
            ActorSystem = ActorSystem.Create("NewActorSystem");

            Props tailCoordinatorActorProps = Props.Create(() => new TailCoordinatorActor());
            IActorRef tailCoordinatorActor = ActorSystem.ActorOf(tailCoordinatorActorProps, "tailCoordinatorActor");

            Props consoleWriterProps = Props.Create<ConsoleWriterActor>();
            IActorRef consoleWriterActor = ActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

            Props fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor));
            IActorRef validationActor = ActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

            Props consoleReaderActorProps = Props.Create<ConsoleReaderActor>();
            IActorRef consoleReaderActor = ActorSystem.ActorOf(consoleReaderActorProps, "consoleReaderActor");

            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            ActorSystem.WhenTerminated.Wait();
        }
    }
}
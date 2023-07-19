using Akka.Actor;

namespace AkkaWalkThrough
{
    public class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        private IActorRef consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            this.consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = Console.ReadLine();

            if(!string.IsNullOrEmpty(msg) && string.Equals(msg, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                Context.System.Terminate();
                return;
            }

            consoleWriterActor.Tell(msg);

            Self.Tell("continue"); 
        }
    }
}

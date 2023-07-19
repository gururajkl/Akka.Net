using Akka.Actor;

namespace AkkaWalkThrough
{
    public class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            var msg = message as string;

            if (string.IsNullOrEmpty(msg))
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Please provide the input\n");
                Console.ResetColor();
                return;
            }

            bool even = msg.Length % 2 == 0;
            var color = even ? ConsoleColor.Red : ConsoleColor.Green;
            Console.ForegroundColor = color;
            var alert = even ? "Your string had an even # of characters.\n" : "Your string had an odd # of characters.\n";
            Console.WriteLine(alert);
            Console.ResetColor();
        }
    }
}

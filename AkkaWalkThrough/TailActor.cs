using Akka.Actor;
using System.Text;

namespace AkkaWalkThrough
{
    public class TailActor : UntypedActor
    {
        public class FileWrite
        {
            public string FileName { get; private set; }

            public FileWrite(string fileName)
            {
                FileName = fileName;
            }
        }

        public class FileError
        {
            public string FileName { get; private set; }
            public string Reason { get; private set; }

            public FileError(string fileName, string reason)
            {
                FileName = fileName;
                Reason = reason;
            }
        }

        public class InitialRead
        {
            public string FileName { get; private set; }
            public string Text { get; private set; }

            public InitialRead(string fileName, string text)
            {
                FileName = fileName;
                Text = text;
            }
        }

        private readonly string filePath;
        private readonly IActorRef reporterActor;
        private readonly FileObserver observer;
        private readonly Stream fileStream;
        private readonly StreamReader fileStreamReader;

        public TailActor(string filePath, IActorRef reporterActor)
        {
            this.filePath = filePath;
            this.reporterActor = reporterActor;

            observer = new FileObserver(Self, filePath);
            observer.Start();

            fileStream = new FileStream(Path.GetFullPath(filePath), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fileStreamReader = new StreamReader(fileStream, Encoding.UTF8);

            var text = fileStreamReader.ReadToEnd();
            Self.Tell(new InitialRead(filePath, text));
        }

        protected override void OnReceive(object message)
        {
            if (message is FileWrite)
            {
                var text = fileStreamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(text))
                {
                    reporterActor.Tell(text);
                }

            }
            else if (message is FileError)
            {
                var fe = message as FileError;
                reporterActor.Tell(string.Format("Tail error: {0}", fe!.Reason));
            }
            else if (message is InitialRead)
            {
                var ir = message as InitialRead;
                reporterActor.Tell(ir!.Text);
            }
        }
    }
}

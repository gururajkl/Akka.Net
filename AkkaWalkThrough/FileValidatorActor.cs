﻿using Akka.Actor;

namespace AkkaWalkThrough
{
    public class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef? consoleWriterActor;

        public FileValidatorActor(IActorRef? consoleWriterActor)
        {
            this.consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;

            if (string.IsNullOrEmpty(msg))
            {
                consoleWriterActor.Tell(new Messages.NullInputError("Input was blank. Please try again.\n"));
                Sender.Tell(new Messages.ContinueProcessing());
            }
            else
            {
                var valid = IsFileUri(msg!);
                if (valid)
                {
                    consoleWriterActor.Tell(new Messages.InputSuccess($"Starting processing for {msg}"));
                    Context.ActorSelection("akka://NewActorSystem/user/tailCoordinatorActor")!
                        .Tell(new TailCoordinatorActor.StartTail(msg, consoleWriterActor!));
                }
                else
                {
                    consoleWriterActor.Tell(new Messages.ValidationError($"{msg} is not an existing URI on disk."));
                    Sender.Tell(new Messages.ContinueProcessing());
                }
            }
        }

        private static bool IsFileUri(string path)
        {
            return File.Exists(path);
        }
    }
}

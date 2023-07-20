using Akka.Actor;
using static AkkaWalkThrough.TailActor;

namespace AkkaWalkThrough
{
    public class FileObserver : IDisposable
    {
        private readonly IActorRef? tailActor;
        private readonly string? absoluteFilePath;
        private FileSystemWatcher? watcher;
        private readonly string? fileDir;
        private readonly string? fileNameOnly;

        public FileObserver(IActorRef tailActor, string absoluteFilePath)
        {
            this.tailActor = tailActor;
            this.absoluteFilePath = absoluteFilePath;
            fileDir = Path.GetDirectoryName(absoluteFilePath);
            fileNameOnly = Path.GetFileName(absoluteFilePath);
        }

        public void Start()
        {
            Environment.SetEnvironmentVariable("MONO_MANAGED_WATCHER", "enabled");

            watcher = new FileSystemWatcher(fileDir!, fileNameOnly!);
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

            // assign callbacks for event types
            watcher.Changed += OnFileChanged;
            watcher.Error += OnFileError;

            // start watching
            watcher.EnableRaisingEvents = true;
        }

        private void OnFileError(object sender, ErrorEventArgs e)
        {
            tailActor!.Tell(new FileError(fileNameOnly!, e.GetException().Message), ActorRefs.NoSender);
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                tailActor!.Tell(new TailActor.FileWrite(e!.Name!), ActorRefs.NoSender);
            }
        }

        public void Dispose()
        {
            watcher?.Dispose();
        }
    }
}

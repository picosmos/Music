namespace Koopakiller.Apps.MusicManager.Dialogs
{
    public abstract class FileDialog : IFileDialog
    {
        public string Filter { get; set; }

        public string FileName { get; protected set; }
        public abstract bool Show();
    }
}
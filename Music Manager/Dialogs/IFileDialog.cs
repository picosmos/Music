namespace Koopakiller.Apps.MusicManager.Dialogs
{
    public interface IFileDialog
    {
        string Filter { get; set; }

        string FileName { get; }
        bool Show();
    }
}
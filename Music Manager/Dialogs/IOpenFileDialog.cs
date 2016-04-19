namespace Koopakiller.Apps.MusicManager.Dialogs
{
    public interface IOpenFileDialog : IFileDialog
    {
        string[] FileNames { get; }

        bool Multiselect { get; set; }
    }
}
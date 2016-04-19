namespace Koopakiller.Apps.MusicManager.Dialogs
{
    public interface IDialogFactory<out TDialog>
    {
        TDialog CreateDialog();
    }
}
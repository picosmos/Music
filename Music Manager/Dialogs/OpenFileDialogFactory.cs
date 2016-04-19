namespace Koopakiller.Apps.MusicManager.Dialogs
{
    public class OpenFileDialogFactory : IDialogFactory<IOpenFileDialog>
    {
        public IOpenFileDialog CreateDialog()
        {
            return new OpenFileDialog();
        }
    }
}
using Koopakiller.Apps.MusicManager.Dialogs;

namespace Koopakiller.Apps.MusicManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            DialogFactories.OpenFileDialogFactory = new OpenFileDialogFactory();
        }
    }
}

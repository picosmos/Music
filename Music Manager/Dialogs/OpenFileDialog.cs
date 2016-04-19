namespace Koopakiller.Apps.MusicManager.Dialogs
{
    public sealed class OpenFileDialog : FileDialog, IOpenFileDialog
    {
        public string[] FileNames { get; private set; }

        public bool Multiselect { get; set; }

        public override bool Show()
        {
            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                Filter = this.Filter,
                FileName = this.FileName,
                Multiselect = this.Multiselect,
            };

            if (ofd.ShowDialog() == true)
            {
                this.FileName = ofd.FileName;
                this.FileNames = ofd.FileNames;
                return true;
            }
            else
            {
                this.FileName = "";
                this.FileNames = new string[0];
                return false;
            }
        }
    }
}
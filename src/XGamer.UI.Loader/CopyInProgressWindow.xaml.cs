using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace XGamer.UI.Loader
{
    /// <summary>
    /// Interaction logic for CopyInProgressWindow.xaml
    /// </summary>
    public partial class CopyInProgressWindow : Window
    {
        public CopyInProgressWindow(string sourcePath, string destinationPath)
        {
            this.InitializeComponent();

            this.SourcePath = sourcePath;
            this.DestinationPath = destinationPath;
            this.Worker = new BackgroundWorker();
            this.Worker.WorkerSupportsCancellation = true;
            this.Worker.DoWork += new DoWorkEventHandler(this.Worker_DoWork);
            this.Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
        }

        public string SourcePath { get; set; }
        
        public string DestinationPath { get; set; }

        private BackgroundWorker Worker { get; set; }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Worker.Dispose();

            if (!e.Cancelled && e.Error == null)
            {
                string parameters = "/F:" + Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\XGamer.lnk /A:C /T:" + Path.Combine(this.DestinationPath, XGamer.UI.Loader.Properties.Resources.XGamerTargetName) + " /W:" + this.DestinationPath + " /R:1";
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("shortcut.exe", parameters) { CreateNoWindow = true, UseShellExecute = false });

                this.Dispatcher.BeginInvoke((System.Action)delegate { DialogResult = true; }, null);
            }

            this.Dispatcher.BeginInvoke((System.Action)delegate { DialogResult = false; }, null);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.CopyDirectory(this.SourcePath, this.DestinationPath);
        }

        private void CopyDirectory(string source, string destination)
        {
            string[] files;
            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
            {
                destination += Path.DirectorySeparatorChar;
            }

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            files = Directory.GetFileSystemEntries(source);
            foreach (string element in files)
            {
                if (Directory.Exists(element))
                {
                    this.CopyDirectory(element, destination + Path.GetFileName(element));
                }
                else
                {
                    File.Copy(element, destination + Path.GetFileName(element), true);
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Worker.CancelAsync();
            this.Dispatcher.BeginInvoke((System.Action)delegate { DialogResult = false; }, null);
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            this.Worker.RunWorkerAsync();
        }
    }
}

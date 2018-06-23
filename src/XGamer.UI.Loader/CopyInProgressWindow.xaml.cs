﻿using System;
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
            InitializeComponent();

            SourcePath = sourcePath;
            DestinationPath = destinationPath;
            Worker = new BackgroundWorker();
            Worker.WorkerSupportsCancellation = true;
            Worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
        }

        public string SourcePath { get; set; }
        
        public string DestinationPath { get; set; }

        private BackgroundWorker Worker { get; set; }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Worker.Dispose();

            if (!e.Cancelled && e.Error == null)
            {
                string parameters = "/F:" + Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\XGamer.lnk /A:C /T:" + Path.Combine(DestinationPath, XGamer.UI.Loader.Properties.Resources.XGamerTargetName) + " /W:" + DestinationPath + " /R:1";
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("shortcut.exe", parameters) { CreateNoWindow = true, UseShellExecute = false });

                Dispatcher.BeginInvoke((System.Action)delegate { DialogResult = true; }, null);
            }

            Dispatcher.BeginInvoke((System.Action)delegate { DialogResult = false; }, null);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CopyDirectory(SourcePath, DestinationPath);
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
                    CopyDirectory(element, destination + Path.GetFileName(element));
                }
                else
                {
                    File.Copy(element, destination + Path.GetFileName(element), true);
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Worker.CancelAsync();
            Dispatcher.BeginInvoke((System.Action)delegate { DialogResult = false; }, null);
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            Worker.RunWorkerAsync();
        }
    }
}

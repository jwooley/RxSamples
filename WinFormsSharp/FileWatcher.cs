using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsSharp
{
    public partial class FileWatcher : Form
    {
        IDisposable eventSub;

        public FileWatcher()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            var watcher = new FileSystemWatcher { Path = @"c:\Temp", EnableRaisingEvents = true };

            eventSub =
                Observable.FromEventPattern<FileSystemEventArgs>(watcher, "Created")
                .Merge(Observable.FromEventPattern<FileSystemEventArgs>(watcher, "Changed"))
                .Merge(Observable.FromEventPattern<FileSystemEventArgs>(watcher, "Deleted"))
                .DistinctUntilChanged()
                .ObserveOn(fileList)
                .Subscribe(_ => RefreshFileList());

            RefreshFileList();
        }

        private void RefreshFileList()
        {
            fileList.Items.Clear();
            var files = from f in new DirectoryInfo(@"c:\temp").GetFiles()
                        orderby f.LastWriteTime descending
                        select f.Name;

            fileList.Items.AddRange(files.ToArray());
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                eventSub?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

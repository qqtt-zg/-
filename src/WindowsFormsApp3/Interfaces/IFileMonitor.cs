using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp3.Interfaces
{
    public interface IFileMonitor
    {
        event EventHandler<FileChangedEventArgs> FileChanged;
        void StartMonitoring(string path, bool includeSubdirectories = true);
        void StopMonitoring();
    }

    public class FileChangedEventArgs : EventArgs
    {
        public string FilePath { get; set; }
        public WatcherChangeTypes ChangeType { get; set; }
    }

    public enum WatcherChangeTypes
    {
        Changed,
        Created,
        Deleted,
        Renamed
    }
}
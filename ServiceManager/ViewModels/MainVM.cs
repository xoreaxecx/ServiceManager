using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ServiceManager.Models;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace ServiceManager.ViewModels
{
    class MainVM : ObservableObject
    {
        #region fields

        private bool _isRunning;
        private bool _isStopped;
        private int _selectedEntryIndex;
        private ObservableCollection<ObservableServiceEntry> _services = new ObservableCollection<ObservableServiceEntry>();
        private ObservableServiceEntry _selectedEntry = new ObservableServiceEntry();

        ICommand _getServices;
        ICommand _startService;
        ICommand _stopService;
        ICommand _restartService;

        #endregion

        #region properties

        public ObservableCollection<ObservableServiceEntry> Services
        {
            get { return _services; }
            set
            {
                if (value != _services)
                {
                    _services = value;
                    OnPropertyChanged("Services");
                }
            }
        }

        public ObservableServiceEntry SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                if (value != _selectedEntry)
                {
                    _selectedEntry = value;
                    OnPropertyChanged("SelectedEntry");
                }
            }
        }

        public int SelectedEntryIndex 
        {
            get { return _selectedEntryIndex; }
            set
            {
                if (value != _selectedEntryIndex)
                {
                    _selectedEntryIndex = value;
                    //OnPropertyChanged("SelectedEntryIndex");

                    if (_selectedEntryIndex >= 0)
                    {
                        IsRunning = Services[_selectedEntryIndex].StatusString == "Running";
                        IsStopped = Services[_selectedEntryIndex].StatusString == "Stopped";
                    }
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (value != _isRunning)
                {
                    _isRunning = value;
                    OnPropertyChanged("IsRunning");
                }
            }
        }

        public bool IsStopped
        {
            get
            {
                return _isStopped;
            }
            set
            {
                if (value != _isStopped)
                {
                    _isStopped = value;
                    OnPropertyChanged("IsStopped");
                }
            }
        }

        #endregion

        #region commands

        public ICommand GetServicesCommand
        {
            get
            {
                if (_getServices == null)
                {
                    _getServices = new RelayCommand(
                        async param => await Task.Run(() => GetServiceEntries()));
                }
                return _getServices;
            }
            set { }
        }

        public ICommand StartServiceCommand
        {
            get
            {
                if (_startService == null)
                {
                    _startService = new RelayCommand(
                        async param => await Task.Run(() => StartService()));
                }
                return _startService;
            }
            set { }
        }

        public ICommand StopServiceCommand
        {
            get
            {
                if (_stopService == null)
                {
                    _stopService = new RelayCommand(
                        async param => await Task.Run(() => StopService()));
                }
                return _stopService;
            }
            set { }
        }

        public ICommand RestartServiceCommand
        {
            get
            {
                if (_restartService == null)
                {
                    _restartService = new RelayCommand(
                        async param => await Task.Run(() => RestartService()));
                }
                return _restartService;
            }
            set { }
        }

        #endregion

        static unsafe ItemsSafeHandle GenerateItemsWrapper(out IntPtr items, out int itemsCount)
        {
            ItemsSafeHandle itemsHandle;

            if (!ServiceControl.GenerateItems(out itemsHandle, out items, out itemsCount))
            {
                throw new InvalidOperationException();
            }
            return itemsHandle;
        }

        private void UpdateStatus(IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                ServiceEntry temp = (ServiceEntry)Marshal.PtrToStructure(ptr, typeof(ServiceEntry));
                App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    Services[SelectedEntryIndex].PID = temp.PID;
                    Services[SelectedEntryIndex].StatusString = temp.StatusString;
                });
            }
        }

        private void GetServiceEntries()
        {
            App.Current.Dispatcher.BeginInvoke((Action)delegate () { Services.Clear(); });

            using (GenerateItemsWrapper(out IntPtr ptr, out int itemsCount))
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    ServiceEntry temp = (ServiceEntry)Marshal.PtrToStructure(ptr, typeof(ServiceEntry));
                    App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        Services.Add(temp.ToObservableServiceEntry());
                    }); 
                    ptr += Marshal.SizeOf(temp);
                }
            }
        }

        private void StartService()
        {
            string name = Services[SelectedEntryIndex].Name;
            IntPtr ptr;

            ServiceControl.CallStartService(name, out ptr);
            UpdateStatus(ptr);
            ServiceControl.ReleaseEntry(ptr);
        }

        private void StopService()
        {
            string name = Services[SelectedEntryIndex].Name;
            IntPtr ptr;

            ServiceControl.CallStopService(name, out ptr);
            UpdateStatus(ptr);
            ServiceControl.ReleaseEntry(ptr);
        }

        private void RestartService()
        {
            string name = Services[SelectedEntryIndex].Name;
            IntPtr ptr;

            ServiceControl.CallRestartService(name, out ptr);
            UpdateStatus(ptr);
            ServiceControl.ReleaseEntry(ptr);
        }

        public MainVM()
        {
            Task.Run(() => GetServiceEntries());
        }
    }
}

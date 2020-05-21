using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Threading;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using ServiceManager.Models;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace ServiceManager.ViewModels
{
    class MainVM : ObservableObject
    {
        #region fields

        ObservableCollection<ObservableServiceEntry> _services = new ObservableCollection<ObservableServiceEntry>();

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

        #endregion

        #region commands

        public ICommand GetServices
        {
            get
            {
                if (_getServices == null)
                {
                    _getServices = new RelayCommand(
                        param => Test());
                }
                return _getServices;
            }
            set { }
        }

        #endregion

        #region import

        [DllImport(@"E:\Room\C++\CppLib\Debug\ServiceLib.dll", EntryPoint = "GenerateItems", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        static unsafe extern bool GenerateItems(out ItemsSafeHandle itemsHandle,
            out IntPtr items, out int itemCount);

        [DllImport(@"E:\Room\C++\CppLib\Debug\ServiceLib.dll", EntryPoint = "ReleaseItems", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        static unsafe extern bool ReleaseItems(IntPtr itemsHandle);

        static unsafe ItemsSafeHandle GenerateItemsWrapper(out IntPtr items, out int itemsCount)
        {
            ItemsSafeHandle itemsHandle;

            if (!GenerateItems(out itemsHandle, out items, out itemsCount))
            {
                throw new InvalidOperationException();
            }
            return itemsHandle;
        }

        class ItemsSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            public ItemsSafeHandle()
                : base(true)
            {
            }

            protected override bool ReleaseHandle()
            {
                return ReleaseItems(handle);
            }
        }

        #endregion

        public void GetServiceEntries()
        {
            Services.Clear();

            using (GenerateItemsWrapper(out IntPtr ptr, out int itemsCount))
            {
                for (int i = 0; i < itemsCount; i++)
                {
                    ServiceEntry temp = (ServiceEntry)Marshal.PtrToStructure(ptr, typeof(ServiceEntry));
                    Services.Add(new ObservableServiceEntry
                    {
                        Name = temp.Name,
                        Description = temp.Description,
                        Group = temp.Group,
                        Path = temp.Path,
                        PID = temp.PID,
                        StatusString = temp.StatusString
                    });

                    ptr += Marshal.SizeOf(temp);
                }
            }
        }

        public MainVM()
        {
            GetServiceEntries();
            //ServiceEntry temp = new ServiceEntry { Name = "some", Description = "desc", Group = "group", Path = "path", PID = "123", StatusString = "running" };

            //Services.Add(new ObservableServiceEntry 
            //{ 
            //    Name = temp.Name,
            //    Description = temp.Description,
            //    Group = temp.Group,
            //    Path = temp.Path,
            //    PID = temp.PID,
            //    StatusString = temp.StatusString
            //});
            //Services.Add(new ObservableServiceEntry
            //{
            //    Name = temp.Name,
            //    Description = temp.Description,
            //    Group = temp.Group,
            //    Path = temp.Path,
            //    PID = temp.PID,
            //    StatusString = temp.StatusString
            //});
        }

        public void Test()
        {
            ServiceEntry temp = new ServiceEntry { Name = "some", Description = "desc", Group = "group", Path = "path", PID = "123", StatusString = "running" };

            Services.Add(new ObservableServiceEntry
            {
                Name = temp.Name,
                Description = temp.Description,
                Group = temp.Group,
                Path = temp.Path,
                PID = temp.PID,
                StatusString = temp.StatusString
            });
        }
    }
}

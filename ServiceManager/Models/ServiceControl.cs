using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Models
{
    public static class ServiceControl
    {
        [DllImport(@"ServiceLib.dll", EntryPoint = "CallStartService", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static unsafe extern bool CallStartService(
            [MarshalAs(UnmanagedType.LPStr)] string name, out IntPtr item);

        [DllImport(@"ServiceLib.dll", EntryPoint = "CallStopService", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static unsafe extern bool CallStopService(
            [MarshalAs(UnmanagedType.LPStr)] string name, out IntPtr item);

        [DllImport(@"ServiceLib.dll", EntryPoint = "CallRestartService", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static unsafe extern bool CallRestartService(
            [MarshalAs(UnmanagedType.LPStr)] string name, out IntPtr item);

        [DllImport(@"ServiceLib.dll", EntryPoint = "GenerateItems", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static unsafe extern bool GenerateItems(out ItemsSafeHandle itemsHandle,
            out IntPtr items, out int itemCount);

        [DllImport(@"ServiceLib.dll", EntryPoint = "ReleaseItems", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static unsafe extern bool ReleaseItems(IntPtr itemsHandle);

        [DllImport(@"ServiceLib.dll", EntryPoint = "ReleaseEntry", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static unsafe extern bool ReleaseEntry(IntPtr itemsHandle);
    }
}

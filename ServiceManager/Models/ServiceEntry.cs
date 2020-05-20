using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ServiceEntry
    {
        [MarshalAsAttribute(UnmanagedType.BStr)]
        public string Name;
        [MarshalAsAttribute(UnmanagedType.BStr)]
        public string Description;
        [MarshalAsAttribute(UnmanagedType.BStr)]
        public string StatusString;
        [MarshalAsAttribute(UnmanagedType.BStr)]
        public string Group;
        [MarshalAsAttribute(UnmanagedType.BStr)]
        public string Path;
        [MarshalAsAttribute(UnmanagedType.BStr)]
        public string PID;
    }
}

using System.Runtime.InteropServices;

namespace ServiceManager.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ServiceEntry
    {
        [MarshalAs(UnmanagedType.BStr)]
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

        public ObservableServiceEntry ToObservableServiceEntry()
        {
            return new ObservableServiceEntry
            {
                Name = this.Name,
                Description = this.Description,
                Group = this.Group,
                Path = this.Path,
                PID = this.PID,
                StatusString = this.StatusString
            };
        }
    }
}

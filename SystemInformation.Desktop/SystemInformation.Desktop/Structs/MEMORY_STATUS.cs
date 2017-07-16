using System.Runtime.InteropServices;

namespace SystemInformation.Desktop.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_STATUS
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public uint dwTotalPhys;
        public uint dwAvailPhys;
        public uint dwTotalPageFile;
        public uint dwAvailPageFile;
        public uint dwTotalVirtual;
        public uint dwAvailVirtual;
    }
}

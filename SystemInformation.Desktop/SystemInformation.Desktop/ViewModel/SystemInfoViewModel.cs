using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using SystemInformation.Desktop.Constants;
using SystemInformation.Desktop.Model;
using SystemInformation.Desktop.Structs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace SystemInformation.Desktop.ViewModel
{
    public class SystemInfoViewModel : ViewModelBase
    {
        private ObservableCollection<SystemModel> collection;

        [DllImport("kernel32")]
        static extern void GetSystemInfo(ref SYSTEM_INFO pSI);

        [DllImport("kernel32")]
        static extern void GlobalMemoryStatus(ref MEMORY_STATUS buf);

        public SystemInfoViewModel()
        {
            this.GetISysInfo();
        }

        public ObservableCollection<SystemModel> SystemCollection
        {
            get => this.collection;
            set
            {
                this.collection = value;
                this.RaisePropertyChanged();
            }
        }

        private void GetISysInfo()
        {
            try
            {
                var pSI = new SYSTEM_INFO();
                GetSystemInfo(ref pSI);
                string CpuType;
                switch (pSI.dwProcessorType)
                {
                    case CpuConstants.PROCESSOR_INTEL_386:
                        CpuType = "Intel 386";
                        break;
                    case CpuConstants.PROCESSOR_INTEL_486:
                        CpuType = "Intel 486";
                        break;
                    case CpuConstants.PROCESSOR_INTEL_PENTIUM:
                        CpuType = "Intel Pentium";
                        break;
                    case CpuConstants.PROCESSOR_MIPS_R4000:
                        CpuType = "MIPS R4000";
                        break;
                    case CpuConstants.PROCESSOR_ALPHA_21064:
                        CpuType = "DEC Alpha 21064";
                        break;
                    default:
                        CpuType = "Unknown";
                        break;
                }

                var memStat = new MEMORY_STATUS();
                GlobalMemoryStatus(ref memStat);

                this.SystemCollection = new ObservableCollection<SystemModel>
                {
                    new SystemModel{ Name = "Active Processor Mask", Message = pSI.dwActiveProcessorMask.ToString()},
                    new SystemModel{ Name = "Allocation Granularity", Message = pSI.dwAllocationGranularity.ToString()},
                    new SystemModel{ Name = "Number Of Processors", Message = pSI.dwNumberOfProcessors.ToString()},
                    new SystemModel{ Name = "OEM ID", Message = pSI.dwOemId.ToString()},
                    new SystemModel{ Name = "Page Size", Message = pSI.dwPageSize.ToString()},
                    new SystemModel{ Name = "Processor Level Value", Message = pSI.dwProcessorLevel.ToString()},
                    new SystemModel{ Name = "Processor Revision", Message = pSI.dwProcessorRevision.ToString()},
                    new SystemModel{ Name = "CPU type", Message = CpuType},
                    new SystemModel{ Name = "Maximum Application Address", Message = pSI.lpMaximumApplicationAddress.ToString()},
                    new SystemModel{ Name = "Available Page File", Message = (memStat.dwAvailPageFile/1024).ToString()},
                    new SystemModel{ Name = "Available Physical Memory", Message = (memStat.dwAvailPhys/1024).ToString()},
                    new SystemModel{ Name = "Available Virtual Memory", Message = (memStat.dwAvailVirtual/1024).ToString()},
                    new SystemModel{ Name = "Size of structur", Message = memStat.dwLength.ToString()},
                    new SystemModel{ Name = "Memory In Use", Message = memStat.dwMemoryLoad.ToString()},
                    new SystemModel{ Name = "Total Page Size", Message = (memStat.dwTotalPageFile/1024).ToString()},
                    new SystemModel{ Name = "Total Physical Memory", Message = (memStat.dwTotalPhys/1024).ToString()},
                    new SystemModel{ Name = "Total Virtual Memory", Message = (memStat.dwTotalVirtual/1024).ToString()},
                };
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.StackTrace}", "Error");
            }
        }
    }
}

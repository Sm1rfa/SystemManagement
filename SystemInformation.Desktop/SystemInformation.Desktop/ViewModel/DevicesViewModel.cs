using System;
using System.Collections.ObjectModel;
using System.Management;
using System.Windows;
using SystemInformation.Desktop.Model;
using GalaSoft.MvvmLight;

namespace SystemInformation.Desktop.ViewModel
{
    public class DevicesViewModel : ViewModelBase
    {
        private ObservableCollection<SystemModel> videoCollection;
        private ObservableCollection<SystemModel> cpuCollection;

        public DevicesViewModel()
        {
            this.GetAllVideoDevices();
            this.GetProcessorInfo();
        }

        public ObservableCollection<SystemModel> VideoCollection
        {
            get => this.videoCollection;
            set
            {
                this.videoCollection = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<SystemModel> CpuCollection
        {
            get => this.cpuCollection;
            set
            {
                this.cpuCollection = value;
                this.RaisePropertyChanged();
            }
        }

        private void GetAllVideoDevices()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT * FROM Win32_VideoController");
                foreach (var item in searcher.Get())
                {
                    var videoState = Constants.Constants.TranslateVideoAvailability(item["Availability"]);
                    this.VideoCollection = new ObservableCollection<SystemModel>
                    {
                        new SystemModel{ Name = "Name", Message = item["Name"].ToString()},
                        new SystemModel{ Name = "Description", Message = item["Description"].ToString()},
                        new SystemModel{ Name = "Adapter Compatibility", Message = item["AdapterCompatibility"].ToString()},
                        new SystemModel{ Name = "Adapter DAC Type", Message = item["AdapterDACType"].ToString()},
                        new SystemModel{ Name = "Caption", Message = item["Caption"].ToString()},
                        new SystemModel{ Name = "Availability", Message = videoState}
                    };
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.StackTrace}", "Error");
            }
        }

        private void GetProcessorInfo()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT * FROM Win32_Processor");
                foreach (var item in searcher.Get())
                {
                    this.CpuCollection = new ObservableCollection<SystemModel>
                    {
                        new SystemModel{ Name = "Name", Message = item["Name"].ToString()},
                        new SystemModel{ Name = "Description", Message = item["Description"].ToString()}
                    };
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.StackTrace}", "Error");
            }
        }
    }
}

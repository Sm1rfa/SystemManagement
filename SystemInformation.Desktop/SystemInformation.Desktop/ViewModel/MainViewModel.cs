using GalaSoft.MvvmLight;

namespace SystemInformation.Desktop.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private int selectedTabIndex;

        public SystemInfoViewModel SystemInfoViewModel { get; set; }
        public DevicesViewModel DevicesViewModel { get; set; }
        public ProcessesViewModel ProcessesViewModel { get; set; }

        public MainViewModel()
        {
            this.SystemInfoViewModel = new SystemInfoViewModel();
            this.DevicesViewModel = new DevicesViewModel();
            this.ProcessesViewModel = new ProcessesViewModel();
        }

        public int SelectedTabIndex
        {
            get
            {
                return this.selectedTabIndex;
            }

            set
            {
                if (value == this.selectedTabIndex)
                {
                    return;
                }

                this.selectedTabIndex = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
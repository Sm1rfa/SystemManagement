using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Data;
using SystemInformation.Desktop.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace SystemInformation.Desktop.ViewModel
{
    public class ProcessesViewModel : ViewModelBase
    {
        private ObservableCollection<SystemModel> processCollection;
        private CollectionViewSource proccessView;
        private string filterText;
        private SystemModel selectedProcess;

        public ProcessesViewModel()
        {
            var list = this.GetAllProcesses();
            this.proccessView = new CollectionViewSource{ Source = list };
            this.proccessView.Filter += ProcessFileter;

            this.KillProcessCommand = new RelayCommand(KillProcess);
        }

        public RelayCommand KillProcessCommand { get; set; }

        public ObservableCollection<SystemModel> ProcessCollection
        {
            get => this.processCollection;
            set
            {
                this.processCollection = value;
                this.RaisePropertyChanged();
            }
        }

        public ICollectionView ProcessView
        {
            get => this.proccessView.View;
        }

        public string FilterText
        {
            get => this.filterText;
            set
            {
                this.filterText = value;
                this.RaisePropertyChanged();
                this.proccessView.View.Refresh();
            }
        }

        public SystemModel SelectedProcess
        {
            get => this.selectedProcess;
            set
            {
                this.selectedProcess = value;
                this.RaisePropertyChanged();
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private IEnumerable<SystemModel> GetAllProcesses()
        {
            Process[] localAll = Process.GetProcesses();
            this.ProcessCollection = new ObservableCollection<SystemModel>();
            foreach (var item in localAll)
            {
                this.ProcessCollection.Add(new SystemModel { Name = item.ProcessName});
            }

            return this.ProcessCollection;
        }

        private void KillProcess()
        {
            var result = MessageBox.Show("Do you want to kill the process?", "Question", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                foreach (var item in Process.GetProcessesByName(this.SelectedProcess.Name))
                {
                    if (item == null)
                    {
                        MessageBox.Show("Error! Can't kill the process.", "Error");
                    }
                    item.Kill();
                }
            }
        }

        private void ProcessFileter(object obj, FilterEventArgs e)
        {
            SystemModel proc = e.Item as SystemModel;
            if (string.IsNullOrEmpty(this.FilterText))
            {
                e.Accepted = true;
                return;
            }
            if (proc.Name.Contains(this.filterText))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }
    }
}

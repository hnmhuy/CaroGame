using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Diagnostics;

namespace CaroGame.ViewModel
{
    public class ChangeDimensionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        // Override the OnPropertyChanged method
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }
        public bool IsConfirmed { get; set; }
        private bool _canConfirm;
        public bool CanConfirm
        {
            get { return _canConfirm; }
            set
            {
                _canConfirm = value;
                OnPropertyChanged(nameof(CanConfirm));
            }
        }

        private int _sizeRow;
        public int SizeRow
        {
            get { return _sizeRow; }
            set
            {
                _sizeRow = value;
                OnPropertyChanged(nameof(SizeRow));
                ConfirmValidation();
            }
        }

        private int _sizeColumn;
        public int SizeColumn
        {
            get { return _sizeColumn; }
            set
            {
                _sizeColumn = value;
                OnPropertyChanged(nameof(SizeColumn));
                ConfirmValidation();
            }
        }


        public ChangeDimensionViewModel(int row, int col)
        {
            this.SizeRow = row;
            this.SizeColumn = col;
            this.CancelCommand = new RelayCommand(OnCancel);
            this.ConfirmCommand = new RelayCommand(OnConfirm);
        }

        private void OnConfirm()
        {
            this.IsConfirmed = true;
        }

        private void OnCancel()
        {
            this.IsConfirmed = false;
        }

        private void ConfirmValidation()
        {
            bool res = SizeRow >= 5 && SizeRow <= 25 && SizeColumn >= 5 && SizeColumn <= 25;
            Debug.WriteLine("CanConfirm: " + res);
            CanConfirm = res;
        }
    }
}

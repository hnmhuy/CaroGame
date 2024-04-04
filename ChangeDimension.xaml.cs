using CaroGame.ViewModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace CaroGame
{
    /// <summary>
    /// Interaction logic for ChangeDimension.xaml
    /// </summary>
    public partial class ChangeDimension : Window
    {
        public ChangeDimensionViewModel viewModel { get; set; }
        public ChangeDimension(int row, int col)
        {
            InitializeComponent();
            viewModel = new ChangeDimensionViewModel(row, col);
            this.DataContext = viewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class DimensionValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int val;
            if (int.TryParse(value.ToString(), out val))
            {
                if (val < 5 || val > 30)
                {
                    return new ValidationResult(false, "The value must be between 5 and 20");
                }
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "The value must be an integer");
        }
    }
}

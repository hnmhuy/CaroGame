using CaroGame.GameMaterial;
using CaroGame.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace CaroGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ChessBoard chessBoard;
        private int BOARD_SIZE_DEFAULT = 12;
        private MainViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            chessBoard.MoveCursor(e.Key);
            this.viewModel = new MainViewModel(chessBoard);
            this.DataContext = this.viewModel;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.chessBoard = new ChessBoard(this.GameBoard, BOARD_SIZE_DEFAULT, BOARD_SIZE_DEFAULT);
        }

        private void ChangeSize_Click(object sender, RoutedEventArgs e)
        {
            ChangeDimension changeDimension = new ChangeDimension(chessBoard.SizeRow, chessBoard.SizeColumn);
            changeDimension.ShowDialog();
            if (changeDimension.viewModel.IsConfirmed)
            {
                chessBoard.Resize(changeDimension.viewModel.SizeRow, changeDimension.viewModel.SizeColumn);
            }
        }
    }
}
using CaroGame.GameMaterial;
using CaroGame.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
        private MediaPlayer backgroundPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            chessBoard.MoveCursor(e.Key);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.chessBoard = new ChessBoard(this.GameBoard, BOARD_SIZE_DEFAULT, BOARD_SIZE_DEFAULT);
            this.viewModel = new MainViewModel(chessBoard);
            this.DataContext = this.viewModel;
            viewModel.BackgroundPlayer = backgroundPlayer;
            viewModel.PlaySound("background.mp3");
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

        private void Background_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Replay the background music when it ends
            (sender as MediaElement).Play();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SaveGame();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.LoadGame();
        }
    }
}
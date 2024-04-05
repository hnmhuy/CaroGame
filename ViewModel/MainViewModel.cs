using CaroGame.GameMaterial;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Media;

namespace CaroGame.ViewModel
{
    public class MainViewModel
    {
        private ChessBoard _chessBoard;
        public ChessBoard ChessBoard
        {
            get { return _chessBoard; }
            set { _chessBoard = value; }
        }
        public RelayCommand RestartGame { get; set; }

        private MediaPlayer backgroundPlayer;
        public MediaPlayer BackgroundPlayer
        {
            get { return backgroundPlayer; }
            set { backgroundPlayer = value; }
        }
        public MainViewModel(ChessBoard chessBoard)
        {
            _chessBoard = chessBoard;
            RestartGame = new RelayCommand(() =>
            {
                _chessBoard.RestartGame();
            });
        }

        public void PlaySound(string soundName)
        {
            // Get the path of running file
            string soundPath = "./Sounds/" + soundName;
            Debug.WriteLine(soundPath);
            // Set the source for the background player
            backgroundPlayer.Open(new System.Uri(soundPath, System.UriKind.Relative));
            // Chekc if the background player can loaded the source
            Debug.WriteLine(backgroundPlayer.Source);

            backgroundPlayer.Play();
            backgroundPlayer.MediaEnded += Replay;
        }

        private void Replay(object? sender, EventArgs e)
        {
            backgroundPlayer?.Play();
        }

        public void ResizeBoard(int row, int col)
        {
            _chessBoard.Resize(row, col);
        }

        // Save and load game section

        public void SaveGame()
        {
            FileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Caro Game (*.caro)|*.caro";
            if (fileDialog.ShowDialog() == true)
            {
                _chessBoard.SaveGame(fileDialog.FileName);
            }
        }

        public void LoadGame()
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Caro Game (*.caro)|*.caro";
            if (fileDialog.ShowDialog() == true)
            {
                _chessBoard.LoadGame(fileDialog.FileName);
            }
        }
    }
}

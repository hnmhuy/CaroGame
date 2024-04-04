using CaroGame.GameMaterial;
using CommunityToolkit.Mvvm.Input;

namespace CaroGame.ViewModel
{
    public class MainViewModel
    {
        private ChessBoard _chessBoard;
        public RelayCommand RestartGame { get; set; }
        public MainViewModel(ChessBoard chessBoard)
        {
            _chessBoard = chessBoard;
            RestartGame = new RelayCommand(() =>
            {
                _chessBoard.RestartGame();
            });
        }

        public void ResizeBoard(int row, int col)
        {
            _chessBoard.Resize(row, col);
        }

    }
}

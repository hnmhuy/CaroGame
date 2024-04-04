using CommunityToolkit.Mvvm.Input;

namespace CaroGame
{
    public class ViewModel
    {
        private ChessBoard _chessBoard;
        public RelayCommand RestartGame { get; set; }
        public ViewModel(ChessBoard chessBoard)
        {
            this._chessBoard = chessBoard;
            RestartGame = new RelayCommand(() =>
            {
                _chessBoard.RestartGame();
            });
        }

    }
}

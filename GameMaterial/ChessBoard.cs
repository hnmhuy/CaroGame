using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CaroGame.GameMaterial
{
    public class ChessBoard
    {
        private Canvas _board;
        private double _cellWidth = 0;
        private double _cellHeight = 0;
        Brush _cellColor = Brushes.White;
        Brush _boardColor = Brushes.LightBlue;
        private int _sizeRow = 12;
        private int _sizeColumn = 12;
        public Mark cursor;
        private bool isControlByKeyboard = false;
        private Point mousePos;
        private bool disableKeyboard = false;
        private CaroStrategy caroStrategy;
        public MarkType playingRole = MarkType.Cross;
        private Line? winingLine = null;

        private MediaPlayer foregroundPlayer;


        // Getters and setters
        public Brush CellColor
        {
            get { return _cellColor; }
            set { _cellColor = value; }
        }
        public Canvas Board
        {
            get { return _board; }
            set { _board = value; }
        }

        public int SizeRow
        {
            get { return _sizeRow; }
            set { _sizeRow = value; }
        }
        public int SizeColumn
        {
            get { return _sizeColumn; }
            set { _sizeColumn = value; }
        }

        public ChessBoard(Canvas gameBoard, int sizeRow, int sizeColumn)
        {
            Board = gameBoard;
            SizeRow = sizeRow;
            SizeColumn = sizeColumn;
            GenerateChessBoard();
            Board.SizeChanged += GameBoard_SizeChange;
            Board.MouseMove += GameBoard_MouseMove;
            caroStrategy = new CaroStrategy(sizeRow, sizeColumn);
            foregroundPlayer = new MediaPlayer();

            foregroundPlayer.Open(new System.Uri("./Sounds/mark.wav", System.UriKind.Relative));
        }


        private void GameBoard_MouseMove(object sender, MouseEventArgs e)
        {

            // Checking if the mouse is moving on the board
            Point pos = e.GetPosition(Board);
            if (pos == mousePos) return;
            mousePos = pos;
            if (cursor.Type == MarkType.None) cursor.Type = playingRole;

            int row = (int)(pos.Y / _cellHeight);
            int col = (int)(pos.X / _cellWidth);
            if (row >= 0 && row < _sizeRow && col >= 0 && col < _sizeColumn)
            {
                cursor.SetIndex(col, row);
            }
            Debug.WriteLine("Cursor is in: " + row + " - " + col);
        }

        private void GameBoard_SizeChange(object sender, SizeChangedEventArgs e)
        {
            double scaleX = e.NewSize.Width / e.PreviousSize.Width;
            double scaleY = e.NewSize.Height / e.PreviousSize.Height;
            // Update the cell size
            _cellWidth *= scaleX;
            _cellHeight *= scaleY;
            foreach (var item in Board.Children)
            {
                if (item is Rectangle cell)
                {
                    ScaleCell(cell, scaleX, scaleY);
                }
                else if (item is Mark mark)
                {
                    mark.ScaleMark(scaleX, scaleY);
                }
                else if (item is Line line)
                {
                    line.X1 *= scaleX;
                    line.X2 *= scaleX;
                    line.Y1 *= scaleY;
                    line.Y2 *= scaleY;
                }
            }
        }

        private void ScaleCell(Rectangle cell, double scaleX, double scaleY)
        {
            cell.Width *= scaleX;
            cell.Height *= scaleY;
            Canvas.SetLeft(cell, Canvas.GetLeft(cell) * scaleX);
            Canvas.SetTop(cell, Canvas.GetTop(cell) * scaleY);
        }

        private Rectangle GenerateCell()
        {
            Rectangle cell = new Rectangle { Width = (int)_cellWidth, Height = (int)_cellHeight };
            cell.Stroke = _cellColor;
            cell.MouseDown += Cell_MouseDown;
            return cell;
        }

        private void Cell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine(sender);

            if (sender is Mark mark)
            {
                MarkACell(mark.GetIndex());
            }
            else if (sender is Rectangle rect)
            {
                MarkACell(CalIndexOfCell(rect));
            }
        }

        private void GenerateChessBoard()
        {
            Board.IsEnabled = true;
            this.disableKeyboard = false;
            Board.Children.Clear();
            Board.Background = _boardColor;

            double width = Board.ActualWidth;
            double height = Board.ActualHeight;
            _cellWidth = width / _sizeColumn;
            _cellHeight = height / _sizeRow;
            for (int i = 0; i < _sizeRow; i++)
            {
                for (int j = 0; j < _sizeColumn; j++)
                {
                    Rectangle _cell = GenerateCell();
                    Board.Children.Add(_cell);
                    Canvas.SetLeft(_cell, j * _cellWidth);
                    Canvas.SetTop(_cell, i * _cellHeight);
                }
            }
            cursor = new Mark(_cellWidth, _cellHeight, MarkType.None, true);
            cursor.MouseDown += Cell_MouseDown;
            Board.Children.Add(cursor);
            this.cursor.Opacity = 0.5;
        }

        private Point CalIndexOfCell(Rectangle rect)
        {
            double left = Canvas.GetLeft(rect) / _cellWidth;
            double top = Canvas.GetTop(rect) / _cellHeight;
            int x = (int)Math.Round(left);
            int y = (int)Math.Round(top);
            Point res = new Point(x, y);
            Debug.WriteLine("New point: " + res.X + " " + res.Y);
            return res;
        }


        public void MoveCursor(Key key)
        {
            if (disableKeyboard) return;
            if ((Keyboard.GetKeyStates(key) & KeyStates.Down) > 0 && key != Key.Return)
            {
                if (cursor.Type == MarkType.None) cursor.Type = playingRole;
                Point currPos = cursor.GetIndex();
                switch (key)
                {
                    case Key.Up:
                        if (currPos.Y > 0) currPos.Y--;
                        break;
                    case Key.Down:
                        if (currPos.Y < _sizeRow - 1) currPos.Y++;
                        break;
                    case Key.Left:
                        if (currPos.X > 0) currPos.X--;
                        break;
                    case Key.Right:
                        if (currPos.X < _sizeColumn - 1) currPos.X++;
                        break;
                    default:
                        break;
                }
                cursor.SetIndex((int)currPos.X, (int)currPos.Y);
            }
            else if (key == Key.Return)
            {
                MarkACell(cursor.GetIndex());
            }
        }

        public void playSound(string filename)
        {
            foregroundPlayer.Open(new System.Uri("./Sounds/" + filename, System.UriKind.Relative));
            foregroundPlayer.Play();
        }

        public void MarkACell(Point index)
        {
            //Mark mark = new Mark(_cellWidth, _cellHeight, playingRole);
            //mark.Color = playingRole == MarkType.Cross ? Brushes.Orange : Brushes.Blue;
            //Board.Children.Add(mark);
            //mark.SetIndex(index);
            //this.cursor.Type = MarkType.None;
            //playingRole = playingRole == MarkType.Cross ? MarkType.Circle : MarkType.Cross;

            if (caroStrategy.Mark(index, playingRole))
            {

                Mark newMark = new Mark(_cellWidth, _cellHeight, playingRole);
                playSound("mark.wav");
                Board.Children.Add(newMark);
                newMark.SetIndex(index);
                playingRole = playingRole == MarkType.Cross ? MarkType.Circle : MarkType.Cross;
                newMark.Color = playingRole == MarkType.Cross ? Brushes.Orange : Brushes.Blue;
                if (caroStrategy.IsOver(index))
                {
                    playSound("winning.wav");
                    string message;
                    if (caroStrategy.Winner != MarkType.None)
                    {
                        DrawWinningLine();
                        message = "The winner is: " + caroStrategy.Winner + "!!! Do you want to restart the game?";
                    }
                    else
                    {
                        message = "The game is draw!!! Do you want to restart the game?";
                    }
                    var res = MessageBox.Show(message, "Notification", MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        RestartGame();
                    }
                    else
                    {
                        this.Board.IsEnabled = false;
                        disableKeyboard = true;
                    }
                }
                cursor.Type = MarkType.None;
            }
            else
            {
                MessageBox.Show("This cell is already marked!");
                return;
            }

        }

        public void RestartGame()
        {
            caroStrategy.ResetGame();
            Board.Children.Clear();
            GenerateChessBoard();

        }

        private void DrawWinningLine()
        {
            // Get the smallest and the largest point in the list
            Point min = caroStrategy.listPoint.OrderBy(p => p.X).ThenBy(p => p.Y).First();
            Point max = caroStrategy.listPoint.OrderByDescending(p => p.X).ThenByDescending(p => p.Y).First();
            // Draw the line

            Line line = new Line()
            {
                X1 = min.X * _cellWidth + _cellWidth / 2,
                Y1 = min.Y * _cellHeight + _cellHeight / 2,
                X2 = max.X * _cellWidth + _cellWidth / 2,
                Y2 = max.Y * _cellHeight + _cellHeight / 2,
                Stroke = Brushes.Red,
                StrokeThickness = 3
            };

            Board.Children.Add(line);
            this.winingLine = line;
        }

        public static void RestartGame(ChessBoard chessBoard)
        {
            if (chessBoard == null)
            {
                return;
            }
            else chessBoard.RestartGame();
        }

        public void Resize(int sizeRow, int sizeColumn)
        {
            SizeRow = sizeRow;
            SizeColumn = sizeColumn;
            this.caroStrategy.Resize(sizeRow, sizeColumn);
            RestartGame();
        }

        public void SaveGame(string filename)
        {
            // Save as binary file
            caroStrategy.SaveGame(filename);
        }

        public void LoadGame(string filename)
        {
            // Load the game from the file
            caroStrategy.LoadGame(filename);
            this.playingRole = caroStrategy.CurRole == MarkType.Cross ? MarkType.Circle : MarkType.Cross;
            this.cursor.Type = playingRole;
            this.SizeColumn = caroStrategy.SizeColumn;
            this.SizeRow = caroStrategy.SizeRow;
            // Reload the game board
            this.Board.Children.Clear();
            GenerateChessBoard();
            RemarkGame(caroStrategy.Board);
            if (caroStrategy.Winner != MarkType.None)
            {
                DrawWinningLine();
                MessageBox.Show("The winner is: " + caroStrategy.Winner);
                this.Board.IsEnabled = false;
                disableKeyboard = true;
                // remove the cursor
                this.Board.Children.Remove(cursor);
            }
        }

        private void RemarkGame(int[,] board)
        {
            for (int i = 0; i < SizeRow; i++)
            {
                for (int j = 0; j < SizeColumn; j++)
                {
                    if (board[i, j] != 0)
                    {
                        Mark mark = new Mark(_cellWidth, _cellHeight, (MarkType)board[i, j]);
                        mark.SetIndex(i, j);
                        mark.Color = board[i, j] == (int)MarkType.Cross ? Brushes.Blue : Brushes.Orange;
                        Board.Children.Add(mark);
                    }
                }
            }
        }
    }
}

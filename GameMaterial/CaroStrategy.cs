
using System.Windows;

namespace CaroGame.GameMaterial
{
    public class CaroStrategy
    {
        int _sizeRow;
        int _sizeColumn;
        int[,] board;
        MarkType curRole;
        MarkType winner = MarkType.None;
        public List<Point> listPoint = new List<Point>();
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
        public MarkType Winner
        {
            get { return winner; }
        }

        public CaroStrategy(int sizeRow, int sizeColumn)
        {
            SizeRow = sizeRow;
            SizeColumn = sizeColumn;
            // Generate the board with default value is 0
            board = new int[sizeRow, sizeColumn];
            ResetGame();
        }

        public void ResetGame()
        {
            Array.Clear(board, (int)MarkType.None, board.Length);
        }

        public bool Mark(Point pos, MarkType role)
        {
            if (board[(int)pos.X, (int)pos.Y] == (int)MarkType.None)
            {
                curRole = role;
                board[(int)pos.X, (int)pos.Y] = (int)role;
                return true;
            }
            else return false;
        }

        public void Resize(int row, int col)
        {
            this.SizeRow = row;
            this.SizeColumn = col;
            // Delete the current board
            board = new int[row, col];
            Array.Clear(board, (int)MarkType.None, board.Length);
        }

        public bool IsOver(Point pos)
        {
            int count = 1;
            listPoint.Clear();
            int row = (int)pos.X;
            int col = (int)pos.Y;
            // Check row 
            for (int i = 1; i < 5; i++)
            {
                if (row - i >= 0 && board[row - i, col] == (int)curRole)
                {
                    count++;
                    listPoint.Add(new Point(row - i, col));
                }
                if (row + i < _sizeRow && board[row + i, col] == (int)curRole)
                {
                    count++;
                    listPoint.Add(new Point(row + i, col));
                }
                if (count == 5)
                {
                    winner = curRole;
                    listPoint.Add(new Point(row, col));
                    return true;
                }
            }

            // Have not return yet, means that the row is not over
            // Check column 
            count = 1;
            listPoint.Clear();
            for (int i = 1; i < 5; i++)
            {
                if (col - i >= 0 && board[row, col - i] == (int)curRole)
                {
                    count++;
                    listPoint.Add(new Point(row, col - i));
                }
                if (col + i < _sizeColumn && board[row, col + i] == (int)curRole)
                {
                    {
                        count++;
                        listPoint.Add(new Point(row, col + i));
                    }
                }
                if (count == 5)
                {
                    winner = curRole;
                    listPoint.Add(new Point(row, col));
                    return true;
                }
            }
            // Check the main dianogal
            count = 1;
            listPoint.Clear();
            for (int i = 1; i < 5; i++)
            {
                if (col - i >= 0 && row - i >= 0 && board[row - i, col - i] == (int)curRole)
                {
                    count++;
                    listPoint.Add(new Point(row - i, col - i));
                }
                if (col + i < _sizeColumn && row + i < _sizeRow && board[row + i, col + i] == (int)curRole)
                {
                    {
                        count++;
                        listPoint.Add(new Point(row + i, col + i));
                    }
                }
                if (count == 5)
                {
                    winner = curRole;
                    listPoint.Add(new Point(row, col));
                    return true;
                }
            }

            // Check the sub diagonal
            count = 1;
            listPoint.Clear();
            for (int i = 1; i < 5; i++)
            {
                if (col - i >= 0 && row + i < _sizeRow && board[row + i, col - i] == (int)curRole)
                {
                    count++;
                    listPoint.Add(new Point(row + i, col - i));
                }
                if (col + i < _sizeColumn && row - i >= 0 && board[row - i, col + i] == (int)curRole)
                {
                    count++;
                    listPoint.Add(new Point(row - i, col + i));
                }
                if (count == 5)
                {
                    winner = curRole;
                    listPoint.Add(new Point(row, col));
                    return true;
                }
            }
            return false;
        }
    }
}

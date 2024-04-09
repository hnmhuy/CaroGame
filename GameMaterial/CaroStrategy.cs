
using System.IO;
using System.Windows;

namespace CaroGame.GameMaterial
{
    public class CaroStrategy
    {
        int _sizeRow;
        int _sizeColumn;
        int _markedCount = 0;
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

        public MarkType CurRole
        {
            get { return curRole; }
        }

        public int[,] Board
        {
            get { return board; }
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
            curRole = MarkType.None;
            winner = MarkType.None;
            Array.Clear(board, (int)MarkType.None, board.Length);
            _markedCount = 0;
        }

        public bool Mark(Point pos, MarkType role)
        {
            if (board[(int)pos.X, (int)pos.Y] == (int)MarkType.None)
            {
                curRole = role;
                board[(int)pos.X, (int)pos.Y] = (int)role;
                _markedCount++;
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
            int countLeft = 1;
            int countRight = 1;
            listPoint.Clear();
            int row = (int)pos.X;
            int col = (int)pos.Y;
            // Check row 
            for (int i = 1; i < 5; i++)
            {
                if (row - i >= 0 && board[row - i, col] == (int)curRole)
                {
                    countLeft++;
                    listPoint.Add(new Point(row - i, col));
                }
                if (row + i < _sizeRow && board[row + i, col] == (int)curRole)
                {
                    countRight++;
                    listPoint.Add(new Point(row + i, col));
                }
                if (countLeft == 5 || countRight == 5)
                {
                    winner = curRole;
                    listPoint.Add(new Point(row, col));
                    return true;
                }
            }

            // Have not return yet, means that the row is not over
            // Check column 
            countLeft = 1;
            countRight = 1;
            listPoint.Clear();
            for (int i = 1; i < 5; i++)
            {
                if (col - i >= 0 && board[row, col - i] == (int)curRole)
                {
                    countLeft++;
                    listPoint.Add(new Point(row, col - i));
                }
                if (col + i < _sizeColumn && board[row, col + i] == (int)curRole)
                {
                    countRight++;
                    listPoint.Add(new Point(row, col + i));

                }
                if (countLeft == 5 || countRight == 5)
                {
                    winner = curRole;
                    listPoint.Add(new Point(row, col));
                    return true;
                }
            }
            // Check the main dianogal
            countLeft = 1;
            countRight = 1;
            listPoint.Clear();
            for (int i = 1; i < 5; i++)
            {
                if (col - i >= 0 && row - i >= 0 && board[row - i, col - i] == (int)curRole)
                {
                    countLeft++;
                    listPoint.Add(new Point(row - i, col - i));
                }
                if (col + i < _sizeColumn && row + i < _sizeRow && board[row + i, col + i] == (int)curRole)
                {

                    countRight++;
                    listPoint.Add(new Point(row + i, col + i));

                }
                if (countLeft == 5 || countRight == 5)
                {
                    winner = curRole;
                    listPoint.Add(new Point(row, col));
                    return true;
                }
            }

            // Check the sub diagonal
            countLeft = 1;
            countRight = 1;
            listPoint.Clear();
            for (int i = 1; i < 5; i++)
            {
                if (col - i >= 0 && row + i < _sizeRow && board[row + i, col - i] == (int)curRole)
                {
                    countLeft++;
                    listPoint.Add(new Point(row + i, col - i));
                }
                if (col + i < _sizeColumn && row - i >= 0 && board[row - i, col + i] == (int)curRole)
                {
                    countRight++;
                    listPoint.Add(new Point(row - i, col + i));
                }
                if (countLeft == 5 || countRight == 5)
                {
                    winner = curRole;
                    listPoint.Add(new Point(row, col));
                    return true;
                }
            }
            if (_markedCount == _sizeRow * _sizeColumn)
            {
                winner = MarkType.None;
                return true;
            }

            return false;
        }

        public void SaveGame(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(SizeRow);
                        bw.Write(SizeColumn);
                        bw.Write((int)curRole);
                        bw.Write((int)winner);
                        for (int i = 0; i < SizeRow; i++)
                        {
                            for (int j = 0; j < SizeColumn; j++)
                            {
                                bw.Write(board[i, j]);
                            }
                        }
                        if (winner != MarkType.None)
                        {
                            foreach (Point p in listPoint)
                            {
                                bw.Write((int)p.X);
                                bw.Write((int)p.Y);
                            }
                        }
                    }
                }
                MessageBox.Show("Game saved successfully!");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void LoadGame(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        SizeRow = br.ReadInt32();
                        SizeColumn = br.ReadInt32();
                        curRole = (MarkType)br.ReadInt32();
                        winner = (MarkType)br.ReadInt32();
                        board = new int[SizeRow, SizeColumn];
                        for (int i = 0; i < SizeRow; i++)
                        {
                            for (int j = 0; j < SizeColumn; j++)
                            {
                                board[i, j] = br.ReadInt32();
                            }
                        }
                        if (winner != MarkType.None)
                        {
                            listPoint.Clear();
                            for (int i = 0; i < 5; i++)
                            {
                                int x = br.ReadInt32();
                                int y = br.ReadInt32();
                                listPoint.Add(new Point(x, y));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}

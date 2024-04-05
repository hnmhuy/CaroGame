using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CaroGame.GameMaterial
{
    public enum MarkType { None, Cross, Circle }
    public class Mark : Canvas
    {
        private MarkType _type;
        private bool haveBorder = false;
        private Brush _color = Brushes.Red;
        private Brush _background = Brushes.LightBlue;
        private int colIndex;
        private int rowIndex;

        public MarkType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                UpdateMark(haveBorder);
            }
        }
        public Brush Color
        {
            get { return _color; }
            set { _color = value; UpdateColor(); }
        }

        public bool HaveBorder
        {
            get => haveBorder;
            set
            {
                haveBorder = value;

            }
        }

        public Mark(double width, double height, MarkType type, bool haveBorder = false)
        {
            Width = width;
            Height = height;
            this.haveBorder = haveBorder;
            Type = type;
            RowIndex = 0;
            colIndex = 0;
        }

        public int RowIndex
        {
            get => rowIndex;
            set { rowIndex = value; }
        }

        public int ColIndex
        {
            get => colIndex;
            set
            {
                colIndex = value;
            }
        }

        private void GenerateRect()
        {
            Rectangle rect = new Rectangle() { Width = Width, Height = Height, Stroke = Color, StrokeThickness = 2, Fill = _background };
            //if (Type != MarkType.None) rect.Fill = this._background;
            Children.Add(rect);
        }

        private void GenerateEllipse()
        {
            double circleSize = Math.Abs(Math.Min(Width, Height) - 20);
            Ellipse ellipse = new Ellipse() { Width = circleSize, Height = circleSize, Stroke = Color, StrokeThickness = 2 };
            Children.Add(ellipse);
            SetTop(ellipse, (Height - circleSize) / 2);
            SetLeft(ellipse, (Width - circleSize) / 2);
        }

        private void GenerateCross()
        {
            int crossSize = Math.Abs(Math.Min((int)Width, (int)Height) - 20);
            int startX = (int)(Width - crossSize) / 2;
            int startY = (int)(Height - crossSize) / 2;
            Line line1 = new Line() { X1 = startX, Y1 = startY, X2 = Width - startX, Y2 = Height - startY, Stroke = Color, StrokeThickness = 2 };
            Line line2 = new Line() { X1 = Width - startX, Y1 = startY, X2 = startX, Y2 = Height - startY, Stroke = Color, StrokeThickness = 2 };
            Children.Add(line1);
            Children.Add(line2);
        }

        public void UpdateColor()
        {
            foreach (var item in Children)
            {
                if (item is Shape shape)
                {
                    shape.Stroke = Color;
                }
            }
        }

        public void UpdateMark(bool haveBorder = false)
        {
            Children.Clear();
            if (haveBorder)
            {
                GenerateRect();
                Opacity = 0.5;
            }
            switch (Type)
            {
                case MarkType.Cross:
                    GenerateCross();
                    break;
                case MarkType.Circle:
                    GenerateEllipse();
                    break;
                case MarkType.None:
                    GenerateRect();
                    return;
                default:
                    break;
            }

        }

        public void ScaleMark(double scaleX, double scaleY)
        {
            Width *= scaleX;
            Height *= scaleY;
            double newSize = Math.Abs(Math.Min(Width, Height) - 20);
            double offsetX = (Width - newSize) / 2;
            double offsetY = (Height - newSize) / 2;
            foreach (var item in Children)
            {
                if (item is Shape shape)
                {
                    if (item is Ellipse ellipse)
                    {
                        ellipse.Width = newSize;
                        ellipse.Height = newSize;
                        SetLeft(ellipse, (Width - newSize) / 2);
                        SetTop(ellipse, (Height - newSize) / 2);
                    }
                    else if (item is Rectangle rect)
                    {
                        rect.Width *= scaleX;
                        rect.Height *= scaleY;
                    }
                    else if (item is Line line)
                    {
                        if (line.X1 > line.X2) // subline 
                        {
                            line.X1 = Width - offsetX;
                            line.Y1 = offsetY;
                            line.X2 = offsetX;
                            line.Y2 = Height - offsetY;
                        }
                        else
                        {
                            line.X1 = offsetX;
                            line.Y1 = offsetY;
                            line.X2 = Width - offsetX;
                            line.Y2 = Height - offsetY;
                        }
                    }
                }
            }
            SetLeft(this, GetLeft(this) * scaleX);
            SetTop(this, GetTop(this) * scaleY);
        }

        public void SetIndex(Point point)
        {
            SetIndex((int)point.X, (int)point.Y);
        }

        public void SetIndex(int colIndex, int rowIndex)
        {
            this.colIndex = colIndex;
            this.rowIndex = rowIndex;
            SetPosition();
        }

        public Point GetIndex() { return new Point(colIndex, rowIndex); }

        public void SetPosition()
        {
            SetLeft(this, colIndex * Width);
            SetTop(this, rowIndex * Height);
        }
    }
}

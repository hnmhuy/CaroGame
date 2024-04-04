using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CaroGame
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
                UpdateMark(this.haveBorder);
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
            this.Width = width;
            this.Height = height;
            this.haveBorder = haveBorder;
            this.Type = type;
            this.RowIndex = 0;
            this.colIndex = 0;
            this.Background = null;
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
            Rectangle rect = new Rectangle() { Width = this.Width, Height = this.Height, Stroke = this.Color, StrokeThickness = 2, Fill = this._background };
            //if (Type != MarkType.None) rect.Fill = this._background;
            this.Children.Add(rect);
        }

        private void GenerateEllipse()
        {
            double circleSize = Math.Abs(Math.Min(this.Width, this.Height) - 20);
            Ellipse ellipse = new Ellipse() { Width = circleSize, Height = circleSize, Stroke = this.Color, StrokeThickness = 2 };
            this.Children.Add(ellipse);
            SetTop(ellipse, (this.Height - circleSize) / 2);
            SetLeft(ellipse, (this.Width - circleSize) / 2);
        }

        private void GenerateCross()
        {
            int crossSize = Math.Abs(Math.Min((int)this.Width, (int)this.Height) - 20);
            int startX = (int)(this.Width - crossSize) / 2;
            int startY = (int)(this.Height - crossSize) / 2;
            Line line1 = new Line() { X1 = startX, Y1 = startY, X2 = this.Width - startX, Y2 = this.Height - startY, Stroke = this.Color, StrokeThickness = 2 };
            Line line2 = new Line() { X1 = this.Width - startX, Y1 = startY, X2 = startX, Y2 = this.Height - startY, Stroke = this.Color, StrokeThickness = 2 };
            this.Children.Add(line1);
            this.Children.Add(line2);
        }

        public void UpdateColor()
        {
            foreach (var item in Children)
            {
                if (item is Shape shape)
                {
                    shape.Stroke = this.Color;
                }
            }
        }

        public void UpdateMark(bool haveBorder = false)
        {
            this.Children.Clear();
            if (haveBorder)
            {
                GenerateRect();
                Opacity = 0.5;
            }
            switch (this.Type)
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
            this.Width *= scaleX;
            this.Height *= scaleY;
            double newSize = Math.Abs(Math.Min(this.Width, this.Height) - 20);
            double offsetX = (this.Width - newSize) / 2;
            double offsetY = (this.Height - newSize) / 2;
            foreach (var item in this.Children)
            {
                if (item is Shape shape)
                {
                    if (item is Ellipse ellipse)
                    {
                        ellipse.Width = newSize;
                        ellipse.Height = newSize;
                        Canvas.SetLeft(ellipse, (Width - newSize) / 2);
                        Canvas.SetTop(ellipse, (Height - newSize) / 2);
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
            Canvas.SetLeft(this, Canvas.GetLeft(this) * scaleX);
            Canvas.SetTop(this, Canvas.GetTop(this) * scaleY);
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

        public Point GetIndex() { return new Point(this.colIndex, this.rowIndex); }

        public void SetPosition()
        {
            Canvas.SetLeft(this, colIndex * this.Width);
            Canvas.SetTop(this, rowIndex * this.Height);
        }
    }
}

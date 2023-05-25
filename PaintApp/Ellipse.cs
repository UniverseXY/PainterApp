using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace PaintApp
{
    class Ellipse : PaintableObject
    {
        public Point pTop;
        public int Height = 100;
        public int Width = 200;
        private int thickness;
        public bool IsFilled = false;
        public Color ForeColor = Color.Black;
        public int Thickness
        {
            get => thickness;
            set
            {
                if (value < 1) thickness = 1;
                else if (value > 15) thickness = 15;
                else thickness = value;
            }
        }
        public Color FillColor = Color.White;
            
        public void Paint(Graphics g)
        {
            if (pTop == null) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var p = new Pen(ForeColor, Thickness);
            Brush b = new SolidBrush(FillColor);
            g.DrawEllipse(p, pTop.X, pTop.Y, Width, Height );
            if (IsFilled) g.FillEllipse(b, pTop.X, pTop.Y, Width, Height);
        }
    }
}

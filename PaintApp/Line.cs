using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace PaintApp
{
    class Line : PaintableObject
    {
        public Point p1;
        public Point p2;
        public bool IsArrow = false;
        public Color FgColor = Color.Black;
        private int thickness;
        private bool isEraser = false;
        public bool IsEraser
        {
            get => isEraser;
            set => isEraser = value;
        }
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

        public Line()
        {
        }

        public void Paint(Graphics g)
        {
            if (p1 == null || p2 == null) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;
           // if (!isEraser)
             var p = new Pen(FgColor, Thickness);
          //  else var p = new Pen(Color.White, Thickness);
            if (IsArrow)
                p.EndCap = LineCap.ArrowAnchor;
            g.DrawLine(p, p1, p2);
        }
     /*   public void DiagPaint(Graphics g)
        {
            if (p1 == null || p2 == null) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var p = new Pen(FgColor, Thickness);
            if (IsArrow)
                p.EndCap = LineCap.ArrowAnchor;
            int distX = p2.X - p1.X;
            int distY = p2.Y - p1.Y;
            if ((distX == 0) || (distY == 0)) g.DrawLine(p, p1.X, p1.Y, p2.X, p2.Y);
            else if (Math.Abs(distY/distX) < 0.5) g.DrawLine(p, p1.X, p1.Y, p2.X, p1.Y);
            else if (Math.Abs(distY / distX) >= 0.5 && Math.Abs(distY / distX) < 1) g.DrawLine(p, p1.X, p1.Y, p2.X, p1.Y);
            else if (Math.Abs(distY / distX) == 1) g.DrawLine(p, p1.X, p1.Y, p2.X, p1.Y);


        }*/
    }
}
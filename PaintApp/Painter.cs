using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintApp
{
    class Painter
    {
        public Size oldMaxSize;
        private bool isAngleDrawing;
        const double epsilon = 1e-20;
        private bool isFreeDrawing;
        private bool isErasing;
        private bool isFilled;
        public bool IsFilled
        {
            get => isFilled;
            set => isFilled = value;
        }
         private Size imageSize;
         public Painter(Size sz) {
             imageSize = sz;
         }

        public bool IsErasing
        {
            get => isErasing;
            set => isErasing = value;
        }
        public Size ImageSize
        {
            get => imageSize;
            set
            {
                imageSize = value;
                int w = Math.Max(oldMaxSize.Width, imageSize.Width);
                int h = Math.Max(oldMaxSize.Height, imageSize.Height);
                if ( imageSize.Width > oldMaxSize.Width) oldMaxSize.Width = imageSize.Width;
                if (imageSize.Height > oldMaxSize.Width) oldMaxSize.Height = imageSize.Height;
                var img = new Bitmap(w, h);
                var ig = Graphics.FromImage(img);
                ig.Clear(Color.White);
                if (mainImg != null)
                {
                    ig.DrawImage(mainImg, 0, 0);
                }
                
                mainImg = img;
            }
        }
        public bool IsAngleDrawing
        {
            get => isAngleDrawing;
            set => isAngleDrawing = value;
        }
        public bool IsFreeDrawing {
            get => isFreeDrawing;
            set => isFreeDrawing = value;
        }
        internal enum DrawType
        {
            Line, Arrow, Rectangle, FilledRectangle, Ellipse, FilledEllipse
        }
       
        public Color CurrColor { get; set; }
        private int thickness;
        public int Thickness {
            get => thickness;
            set {
                if (value < 1) thickness = 1;
                else if (value > 15) thickness = 15;
                else thickness = value;
            }    
                }
        private Image mainImg;
        public Image MainImage
        {
            get
            {
               /* var i = (Image)mainImg.Clone();
                var g = Graphics.FromImage(i);
                g.DrawImage(mainImg, new Point(0, 0));
                return i;*/
                return mainImg;
            }
            set { mainImg = value; }
        }
        private DrawType objectType = DrawType.Line;
        private PaintableObject obj;
        public DrawType ObjectType
        {
            get => objectType;
            set
            {
                objectType = value;
                switch (value)
                {
                    case DrawType.Line:
                        obj = new Line();
                        
                        ((Line)obj).Thickness = thickness;
                        ((Line)obj).FgColor = CurrColor;
                        break;
                    case DrawType.Arrow:
                        obj = new Line();
                        ((Line)obj).IsArrow = true;
                        ((Line)obj).Thickness = thickness;
                        ((Line)obj).FgColor = CurrColor;
                        break;
                    case DrawType.Ellipse:
                        obj = new Ellipse();
                        ((Ellipse)obj).Thickness = thickness;
                        ((Ellipse)obj).ForeColor = CurrColor;
                        break;
                    case DrawType.FilledEllipse:
                        obj = new Ellipse();
                        ((Ellipse)obj).IsFilled = true;
                        ((Ellipse)obj).Thickness = thickness;
                        ((Ellipse)obj).ForeColor = CurrColor;
                        ((Ellipse)obj).FillColor = CurrColor;
                        break;
                    case DrawType.Rectangle:
                        obj = new RectFigure();
                        ((RectFigure)obj).Thickness = thickness;
                        ((RectFigure)obj).ForeColor = CurrColor;
                        break;
                    case DrawType.FilledRectangle:
                        obj = new RectFigure();
                        ((RectFigure)obj).IsFilled = true;
                        ((RectFigure)obj).Thickness = thickness;
                        ((RectFigure)obj).ForeColor = CurrColor;
                        ((RectFigure)obj).FillColor = CurrColor;
                        break;
                }
            }
        }

        /* public void Paint(Graphics g)
         {
             if (Img == null)
             {
                 // ... создаем его
                 Img = new Bitmap(
                     (int)g.VisibleClipBounds.Width,
                     (int)g.VisibleClipBounds.Height);
                 // получаем Graphics для этой картинки
                 var ig = Graphics.FromImage(Img);
                 // Устанавливаем сглаживание
                 ig.SmoothingMode = SmoothingMode.AntiAlias;
                 DrawWithPen(ig);

             }
             g.Clear(Color.White);

             // Массив точек во втором параметре
             // позволяет растягивать и сжимать картинку в
             // зависимости от фактических размеров панели.
             g.DrawImage(Img,
                 new PointF[]
                 {
                     new PointF(0, 0),
                     new PointF(g.VisibleClipBounds.Width, 0),
                     new PointF(0, g.VisibleClipBounds.Height)
                 });
         }*/
          private void DrawWithPen(Graphics g)
          {
              Pen p = new Pen(CurrColor, Thickness);
              p.StartCap = p.EndCap = LineCap.Round;
              g.SmoothingMode = SmoothingMode.AntiAlias;
              g.DrawLine(p, points[0], points[1]);
              
          }
         private void Erase(Graphics g)
            {
            Pen p = new Pen(Color.White, Thickness);
            p.StartCap = p.EndCap = LineCap.Round;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawLine(p, points[0], points[1]);
         }
        private Point DeterminePoint() {
            double tan = (points[1].Y - points[0].Y) * 1.0 / (points[1].X - points[0].X);
            Point sp = new Point();
            sp.X = points[1].X;
            if (tan + 0.41 >= epsilon && tan - 0.41 < epsilon) {
                sp.Y = points[0].Y;
            }
            if (tan - 0.41 >= epsilon && tan - 2.41 < epsilon)
            {
                sp.Y = sp.X - points[0].X + points[0].Y;
            }
            if (Math.Abs(tan) - 2.41 >= epsilon)
            {
                sp.X = points[0].X;
                sp.Y = points[1].Y;
            }
            if (tan + 2.41 > epsilon && tan + 0.41 < epsilon)
            {
                sp.Y = -points[1].X + points[0].X + points[0].Y;
            }
            return sp;
        }
        private List<Point> points = new List<Point>();
        private void Draw(Graphics g)
        {
            if (isFreeDrawing == false)
            {
                switch (ObjectType)
                {
                    case DrawType.Line:
                    case DrawType.Arrow:
                        if (points.Count > 1)
                        {
                            Line ln = (Line)obj;
                            ln.Thickness = thickness;
                            ln.FgColor = CurrColor;
                            ln.p1 = points[0];
                            
                            if (isAngleDrawing) ln.p2 = DeterminePoint();
                            else ln.p2 = points[1];
                            ln.Paint(g);
                        }

                        break;
                    case DrawType.Ellipse:
                    case DrawType.FilledEllipse:
                        Ellipse el = (Ellipse)obj;
                        el.ForeColor = CurrColor;
                        el.Thickness = thickness;
                        el.FillColor = CurrColor;
                        el.pTop = points[0];
                        el.Width = points[1].X - points[0].X;
                        el.Height = points[1].Y - points[0].Y;
                        el.Paint(g);
                        break;
                    case DrawType.Rectangle:
                    case DrawType.FilledRectangle:
                        RectFigure r = (RectFigure)obj;
                        r.ForeColor = CurrColor;
                        r.Thickness = thickness;
                        r.FillColor = CurrColor;
                        r.pTop = points[0];
                        if (points[1].X <= points[0].X) r.pTop.X = points[1].X;
                        if (points[1].Y <= points[0].Y) r.pTop.Y = points[1].Y;
                        r.Width = Math.Abs(points[1].X - points[0].X);
                        r.Height = Math.Abs(points[1].Y - points[0].Y);
                        r.Paint(g);
                        break;

                }
            }
            else if (points.Count > 1 && !isErasing) DrawWithPen(g);
            else if (isErasing) Erase(g);
        }
        public void Paint(Graphics g)
        {
            if (mainImg == null)
            {
                mainImg = new Bitmap(
                    (int)g.VisibleClipBounds.Width,
                    (int)g.VisibleClipBounds.Height,
                    g
                    );
            }
            var tg = Graphics.FromImage(mainImg);
            Draw(tg);
        }

        public void Preview(Graphics g)
         {
             if (mainImg == null)
             {
                 mainImg = new Bitmap(
                     (int)g.VisibleClipBounds.Width,
                     (int)g.VisibleClipBounds.Height,
                     g
                 );
             }
             var img = (Image)mainImg.Clone();
             var tg = Graphics.FromImage(img);
             tg.Clear(Color.White);
             tg.DrawImage(mainImg, new Point(0, 0));
             Draw(tg);
             g.DrawImage(img, new Point(0, 0));

         }
        
        public void SetStartPoint(Point pt)
        {
            points.Clear();
            points.Add(pt);
        }

        public void SetPoint(Point pt, int num = 0)
        {
            num -= 1;
            if (points.Count > num)
                points[num] = pt;
            else points.Add(pt);
        }
      /*  public Point GetStartPoint()
        {
            return points[0];
        }*/
        public void Show(Graphics g)
        {
            if (mainImg != null)
                g.DrawImage(mainImg, new Point(0, 0));
        }

    }
}
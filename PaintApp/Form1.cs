using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintApp
{
    public partial class Form1 : Form
    {
        Painter p;
        public int formWidth;
        public int formHeight;
        
        public Form1()
        {
            InitializeComponent();
            p = new Painter(mainPanel.Size);
            p.CurrColor = Color.Black;
            // p.LastPos = new Point(0, 0);
            p.Thickness = 3;
            p.IsFreeDrawing = true;
            p.IsAngleDrawing = false;
            p.IsFilled = false;
            p.IsErasing = false;
           


        }


        private void btnColorDialog_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            MyDialog.Color = btnCurrColor.BackColor;

            // Update the button color if the user clicks OK 
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                btnPrevColor.BackColor = btnCurrColor.BackColor;
                btnCurrColor.BackColor = MyDialog.Color;
                p.CurrColor = MyDialog.Color;
            }

        }


        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {
            //p.Preview(mainPanel.CreateGraphics());
            p.Show(mainPanel.CreateGraphics());
        }


        private void button1_Click(object sender, EventArgs e)
        {
            p.CurrColor = button1.BackColor;
            btnPrevColor.BackColor = btnCurrColor.BackColor;
            btnCurrColor.BackColor = p.CurrColor;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            p.CurrColor = button2.BackColor;
            btnPrevColor.BackColor = btnCurrColor.BackColor;
            btnCurrColor.BackColor = p.CurrColor;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            p.CurrColor = button3.BackColor;
            btnPrevColor.BackColor = btnCurrColor.BackColor;
            btnCurrColor.BackColor = p.CurrColor;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            p.CurrColor = button4.BackColor;
            btnPrevColor.BackColor = btnCurrColor.BackColor;
            btnCurrColor.BackColor = p.CurrColor;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            p.CurrColor = button5.BackColor;
            btnPrevColor.BackColor = btnCurrColor.BackColor;
            btnCurrColor.BackColor = p.CurrColor;
        }

        private bool pressed = false;
       
        private void button6_Click(object sender, EventArgs e)
        {
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.DefaultExt = ".jpg";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                var filename = saveFileDialog.FileName;
                //p.Show(mainPanel.CreateGraphics());
                p.ImageSize = p.MainImage.Size;
                p.MainImage.Save(filename);
            }
        }
       

        private void mainPanel_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (pressed)
            {
      
                p.SetPoint(e.Location, 2);
                    switch (p.IsFreeDrawing)
                    {
                        case false:
                            p.Preview(mainPanel.CreateGraphics());
                            break;
                        case true:
                        p.Preview(mainPanel.CreateGraphics());
                        
                        p.Paint(mainPanel.CreateGraphics());
                       // p.Preview(mainPanel.CreateGraphics());
                        // p.Show(mainPanel.CreateGraphics());
                        p.SetPoint(e.Location, 1);
                            break;
                    }
                }
            }
    

        private void mainPanel_MouseUp(object sender, MouseEventArgs e)
        {
            pressed = false;
            //p.IsAngleDrawing = false;
            p.Paint(mainPanel.CreateGraphics());
            p.IsAngleDrawing = false;
            
            mainPanel.Update();
            

        }

        private void btnCurrColor_Click(object sender, EventArgs e)
        {
            p.CurrColor = btnCurrColor.BackColor;
        }

        private void btnPrevColor_Click(object sender, EventArgs e)
        {
            p.CurrColor = btnPrevColor.BackColor;
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            p.Thickness = (int)numericUpDown1.Value;
        }

        
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (!p.IsFilled)
                p.ObjectType = Painter.DrawType.Rectangle;
            else p.ObjectType = Painter.DrawType.FilledRectangle;
            label8.Text = "Прямоугольник";
            p.IsFreeDrawing = false;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            p.ObjectType = Painter.DrawType.Arrow;
            label8.Text = "Стрелка";
            p.IsFreeDrawing = false;
        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {
            p.ObjectType = Painter.DrawType.Line;
            label8.Text = "Линия";
            p.IsFreeDrawing = false;
        }

        private void pictureBox1_Click_2(object sender, EventArgs e)
        {
            if (!p.IsFilled)
                p.ObjectType = Painter.DrawType.Ellipse;
            else p.ObjectType = Painter.DrawType.FilledEllipse;
            label8.Text = "Эллипс";
            p.IsFreeDrawing = false;
        }

        private void mainPanel_MouseDown(object sender, MouseEventArgs e)
        {
            pressed = true;
            if (Control.ModifierKeys == Keys.Shift)
                p.IsAngleDrawing = true;
            p.SetStartPoint(e.Location);
            p.SetPoint(e.Location, 2);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (openPictureDialog.ShowDialog() == DialogResult.OK)
            {
                // Если пользователь выбрал файл (не передумал),
                // получаем указанное имя файла
                var filename = openPictureDialog.FileName;

                // Создаем изображение из указанного файла
                // и записываем его в свойство Img объекта p класса Painter
                p.MainImage = Image.FromFile(filename);
                p.ImageSize = mainPanel.Size;
                // Требуем перерисовать панель,
                // чтобы на ней нарисовалось новое изобрбажение
                mainPanel.Refresh();
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            p.IsFreeDrawing = true;
            p.IsErasing = false;
            label8.Text = "Ручка";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!p.IsFilled)
            {
                p.IsFilled = true;
                pictureBox2.BorderStyle = BorderStyle.Fixed3D;
                if (p.ObjectType == Painter.DrawType.Ellipse) p.ObjectType = Painter.DrawType.FilledEllipse;
                if (p.ObjectType == Painter.DrawType.Rectangle) p.ObjectType = Painter.DrawType.FilledRectangle;

            }
            else
            {
                p.IsFilled = false;
                if (p.ObjectType == Painter.DrawType.FilledEllipse) p.ObjectType = Painter.DrawType.Ellipse;
                if (p.ObjectType == Painter.DrawType.FilledRectangle) p.ObjectType = Painter.DrawType.Rectangle;
                pictureBox2.BorderStyle = BorderStyle.FixedSingle;
             
            }
        }

      

        private void Form1_Resize(object sender, EventArgs e)
        {
          
            mainPanel.Refresh();
        }
        
      
    
        

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            p.IsFreeDrawing = true;
            p.IsErasing = true;
            label8.Text = "Ластик";
        }
       private void mainPanel_Resize(object sender, EventArgs e)
        {

            if (p != null)
            {
                p.ImageSize = mainPanel.Size;
            }
            mainPanel.Refresh();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void mainPanel_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void mainPanel_SizeChanged(object sender, EventArgs e)
        {
            
            mainPanel.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    }


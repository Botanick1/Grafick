using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Графический_редактор
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetSize();
        }
        private class ArPoint  // создаем клас
        {
            private int index = 0; // номер точки в массиве
            private Point[] points; // массив с точками
            public ArPoint(int size)  // размер массива
            {
                points = new Point[size];
            }
            public void SetPoint(int x, int y) // ставим точку
            {
                if (index >= points.Length)
                {
                    index = 0; // проверка не выходит ли точка за границу
                }
                points[index] = new Point(x, y);
                index++;

            }
            public void Reset() // сброс точки, чтобы отпустить мышку и начать рисовать в другом месте
            {
                index = 0;
            }
            public int GetCountPoints() // возвращает колво точек
            {
                return index;
            }
            public Point[] GetPoints()
            {
                return points; // возвращает массив
            }

        }

        private ArPoint arpoint = new ArPoint(2);

        Bitmap map = new Bitmap(2, 2); // карта
        Graphics graphics;
        Pen pen = new Pen(Color.Black, 3f); // карандаш
        
        private void SetSize()
        {
            Rectangle rectangle = Screen.PrimaryScreen.Bounds; // узнаем разрешение
            map = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(map);

            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round; // округление линии
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private bool Mouse = false; // проверка зажатости левой кнопки мыши

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Mouse = false;
            arpoint.Reset();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(!Mouse) {return;} // если мышка не зажата - выход
            arpoint.SetPoint(e.X, e.Y);   // заполняем массив координатами мышки
            if(arpoint.GetCountPoints() >= 2)// проверяем на заполненость
            {
                graphics.DrawLines(pen,arpoint.GetPoints()); // рисуем
                pictureBox1.Image = map; // присваевание рисунка 
                arpoint.SetPoint(e.X, e.Y); // сплошная линия
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pen.Color = ((Button)sender).BackColor; // для выбора цвета
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if( colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pen.Color = colorDialog1.Color;
                
            }
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphics.Clear(pictureBox1.BackColor); // очистка
            pictureBox1.Image = map; // новый лист
        }

        private void button9_Click(object sender, EventArgs e)
        {
            pen.Color = pictureBox1.BackColor; // делаем ластик
        
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            pen.Width = trackBar1.Value;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPG(*JPG)|*jpg";
            if( saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName); // нажали сохранить указываем путь
            }
        }
    }
}

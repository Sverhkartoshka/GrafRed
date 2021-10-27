using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class MainForm : Form
    {
        bool isPressed;
        int x1, y1, x2, y2;
        Bitmap snapshot, tempDraw;
        Color foreColor;
        int lineWidth;
        string selectedTool;

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            if (selectedTool != "Pencil") tempDraw = (Bitmap)snapshot.Clone();
            Graphics g = Graphics.FromImage(tempDraw);
            Pen myPen = new Pen(foreColor, lineWidth);
            switch (selectedTool)
            {
                case "Line":
                    if (tempDraw != null)
                        g.DrawLine(myPen, x1, y1, x2, y2);
                    break;
                case "Rectangle":
                    if (tempDraw != null)
                        g.DrawRectangle(myPen, x1, y1, x2 - x1, y2 - y1);
                    break;
                case "Pencil":
                    if (tempDraw != null)
                    {
                        g.DrawLine(myPen, x1, y1, x2, y2);
                        x1 = x2;
                        y1 = y2;
                    }
                    break;
                case "Ellipse":
                    if (tempDraw != null)
                    {
                        g.DrawEllipse(myPen, x1, y1, x2 - x1, y2 - y1);
                    }
                    break;

                default: break;
            }
            myPen.Dispose();
            e.Graphics.DrawImageUnscaled(tempDraw, 0, 0);
            g.Dispose();
        }

        private void tool_Click(object sender, EventArgs e)
        {
            Line.Checked = false;
            Rectangle.Checked = false;
            Pencil.Checked = false;
            Ellipse.Checked = false;
            ToolStripButton btnClicked = sender as ToolStripButton;
            btnClicked.Checked = true;
            selectedTool = btnClicked.Name;
        }

        private void toolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            lineWidth = 
                int.Parse(toolStripComboBox.SelectedItem.ToString().Remove(1));
            Console.WriteLine(lineWidth);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox.CreateGraphics();
            g.Clear(Color.White);
            pictureBox.Image = null;
            snapshot = new Bitmap(pictureBox.ClientRectangle.Width,
                                  pictureBox.ClientRectangle.Height);
            tempDraw = (Bitmap)snapshot.Clone();
            g.Dispose();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image = Image.FromFile(openFileDialog1.FileName);
                snapshot = (Bitmap)pictureBox.Image;
                tempDraw = (Bitmap)snapshot.Clone();
                this.Text = openFileDialog1.FileName;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                string strFilExtn = fileName.Remove(0, fileName.Length - 3);
                this.Text = saveFileDialog1.FileName;

                switch (strFilExtn)
                {
                    case "bmp": snapshot.Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp); break;
                    case "jpg": snapshot.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                    case "gif": snapshot.Save(fileName, System.Drawing.Imaging.ImageFormat.Gif); break;
                    case "tif": snapshot.Save(fileName, System.Drawing.Imaging.ImageFormat.Tiff); break;
                    case "png": snapshot.Save(fileName, System.Drawing.Imaging.ImageFormat.Png); break;
                    default: break;
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                foreColor = colorDialog1.Color;
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
            x1 = e.X;
            y1 = e.Y;
            tempDraw = (Bitmap)snapshot.Clone();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = pictureBox.CreateGraphics();
            if (isPressed)
            {
                x2 = e.X;
                y2 = e.Y;
                pictureBox.Invalidate();
                pictureBox.Update();
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
            snapshot = (Bitmap)tempDraw.Clone();
        }

        public MainForm()
        {
            InitializeComponent();
            snapshot = new Bitmap(pictureBox.ClientRectangle.Width,
                                  pictureBox.ClientRectangle.Height);
            tempDraw = (Bitmap)snapshot.Clone();
            foreColor = Color.Red;
            lineWidth = 2;

            Pencil.Checked = true;
            selectedTool = "Pencil";
        }
    }
}

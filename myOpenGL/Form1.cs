using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGL;
using System.Runtime.InteropServices;
//3D model b1
using Milkshape;
//3D model e

namespace myOpenGL
{
    public partial class Form1 : Form
    {

        cOGL cGL;

        public Form1()
        {

            InitializeComponent();

            cGL = new cOGL(panel1);

            //3D model b4
            //listBox1.Items.Add("Stop");
            //foreach (Animation anim in cGL.ch.Animations)
                //listBox1.Items.Add(anim.Name);
            //cGL.ch.Stop();
            //3D model e

            //apply the bars values as cGL.ScrollValue[..] properties
                                         //!!!
            hScrollBarScroll(hScrollBar1, null);
            hScrollBarScroll(hScrollBar2, null);
            hScrollBarScroll(hScrollBar3, null);
            hScrollBarScroll(hScrollBar4, null);
            hScrollBarScroll(hScrollBar5, null);
            hScrollBarScroll(hScrollBar6, null);
            hScrollBarScroll(hScrollBar7, null);
            hScrollBarScroll(hScrollBar8, null);
            hScrollBarScroll(hScrollBar9, null);
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            cGL.Draw();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            cGL.OnResize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void hScrollBarScroll(object sender, ScrollEventArgs e)
        {
            cGL.intOptionC = 0;
            HScrollBar hb = (HScrollBar)sender;
            int n = int.Parse(hb.Name.Substring(hb.Name.Length - 1));
            cGL.ScrollValue[n - 1] = (hb.Value - 100) / 10.0f;
            if (e != null)
                cGL.Draw();
        }

        public float[] oldPos = new float[7];

        private void numericUpDownValueChanged(object sender, EventArgs e)
        {
            NumericUpDown nUD = (NumericUpDown)sender;
            int i = int.Parse(nUD.Name.Substring(nUD.Name.Length - 1));
            int pos = (int)nUD.Value;
            switch (i)
            {
                case 1:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xShift += 0.25f;
                        cGL.intOptionC = 4;
                    }
                    else
                    {
                        cGL.xShift -= 0.25f;
                        cGL.intOptionC = -4;
                    }
                    break;
                case 2:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yShift += 0.25f;
                        cGL.intOptionC = 5;
                    }
                    else
                    {
                        cGL.yShift -= 0.25f;
                        cGL.intOptionC = -5;
                    }
                    break;
                case 3:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.zShift += 0.25f;
                        cGL.intOptionC = 6;
                    }
                    else
                    {
                        cGL.zShift -= 0.25f;
                        cGL.intOptionC = -6;
                    }
                    break;
                case 4:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xAngle += 5;
                        cGL.intOptionC = 1;
                    }
                    else
                    {
                        cGL.xAngle -= 5;
                        cGL.intOptionC = -1;
                    }
                    break;
                case 5:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yAngle += 5;
                        cGL.intOptionC = 2;
                    }
                    else
                    {
                        cGL.yAngle -= 5;
                        cGL.intOptionC = -2;
                    }
                    break;
                case 6:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.zAngle += 5;
                        cGL.intOptionC = 3;
                    }
                    else
                    {
                        cGL.zAngle -= 5;
                        cGL.intOptionC = -3;
                    }
                    break;
            }
            cGL.Draw();
            oldPos[i - 1] = pos;
            cGL.intOptionC = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            cGL.Draw();
        }

        //3D model b5
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string curItem = listBox1.SelectedItem.ToString();
            //if (curItem == "Stop")
            //    cGL.ch.Stop();
            //else
            //    cGL.ch.PlayAnimation(curItem);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            bool isShiftPressed = e.Shift; // Check if the Shift key is pressed

            if (e.KeyCode == Keys.Back)
            {
                // Reset position and rotation to default values
                cGL.xShift = 0.0f;
                cGL.yShift = 0.0f;
                cGL.zShift = 0.0f;
                cGL.xAngle = 0.0f;
                cGL.yAngle = 0.0f;
                cGL.zAngle = 0.0f;
                cGL.intOptionC = 0; // Reset transformations
            }
            else if (isShiftPressed)
            {
                // Handle rotation when Shift is held down
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        cGL.yAngle += 5; // Rotate around Y-axis
                        cGL.intOptionC = 2; // Indicating a rotation around Y
                        break;
                    case Keys.Left:
                        cGL.yAngle -= 5; // Rotate around Y-axis
                        cGL.intOptionC = -2; // Indicating a rotation around Y in the opposite direction
                        break;
                    case Keys.Up:
                        cGL.xAngle += 5; // Rotate around X-axis
                        cGL.intOptionC = 1; // Indicating a rotation around X
                        break;
                    case Keys.Down:
                        cGL.xAngle -= 5; // Rotate around X-axis
                        cGL.intOptionC = -1; // Indicating a rotation around X in the opposite direction
                        break;
                }
            }
            else
            {
                // Handle translation when Shift is not held down
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        cGL.xShift += 0.1f;
                        cGL.intOptionC = 4; // Indicating a shift along X
                        break;
                    case Keys.Left:
                        cGL.xShift -= 0.1f;
                        cGL.intOptionC = -4; // Indicating a shift along X in the opposite direction
                        break;
                    case Keys.Up:
                        cGL.yShift += 0.1f;
                        cGL.intOptionC = 5; // Indicating a shift along Y
                        break;
                    case Keys.Down:
                        cGL.yShift -= 0.1f;
                        cGL.intOptionC = -5; // Indicating a shift along Y in the opposite direction
                        break;
                }
            }

            cGL.Draw();
            e.Handled = true; // Mark the event as handled

            // Reset intOptionC to default after applying transformation and redrawing
            cGL.intOptionC = 0;
        }



        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                cGL.zShift += 0.5f; // Zoom in
                cGL.intOptionC = 6; // Assuming positive intOptionC value for zoom in
            }
            else if (e.Delta < 0)
            {
                cGL.zShift -= 0.5f; // Zoom out
                cGL.intOptionC = -6; // Assuming negative intOptionC value for zoom out
            }
            cGL.Draw();
            cGL.intOptionC = 0; // Reset to default after drawing
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.Focus();
        }

    }
}
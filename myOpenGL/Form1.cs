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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Utils;
using Models;
//3D model e

namespace myOpenGL
{
    public partial class Form1 : Form
    {

        cOGL cGL;
        object selectedLightingMaterialRadio;

        public Form1()
        {

            InitializeComponent();

            cGL = new cOGL(panel1, textBox1);


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
            var parentGroupBox = hb.Parent as GroupBox;
            string scrollBarText = hb.Name;

            if (scrollBarText.Contains("hScrollBar"))
            {
                int n = int.Parse(hb.Name.Substring(hb.Name.Length - 1));
                cGL.ScrollValue[n - 1] = (hb.Value - 100) / 10.0f;
            } 
            else if (parentGroupBox != null && parentGroupBox.Text == "Lighting and Material")
            {
                if (selectedLightingMaterialRadio is LightProperty lightProperty)
                {
                    switch (lightProperty)
                    {
                        case LightProperty.AMBIENT:
                        case LightProperty.DIFFUSE:
                        case LightProperty.SPECULAR:
                            float normalizedRValue = slider1.Value / 100.0f;
                            float normalizedGValue = slider2.Value / 100.0f;
                            float normalizedBValue = slider3.Value / 100.0f;
                            float normalizedAValue = slider4.Value / 100.0f;
                            cGL.LightPropertyUpdatedValue = new LightPropertyUpdateKeyAndValue { Key = lightProperty, NewValues = new float[] { normalizedRValue, normalizedGValue, normalizedBValue, normalizedAValue } };
                            break;
                        case LightProperty.POSITION:
                            int w = slider4.Value > 50? 1 : 0;
                            cGL.LightPropertyUpdatedValue = new LightPropertyUpdateKeyAndValue { Key = lightProperty, NewValues = new float[] {slider1.Value, slider2.Value, slider3.Value, w} };
                            Sun sun = cGL.sun;
                            sun.Coords = new Vector3(X: slider1.Value, Y: slider2.Value, Z: slider3.Value);
                            float rotateAngle = slider4.Value > sun.Coords.Z ? sun.Angle + 15 : sun.Angle - 15;
                            //sun.Angle = rotateAngle;
                            break;
                        default:
                            break;
                    }
                }
                else if (selectedLightingMaterialRadio is MaterialProperty materialProperty)
                {
                        switch (materialProperty)
                        {
                            case MaterialProperty.AMBIENT:
                            case MaterialProperty.DIFFUSE:
                            case MaterialProperty.SPECULAR:
                                float normalizedRValue = slider1.Value / 100.0f; 
                                float normalizedGValue = slider2.Value / 100.0f; 
                                float normalizedBValue = slider3.Value / 100.0f; 
                                float normalizedAValue = slider4.Value / 100.0f; 
                                cGL.MaterialPropertyUpdatedValue = new MaterialPropertyUpdateKeyAndValue { Key = materialProperty, NewValues = new float[] { normalizedRValue, normalizedGValue, normalizedBValue, normalizedAValue } };
                                break;
                            case MaterialProperty.SHININESS:
                                cGL.MaterialPropertyUpdatedValue = new MaterialPropertyUpdateKeyAndValue { Key = materialProperty, NewValue = slider1.Value };
                                break;
                            default:
                                break;
                        }
                }

                //MaterialProperty? materialProperty = MaterialConfig.Instance.GetMaterialByString(scrollBarText.Replace("property_mat_", ""));
                //if (materialProperty != null)
                //{
                //    switch (materialProperty)
                //    {
                //        case MaterialProperty.AMBIENT:
                //        case MaterialProperty.DIFFUSE:
                //        case MaterialProperty.SPECULAR:
                //            float normalizedValue = hb.Value / 100.0f; 
                //            cGL.UpdateValue = new MaterialPropertyUpdateKeyAndValue { Key = materialProperty, NewValues = new float[] { normalizedValue, normalizedValue, normalizedValue, 1.0f } };
                //            break;
                //        case MaterialProperty.SHININESS:
                //            cGL.UpdateValue = new MaterialPropertyUpdateKeyAndValue { Key = materialProperty, NewValue = hb.Value };
                //            break;
                //        default:
                //            break;
                //    }
                //}



                //    if (Enum.TryParse<MaterialProperty>(materialProperty, ignoreCase: true, out var property))
                //{
                //    switch (property)
                //    {
                //        case MaterialProperty.MATAMBIENT:
                //        case MaterialProperty.MATDIFFUSE:
                //        case MaterialProperty.MATSPECULAR:
                //        float normalizedValue = hb.Value / 100;
                //            cGL.UpdateValue = new PropertyUpdateKeyAndValue { Key = materialProperty.ToUpper(), NewValues = new float[] { normalizedValue, normalizedValue, normalizedValue, 1.0f } };
                //            break;
                //        case MaterialProperty.SHININESS:
                //            cGL.UpdateValue = new PropertyUpdateKeyAndValue { Key = materialProperty.ToUpper(), NewValue = hb.Value };
                //            break;
                //        default:
                //            cGL.UpdateValue = new PropertyUpdateKeyAndValue { Key = materialProperty.ToUpper(), NewValue = hb.Value };
                //            break;
                //    }
                //}
                //MaterialConfig.Instance.UpdateMaterialProperty(materialProperty.ToUpper(), newValue: hb.Value);
                //cGL.UpdateValue = new PropertyUpdateKeyAndValue { Key = "SHININESS",  NewValue = 120.0f };


                //cGL.ScrollValue[9] = hb.Value;
            }

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
                        //cGL.xShift += 0.25f;
                        cGL.intOptionC = 4;
                    }
                    else
                    {
                        //cGL.xShift -= 0.25f;
                        cGL.intOptionC = -4;
                    }
                    break;
                case 2:
                    if (pos > oldPos[i - 1])
                    {
                        //cGL.yShift += 0.25f;
                        cGL.intOptionC = 5;
                    }
                    else
                    {
                        //cGL.yShift -= 0.25f;
                        cGL.intOptionC = -5;
                    }
                    break;
                case 3:
                    if (pos > oldPos[i - 1])
                    {
                        //cGL.zShift += 0.25f;
                        cGL.intOptionC = 6;
                    }
                    else
                    {
                        //cGL.zShift -= 0.25f;
                        cGL.intOptionC = -6;
                    }
                    break;
                case 4:
                    if (pos > oldPos[i - 1])
                    {
                        //cGL.xAngle += 5;
                        cGL.intOptionC = 1;
                    }
                    else
                    {
                        //cGL.xAngle -= 5;
                        cGL.intOptionC = -1;
                    }
                    break;
                case 5:
                    if (pos > oldPos[i - 1])
                    {
                        //cGL.yAngle += 5;
                        cGL.intOptionC = 2;
                    }
                    else
                    {
                        //cGL.yAngle -= 5;
                        cGL.intOptionC = -2;
                    }
                    break;
                case 6:
                    if (pos > oldPos[i - 1])
                    {
                        //cGL.zAngle += 5;
                        cGL.intOptionC = 3;
                    }
                    else
                    {
                        //cGL.zAngle -= 5;
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
            //cGL.Draw();
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
                //cGL.xShift = 0.0f;
                //cGL.yShift = 0.0f;
                //cGL.zShift = 0.0f;
                //cGL.xAngle = 0.0f;
                //cGL.yAngle = 0.0f;
                //cGL.zAngle = 0.0f;
                cGL.AccumulatedRotationsTraslations = new double[]{
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                };
                cGL.intOptionC = 0; // Reset transformations
            }
            else if (isShiftPressed)
            {
                // Handle rotation when Shift is held down
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        //cGL.yAngle += 5; // Rotate around Y-axis
                        cGL.intOptionC = 2; // Indicating a rotation around Y
                        break;
                    case Keys.Left:
                        //cGL.yAngle -= 5; // Rotate around Y-axis
                        cGL.intOptionC = -2; // Indicating a rotation around Y in the opposite direction
                        break;
                    case Keys.Up:
                        //cGL.xAngle += 5; // Rotate around X-axis
                        cGL.intOptionC = 1; // Indicating a rotation around X
                        break;
                    case Keys.Down:
                        //cGL.xAngle -= 5; // Rotate around X-axis
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
                        //cGL.xShift += 0.1f;
                        cGL.intOptionC = 4; // Indicating a shift along X
                        break;
                    case Keys.Left:
                        //cGL.xShift -= 0.1f;
                        cGL.intOptionC = -4; // Indicating a shift along X in the opposite direction
                        break;
                    case Keys.Up:
                        //cGL.yShift += 0.1f;
                        cGL.intOptionC = 5; // Indicating a shift along Y
                        break;
                    case Keys.Down:
                        //cGL.yShift -= 0.1f;
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
                //cGL.zShift += 0.5f; // Zoom in
                cGL.intOptionC = 6; // Assuming positive intOptionC value for zoom in
            }
            else if (e.Delta < 0)
            {
                //cGL.zShift -= 0.5f; // Zoom out
                cGL.intOptionC = -6; // Assuming negative intOptionC value for zoom out
            }
            cGL.Draw();
            cGL.intOptionC = 0; // Reset to default after drawing
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            // Update textBox1 with the mouse coordinates
            textBox1.Text = $"X: {e.X}, Y: {e.Y}";
        }

        private void lightOrMaterialRaddioChange(object sender, EventArgs e)
        {
            cGL.intOptionC = 0;
            RadioButton rb = (RadioButton)sender;
            string radioButtonName = rb.Name;
            bool isLighting = radioButtonName.Contains("Light");
            slider1.Visible = true;
            sliderLablel1.Visible = true;
            slider2.Visible = true;
            sliderLablel2.Visible = true;
            slider3.Visible = true;
            sliderLablel3.Visible = true;
            sliderLablel4.Visible = true;
            slider4.Visible = true;
            slider4.SmallChange = 1;
            slider4.LargeChange = 5;

            if (radioButtonName.Contains("Position"))
            {
                sliderLablel1.Text = "X";
                slider1.Minimum = -18;
                slider1.Maximum= 18;
                slider1.Value = (int)cGL.sun.Coords.X;
                
                sliderLablel2.Text = "Y";
                slider2.Minimum = -2;
                slider2.Maximum = 25;
                slider2.Value = (int)cGL.sun.Coords.Y;
                
                sliderLablel3.Text = "Z";
                slider3.Minimum = -50;
                slider3.Maximum = 50;
                slider3.Value = (int)cGL.sun.Coords.Z;
                
                sliderLablel4.Text = "W";
                slider4.Minimum = 0;
                slider4.Maximum = 100;
                slider4.SmallChange = 51;
                slider4.LargeChange = 51;
            }
            else if (radioButtonName.Contains("Shininess"))
            {
                sliderLablel1.Text = "";
                slider1.Minimum = 0;
                slider1.Maximum= 128;

                slider2.Visible = false;
                sliderLablel2.Visible = false;
                slider3.Visible = false;
                sliderLablel3.Visible = false;
                sliderLablel4.Visible = false;
                slider4.Visible = false;
            }
            else
            {
                sliderLablel1.Text = "R";
                slider1.Minimum = 0;
                slider1.Maximum = 100;

                sliderLablel2.Text = "G";
                slider2.Minimum = 0;
                slider2.Maximum = 100;

                sliderLablel3.Text = "B";
                slider3.Minimum = 0;
                slider3.Maximum = 100;

                sliderLablel4.Text = "A";
                slider4.Minimum = 0;
                slider4.Maximum = 100;
            }

            if (isLighting)
            {
                selectedLightingMaterialRadio = LightConfig.Instance.GetLightByString(radioButtonName.ToLower().Replace("light", ""));
            }
            else
            {
                selectedLightingMaterialRadio = MaterialConfig.Instance.GetMaterialByString(radioButtonName.ToLower().Replace("mat", ""));
            }
        }
    }
}
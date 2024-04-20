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
using GraphicProject.Utils.Math;
using System.Diagnostics;
//3D model e

namespace myOpenGL
{
    public partial class Form1 : Form
    {

        cOGL cGL;
        object selectedLightingMaterialRadio;
        int formRightMargin;
        float panel1Ratio;
        private DateTime previousUpdateTime;

        public Form1()
        {
            InitializeComponent();

            cGL = new cOGL(panel1, textBox1);
            previousUpdateTime = DateTime.Now; // Initialize the timestamp

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

        private int[] calculateNewSize()
        {
            int rightMargin = 20; // Margin from the right edge of the form.
            int bottomMargin = 20; // Margin from the bottom edge of the form.

            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;
            formRightMargin = formRightMargin == 0 ? formWidth - panel1.Size.Width : formRightMargin;
            panel1Ratio = panel1Ratio == 0 ? (float)panel1.Size.Width / panel1.Size.Height : panel1Ratio;
            // Calculate the available space for panel1
            int maxWidth = formWidth - formRightMargin - rightMargin;
            int maxHeight = formHeight - bottomMargin; // Assuming there's also a topMargin

            // Calculate panelNewWidth and panelNewHeight based on the aspect ratio
            int panelNewWidth = maxWidth;
            int panelNewHeight = (int)(panelNewWidth / panel1Ratio);

            // Adjust if the calculated height exceeds the maximum allowed height
            if (panelNewHeight > maxHeight)
            {
                panelNewHeight = maxHeight;
                panelNewWidth = (int)(panelNewHeight * panel1Ratio);
            }

            // Ensure panelNewWidth does not exceed maxWidth after adjustment
            if (panelNewWidth > maxWidth)
            {
                panelNewWidth = maxWidth;
                // Recalculate height if necessary, though this should not be needed after the initial adjustment
                panelNewHeight = (int)(panelNewWidth / panel1Ratio);
            }
            return new int[] { panelNewHeight, panelNewWidth };
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            // Adjust panel1 size to maintain a dynamic width and a 20px margin from the bottom and right edges of the form.
            int[] newSize = calculateNewSize();
            int panelNewHeight = newSize[0];
            int panelNewWidth = newSize[1];
            panel1.Size = new Size(panelNewWidth, panelNewHeight);

            // Notify cGL of the resize event.
            if (cGL != null)
            {
                cGL.OnResize();
            } 
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            cGL.Draw();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void hScrollBarScroll(object sender, ScrollEventArgs e)
        {
            cGL.intOptionC = TransformationsOperations.NONE;
            HScrollBar hb = (HScrollBar)sender;
            var parentGroupBox = hb.Parent as GroupBox;
            string scrollBarText = hb.Name;

            if (scrollBarText.Contains("hScrollBar"))
            {
                int n = int.Parse(hb.Name.Substring(hb.Name.Length - 1));
                cGL.CameraPointOfView[n - 1] = (hb.Value - 100) / 10.0f;
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
                            switch (scrollBarText)
                            {
                                case "slider1":
                                    slider2.Value = slider3.Value = slider1.Value;
                                    break;
                                case "slider2":
                                    slider1.Value = slider3.Value = slider2.Value;
                                    break;
                                case "slider3":
                                    slider2.Value = slider1.Value = slider3.Value;
                                    break;
                            }
                            float normalizedRValue = slider1.Value / 100.0f;
                            float normalizedGValue = slider2.Value / 100.0f;
                            float normalizedBValue = slider3.Value / 100.0f;
                            float normalizedAValue = slider4.Value / 100.0f;
                            cGL.LightPropertyUpdatedValue = new LightPropertyUpdateKeyAndValue { Key = lightProperty, NewValues = new float[] { normalizedRValue, normalizedGValue, normalizedBValue, normalizedAValue } };
                            break;
                        case LightProperty.POSITION:
                            int w = slider4.Value > 40? 1 : 0;
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
            }

            if (e != null)
                cGL.Draw();
        }

        public float[] oldPos = new float[7];
        private float SHIFT_STEP = 0.25f;
        private int ROTATION_STEP_ANGLE = 5;

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
                        cGL.xShift = SHIFT_STEP;
                        cGL.intOptionC = TransformationsOperations.SHIFT_X;
                    }
                    else
                    {
                        cGL.xShift = -SHIFT_STEP;
                        cGL.intOptionC = TransformationsOperations.SHIFT_OPPOSITE_X;
                    }
                    break;
                case 2:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yShift = SHIFT_STEP;
                        cGL.intOptionC = TransformationsOperations.SHIFT_Y;
                        textBox1.Text = pos.ToString();
                    }
                    else
                    {
                        cGL.yShift = -SHIFT_STEP;
                        cGL.intOptionC = TransformationsOperations.SHIFT_OPPOSITE_Y;
                    }
                    break;
                case 3:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.zShift = SHIFT_STEP;
                        cGL.intOptionC = TransformationsOperations.SHIFT_Z;
                    }
                    else
                    {
                        cGL.zShift = -SHIFT_STEP;
                        cGL.intOptionC = TransformationsOperations.SHIFT_OPPOSITE_Z;
                    }
                    break;
                case 4:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.xAngle = ROTATION_STEP_ANGLE;
                        cGL.intOptionC = TransformationsOperations.ROTATE_X;
                    }
                    else
                    {
                        cGL.xAngle = -ROTATION_STEP_ANGLE;
                        cGL.intOptionC = TransformationsOperations.ROTATE_OPPOSITE_X;
                    }
                    break;
                case 5:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.yAngle = ROTATION_STEP_ANGLE;
                        cGL.intOptionC = TransformationsOperations.ROTATE_Y;
                    }
                    else
                    {
                        cGL.yAngle = -ROTATION_STEP_ANGLE;
                        cGL.intOptionC = TransformationsOperations.ROTATE_OPPOSITE_Y;
                    }
                    break;
                case 6:
                    if (pos > oldPos[i - 1])
                    {
                        cGL.zAngle = ROTATION_STEP_ANGLE;
                        cGL.intOptionC = TransformationsOperations.ROTATE_Z;
                    }
                    else
                    {
                        cGL.zAngle = -ROTATION_STEP_ANGLE;
                        cGL.intOptionC = TransformationsOperations.ROTATE_OPPOSITE_Z;
                    }
                    break;
            }
            cGL.Draw();
            oldPos[i - 1] = pos;
            cGL.intOptionC = TransformationsOperations.NONE;
        }

        private void OnTick(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            float deltaTime = (float)(currentTime - previousUpdateTime).TotalSeconds;
            previousUpdateTime = currentTime;
            this.cGL.Draw(); // Proceed to draw your scene

            // Use deltaTime for updates
            this.cGL.train.Update(deltaTime); // Update your train (and other objects as needed) with deltaTime


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
                if (cGL.DefaultCameraPointOfView != null)
                {
                    // Iterate through all controls in the form or a specific container
                    foreach (Control ctrl in this.Controls) // If the scroll bars are in a specific container, use that instead of 'this.Controls'
                    {
                        if (ctrl is HScrollBar && ctrl.Name.Contains("hScrollBar"))
                        {
                            HScrollBar hb = (HScrollBar)ctrl;
                            int n = int.Parse(hb.Name.Substring("hScrollBar".Length));

                            // Set the scroll bar to its default value, assuming the default value is calculated somehow related to 'DefaultCameraPointOfView'
                            int defaultValue = (int)(cGL.DefaultCameraPointOfView[n - 1] * 10 + 100); // This calculation should be adjusted based on how 'DefaultCameraPointOfView' relates to the scroll bar value
                            hb.Value = Math.Max(hb.Minimum, Math.Min(hb.Maximum, defaultValue)); // Ensure the value is within the min-max range of the scroll bar

                            // Update the CameraPointOfView to reflect the default value
                            cGL.CameraPointOfView[n - 1] = cGL.DefaultCameraPointOfView[n - 1];
                        }
                    }
                }
                // Reset position and rotation to default values
                cGL.xShift = 0.0f;
                cGL.yShift = 0.0f;
                cGL.zShift = 0.0f;
                cGL.xAngle = 0.0f;
                cGL.yAngle = 0.0f;
                cGL.zAngle = 0.0f;
                cGL.AccumulatedRotationsTraslations = new double[]{
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                };
                cGL.intOptionC = TransformationsOperations.NONE; // Reset transformations
            }
            else if (isShiftPressed)
            {
                // Handle rotation when Shift is held down
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        cGL.yAngle = ROTATION_STEP_ANGLE; // Rotate around Y-axis
                        cGL.intOptionC = TransformationsOperations.ROTATE_Y; // Indicating a rotation around Y
                        break;
                    case Keys.Left:
                        cGL.yAngle = -ROTATION_STEP_ANGLE; // Rotate around Y-axis
                        cGL.intOptionC = TransformationsOperations.ROTATE_OPPOSITE_Y; // Indicating a rotation around Y in the opposite direction
                        break;
                    case Keys.Up:
                        cGL.xAngle = ROTATION_STEP_ANGLE; // Rotate around X-axis
                        cGL.intOptionC = TransformationsOperations.ROTATE_X; // Indicating a rotation around X
                        break;
                    case Keys.Down:
                        cGL.xAngle = -ROTATION_STEP_ANGLE; // Rotate around X-axis
                        cGL.intOptionC = TransformationsOperations.ROTATE_OPPOSITE_X; // Indicating a rotation around X in the opposite direction
                        break;
                }
            }
            else
            {
                // Handle translation when Shift is not held down
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        cGL.xShift = SHIFT_STEP;
                        cGL.intOptionC = TransformationsOperations.SHIFT_X; // Indicating a shift along X
                        break;
                    case Keys.Left:
                        cGL.xShift = -SHIFT_STEP;
                        cGL.intOptionC = TransformationsOperations.SHIFT_OPPOSITE_X; // Indicating a shift along X in the opposite direction
                        break;
                    case Keys.Up:
                        cGL.yShift = 0.25f;
                        cGL.intOptionC = TransformationsOperations.SHIFT_Y; // Indicating a shift along Y
                        break;
                    case Keys.Down:
                        cGL.yShift = -SHIFT_STEP;
                        cGL.intOptionC = TransformationsOperations.SHIFT_OPPOSITE_Y; // Indicating a shift along Y in the opposite direction
                        break;
                }
            }

            cGL.Draw();
            e.Handled = true; // Mark the event as handled

            // Reset intOptionC to default after applying transformation and redrawing
            cGL.intOptionC = TransformationsOperations.NONE;
        }



        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                cGL.zShift = SHIFT_STEP; // Zoom in
                cGL.intOptionC = TransformationsOperations.SHIFT_Z; // Assuming positive intOptionC value for zoom in
            }
            else if (e.Delta < 0)
            {
                cGL.zShift = -SHIFT_STEP; // Zoom out
                cGL.intOptionC = TransformationsOperations.SHIFT_OPPOSITE_Z; // Assuming negative intOptionC value for zoom out
            }
            cGL.Draw();
            cGL.intOptionC = TransformationsOperations.NONE; // Reset to default after drawing
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
            cGL.intOptionC = TransformationsOperations.NONE;
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

        private void ShadowReflectionToggle_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            cGL.isReflectionEnabled = ShadowReflectionToggle.Checked;
            cGL.isShadowEnabled = !ShadowReflectionToggle.Checked;

            if (cGL.isReflectionEnabled) cb.Text = "Shadow";
            else cb.Text = "Reflection";
        }
    }
}
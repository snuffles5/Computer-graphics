using Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace OpenGL
{
    class cOGL
    {
        Control p;
        TextBox debugTextBox;
        int Width;
        int Height;
        uint m_uint_HWND = 0;
        uint m_uint_DC = 0;
        uint m_uint_RC = 0;
        gluNewQuadric obj;
        public float[] ScrollValue = new float[10];
        public float zShift = 0.0f;
        public float yShift = 0.0f;
        public float xShift = 0.0f;
        public float zAngle = 0.0f;
        public float yAngle = 0.0f;
        public float xAngle = 0.0f;
        public int intOptionC = 0;
        public Vector3 sunCoords;
        public bool isLightingOn;

        // Initialization of AccumulatedRotationsTraslations to the identity matrix
        double[] AccumulatedRotationsTraslations = new double[]{
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        };
        public Train train;
        public Sun sun;

        public MaterialPropertyUpdateKeyAndValue MaterialPropertyUpdatedValue { get; internal set; }
        public LightPropertyUpdateKeyAndValue LightPropertyUpdatedValue { get; internal set; }

        public cOGL(Control pb, TextBox debugTextBox)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            InitializeGL();
            obj = GLU.gluNewQuadric();
            this.debugTextBox = debugTextBox;
            train = new Train(debugTextBox, 1);
            sun = new Sun(debugTextBox);
            sunCoords = new Vector3();
            debugTextBox.Text = Width + "w, " + Height + "h\n";
            isLightingOn = true;
        }

        ~cOGL()
        {
            GLU.gluDeleteQuadric(obj);
            WGL.wglDeleteContext(m_uint_RC);
        }


        protected virtual void InitializeGL()
        {
            m_uint_HWND = (uint)p.Handle.ToInt32();
            m_uint_DC = WGL.GetDC(m_uint_HWND);
            WGL.wglSwapBuffers(m_uint_DC);

            WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
            WGL.ZeroPixelDescriptor(ref pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = WGL.PFD_DRAW_TO_WINDOW | WGL.PFD_SUPPORT_OPENGL | WGL.PFD_DOUBLEBUFFER;
            pfd.iPixelType = (byte)WGL.PFD_TYPE_RGBA;
            pfd.cColorBits = 32;
            pfd.cDepthBits = 32;
            pfd.iLayerType = (byte)WGL.PFD_MAIN_PLANE;

            int pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
            if (pixelFormatIndex == 0)
            {
                MessageBox.Show("Unable to retrieve pixel format");
                return;
            }

            if (WGL.SetPixelFormat(m_uint_DC, pixelFormatIndex, ref pfd) == 0)
            {
                MessageBox.Show("Unable to set pixel format");
                return;
            }
            m_uint_RC = WGL.wglCreateContext(m_uint_DC);
            if (m_uint_RC == 0)
            {
                MessageBox.Show("Unable to get rendering context");
                return;
            }
            if (WGL.wglMakeCurrent(m_uint_DC, m_uint_RC) == 0)
            {
                MessageBox.Show("Unable to make rendering context current");
                return;
            }

            initRenderingGL();
        }

        public void OnResize()
        {
            GL.glViewport(0, 0, p.Width, p.Height);

            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();
            // Setting up an orthographic projection where the origin (0,0) is the center of the panel
            GLU.gluOrtho2D(-p.Width / 2, p.Width / 2, -p.Height / 2, p.Height / 2);

            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();
        }

        protected virtual void initRenderingGL()
        {
            if (this.Width == 0 || this.Height == 0) return;
            GL.glClearColor(1.0f, 1.0f, 1.0f, 0.0f);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);

            GL.glViewport(0, 0, this.Width, this.Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();
            GLU.gluPerspective(45.0f, (float)Width / (float)Height, 0.1f, 100.0f);

            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();
        }

        private void SetupLightingAndMaterial(bool isLightingOn = true)
        {
            if (isLightingOn)
            {
                GL.glShadeModel(GL.GL_SMOOTH);

                // Lighting setup
                GL.glEnable(GL.GL_LIGHT0);
                GL.glEnable(GL.GL_LIGHTING);

                // Set lighting parameters
                //GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, LightConfig.Instance.Ambient);
                //GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, LightConfig.Instance.Diffuse);
                //GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, LightConfig.Instance.Specular);
                //GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, LightConfig.Instance.Position);

                GL.glEnable(GL.GL_NORMALIZE);
                
                // Enable color material
                GL.glEnable(GL.GL_COLOR_MATERIAL);
                GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);
                if (MaterialPropertyUpdatedValue != null)
                {
                    MaterialConfig.Instance.SetMaterialProperty(MaterialPropertyUpdatedValue.Key, MaterialPropertyUpdatedValue.NewValues, MaterialPropertyUpdatedValue.NewValue);
                    MaterialPropertyUpdatedValue = null;
                }
                if (LightPropertyUpdatedValue != null)
                {
                    LightConfig.Instance.SetLightProperty(LightPropertyUpdatedValue.Key, LightPropertyUpdatedValue.NewValues);
                    LightPropertyUpdatedValue = null;
                }
            }

        }

        public void Draw()
        {
            // Check if device contexts are initialized
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;


            // Clear color and depth buffers
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            // Load identity matrix
            GL.glLoadIdentity();

            // Define arrays to store matrix values
            double[] ModelVievMatrixBeforeSpecificTransforms = new double[16];


            SetupLightingAndMaterial();


            // Set up viewing transformation
            GLU.gluLookAt(ScrollValue[0], ScrollValue[1], ScrollValue[2],
                          ScrollValue[3], ScrollValue[4], ScrollValue[5],
                          ScrollValue[6], ScrollValue[7], ScrollValue[8]);
            GL.glTranslatef(0.0f, 0.0f, -10.0f);

            

            // Save current ModelView Matrix values before specific transformations
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, ModelVievMatrixBeforeSpecificTransforms);
            // Reset ModelView Matrix to identity matrix
            GL.glLoadIdentity();

            // Apply transformation according to KeyCode
            float delta;
            if (intOptionC != 0)
            {
                delta = 5.0f * Math.Abs(intOptionC) / intOptionC; // signed 5

                switch (Math.Abs(intOptionC))
                {
                    case 1:
                        GL.glRotatef(delta, 1, 0, 0);
                        break;
                    case 2:
                        GL.glRotatef(delta, 0, 1, 0);
                        break;
                    case 3:
                        GL.glRotatef(delta, 0, 0, 1);
                        break;
                    case 4:
                        GL.glTranslatef(delta / 20, 0, 0);
                        break;
                    case 5:
                        GL.glTranslatef(0, delta / 20, 0);
                        break;
                    case 6:
                        GL.glTranslatef(0, 0, delta / 20);
                        break;
                }
            }
            // The ModelView Matrix now represents only the KeyCode transform

            
            ApplyAndAccumulateTransformations(ModelVievMatrixBeforeSpecificTransforms);

            GL.glDisable(GL.GL_LIGHT0);
            GL.glDisable(GL.GL_LIGHTING);
            sun.Draw();
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_LIGHTING);
            train.Draw();

            // Flush GL pipeline
            GL.glFlush();

            // Swap buffers
            WGL.wglSwapBuffers(m_uint_DC);
        }

        public void ApplyAndAccumulateTransformations(double[] ModelVievMatrixBeforeSpecificTransforms)
        {

            double[] CurrentRotationTraslation = new double[16];
            // Save current ModelView Matrix values after transformations
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, CurrentRotationTraslation);

            // Replace the current matrix with the global matrix
            GL.glLoadMatrixd(AccumulatedRotationsTraslations);

            // Multiply the current matrix by the transformation matrix
            GL.glMultMatrixd(CurrentRotationTraslation);

            // Save the matrix product in AccumulatedRotationsTraslations
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);

            // Restore ModelView Matrix to the state before KeyCode transformations
            GL.glLoadMatrixd(ModelVievMatrixBeforeSpecificTransforms);
            // Multiply it by the accumulated transformations matrix
            GL.glMultMatrixd(AccumulatedRotationsTraslations);
        }


    }
}

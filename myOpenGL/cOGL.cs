using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenGL
{
    class cOGL
    {
        Control p;
        int Width;
        int Height;
        uint m_uint_HWND = 0;
        uint m_uint_DC = 0;
        uint m_uint_RC = 0;
        GLUquadric obj;
        public float[] ScrollValue = new float[10];
        public float zShift = 0.0f;
        public float yShift = 0.0f;
        public float xShift = 0.0f;
        public float zAngle = 0.0f;
        public float yAngle = 0.0f;
        public float xAngle = 0.0f;
        public int intOptionC = 0;
        double[] ModelVievMatrixBeforeSpecificTransforms = new double[16];
        double[] CurrentRotationTraslation = new double[16];
        double[] AccumulatedRotationsTraslations = new double[16];




        public cOGL(Control pb)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            InitializeGL();
            obj = GLU.gluNewQuadric();
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
			if(WGL.wglMakeCurrent(m_uint_DC,m_uint_RC) == 0)
            {
                MessageBox.Show("Unable to make rendering context current");
                return;
            }

            initRenderingGL();
        }

        public void OnResize()
        {
            Width = p.Width;
            Height = p.Height;
            GL.glViewport(0, 0, Width, Height);
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

        public void Draw()
        {
            // Check if the OpenGL rendering context is properly initialized
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return; // Early return if the context is not available, to prevent errors

            // Clear the screen and depth buffer to prepare for a new frame
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            // Set the viewport to cover the entire window area
            GL.glViewport(0, 0, Width, Height);

            // Reset the current matrix to the identity matrix
            GL.glLoadIdentity();

            // Setup the camera using gluLookAt to control the viewpoint
            // This sets where the camera is located, where it is looking, and which direction is up
            GLU.gluLookAt(ScrollValue[0], ScrollValue[1], ScrollValue[2], // Camera position (eye)
                          ScrollValue[3], ScrollValue[4], ScrollValue[5], // Look-at point (center)
                          ScrollValue[6], ScrollValue[7], ScrollValue[8]); // Up vector

            // Apply translations based on user input for moving the scene
            // These variables (xShift, yShift, zShift) are adjusted elsewhere in the program based on user interactions
            GL.glTranslatef(xShift, yShift, zShift);

            // Apply rotations around each axis based on user input for rotating the scene
            // xAngle, yAngle, and zAngle are adjusted elsewhere in the program based on user interactions
            GL.glRotatef(xAngle, 1.0f, 0.0f, 0.0f); // Rotate around the X-axis by xAngle degrees
            GL.glRotatef(yAngle, 0.0f, 1.0f, 0.0f); // Rotate around the Y-axis by yAngle degrees
            GL.glRotatef(zAngle, 0.0f, 0.0f, 1.0f); // Rotate around the Z-axis by zAngle degrees


            DrawSimpleTrain();

            // Complete any pending OpenGL commands and ensure they are executed
            GL.glFlush();

            // Swap the front and back buffers to display the newly rendered frame
            WGL.wglSwapBuffers(m_uint_DC);
        }


        void DrawSimpleTrain()
        {
            // Example: Draw a simple train using GLU quadrics
            GL.glColor3f(0.5f, 0.0f, 0.0f); // Set the color to red
            GLU.gluCylinder(obj, 0.5, 0.5, 2.0, 32, 32); // Draw the engine

        }
    }
}

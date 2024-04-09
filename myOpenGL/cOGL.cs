using GraphicProject.Utils.Math;
using Milkshape;
using Models;
using System;
using System.Drawing;
using System.Security.Cryptography;
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
        public Milkshape.Character ch;
        public float[] DefaultCameraPointOfView;
        public float[] CameraPointOfView = new float[10];
        public TransformationsOperations intOptionC = TransformationsOperations.NONE;
        public Vector3 sunCoords;
        public bool isLightingOn;

        public float INITIALIZED_ZOOM_VALUE = -2.0f;
        float[,] ground = new float[3, 3];
        float[,] wall = new float[3, 3];
        float[] planeCoeff = { 1, 1, 1, 1 };
        float[] cubeXform = new float[16];
        const int x = 0;
        const int y = 1;
        const int z = 2;
        bool isFirstDraw = true;

        // Initialization of AccumulatedRotationsTraslations to the identity matrix
        public double[] AccumulatedRotationsTraslations = new double[]{
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        };
        public Train train;
        public Sun sun;
        public float xShift;
        public float yShift;
        public float zShift;
        public float xAngle;
        public float yAngle;
        public float zAngle;
        private Rail rails;

        public MaterialPropertyUpdateKeyAndValue MaterialPropertyUpdatedValue { get; internal set; }
        public LightPropertyUpdateKeyAndValue LightPropertyUpdatedValue { get; internal set; }

        public cOGL(Control pb, TextBox debugTextBox)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            InitializeGL();
            obj = GLU.gluNewQuadric();
            Vector3 characterShiftOffset = new Vector3(0, -0.5d, 0);
            Vector3WithAngle characterRotationOffset = new Vector3WithAngle(x: 0.0f,y: 1.0f, z:0.0f, angle: 360);
            ch = new Character("ninja.ms3d", characterShiftOffset, characterRotationOffset);
            this.debugTextBox = debugTextBox;
            train = new Train(debugTextBox, 1);
            sun = new Sun(debugTextBox);
            rails = new Rail();
            sunCoords = new Vector3();
            debugTextBox.Text = Width + "w, " + Height + "h\n";
            isLightingOn = true;

            // Ground and Walls 
            ground[0, 0] = 1;
            ground[0, 1] = 1;
            ground[0, 2] = -0.5f;

            ground[1, 0] = 0;
            ground[1, 1] = 1;
            ground[1, 2] = -0.5f;

            ground[2, 0] = 1;
            ground[2, 1] = 0;
            ground[2, 2] = -0.5f;

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
            Height = p.Height;
            Width = p.Width;
            GL.glViewport(0, 0, Width, Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();
            GLU.gluOrtho2D(-Width / 2, Width / 2, -Height / 2, Height / 2);
            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();

            initRenderingGL();
            // Redraw the scene after resizing
            Draw(); // Assuming Draw() is the method to redraw your scene
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
            GL.glShadeModel(GL.GL_SMOOTH);
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

            if (DefaultCameraPointOfView == null)
            {
                DefaultCameraPointOfView = (float[])CameraPointOfView.Clone();
            }
            // Set up viewing transformation
            GLU.gluLookAt(CameraPointOfView[0], CameraPointOfView[1], CameraPointOfView[2],
                          CameraPointOfView[3], CameraPointOfView[4], CameraPointOfView[5],
                          CameraPointOfView[6], CameraPointOfView[7], CameraPointOfView[8]);
            GL.glTranslatef(0.0f, 0.0f, INITIALIZED_ZOOM_VALUE);

            

            // Save current ModelView Matrix values before specific transformations
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, ModelVievMatrixBeforeSpecificTransforms);
            // Reset ModelView Matrix to identity matrix
            GL.glLoadIdentity();

            MakeTransformation();
            ApplyAndAccumulateTransformations(ModelVievMatrixBeforeSpecificTransforms);

            GL.glDisable(GL.GL_LIGHT0);
            GL.glDisable(GL.GL_LIGHTING);
            sun.Draw();

            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_LIGHTING);
            train.Draw(isShadowDrawing: false);
            //DrawSuprise();
            //DrawRails();

            //DrawShadows();

            // Flush GL pipeline
            GL.glFlush();

            // Swap buffers
            WGL.wglSwapBuffers(m_uint_DC);

            // Todo better:
            isFirstDraw = false;
        }

        private void DrawRails()
        {
            GL.glMatrixMode(GL.GL_MODELVIEW);

            GL.glPushMatrix();

            // Assuming the train is on the ground (y = 0), lower the rails slightly below.
            // This translation moves the rails below the train and positions them to start just behind the front of the train.
            GL.glTranslatef(0.0f, -0.8f, 0.0f); // Adjust these values as needed

            rails.Draw(); // Draw the rail model

            GL.glPopMatrix();
        }


        private void DrawShadows()
        {
            // Shadows
            GL.glDisable(GL.GL_LIGHT0);
            GL.glDisable(GL.GL_LIGHTING);
            // floor shadow
            //!!!!!!!!!!!!!
            GL.glPushMatrix();
            //!!!!!!!!!!!!    		
            MakeShadowMatrix(ground);
            GL.glMultMatrixf(cubeXform);

            train.Draw(isShadowDrawing: true);
            //!!!!!!!!!!!!!
            GL.glPopMatrix();
            //!!!!!!!!!!!!!

            //// wall shadow
            ////!!!!!!!!!!!!!
            //GL.glPushMatrix();
            ////!!!!!!!!!!!!       
            //MakeShadowMatrix(wall);
            //GL.glMultMatrixf(cubeXform);
            //train.Draw(isShadowDrawing: true);
            ////!!!!!!!!!!!!!
            //GL.glPopMatrix();
            ////!!!!!!!!!!!!!       
        }

        private void DrawSuprise()
        {
            GL.glMatrixMode(GL.GL_MODELVIEW);

            MakeTransformation();

            ch.scaleFactor = 0.1f;
            ch.DrawModel();

            // Pop the matrix off the stack, reverting to the state before we applied the translation and scaling
            GL.glPopMatrix();

        }

        private void MakeTransformation()
        {
            // Apply transformation according to KeyCode
            float delta;
            if (intOptionC != TransformationsOperations.NONE)
            {
                switch (intOptionC)
                {
                    case TransformationsOperations.ROTATE_X:
                    case TransformationsOperations.ROTATE_OPPOSITE_X:
                        GL.glRotatef(xAngle, 1, 0, 0);
                        break;
                    case TransformationsOperations.ROTATE_Y:
                    case TransformationsOperations.ROTATE_OPPOSITE_Y:
                        GL.glRotatef(yAngle, 0, 1, 0);
                        break;
                    case TransformationsOperations.ROTATE_Z:
                    case TransformationsOperations.ROTATE_OPPOSITE_Z:
                        GL.glRotatef(zAngle, 0, 0, 1);
                        break;
                    case TransformationsOperations.SHIFT_X:
                    case TransformationsOperations.SHIFT_OPPOSITE_X:
                        GL.glTranslatef(xShift, 0, 0);
                        break;
                    case TransformationsOperations.SHIFT_Y:
                    case TransformationsOperations.SHIFT_OPPOSITE_Y:
                        GL.glTranslatef(0, yShift, 0);
                        break;
                    case TransformationsOperations.SHIFT_Z:
                    case TransformationsOperations.SHIFT_OPPOSITE_Z:
                        GL.glTranslatef(0, 0, zShift);
                        break;
                    default:
                        // No transformation
                        break;
                }

            }
            // The ModelView Matrix now represents only the KeyCode transform
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

        void ReduceToUnit(float[] vector)
        {
            float length;

            // Calculate the length of the vector		
            length = (float)Math.Sqrt((vector[0] * vector[0]) +
                                (vector[1] * vector[1]) +
                                (vector[2] * vector[2]));

            // Keep the program from blowing up by providing an exceptable
            // value for vectors that may calculated too close to zero.
            if (length == 0.0f)
                length = 1.0f;

            // Dividing each element by the length will result in a
            // unit normal vector.
            vector[0] /= length;
            vector[1] /= length;
            vector[2] /= length;
        }

        void calcNormal(float[,] v, float[] outp)
        {
            float[] v1 = new float[3];
            float[] v2 = new float[3];

            // Calculate two vectors from the three points
            v1[x] = v[0, x] - v[1, x];
            v1[y] = v[0, y] - v[1, y];
            v1[z] = v[0, z] - v[1, z];

            v2[x] = v[1, x] - v[2, x];
            v2[y] = v[1, y] - v[2, y];
            v2[z] = v[1, z] - v[2, z];

            // Take the cross product of the two vectors to get
            // the normal vector which will be stored in out
            outp[x] = v1[y] * v2[z] - v1[z] * v2[y];
            outp[y] = v1[z] * v2[x] - v1[x] * v2[z];
            outp[z] = v1[x] * v2[y] - v1[y] * v2[x];

            // Normalize the vector (shorten length to one)
            ReduceToUnit(outp);
        }

        void MakeShadowMatrix(float[,] points)
        {
            float[] planeCoeff = new float[4];
            float dot;

            // Find the plane equation coefficients
            // Find the first three coefficients the same way we
            // find a normal.
            calcNormal(points, planeCoeff);

            // Find the last coefficient by back substitutions
            planeCoeff[3] = -(
                (planeCoeff[0] * points[2, 0]) + (planeCoeff[1] * points[2, 1]) +
                (planeCoeff[2] * points[2, 2]));


            // Dot product of plane and light position
            dot = planeCoeff[0] * LightConfig.Instance.Position[0] +
                    planeCoeff[1] * LightConfig.Instance.Position[1] +
                    planeCoeff[2] * LightConfig.Instance.Position[2] +
                    planeCoeff[3];

            // Now do the projection
            // First column

            cubeXform[0] = dot - LightConfig.Instance.Position[0] * planeCoeff[0];
            cubeXform[4] = 0.0f - LightConfig.Instance.Position[0] * planeCoeff[1];
            cubeXform[8] = 0.0f - LightConfig.Instance.Position[0] * planeCoeff[2];
            cubeXform[12] = 0.0f - LightConfig.Instance.Position[0] * planeCoeff[3];

            // Second column
            cubeXform[1] = 0.0f - LightConfig.Instance.Position[1] * planeCoeff[0];
            cubeXform[5] = dot - LightConfig.Instance.Position[1] * planeCoeff[1];
            cubeXform[9] = 0.0f - LightConfig.Instance.Position[1] * planeCoeff[2];
            cubeXform[13] = 0.0f - LightConfig.Instance.Position[1] * planeCoeff[3];

            // Third Column
            cubeXform[2] = 0.0f - LightConfig.Instance.Position[2] * planeCoeff[0];
            cubeXform[6] = 0.0f - LightConfig.Instance.Position[2] * planeCoeff[1];
            cubeXform[10] = dot - LightConfig.Instance.Position[2] * planeCoeff[2];
            cubeXform[14] = 0.0f - LightConfig.Instance.Position[2] * planeCoeff[3];

            // Fourth Column
            cubeXform[3] = 0.0f - LightConfig.Instance.Position[3] * planeCoeff[0];
            cubeXform[7] = 0.0f - LightConfig.Instance.Position[3] * planeCoeff[1];
            cubeXform[11] = 0.0f - LightConfig.Instance.Position[3] * planeCoeff[2];
            cubeXform[15] = dot - LightConfig.Instance.Position[3] * planeCoeff[3];
        }


        void DrawObjects(bool isForShades, int c)
        {

            if (!isForShades)
                GL.glColor3d(1, 0, 0);
            else
                if (c == 1)
                GL.glColor3d(0.5, 0.5, 0.5);
            else
                GL.glColor3d(0.8, 0.8, 0.8);
            GLUT.glutSolidCube(1);

            GL.glTranslated(1, 2, 0.3);
            GL.glRotated(90, 1, 0, 0);
            if (!isForShades)
                GL.glColor3d(0, 1, 1);
            else
                if (c == 1)
                GL.glColor3d(0.5, 0.5, 0.5);
            else
                GL.glColor3d(0.8, 0.8, 0.8);
            GLUT.glutSolidTeapot(1);
            GL.glRotated(-90, 1, 0, 0);
            GL.glTranslated(-1, -2, -0.3);
        }

    }
}

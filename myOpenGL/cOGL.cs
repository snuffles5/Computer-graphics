using GraphicProject.Utils.Math;
using Milkshape;
using Models;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Policy;
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
        public float[] CameraPointOfView = new float[10];
        public float[] DefaultCameraPointOfView = new float[10];
        public TransformationsOperations intOptionC = TransformationsOperations.NONE;
        public Vector3 sunCoords;
        public bool isLightingEnabled;
        public bool isShadowEnabled;
        public bool isReflectionEnabled;
        public bool isToDrawGround;

        public float INITIALIZED_ZOOM_VALUE = -2.0f;
        //float[,] ground = new float[3, 3];
        Vector3[] groundVertices = new Vector3[3];
        float[] groundPlane;
        Vector3[] shadowPlaneVertices = new Vector3[4];
        Vector3[] wallVertexes = new Vector3[4];
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
        private Color groundColor = Color.LightGray;
        private Color wallColor = Color.SkyBlue;

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
            Vector3WithAngle characterRotationOffset = new Vector3WithAngle(x: 0.0f, y: 1.0f, z: 0.0f, angle: 360);
            ch = new Character("ninja.ms3d", characterShiftOffset, characterRotationOffset);
            this.debugTextBox = debugTextBox;
            train = new Train(debugTextBox, 1, isTextureEnabled: true);
            sun = new Sun(debugTextBox);
            rails = new Rail();
            sunCoords = new Vector3();
            debugTextBox.Text = Width + "w, " + Height + "h\n";
            isLightingEnabled = true;
            isShadowEnabled = true;
            isReflectionEnabled = false;
            isToDrawGround = true;

            DefaultCameraPointOfView = new float[]
            {
                0.0f, 0.0f, 10.0f,  // Eye position (X right-left, Y up-down, Z depth)
                0.0f, 0.0f, 0.0f,   // Look at the origin
                0.0f, 1.0f, 0.0f
            };
            // Ground and Walls 
            groundVertices[0] = new Vector3(1.0f, 1.0f, -0.5f);
            groundVertices[1] = new Vector3(0.0f, 1.0f, -0.5f);
            groundVertices[2] = new Vector3(1.0f, 0.0f, -0.5f);
            //groundVertices[3] = new Vector3(-0.5f, -0.5f, -0.5f);  // Assuming a fourth vertex if needed, adjust as necessary
            //ground[0, 0] = 1;
            //ground[0, 1] = 1;
            //ground[0, 2] = -0.5f;

            //ground[1, 0] = 0;
            //ground[1, 1] = 1;
            //ground[1, 2] = -0.5f;

            //ground[2, 0] = 1;
            //ground[2, 1] = 0;
            //ground[2, 2] = -0.5f;
            shadowPlaneVertices[0] = new Vector3(-50.0f, -1.5f, -50.0f);
            shadowPlaneVertices[1] = new Vector3(-50.0f, -1.5f, 50.0f);
            shadowPlaneVertices[2] = new Vector3(50.0f, -1.5f, 50.0f);
            shadowPlaneVertices[3] = new Vector3(50.0f, -1.5f, -50.0f);
            
            groundPlane = new float[] { 0.0f, 1.0f, 0.0f, 0.0f };
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
            pfd.cStencilBits = 32; //for Stencil support 

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

        private void SetupLightingAndMaterial(bool isLightingEnabled = true)
        {
            if (!isLightingEnabled)
                return;
            GL.glShadeModel(GL.GL_SMOOTH);

            // Lighting setup
            EnableLighting();


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

        public void EnableLighting()
        {
            if (!isLightingEnabled)
                return;
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_LIGHTING);
            //GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, LightConfig.Instance.Position);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, LightConfig.Instance.Ambient);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, LightConfig.Instance.Diffuse);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, LightConfig.Instance.Specular);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, LightConfig.Instance.Position);
        }
        public void DisableLighting()
        {
            GL.glDisable(GL.GL_LIGHT0);
            GL.glDisable(GL.GL_LIGHTING);
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

            if (CameraPointOfView == null)
            {
                CameraPointOfView = (float[])DefaultCameraPointOfView.Clone();
            }
            GLU.gluLookAt(CameraPointOfView[0], CameraPointOfView[1], CameraPointOfView[2],
                          CameraPointOfView[3], CameraPointOfView[4], CameraPointOfView[5],
                          CameraPointOfView[6], CameraPointOfView[7], CameraPointOfView[8]);  // Up vector is along Y-axis

            GL.glTranslatef(0.0f, 0.0f, INITIALIZED_ZOOM_VALUE);

            // Save current ModelView Matrix values before specific transformations
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, ModelVievMatrixBeforeSpecificTransforms);
            // Reset ModelView Matrix to identity matrix
            GL.glLoadIdentity();

            MakeTransformation();
            ApplyAndAccumulateTransformations(ModelVievMatrixBeforeSpecificTransforms);

            Vector3 lightDirection = new Vector3(
                LightConfig.Instance.Position[0],
                LightConfig.Instance.Position[1],
                LightConfig.Instance.Position[2]
            );

            DrawGround();
            DrawWalls();

            DrawDebug(-1 * lightDirection, sun.Coords, 10.0f);
            DrawShadow();
            DrawReflections();

            DrawScene();

            // Flush GL pipeline
            GL.glFlush();

            // Swap buffers
            WGL.wglSwapBuffers(m_uint_DC);

            // Todo better:
            isFirstDraw = false;
        }

        private void DrawScene()
        {
            DisableLighting();
            sun.Draw();

            EnableLighting();
            train.Draw(isShadowDrawing: false);
            DrawSuprise();
            DrawRails();

        }

        public void DrawGround()
        {
            if (!isToDrawGround)
                return;

            // Enable lighting
            EnableLighting();

            // Set material properties for the ground here if needed
            float[] matAmbient = new float[] { 0.7f, 0.7f, 0.7f, 1.0f };
            float[] matDiffuse = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };
            //GL.glMaterialfv(GL.GL_FRONT, GL.GL_AMBIENT, matAmbient);
            //GL.glMaterialfv(GL.GL_FRONT, GL.GL_DIFFUSE, matDiffuse);

            // Make sure the normal vector is set correctly
            GL.glNormal3f(0.0f, 1.0f, 0.0f); // Assuming ground plane is XZ plane

            // Draw the ground plane
            ColorUtil.SetColor(groundColor);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3d(shadowPlaneVertices[0].X, shadowPlaneVertices[0].Y, shadowPlaneVertices[0].Z);
            GL.glVertex3d(shadowPlaneVertices[1].X, shadowPlaneVertices[1].Y, shadowPlaneVertices[1].Z);
            GL.glVertex3d(shadowPlaneVertices[2].X, shadowPlaneVertices[2].Y, shadowPlaneVertices[2].Z);
            GL.glVertex3d(shadowPlaneVertices[3].X, shadowPlaneVertices[3].Y, shadowPlaneVertices[3].Z);
            GL.glEnd();

            // Disable lighting if it's not needed afterwards
            DisableLighting();
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

        void DrawDebug(Vector3 lightDirection, Vector3 startPoint, float length)
        {
            // DrawLightDirection
            // Scale the light direction by the desired length
            Vector3 endPoint = new Vector3(
                startPoint.X + lightDirection.X * length,
                startPoint.Y + lightDirection.Y * length,
                startPoint.Z + lightDirection.Z * length
            );

            // Set the color for the line (red for visibility)
            ColorUtil.SetColor(Color.LightGoldenrodYellow);

            // Begin drawing lines
            GL.glBegin(GL.GL_LINES);
            GL.glVertex3d(startPoint.X, startPoint.Y, startPoint.Z);
            GL.glVertex3d(endPoint.X, endPoint.Y, endPoint.Z);
            GL.glEnd();
        }

private void DrawShadow()
        {
            if (!isShadowEnabled)
                return;

            // Shadows
            DisableLighting();
            GL.glDisable(GL.GL_DEPTH_TEST);
            GL.glEnable(GL.GL_STENCIL_TEST);

            // floor shadow
            GL.glPushMatrix();
            MakeShadowMatrix(shadowPlaneVertices);
            GL.glMultMatrixf(cubeXform);
            train.Draw(isShadowDrawing: true);
            GL.glPopMatrix();
            GL.glEnable(GL.GL_DEPTH_TEST);
            EnableLighting();
        }

        public void DrawReflections()
        {
            if (!isReflectionEnabled)
                return;
            // Enable stencil buffer to restrict the drawing area of the reflection
            GL.glEnable(GL.GL_STENCIL_TEST);
            GL.glStencilOp(GL.GL_REPLACE, GL.GL_REPLACE, GL.GL_REPLACE);
            GL.glStencilFunc(GL.GL_ALWAYS, 1, 0xffffffff);
            GL.glClearStencil(0);

            // Clear the stencil buffer
            GL.glClear(GL.GL_STENCIL_BUFFER_BIT);

            // Disable writing to color and depth buffers
            GL.glColorMask(0, 0, 0, 0);
            GL.glDisable(GL.GL_DEPTH_TEST);

            // Draw ground to the stencil buffer
            DrawGround();

            // Enable color and depth buffers again
            GL.glColorMask(1, 1, 1, 1);
            GL.glEnable(GL.GL_DEPTH_TEST);

            // Make sure that the reflection only gets drawn where the stencil buffer is set to 1
            GL.glStencilFunc(GL.GL_EQUAL, 1, 0xffffffff);
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

            // Set up the reflection transformation matrix
            GL.glPushMatrix();
            GL.glScalef(1.0f, -1.0f, 1.0f);  // Reflect along the Y-axis

            // Draw the reflected objects
            DrawScene();

            // Restore the original matrix
            GL.glPopMatrix();

            // Disable stencil test to draw normally again
            GL.glDisable(GL.GL_STENCIL_TEST);

            // Enable blending to smooth out the reflection
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

            // Set a transparent color for the ground
            ColorUtil.SetColor(groundColor, 0.5f);

            // Draw the ground again to blend with the reflection
            DrawGround();

            // Disable blending
            GL.glDisable(GL.GL_BLEND);
        }

        private void DrawWalls()
        {
            DisableLighting();
            ColorUtil.SetColor(wallColor);
            float wallHeight = 200;
            for (int i = 0; i < 4; i++)
            {
                wallVertexes[i] = new Vector3(shadowPlaneVertices[i].X, shadowPlaneVertices[i].Y + wallHeight, shadowPlaneVertices[i].Z);
            }

            // Define each wall with 4 vertices (2 ground vertices and 2 top vertices)
            // Assuming we're creating quads for simplicity, though you could use triangles as well
            for (int i = 0; i < 4; i++)
            {
                // Define vertices for wall i
                Vector3 bottomStart = shadowPlaneVertices[i];
                Vector3 bottomEnd = shadowPlaneVertices[(i + 1) % 4]; // Loop around with modulo
                Vector3 topStart = wallVertexes[i];
                Vector3 topEnd = wallVertexes[(i + 1) % 4];

                GL.glBegin(GL.GL_QUADS);
                GL.glVertex3d(bottomStart.X, bottomStart.Y, bottomStart.Z);
                GL.glVertex3d(bottomEnd.X, bottomEnd.Y, bottomEnd.Z);
                GL.glVertex3d(topEnd.X, topEnd.Y, topEnd.Z);
                GL.glVertex3d(topStart.X, topStart.Y, topStart.Z);
                GL.glEnd();
            }
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

        void calculateNormal(float[,] v, float[] outp)
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
            calculateNormal(points, planeCoeff);

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

        void MakeShadowMatrix(Vector3[] points)
        {
            Vector3 lightPosition = new Vector3(
                LightConfig.Instance.Position[0],
                LightConfig.Instance.Position[1],
                LightConfig.Instance.Position[2]
            );
            double lightW = LightConfig.Instance.Position[3];  // Homogeneous coordinate

            Vector3 v1 = points[1] - points[0];
            Vector3 v2 = points[2] - points[0];
            Vector3 normal = v1.CrossProduct(v2).Normalize();
            //if (normal.Y < 0) normal = -1 * normal;

            // Calculate the plane coefficient D (Ax + By + Cz + D = 0)
            double planeD = -(normal.X * points[0].X + normal.Y * points[0].Y + normal.Z * points[0].Z);

            // Compute dot product of plane normal and light position
            double dot = normal.X * lightPosition.X + normal.Y * lightPosition.Y + normal.Z * lightPosition.Z + planeD * lightW;
            // Initialize shadow matrix
            float[] shadowMatrix = new float[16];
            //lightPosition = -1 * lightPosition;  // Invert the light position to ensure shadows cast away from the light

            //dot = -dot;
            // Fill the shadow matrix according to the shadow matrix formula
            shadowMatrix[0] = (float)(dot - lightPosition.X * normal.X);
            shadowMatrix[4] = (float)(-lightPosition.X * normal.Y);
            shadowMatrix[8] = (float)(-lightPosition.X * normal.Z);
            shadowMatrix[12] = (float)(-lightPosition.X * planeD);

            shadowMatrix[1] = (float)(-lightPosition.Y * normal.X);
            shadowMatrix[5] = (float)(dot - lightPosition.Y * normal.Y);
            shadowMatrix[9] = (float)(-lightPosition.Y * normal.Z);
            shadowMatrix[13] = (float)(-lightPosition.Y * planeD);

            shadowMatrix[2] = (float)(-lightPosition.Z * normal.X);
            shadowMatrix[6] = (float)(-lightPosition.Z * normal.Y);
            shadowMatrix[10] = (float)(dot - lightPosition.Z * normal.Z);
            shadowMatrix[14] = (float)(-lightPosition.Z * planeD);

            shadowMatrix[3] = 0.0f;
            shadowMatrix[7] = 0.0f;
            shadowMatrix[11] = 0.0f;
            shadowMatrix[15] = (float)dot;

            // Replace cubeXform with the calculated shadowMatrix
            cubeXform = shadowMatrix;

            // Print the matrix for debugging
            Console.WriteLine("Shadow Matrix:");
            for (int i = 0; i < 16; i++)
            {
                if (i % 4 == 0) Console.WriteLine();
                Console.Write(shadowMatrix[i] + " ");
            }
            Console.WriteLine();
        }


        Vector3 calculateNormal(Vector3[] points)
        {
            Vector3 v1 = points[1] - points[0];
            Vector3 v2 = points[2] - points[0];
            Vector3 normal = v1.CrossProduct(v2);

            // Ensure the normal points upwards by checking its Y component
            if (normal.Y < 0)
                normal = -1 * normal;

            return normal.Normalize();
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

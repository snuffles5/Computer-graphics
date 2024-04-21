using GraphicProject.Utils.Math;
using Milkshape;
using Models;
using System;
using System.Drawing;
using System.Windows.Forms;
using Utils;
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
        Vector3[] groundPlaneVertices = new Vector3[4];
        Vector3[] wallVertexes = new Vector3[4];
        float[] cubeXform = new float[16];
        const int x = 0;
        const int y = 1;
        const int z = 2;

        public double[] AccumulatedRotationsTraslations = new double[]{
        1, 0, 0, 0, // Identity matrix row 1
        0, 1, 0, 0, // Identity matrix row 2
        0, 0, 1, 0, // Identity matrix row 3
        0, 0, 0, 1  // Identity matrix row 4
    };
        public Train train;
        public Sun sun;
        public float xShift;
        public float yShift;
        public float zShift;
        public float xAngle;
        public float yAngle;
        public float zAngle;
        public float groundHeight;
        public int numberOfCoaches;
        private Rail rails;
        private Color groundColor = Color.DimGray;
        private Color wallColor = Color.SkyBlue;
        internal float newDoorAngle;
        internal bool isDoorOpened;

        public MaterialPropertyUpdateKeyAndValue MaterialPropertyUpdatedValue { get; internal set; }
        public LightPropertyUpdateKeyAndValue LightPropertyUpdatedValue { get; internal set; }

        // Constructor initializes OpenGL context and objects
        public cOGL(Control pb, TextBox debugTextBox, int numberOfCoaches)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            InitializeGL();
            obj = GLU.gluNewQuadric();
            Vector3 characterShiftOffset = new Vector3(0, -0.5d, 0);
            Vector3WithAngle characterRotationOffset = new Vector3WithAngle(0.0f, 1.0f, 0.0f, 360);
            ch = new Character("ninja.ms3d", characterShiftOffset, characterRotationOffset);
            this.debugTextBox = debugTextBox;
            this.numberOfCoaches = numberOfCoaches;
            train = new Train(debugTextBox, numberOfCoaches: numberOfCoaches, isTextureEnabled: true);
            sun = new Sun(debugTextBox);
            rails = new Rail(train.MainLocomotive.WheelOffsetZ);
            sunCoords = new Vector3();
            debugTextBox.Text = Width + "w, " + Height + "h\n";
            isLightingEnabled = true;
            isShadowEnabled = true;
            isReflectionEnabled = false;
            isToDrawGround = true;

            DefaultCameraPointOfView = new float[]
            {
            0.0f, 12.5f, 5.0f,  // Camera's eye position
            0.0f, 0.0f, 0.0f,   // Camera's target
            0.0f, 1.0f, 0.0f    // Camera's up vector
            };
            groundHeight = -2.0f;
            groundPlaneVertices[0] = new Vector3(-50.0f, groundHeight, -50.0f);
            groundPlaneVertices[1] = new Vector3(-50.0f, groundHeight, 50.0f);
            groundPlaneVertices[2] = new Vector3(50.0f, groundHeight, 50.0f);
            groundPlaneVertices[3] = new Vector3(50.0f, groundHeight, -50.0f);
        }

        // Destructor releases OpenGL resources
        ~cOGL()
        {
            GLU.gluDeleteQuadric(obj);
            WGL.wglDeleteContext(m_uint_RC);
        }


    protected virtual void InitializeGL()
        {
            // Assign handle from control to window handle
            m_uint_HWND = (uint)p.Handle.ToInt32();
            // Get device context for the window
            m_uint_DC = WGL.GetDC(m_uint_HWND);
            // Swap the buffers of the device context for double buffering
            WGL.wglSwapBuffers(m_uint_DC);

            WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
            WGL.ZeroPixelDescriptor(ref pfd);  // Initialize pixel format descriptor
            pfd.nVersion = 1;  // Set version number
            pfd.dwFlags = WGL.PFD_DRAW_TO_WINDOW | WGL.PFD_SUPPORT_OPENGL | WGL.PFD_DOUBLEBUFFER;  // Set flags to use double-buffered window with OpenGL
            pfd.iPixelType = (byte)WGL.PFD_TYPE_RGBA;  // Use RGBA pixel type
            pfd.cColorBits = 32;  // Set color depth
            pfd.cDepthBits = 32;  // Set depth buffer precision
            pfd.cStencilBits = 32;  // Set stencil buffer for shadow
            pfd.iLayerType = (byte)WGL.PFD_MAIN_PLANE;  // Set layer type

            // Choose pixel format that best matches the specified one
            int pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
            if (pixelFormatIndex == 0)
            {
                MessageBox.Show("Unable to retrieve pixel format");  // Error handling if pixel format not found
                return;
            }

            // Set the pixel format for the device context
            if (WGL.SetPixelFormat(m_uint_DC, pixelFormatIndex, ref pfd) == 0)
            {
                MessageBox.Show("Unable to set pixel format");  // Error handling if pixel format setting fails
                return;
            }
            // Create rendering context
            m_uint_RC = WGL.wglCreateContext(m_uint_DC);
            if (m_uint_RC == 0)
            {
                MessageBox.Show("Unable to get rendering context");  // Error handling if rendering context creation fails
                return;
            }
            // Set the current rendering context
            if (WGL.wglMakeCurrent(m_uint_DC, m_uint_RC) == 0)
            {
                MessageBox.Show("Unable to make rendering context current");  // Error handling if setting context fails
                return;
            }
            initRenderingGL();  // Initialize rendering settings
        }

        public void OnResize()
        {
            Height = p.Height; // Update height from the control
            Width = p.Width;   // Update width from the control
            GL.glViewport(0, 0, Width, Height); // Set viewport to match new dimensions
            GL.glMatrixMode(GL.GL_PROJECTION); // Switch to projection matrix
            GL.glLoadIdentity(); // Reset projection matrix
                                 // Set orthographic projection to cover new dimensions
            GLU.gluOrtho2D(-Width / 2, Width / 2, -Height / 2, Height / 2);
            GL.glMatrixMode(GL.GL_MODELVIEW); // Switch back to model view matrix
            GL.glLoadIdentity(); // Reset model view matrix

            initRenderingGL(); // Initialize OpenGL rendering settings
            Draw(); // Redraw the scene after resizing
        }

        protected virtual void initRenderingGL()
        {
            if (this.Width == 0 || this.Height == 0) return; // Prevent division by zero in perspective calculation
            GL.glClearColor(1.0f, 1.0f, 1.0f, 0.0f); // Set clear color to white
            GL.glEnable(GL.GL_DEPTH_TEST); // Enable depth testing
            GL.glDepthFunc(GL.GL_LEQUAL); // Specify depth comparison function

            GL.glViewport(0, 0, this.Width, this.Height); // Set viewport to current dimensions
            GL.glMatrixMode(GL.GL_PROJECTION); // Switch to projection matrix
            GL.glLoadIdentity(); // Reset projection matrix
                                 // Set perspective projection with a 45-degree field of view and depth range 0.1 to 100.0
            GLU.gluPerspective(45.0f, (float)Width / (float)Height, 0.1f, 100.0f);

            GL.glMatrixMode(GL.GL_MODELVIEW); // Switch back to model view matrix
            GL.glShadeModel(GL.GL_SMOOTH); // Set shading to smooth
            GL.glLoadIdentity(); // Reset model view matrix
        }

        private void SetupLightingAndMaterial(bool isLightingEnabled = true)
        {
            if (!isLightingEnabled) return; // Exit if lighting is not enabled

            GL.glShadeModel(GL.GL_SMOOTH); // Set smooth shading

            EnableLighting(); // Configure initial lighting settings

            GL.glEnable(GL.GL_NORMALIZE); // Enable normalization of normal vectors

            GL.glEnable(GL.GL_COLOR_MATERIAL); // Allow materials to use glColor values
            GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE); // Apply ambient and diffuse components

            if (MaterialPropertyUpdatedValue != null)
            {
                // Update material properties from stored settings
                MaterialConfig.Instance.SetMaterialProperty(MaterialPropertyUpdatedValue.Key, MaterialPropertyUpdatedValue.NewValues, MaterialPropertyUpdatedValue.NewValue);
                MaterialPropertyUpdatedValue = null; // Clear the update after applying
            }
            if (LightPropertyUpdatedValue != null)
            {
                // Update lighting properties from stored settings
                LightConfig.Instance.SetLightProperty(LightPropertyUpdatedValue.Key, LightPropertyUpdatedValue.NewValues);
                LightPropertyUpdatedValue = null; // Clear the update after applying
            }
        }

        public void EnableLighting()
        {
            if (!isLightingEnabled) return; // Exit if lighting is globally disabled

            GL.glEnable(GL.GL_LIGHT0); // Enable the first light source
            GL.glEnable(GL.GL_LIGHTING); // Enable lighting
            // Set light source parameters (ambient, diffuse, specular, position)
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, LightConfig.Instance.Ambient);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, LightConfig.Instance.Diffuse);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, LightConfig.Instance.Specular);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, LightConfig.Instance.Position);
        }

        public void DisableLighting()
        {
            GL.glDisable(GL.GL_LIGHT0); // Disable the first light source
            GL.glDisable(GL.GL_LIGHTING); // Turn off lighting
        }

        public void Draw()
        {
            // Check if device contexts are initialized
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;

            // Clear color, depth, and stencil buffers to prepare for new drawing
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);

            // Load the identity matrix to reset transformations
            GL.glLoadIdentity();

            // Array to store matrix values before applying transformations
            double[] ModelVievMatrixBeforeSpecificTransforms = new double[16];

            // Setup lighting and material properties
            SetupLightingAndMaterial();

            // Use default camera position if not set
            if (CameraPointOfView == null)
            {
                CameraPointOfView = (float[])DefaultCameraPointOfView.Clone();
            }

            // Set camera position and orientation
            GLU.gluLookAt(CameraPointOfView[0], CameraPointOfView[1], CameraPointOfView[2],
                          CameraPointOfView[3], CameraPointOfView[4], CameraPointOfView[5],
                          CameraPointOfView[6], CameraPointOfView[7], CameraPointOfView[8]); // Camera setup with up vector

            // Apply initial zoom transformation
            GL.glTranslatef(0.0f, 0.0f, INITIALIZED_ZOOM_VALUE);

            // Save current ModelView matrix before applying specific transformations
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, ModelVievMatrixBeforeSpecificTransforms);

            // Reset ModelView matrix to identity
            GL.glLoadIdentity();

            // Perform object-specific transformations
            MakeTransformation();

            // Apply accumulated transformations
            ApplyAndAccumulateTransformations(ModelVievMatrixBeforeSpecificTransforms);

            // Update light direction based on transformations
            Vector3 lightDirection = new Vector3(
                LightConfig.Instance.Position[0],
                LightConfig.Instance.Position[1],
                LightConfig.Instance.Position[2]
            );

            // Adjust locomotive door angle based on user input
            train.MainLocomotive.SetDoorAngle(newDoorAngle);

            // Set ground vertices to current ground height
            groundPlaneVertices[0].Y = groundPlaneVertices[1].Y = groundPlaneVertices[2].Y = groundPlaneVertices[3].Y = groundHeight;

            // Draw reflections if enabled (including ground), otherwise draw the ground
            if (isReflectionEnabled) DrawReflections();
            else DrawGroundWithStencil();

            // Draw walls that form the skyline
            DrawSkylineWalls();

            // Draw shadows for objects
            DrawShadow();

            // Render the entire scene
            DrawScene();

            // Flush OpenGL commands to ensure all operations are completed
            GL.glFlush();

            // Swap the front and back buffers to display the rendered image
            WGL.wglSwapBuffers(m_uint_DC);
        }

        private void DrawScene()
        {
            // Disable lighting for drawing the sun, as it emits its own light
            DisableLighting();
            GL.glPushMatrix(); // Preserve the current matrix state
            sun.Draw(); // Draw the sun
            GL.glPopMatrix(); // Restore the previous matrix state

            // Re-enable lighting for other elements
            EnableLighting();
            GL.glPushMatrix(); // Preserve the matrix state again
            DrawTrain(isShadowDrawing: false); // Draw the train without shadow effects
            GL.glPopMatrix();
            DrawRails(); // Draw rails on which the train moves
        }

        public void DrawTrain(bool isShadowDrawing)
        {
            GL.glPushMatrix(); // Save the current transformation state
            GL.glTranslatef(this.train.PositionX, 0.0f, 0.0f); // Apply the train's current position
            train.Draw(isShadowDrawing: isShadowDrawing); // Draw the train
            GL.glPopMatrix(); // Restore the previous transformation state
        }


        public void DrawGround(float alpha = 0.5f)
        {
            if (!isToDrawGround) return; // Check if ground drawing is enabled

            EnableLighting(); // Ensure lighting is enabled for realistic effects
            if (isReflectionEnabled) ColorUtil.SetColor(groundColor, alpha); // Set translucent ground color for reflections
            else ColorUtil.SetColor(groundColor); // Set opaque ground color

            // Normal vector for the ground plane is upward
            GL.glNormal3f(0.0f, 1.0f, 0.0f);

            // Draw ground plane as a quadrilateral
            GL.glBegin(GL.GL_QUADS);
            foreach (Vector3 vertex in groundPlaneVertices)
            {
                GL.glVertex3d(vertex.X, vertex.Y, vertex.Z);
            }
            GL.glEnd();

            GL.glDisable(GL.GL_TEXTURE_2D); // Disable textures after drawing ground
        }

        private void DrawGroundWithStencil()
        {
            GL.glEnable(GL.GL_STENCIL_TEST); // Enable the stencil test
            GL.glStencilFunc(GL.GL_ALWAYS, 1, 0xFF); // Set the stencil buffer to always pass
            GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_REPLACE); // Set stencil operation
            GL.glStencilMask(0xFF); // Enable writing to the stencil buffer
            GL.glDepthMask((Byte)GL.GL_FALSE); // Disable writing to the depth buffer
            GL.glClear(GL.GL_STENCIL_BUFFER_BIT); // Clear the stencil buffer

            // Draw the ground
            DrawGround();

            GL.glDepthMask((Byte)GL.GL_TRUE); // Enable writing to the depth buffer
            GL.glStencilMask(0x00); // Disable writing to the stencil buffer
        }


        private void DrawRails()
        {
            GL.glMatrixMode(GL.GL_MODELVIEW); // Set mode to ModelView for transformations

            GL.glPushMatrix(); // Save current matrix state

            float railYOffset = -1.35f; // Offset to position rails slightly below the train
            GL.glTranslatef(0.0f, railYOffset, 0.0f); // Translate rails to correct position

            rails.Draw(); // Draw the rail model

            GL.glPopMatrix(); // Restore previous matrix state
        }

        private void DrawShadow()
        {
            if (!isShadowEnabled) return; // Skip if shadow drawing is disabled

            GL.glEnable(GL.GL_STENCIL_TEST); // Ensure stencil test is enabled
            GL.glStencilFunc(GL.GL_EQUAL, 1, 0xFF); // Stencil must equal the reference value
            GL.glStencilMask(0x00); // Disable writing to the stencil buffer

            DisableLighting();
            GL.glDisable(GL.GL_DEPTH_TEST);

            GL.glPushMatrix();
            MakeShadowMatrix(groundPlaneVertices);
            GL.glMultMatrixf(cubeXform);
            DrawTrain(isShadowDrawing: true);
            GL.glPopMatrix();

            GL.glEnable(GL.GL_DEPTH_TEST);
            EnableLighting();

            GL.glDisable(GL.GL_STENCIL_TEST); // Disable stencil test after drawing shadows
        }

        void DrawReflections()
        {
            if (!isReflectionEnabled) return; // Skip if reflections are disabled

            GL.glEnable(GL.GL_BLEND); // Enable blending for transparency
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA); // Set blend function

            float objectsHeight = 3.3f; // Height offset for reflection
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, -objectsHeight, 0.0f); // Translate for reflection
            GL.glScalef(1, -1, 1); // Invert Y axis for reflection effect

            GL.glEnable(GL.GL_CULL_FACE); // Enable face culling
            GL.glCullFace(GL.GL_BACK); // Cull back faces first
            DrawScene(); // Draw reflected scene
            GL.glCullFace(GL.GL_FRONT); // Cull front faces next
            DrawScene(); // Draw reflected scene again
            GL.glDisable(GL.GL_CULL_FACE); // Disable face culling

            GL.glPopMatrix();

            GL.glDepthMask((byte)GL.GL_FALSE); // Disable depth buffer writing
            DrawGroundWithStencil(); // Draw reflective ground surface
            GL.glDepthMask((byte)GL.GL_TRUE); // Enable depth buffer writing
        }

        private void DrawSkylineWalls()
        {
            DisableLighting();  // Turn off lighting to draw walls
            ColorUtil.SetColor(wallColor);  // Set the color for the walls
            float wallHeight = 200;  // Height of the skyline walls
            for (int i = 0; i < 4; i++)
            {
                // Extend each ground vertex vertically to create the top vertices of the walls
                wallVertexes[i] = new Vector3(groundPlaneVertices[i].X, groundPlaneVertices[i].Y + wallHeight, groundPlaneVertices[i].Z);
            }

            // Draw each wall as a quad, using four vertices
            for (int i = 0; i < 4; i++)
            {
                Vector3 bottomStart = groundPlaneVertices[i];
                Vector3 bottomEnd = groundPlaneVertices[(i + 1) % 4]; // Loop around with modulo for seamless walls
                Vector3 topStart = wallVertexes[i];
                Vector3 topEnd = wallVertexes[(i + 1) % 4];

                GL.glBegin(GL.GL_QUADS);  // Start drawing quads
                GL.glVertex3d(bottomStart.X, bottomStart.Y, bottomStart.Z);
                GL.glVertex3d(bottomEnd.X, bottomEnd.Y, bottomEnd.Z);
                GL.glVertex3d(topEnd.X, topEnd.Y, topEnd.Z);
                GL.glVertex3d(topStart.X, topStart.Y, topStart.Z);
                GL.glEnd();  // End drawing quads
            }
        }

        private void DrawSuprise()
        {
            GL.glMatrixMode(GL.GL_MODELVIEW); // Set matrix mode to ModelView

            MakeTransformation();  // Apply transformations based on user input or other factors

            ch.scaleFactor = 0.1f;  // Scale down the character for drawing
            ch.DrawModel();  // Draw the character model

            GL.glPopMatrix();  // Restore the matrix state before the transformations
        }

        private void MakeTransformation()
        {
            // Apply transformations based on the current transformation operation selected
            if (intOptionC != TransformationsOperations.NONE)
            {
                switch (intOptionC)
                {
                    case TransformationsOperations.ROTATE_X:
                    case TransformationsOperations.ROTATE_OPPOSITE_X:
                        GL.glRotatef(xAngle, 1, 0, 0);  // Rotate about the X-axis
                        break;
                    case TransformationsOperations.ROTATE_Y:
                    case TransformationsOperations.ROTATE_OPPOSITE_Y:
                        GL.glRotatef(yAngle, 0, 1, 0);  // Rotate about the Y-axis
                        break;
                    case TransformationsOperations.ROTATE_Z:
                    case TransformationsOperations.ROTATE_OPPOSITE_Z:
                        GL.glRotatef(zAngle, 0, 0, 1);  // Rotate about the Z-axis
                        break;
                    case TransformationsOperations.SHIFT_X:
                    case TransformationsOperations.SHIFT_OPPOSITE_X:
                        GL.glTranslatef(xShift, 0, 0);  // Translate along the X-axis
                        break;
                    case TransformationsOperations.SHIFT_Y:
                    case TransformationsOperations.SHIFT_OPPOSITE_Y:
                        GL.glTranslatef(0, yShift, 0);  // Translate along the Y-axis
                        break;
                    case TransformationsOperations.SHIFT_Z:
                    case TransformationsOperations.SHIFT_OPPOSITE_Z:
                        GL.glTranslatef(0, 0, zShift);  // Translate along the Z-axis
                        break;
                }
            }
        }

        public void ApplyAndAccumulateTransformations(double[] ModelVievMatrixBeforeSpecificTransforms)
        {
            double[] CurrentRotationTranslation = new double[16];
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, CurrentRotationTranslation);  // Get current ModelView matrix

            GL.glLoadMatrixd(AccumulatedRotationsTraslations);  // Load the accumulated transformations matrix

            GL.glMultMatrixd(CurrentRotationTranslation);  // Apply the current transformation

            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);  // Save the resulting matrix to accumulated transformations

            GL.glLoadMatrixd(ModelVievMatrixBeforeSpecificTransforms);  // Restore the matrix before transformations
            GL.glMultMatrixd(AccumulatedRotationsTraslations);  // Apply accumulated transformations to it
        }


        void MakeShadowMatrix(Vector3[] points)
        {
            // Extract light position from configuration
            Vector3 lightPosition = new Vector3(
                LightConfig.Instance.Position[0],
                LightConfig.Instance.Position[1],
                LightConfig.Instance.Position[2]
            );
            double lightW = LightConfig.Instance.Position[3]; // Homogeneous coordinate of the light

            // Compute two vectors on the plane defined by three points
            Vector3 v1 = points[1] - points[0];
            Vector3 v2 = points[2] - points[0];

            // Calculate the normal vector of the plane by taking the cross product of v1 and v2
            Vector3 normal = v1.CrossProduct(v2).Normalize();

            // Calculate the plane coefficient D for the plane equation Ax + By + Cz + D = 0
            double planeD = -(normal.X * points[0].X + normal.Y * points[0].Y + normal.Z * points[0].Z);

            // Compute the dot product of the plane normal and the light position plus the plane constant
            double dot = normal.X * lightPosition.X + normal.Y * lightPosition.Y + normal.Z * lightPosition.Z + planeD * lightW;

            // Initialize the shadow matrix
            float[] shadowMatrix = new float[16];

            // Compute each element of the shadow matrix based on the dot product and plane normal
            shadowMatrix[0] = (float)(dot - lightPosition.X * normal.X);
            shadowMatrix[4] = (float)(-lightPosition.X * normal.Y);
            shadowMatrix[8] = (float)(-lightPosition.X * normal.Z);
            shadowMatrix[12] = (float)(-lightPosition.X * (planeD + 0.01));  // Offset shadow slightly below the ground

            shadowMatrix[1] = (float)(-lightPosition.Y * normal.X);
            shadowMatrix[5] = (float)(dot - lightPosition.Y * normal.Y);
            shadowMatrix[9] = (float)(-lightPosition.Y * normal.Z);
            shadowMatrix[13] = (float)(-lightPosition.Y * (planeD + 0.01));  // Same offset applied here

            shadowMatrix[2] = (float)(-lightPosition.Z * normal.X);
            shadowMatrix[6] = (float)(-lightPosition.Z * normal.Y);
            shadowMatrix[10] = (float)(dot - lightPosition.Z * normal.Z);
            shadowMatrix[14] = (float)(-lightPosition.Z * (planeD + 0.01));  // And here

            shadowMatrix[3] = 0.0f;
            shadowMatrix[7] = 0.0f;
            shadowMatrix[11] = 0.0f;
            shadowMatrix[15] = (float)dot;

            // Update the transformation matrix used to render shadows
            cubeXform = shadowMatrix;
        }

        internal void Update(float deltaTime)
        {
            this.train.Update(deltaTime);
            Draw();
        }

    }
}

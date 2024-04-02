using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

//3D model b1
using Milkshape;
//3D model e
namespace OpenGL
{

    class cOGL
    {
        public Milkshape.Character ch;

        Control p;
        int Width;
        int Height;

        GLUquadric obj;

        public cOGL(Control pb)
        {
            p=pb;
            Width = p.Width;
            Height = p.Height; 
            InitializeGL();
            obj = GLU.gluNewQuadric(); //!!!
            //3D model b1
            ch = new Character("ninja.ms3d");
            //3D model e
        }

        ~cOGL()
        {
            GLU.gluDeleteQuadric(obj); //!!!
            WGL.wglDeleteContext(m_uint_RC);
        }

		uint m_uint_HWND = 0;

        public uint HWND
		{
			get{ return m_uint_HWND; }
		}
		
        uint m_uint_DC   = 0;

        public uint DC
		{
			get{ return m_uint_DC;}
		}
		uint m_uint_RC   = 0;

        public uint RC
		{
			get{ return m_uint_RC; }
		}



        //ZBuffer TRANSPARENCY
        void DrawCube()
        {
            GL.glPushMatrix();

            GL.glTranslatef(-1, -1, -1);
            // cube
            GL.glBegin(GL.GL_QUADS);

            //1
            //TRANSPARENCY
            GL.glColor4f(1, 0, 0, 0.3f);
            //GL.glColor3f(0.0f,0.0f,0.0f);						
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 2.0f, 0.0f);
            GL.glVertex3f(2.0f, 2.0f, 0.0f);
            GL.glVertex3f(2.0f, 0.0f, 0.0f);

            //2

            GL.glColor4f(0, 1, 0, 0.3f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(0.0f, 0.0f, 2.0f);
            GL.glVertex3f(0.0f, 2.0f, 2.0f);
            GL.glVertex3f(0.0f, 2.0f, 0.0f);


            //3

            GL.glColor4f(0, 0, 1, 0.3f);
            GL.glVertex3f(0.0f, 0.0f, 0.0f);
            GL.glVertex3f(2.0f, 0.0f, 0.0f);
            GL.glVertex3f(2.0f, 0.0f, 2.0f);
            GL.glVertex3f(0.0f, 0.0f, 2.0f);


            //4

            GL.glColor4f(1, 0, 1, 0.3f);
            GL.glVertex3f(2.0f, 0.0f, 0.0f);
            GL.glVertex3f(2.0f, 0.0f, 2.0f);
            GL.glVertex3f(2.0f, 2.0f, 2.0f);
            GL.glVertex3f(2.0f, 2.0f, 0.0f);


            //5

            GL.glColor4f(0, 1, 1, 0.3f);
            GL.glVertex3f(2.0f, 2.0f, 2.0f);
            GL.glVertex3f(2.0f, 2.0f, 0.0f);
            GL.glVertex3f(0.0f, 2.0f, 0.0f);
            GL.glVertex3f(0.0f, 2.0f, 2.0f);


            //6

            GL.glColor4f(1, 1, 0, 0.3f);
            GL.glVertex3f(2.0f, 2.0f, 2.0f);
            GL.glVertex3f(0.0f, 2.0f, 2.0f);
            GL.glVertex3f(0.0f, 0.0f, 2.0f);
            GL.glVertex3f(2.0f, 0.0f, 2.0f);


            GL.glEnd();

            GL.glPopMatrix();
        }


        //ZBuffer TRANSPARENCY

        int angleSph = 0;
        public bool bDepthTest = false;
        public bool bZbufferShow=false;
        void DrawFigures()
        {
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_LIGHTING);

            GL.glPushMatrix();
            float alpha = 0.5f;

            //TRANSPARENCY	
            GL.glDisable(GL.GL_BLEND); // objects are not transparent

            GL.glColor4f(0.2f, 0.8f, 0.0f, alpha);
            GL.glRotatef(-90, 1, 0, 0);
            GLU.gluCylinder(obj, 1, 0, 2, 16, 16);
            GL.glTranslatef(1.3f, 0.5f, 0.0f);
            GL.glColor4f(1, 0, 0, alpha);
            GLU.gluSphere(obj, 1, 16, 16);
            GL.glTranslatef(0, -2, -2);
            GL.glRotatef(-90, 0, 1, 1);
            GL.glColor4f(0.1f, 0.1f, 0.7f, alpha);
            GLU.gluCylinder(obj, 0.3, 0.3, 6, 16, 16);

            GL.glPopMatrix();

            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glEnable(GL.GL_BLEND);
            //TRANSPARENCY e !!!!	

            if (bDepthTest)
                GL.glEnable(GL.GL_DEPTH_TEST);
            else
                GL.glDisable(GL.GL_DEPTH_TEST);


            GL.glEnable(GL.GL_BLEND);           // "blender" ON
            GL.glEnable(GL.GL_CULL_FACE);       // ON - cull polygons on their winding(twist) in window coordinates
            GL.glCullFace(GL.GL_FRONT);	      // do not show Front Surface of the cube
            DrawCube();
            GL.glCullFace(GL.GL_BACK);	      // do not show Back Surface of the cube
            DrawCube();
            GL.glDisable(GL.GL_CULL_FACE);      // OFF - cull polygons on their winding(twist) 
            GL.glDisable(GL.GL_BLEND);          // "blender" OFF


            angleSph += 15;

            GL.glRotatef(angleSph, 0, 0, 1);
            GL.glTranslatef(2, 0, 0);

            // moving sphere
            GL.glEnable(GL.GL_BLEND);           // "blender" ON
            GL.glEnable(GL.GL_CULL_FACE);       // ON - cull polygons on their winding(twist) in window coordinates
            GL.glCullFace(GL.GL_FRONT);	      // do not show Front Surface of the sphere
            GLU.gluSphere(obj, 1, 40, 40);   // show back spheres' surface
            GL.glCullFace(GL.GL_BACK);	      // do not show Back Surface of the sphere
            GLU.gluSphere(obj, 1, 40, 40);   // show front spheres' surface
            GL.glDisable(GL.GL_CULL_FACE);      // OFF - cull polygons on their winding(twist) 
            GL.glDisable(GL.GL_BLEND);          // "blender" OFF
                ////instead 
                //GL.glEnable(GL.GL_BLEND);           // "blender" ON
                //GLU.gluSphere(obj, 1, 40, 40); // we'll see a spot on sphere 
                //GL.glDisable(GL.GL_BLEND);          // "blender" OFF

            GL.glTranslatef(-2, 0, 0);
            GL.glRotatef(-angleSph, 0, 0, 1);




        }

        void DrawOldAxes()
        {
	        //for this time
	        //Lights positioning is here!!!
	        float []pos=new float[4]; 
	        pos[0] = 10; pos[1] = 10; pos[2] = 10; pos[3] = 1;
            GL.glLightfv ( GL.GL_LIGHT0,  GL.GL_POSITION, pos);
            GL.glDisable( GL.GL_LIGHTING);

	        //INITIAL axes
            GL.glEnable ( GL.GL_LINE_STIPPLE);
            GL.glLineStipple (1, 0xFF00);  //  dotted   
	        GL.glBegin( GL.GL_LINES);	
	            //x  RED
	            GL.glColor3f(1.0f,0.0f,0.0f);						
		        GL.glVertex3f( -3.0f, 0.0f, 0.0f);	
		        GL.glVertex3f( 3.0f, 0.0f, 0.0f);	
	            //y  GREEN 
	            GL.glColor3f(0.0f,1.0f,0.0f);						
		        GL.glVertex3f( 0.0f, -3.0f, 0.0f);	
		        GL.glVertex3f( 0.0f, 3.0f, 0.0f);	
	            //z  BLUE
	            GL.glColor3f(0.0f,0.0f,1.0f);						
		        GL.glVertex3f( 0.0f, 0.0f, -3.0f);	
		        GL.glVertex3f( 0.0f, 0.0f, 3.0f);	
            GL.glEnd();
            GL.glDisable ( GL.GL_LINE_STIPPLE);
        }
        void DrawAxes()
        {
            GL.glBegin( GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(-3.0f, 0.0f, 0.0f);
            GL.glVertex3f(3.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, -3.0f, 0.0f);
            GL.glVertex3f(0.0f, 3.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, -3.0f);
            GL.glVertex3f(0.0f, 0.0f, 3.0f);
            GL.glEnd();
        }

        

        public float[] ScrollValue = new float[10];
        public float zShift = 0.0f;
        public float yShift = 0.0f;
        public float xShift = 0.0f;
        public float zAngle = 0.0f;
        public float yAngle = 0.0f;
        public float xAngle = 0.0f;
        public int intOptionC = 0;
        double[] AccumulatedRotationsTraslations = new double[16];
        
        public void Draw()
        {
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);
            
            GL.glLoadIdentity();

            // not trivial
            double []ModelVievMatrixBeforeSpecificTransforms=new double[16];
            double []CurrentRotationTraslation=new double[16];
                     
            GLU.gluLookAt (ScrollValue[0], ScrollValue[1], ScrollValue[2], 
	                   ScrollValue[3], ScrollValue[4], ScrollValue[5],
		               ScrollValue[6],ScrollValue[7],ScrollValue[8]);


            if ( !bPerspective )
                GL.glTranslatef(0.0f, 0.0f, 8.0f);
            
            //3D model b3 
            GL.glTranslatef(0.0f, -5.0f, -15.0f);
            GL.glRotated(180, 0, 1, 0);
            //3D model e
            
            
            //DrawOldAxes();

            //save current ModelView Matrix values
            //in ModelVievMatrixBeforeSpecificTransforms array
            //ModelView Matrix ========>>>>>> ModelVievMatrixBeforeSpecificTransforms
            GL.glGetDoublev (GL.GL_MODELVIEW_MATRIX, ModelVievMatrixBeforeSpecificTransforms);
            //ModelView Matrix was saved, so
            GL.glLoadIdentity(); // make it identity matrix
                     
            //make transformation in accordance to KeyCode
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
            //as result - the ModelView Matrix now is pure representation
            //of KeyCode transform and only it !!!

            //save current ModelView Matrix values
            //in CurrentRotationTraslation array
            //ModelView Matrix =======>>>>>>> CurrentRotationTraslation
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, CurrentRotationTraslation);

            //The GL.glLoadMatrix function replaces the current matrix with
            //the one specified in its argument.
            //The current matrix is the
            //projection matrix, modelview matrix, or texture matrix,
            //determined by the current matrix mode (now is ModelView mode)
            GL.glLoadMatrixd(AccumulatedRotationsTraslations); //Global Matrix

            //The GL.glMultMatrix function multiplies the current matrix by
            //the one specified in its argument.
            //That is, if M is the current matrix and T is the matrix passed to
            //GL.glMultMatrix, then M is replaced with M • T
            GL.glMultMatrixd(CurrentRotationTraslation);

            //save the matrix product in AccumulatedRotationsTraslations
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);

            //replace ModelViev Matrix with stored ModelVievMatrixBeforeSpecificTransforms
            GL.glLoadMatrixd(ModelVievMatrixBeforeSpecificTransforms);
            //multiply it by KeyCode defined AccumulatedRotationsTraslations matrix
            GL.glMultMatrixd(AccumulatedRotationsTraslations);

            //DrawAxes();
            
            //DrawFigures();
            //if (bZbufferShow)
            //{
            //    //Z-BUFFER SHOW begin   
            //    GL.glPushMatrix(); //save curent MODELVIEW matrix
            //    GL.glReadPixels(0, 0, Width, Height, GL.GL_DEPTH_COMPONENT, GL.GL_FLOAT, Zbuf);
            //    GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            //    GL.glMatrixMode(GL.GL_PROJECTION);
            //    GL.glPushMatrix();  //save current PROJECTION matrix
            //    GL.glLoadIdentity();
            //    GL.glDrawPixels(Width, Height, GL.GL_LUMINANCE, GL.GL_FLOAT, Zbuf);


            //    GL.glPopMatrix();  //restore PROJECTION matrix
            //    GL.glMatrixMode(GL.GL_MODELVIEW);
            //    GL.glPopMatrix(); //restore MODELVIEW matrix
            //    //Z-BUFFER SHOW end   
            //}

            //3D model b2
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_LIGHTING);

            ch.DrawModel();
            //3D model e

            GL.glFlush();

            WGL.wglSwapBuffers(m_uint_DC);

        }

        float [] Zbuf;
		protected virtual void InitializeGL()
		{
			m_uint_HWND = (uint)p.Handle.ToInt32();
			m_uint_DC   = WGL.GetDC(m_uint_HWND);

            // Not doing the following WGL.wglSwapBuffers() on the DC will
			// result in a failure to subsequently create the RC.
			WGL.wglSwapBuffers(m_uint_DC);

			WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
			WGL.ZeroPixelDescriptor(ref pfd);
			pfd.nVersion        = 1; 
			pfd.dwFlags         = (WGL.PFD_DRAW_TO_WINDOW |  WGL.PFD_SUPPORT_OPENGL |  WGL.PFD_DOUBLEBUFFER); 
			pfd.iPixelType      = (byte)(WGL.PFD_TYPE_RGBA);
			pfd.cColorBits      = 32;
			pfd.cDepthBits      = 32;
			pfd.iLayerType      = (byte)(WGL.PFD_MAIN_PLANE);

			int pixelFormatIndex = 0;
			pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
			if(pixelFormatIndex == 0)
			{
				MessageBox.Show("Unable to retrieve pixel format");
				return;
			}

			if(WGL.SetPixelFormat(m_uint_DC,pixelFormatIndex,ref pfd) == 0)
			{
				MessageBox.Show("Unable to set pixel format");
				return;
			}
			//Create rendering context
			m_uint_RC = WGL.wglCreateContext(m_uint_DC);
			if(m_uint_RC == 0)
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
            Draw();
        }

        public bool bPerspective = true;

        public void initRenderingGL()
		{
			if(m_uint_DC == 0 || m_uint_RC == 0)
				return;
			if(this.Width == 0 || this.Height == 0)
				return;
            GL.glClearColor(1.0f, 1.0f, 1.0f, 0.0f);
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);

            GL.glViewport(0, 0, this.Width, this.Height);

            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();

            //Z-BUFFER SHOW begin   
            if ( !bPerspective )
                GL.glOrtho(-4, 4, -4, 4, -4, 4);
            else
            {
                // - no Grey nuances: differences of our objects Z
                //        are relatively 0 to 100.0f ... 0.45f 
                GLU.gluPerspective(45.0f, Width / Height, 0.45f, 200.0f);
                //                                 but here the differences almost cover range 12 ... 4
                // in glOrtho the differences almost cover range 4 ... -4
                //GLU.gluPerspective(45.0f, Width / Height, 4.0f, 12.0f);
                //end
            }
            GL.glMatrixMode(GL.GL_MODELVIEW);
            
			GL.glLoadIdentity();

            //save the current MODELVIEW Matrix (now it is Identity)
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);
            
            Zbuf = new float[Width * Height];

		}

    
    }

}



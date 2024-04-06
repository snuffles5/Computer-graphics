using OpenGL;
using System;
using static Utils.MaterialConfig;
using Utils;
using TextBox = System.Windows.Forms.TextBox;
using System.Drawing;

namespace Models
{
    public class Sun
    {
        public Vector3 Coords { get; set; }
        public float Angle{ get; set; }
        private float radius;
        TextBox debugTextBox;
        gluNewQuadric obj;

        public Sun(TextBox debugTextBox, Vector3 vector = null, float angle = 0, float radius = 0.5f)
        {
            this.obj = GLU.gluNewQuadric();
            this.debugTextBox = debugTextBox;
            if (vector == null)
            {
                Coords = new Vector3(-5, 5, -5);
            }
            else
            {
                Coords = vector;
            }
            Angle = angle;
            this.radius = radius;
        }

        public void Draw()
        {
            GL.glPushMatrix(); // Save the current state
            DrawSun();
            GL.glPopMatrix(); // Restore the original state
        }

        private void DrawSun()
        {
            //GL.glRotatef(Angle, 0, 0, 1);
            GL.glTranslatef(Coords.X, Coords.Y, Coords.Z); 
            //GL.glEnable(GL.GL_BLEND);           // "blender" ON
            GL.glEnable(GL.GL_CULL_FACE);       // ON - cull polygons on their winding(twist) in window coordinates
            ColorUtil.SetColor(ColorName.Yellow);
            GL.glCullFace(GL.GL_FRONT);	      // do not show Front Surface of the sphere
            GLU.gluSphere(obj, radius, 40, 40);   // show back spheres' surface
            GL.glCullFace(GL.GL_BACK);	      // do not show Back Surface of the sphere
            GLU.gluSphere(obj, radius, 40, 40);   // show front spheres' surface
            GL.glDisable(GL.GL_CULL_FACE);      // OFF - cull polygons on their winding(twist) 
            //GL.glDisable(GL.GL_BLEND);

        }
    }
}

using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Models
{
    public class Locomotive
    {
        // Constants for the locomotive parts dimensions
        private readonly float wheelRadius;
        private readonly float wheelThickness;
        private readonly float cabWidth;
        private readonly float cabHeight;
        private readonly float cabDepth;
        private readonly float chimneyBaseRadius;
        private readonly float chimneyTopRadius;
        private readonly float chimneyHeight;

        private float wheelRotation = 0.0f;
        private uint cabList, wheelList, locomotiveList;

        public Locomotive()
        {
            cabWidth = 3.0f;
            cabHeight = 1.0f;
            cabDepth = 3f;
            wheelRadius = 0.4f;
            wheelThickness = 0.1f;
            chimneyBaseRadius = 0.1f;
            chimneyTopRadius = 0.1f;
            chimneyHeight = 0.5f;


            PrepareLists();
        }

        private void PrepareLists()
        {
            // Generate a contiguous block of list identifiers
            locomotiveList = GL.glGenLists(3); // We request 3 lists at once
            cabList = locomotiveList + 1; // The second list is for the cab
            wheelList = locomotiveList + 2; // The third list is for the wheels


            // Define the cab
            GL.glNewList(cabList, GL.GL_COMPILE);
            DrawCabBase();
            //DrawChimney();
            GL.glEndList();

            // Define the wheel
            GL.glNewList(wheelList, GL.GL_COMPILE);
            //DrawWheel(wheelRadius, wheelThickness);
            GL.glEndList();

            // Combine into the locomotive
            GL.glNewList(locomotiveList, GL.GL_COMPILE);
            CreateLocomotiveList();
            GL.glEndList();
        }

        private void CreateLocomotiveList()
        {
            // Place the cab
            GL.glPushMatrix();
            GL.glCallList(cabList);
            GL.glPopMatrix();

            // Place the wheels relative to the cab
            for (int i = 0; i < 4; i++)
            {
                GL.glPushMatrix();
                // Transform to position each wheel
                //GL.glTranslatef( /* ... */ );
                GL.glCallList(wheelList);
                GL.glPopMatrix();
            }
        }

        public void Draw()
        {
            // Set up the view
            GL.glTranslatef(0.0f, 0.0f, -10.0f); // Move the entire scene back to be within view
            //GL.glEnable(GL.GL_COLOR_MATERIAL);
            //GL.glEnable(GL.GL_LIGHT0);
            //GL.glEnable(GL.GL_LIGHTING);
            GL.glCallList(locomotiveList);
        }

        private void DrawCabBase() 
        {
            // Assuming cabWidth, cabHeight, and cabDepth have been defined elsewhere
            float halfWidth = cabWidth / 2;
            float halfHeight = cabHeight / 2;
            float halfDepth = cabDepth / 2;

            // Front Face
            GL.glBegin(GL.GL_QUADS);
            ColorUtil.SetColor(ColorName.Green);
            GL.glVertex3f(-halfWidth, -halfHeight, halfDepth);
            GL.glVertex3f(halfWidth, -halfHeight, halfDepth);
            GL.glVertex3f(halfWidth, halfHeight, halfDepth);
            GL.glVertex3f(-halfWidth, halfHeight, halfDepth);
            GL.glEnd();

            // Back Face
            GL.glBegin(GL.GL_QUADS);
            ColorUtil.SetColor(ColorName.Red);
            GL.glVertex3f(-halfWidth, -halfHeight, -halfDepth);
            GL.glVertex3f(halfWidth, -halfHeight, -halfDepth);
            GL.glVertex3f(halfWidth, halfHeight, -halfDepth);
            GL.glVertex3f(-halfWidth, halfHeight, -halfDepth);
            GL.glEnd();

            // Top Face
            GL.glBegin(GL.GL_QUADS);
            ColorUtil.SetColor(ColorName.Blue);
            GL.glVertex3f(-halfWidth, halfHeight, -halfDepth);
            GL.glVertex3f(halfWidth, halfHeight, -halfDepth);
            GL.glVertex3f(halfWidth, halfHeight, halfDepth);
            GL.glVertex3f(-halfWidth, halfHeight, halfDepth);
            GL.glEnd();

            // Bottom Face
            GL.glBegin(GL.GL_QUADS);
            ColorUtil.SetColor(ColorName.Purple);
            GL.glVertex3f(-halfWidth, -halfHeight, -halfDepth);
            GL.glVertex3f(halfWidth, -halfHeight, -halfDepth);
            GL.glVertex3f(halfWidth, -halfHeight, halfDepth);
            GL.glVertex3f(-halfWidth, -halfHeight, halfDepth);
            GL.glEnd();

            // Right Face
            GL.glBegin(GL.GL_QUADS);
            ColorUtil.SetColor(ColorName.Black);
            GL.glVertex3f(halfWidth, -halfHeight, -halfDepth);
            GL.glVertex3f(halfWidth, halfHeight, -halfDepth);
            GL.glVertex3f(halfWidth, halfHeight, halfDepth);
            GL.glVertex3f(halfWidth, -halfHeight, halfDepth);
            GL.glEnd();

            // Left Face
            GL.glBegin(GL.GL_QUADS);
            ColorUtil.SetColor(ColorName.Yellow);
            GL.glVertex3f(-halfWidth, -halfHeight, -halfDepth);
            GL.glVertex3f(-halfWidth, halfHeight, -halfDepth);
            GL.glVertex3f(-halfWidth, halfHeight, halfDepth);
            GL.glVertex3f(-halfWidth, -halfHeight, halfDepth);
            GL.glEnd();
        }


        private void DrawChimney()
        {
            // Setup color and materials
            ColorUtil.SetColor(ColorName.Black);

            // Draw chimney - replace with actual OpenGL drawing calls
            // This could be a cylinder or a series of quads/triangles to make a custom shape
            // ...

            // Assume you have a function to draw a cylinder given base radius, top radius, and height
            DrawCylinder(baseRadius: chimneyBaseRadius, topRadius: chimneyTopRadius, height: chimneyHeight);
        }

        private void DrawWheels()
        {
            // Setup color and materials
            ColorUtil.SetColor(ColorName.Grey);

            for (int i = -1; i <= 1; i += 2) // Two wheels on each side
            {
                GL.glPushMatrix();
                GL.glTranslatef(i * 1.5f, -0.5f, -0.25f); // Position the wheel
                DrawWheel(radius: wheelRadius, thickness: wheelThickness); // A simple wheel representation
                GL.glPopMatrix();

                GL.glPushMatrix();
                GL.glTranslatef(i * 1.5f, -0.5f, 0.25f); // Position the wheel on the other side
                DrawWheel(0.4f, 0.1f); // A simple wheel representation
                GL.glPopMatrix();
            }
        }

        public void Update(float deltaTime)
        {
            // Update the wheel rotation based on delta time
            wheelRotation += deltaTime * 20.0f; // Adjust the speed as necessary
        }

        // Placeholder methods to represent drawing primitives, which would use actual OpenGL calls
        private void DrawBox(float width, float height, float depth)
        {
            GL.glBegin(GL.GL_QUADS);

            // Front face
            GL.glVertex3f(-width / 2, -height / 2, depth / 2);
            GL.glVertex3f(width / 2, -height / 2, depth / 2);
            GL.glVertex3f(width / 2, height / 2, depth / 2);
            GL.glVertex3f(-width / 2, height / 2, depth / 2);

            // Repeat for other faces...

            GL.glEnd();
        }

        private void DrawCylinder(float baseRadius, float topRadius, float height)
        {
            // Replace with actual OpenGL code to draw a cylinder
        }

        private void DrawWheel(float radius, float thickness)
        {
            // Create a flat disc to represent a wheel, for simplicity
            GL.glBegin(GL.GL_TRIANGLE_FAN);

            GL.glVertex3f(0.0f, 0.0f, -thickness / 2); // Center of the wheel

            for (int angle = 0; angle <= 360; angle++) // Full circle
            {
                float rad = (float)(angle * Math.PI / 180.0f); // Convert degrees to radians
                GL.glVertex3f((float)Math.Cos(rad) * radius, (float)Math.Sin(rad) * radius, -thickness / 2);
            }

            GL.glEnd();
        }
    }

}

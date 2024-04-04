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
        private readonly float cabBottomBaseWidth;
        private readonly float cabBottomBaseHeight;
        private float cabAngleX = 0.0f;
        private float cabAngleY = 0.0f;
        private float cabAngleZ = 0.0f;
        private readonly float chimneyBaseRadius;
        private readonly float chimneyTopRadius;
        private readonly float chimneyHeight;

        GLUquadric obj;


        private float wheelRotation = 0.0f;
        private uint cabList, wheelList, locomotiveList;

        public Locomotive()
        {
            cabWidth = 7.0f;
            cabHeight = 1.5f;
            cabDepth = 1.5f;
            cabBottomBaseWidth = cabWidth * 1.1f;
            cabBottomBaseHeight = 0.5f;
            wheelRadius = 0.4f;
            wheelThickness = 0.1f;
            chimneyBaseRadius = 0.3f;
            chimneyTopRadius = 0.8f;
            chimneyHeight = 1.5f;
            obj = GLU.gluNewQuadric();


            PrepareLists();
        }

        ~Locomotive()
        {
            GLU.gluDeleteQuadric(obj);
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
            DrawChimney();
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
            // Example: Assuming you want to rotate the locomotive around its center
            GL.glPushMatrix(); // Save the current state

            //GL.glRotatef(wheelRotation, 0.0f, 0.0f, 1.0f); // This uses the wheelRotation, adjust as needed

            // Now draw the locomotive
            GL.glCallList(locomotiveList);

            GL.glPopMatrix(); // Restore the original state
        }

        private void DrawCuboid(float width, float height, float depth, ColorName color)
        {
            GL.glPushMatrix(); // Save the current state

            // Set the color for the cuboid
            ColorUtil.SetColor(color);

            // Front Face
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-width, -height, depth);
            GL.glVertex3f(width, -height, depth);
            GL.glVertex3f(width, height, depth);
            GL.glVertex3f(-width, height, depth);
            GL.glEnd();

            // Back Face
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-width, -height, -depth);
            GL.glVertex3f(width, -height, -depth);
            GL.glVertex3f(width, height, -depth);
            GL.glVertex3f(-width, height, -depth);
            GL.glEnd();

            // Top Face
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-width, height, -depth);
            GL.glVertex3f(width, height, -depth);
            GL.glVertex3f(width, height, depth);
            GL.glVertex3f(-width, height, depth);
            GL.glEnd();

            // Bottom Face
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-width, -height, -depth);
            GL.glVertex3f(width, -height, -depth);
            GL.glVertex3f(width, -height, depth);
            GL.glVertex3f(-width, -height, depth);
            GL.glEnd();

            // Right Face
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(width, -height, -depth);
            GL.glVertex3f(width, height, -depth);
            GL.glVertex3f(width, height, depth);
            GL.glVertex3f(width, -height, depth);
            GL.glEnd();

            // Left Face
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(-width, -height, -depth);
            GL.glVertex3f(-width, height, -depth);
            GL.glVertex3f(-width, height, depth);
            GL.glVertex3f(-width, -height, depth);
            GL.glEnd();
            
            GL.glPopMatrix(); // Restore the original state
        }

        private void DrawCabBase()
        {
            // Assuming cabWidth, cabHeight, and cabDepth have been defined elsewhere
            float halfWidth = cabWidth / 2;
            float halfHeight = cabHeight / 2;
            float halfDepth = cabDepth / 2;
            DrawCuboid(halfWidth, halfHeight, halfDepth, ColorName.Beige);
            DrawCabBottomBase();

        }
            private void DrawCabBottomBase()
        {
            // Assuming cabWidth, cabHeight, and cabDepth have been defined elsewhere
            float halfWidth = cabBottomBaseWidth / 2;
            float halfHeight = cabBottomBaseHeight / 2;
            float halfDepth = cabDepth / 2;
            // Translate down to position the bottom base at the bottom of the cab
            float translateY = -(cabHeight / 2 + cabBottomBaseHeight / 2);
            GL.glTranslatef(0.0f, translateY, 0.0f); // No translation on X and Z axes

    
            DrawCuboid(halfWidth, halfHeight, halfDepth, ColorName.Bronze);
        }


        private void DrawChimney()
        {
            // Setup color and materials
            ColorUtil.SetColor(ColorName.Black);

            // Calculate the translation values before drawing the chimney
            float translateX = cabWidth * 0.25f; // 75% to the right, starting from the center
            float translateY = cabHeight / 1.1f; // On top of the cab
            float translateZ = 0.0f; // Centered along the cab's depth


            // Assuming you're using gluCylinder to draw the chimney
            GL.glPushMatrix(); // Save the current transformation state
            GL.glTranslatef(translateX, translateY, translateZ); // Apply the calculated translation
            DrawCylinder(baseRadius: chimneyBaseRadius, topRadius: chimneyTopRadius, height: chimneyHeight);
            GL.glPopMatrix(); // Restore the previous transformation state

            // Assume you have a function to draw a cylinder given base radius, top radius, and height
        }

        private void DrawCylinder(float baseRadius, float topRadius, float height, bool isRotateUpwards = true)
        {
            if (isRotateUpwards)
            {
                // Rotate -90 degrees around the x-axis to make the chimney's top face upwards
                GL.glRotatef(-90.0f, 1.0f, 0.0f, 0.0f);
            }

            GLU.gluCylinder(obj, baseRadius, topRadius, height, 32, 32);
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

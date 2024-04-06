using OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;
using Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace Models
{
    public class Train
    {
        public bool isLocomotive = false;
        private Locomotive locomotive;
        private Coach[] coaches;
        public TextBox debugTextBox;

        public Train(TextBox debugTextBox, int numberOfCoaches)
        {
            this.debugTextBox = debugTextBox;
            this.coaches = new Coach[numberOfCoaches];
            for (int i = 0; i < numberOfCoaches; i++)
            {
                // Initialize each coach instance here
                this.coaches[i] = new Coach(debugTextBox);
            }
            this.locomotive = new Locomotive(debugTextBox);
        }

        public void Draw()
        {
            GL.glPushMatrix(); // Save the current state

            if (isLocomotive)
            {
                locomotive.Draw();
            }
            else
            {
                DrawCoaches();
            }

            //locomotive.Draw();
            //DrawCoaches();

            GL.glPopMatrix(); // Restore the original state
        }

        public void DrawCoaches()
        {
            for (int i = 0; i < coaches.Length; i++)
            {
                coaches[i].Draw();
            }
        }
    }

    public class Locomotive
    {
        public Locomotive(TextBox debugTextBox)
        {

        }
        public void Draw()
        {
        }

    }

    public class Coach
    {
        // Constants for the coach parts dimensions
        private readonly int numOfWheels;
        private readonly float wheelRadius;
        private readonly float wheelThickness;
        private readonly float cabWidth;
        private readonly float cabHeight;
        private readonly float cabDepth;
        private readonly float windowWidth;
        private readonly float windowHeight;
        private readonly float windowDepth;

        private readonly float cabBottomBaseWidth;
        private readonly float cabBottomBaseHeight;
        private readonly float cabBottomCouplerWidth;
        private readonly float cabBottomCouplerHeight;
        private readonly float cabBottomCouplerDepth;

        GLUquadric obj;


        private float wheelRotation = 0.0f;
        private uint cabList, wheelList, coachList;
        TextBox debugTextBox;
        private int numOfWindowsPerCab;

        public Coach(TextBox debugTextBox)
        {
            cabWidth = 7.0f;
            cabHeight = 1.0f;
            cabDepth = 0.7f;
            windowWidth = cabWidth * 0.05f;
            windowHeight = cabHeight * 0.5f;
            windowDepth = 0.02f;
            cabBottomBaseWidth = cabWidth * 1.1f;
            cabBottomBaseHeight = 0.2f;
            cabBottomCouplerWidth = 0.3f;
            cabBottomCouplerHeight = 0.2f;
            cabBottomCouplerDepth = 0.2f;
            numOfWindowsPerCab = 4;
            numOfWheels = 8;
            wheelRadius = 0.4f;
            wheelThickness = 0.2f;
            obj = GLU.gluNewQuadric();
            this.debugTextBox = debugTextBox;

            PrepareLists();
        }

        ~Coach()
        {
            GLU.gluDeleteQuadric(obj);
        }


        private void PrepareLists()
        {
            // Generate a contiguous block of list identifiers
            coachList = GL.glGenLists(3); // We request 3 lists at once
            cabList = coachList + 1; // The second list is for the cab
            wheelList = coachList + 2; // The third list is for the wheels


            // Define the cab
            GL.glNewList(cabList, GL.GL_COMPILE);
            DrawCabBase();
            GL.glEndList();

            // Define the wheel
            GL.glNewList(wheelList, GL.GL_COMPILE);
            DrawWheel(wheelRadius, wheelThickness, ColorName.Black);
            GL.glEndList();

            // Combine into the coach
            GL.glNewList(coachList, GL.GL_COMPILE);
            CreateCoachList();
            GL.glEndList();
        }

        private void CreateCoachList()
        {
            // Place the cab
            GL.glPushMatrix();
            GL.glCallList(cabList);
            GL.glPopMatrix();

            DrawWheels();
        }

        public void Draw()
        {
            GL.glPushMatrix(); // Save the current state

            // Translate to set the new center
            //float centerTranslationX = cabinWidth / 2;
            //GL.glTranslatef(centerTranslationX, 0.0f, 0.0f); // Shift everything to the left

            //GL.glRotatef(wheelRotation, 0.0f, 0.0f, 1.0f); // This uses the wheelRotation, adjust as needed

            // Now draw the coach
            GL.glCallList(coachList);

            GL.glPopMatrix(); // Restore the original state
        }

        private void DrawCuboid(float width, float height, float depth, ColorName color, float alpha = 1, bool isWithWindows = false)
        {
            // Set the color for the cuboid
            ColorUtil.SetColor(color, alpha);

            if (isWithWindows)
            {
                // Front Face
                DrawCabinFaceWithWindows(width, height, depth, color, alpha);

                // Back Face
                //DrawCabinFaceWithWindows(width, height, -depth, color, alpha);
            }
            else
            {
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
            }

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
        }

        private void DrawCabinFaceWithWindows(float width, float height, float zPosition, ColorName color, float alpha = 1)
        {
            ColorUtil.SetColor(ColorName.Red, alpha);

            //// Assuming a vertical distribution of windows for simplicity
            //float spacingBetweenWindows = (width - (numOfWindowsPerCab * windowWidth)) / (numOfWindowsPerCab + 1);
            //float currentYPosition = height / 2 - spacingBetweenWindows - windowHeight / 2;

            //// Draw top part of the face
            //GL.glBegin(GL.GL_QUADS);
            //GL.glVertex3f(-width / 2, height / 2, zPosition);
            //GL.glVertex3f(width / 2, height / 2, zPosition);
            //GL.glVertex3f(width / 2, currentYPosition + windowHeight / 2, zPosition);
            //GL.glVertex3f(-width / 2, currentYPosition + windowHeight / 2, zPosition);
            //GL.glEnd();

            //// Draw windows and the space between them
            //for (int i = 0; i < numOfWindowsPerCab; i++)
            //{
            //    // Draw bottom part of the window space
            //    if (i == 0)
            //    {
            //        GL.glBegin(GL.GL_QUADS);
            //        GL.glVertex3f(-width / 2, -height / 2, zPosition);
            //        GL.glVertex3f(width / 2, -height / 2, zPosition);
            //        GL.glVertex3f(width / 2, currentYPosition - windowHeight / 2, zPosition);
            //        GL.glVertex3f(-width / 2, currentYPosition - windowHeight / 2, zPosition);
            //        GL.glEnd();
            //    }

            //    // Adjust currentYPosition for the next window
            //    currentYPosition -= (windowHeight + spacingBetweenWindows);

            //    // If it's not the last window, draw the space between this and the next window
            //    if (i < numOfWindowsPerCab - 1)
            //    {
            //        GL.glBegin(GL.GL_QUADS);
            //        GL.glVertex3f(-width / 2, currentYPosition + spacingBetweenWindows + windowHeight / 2, zPosition);
            //        GL.glVertex3f(width / 2, currentYPosition + spacingBetweenWindows + windowHeight / 2, zPosition);
            //        GL.glVertex3f(width / 2, currentYPosition - windowHeight / 2, zPosition);
            //        GL.glVertex3f(-width / 2, currentYPosition - windowHeight / 2, zPosition);
            //        GL.glEnd();
            //    }
            //}
            // Calculate spacing and initial position
            GL.glPushMatrix();
            ColorUtil.SetColor(ColorName.Black, alpha);
            GL.glTranslatef(0.0f, 0.0f, zPosition); // Translate the sphere to the zPosition of the front face
            GLU.gluSphere(obj, height / 2, 20, 20); // Sphere with radius half of the cab height
            ColorUtil.SetColor(ColorName.Red, alpha);
            GL.glPopMatrix();

            float spacingBetweenWindows = (width - (windowWidth * numOfWindowsPerCab)) / (numOfWindowsPerCab + 1);
            float topAndBottomHeight = (height - windowHeight) / 3;

            // Draw top part of the face
            //GL.glBegin(GL.GL_QUADS);
            //GL.glVertex3f(-width, height, zPosition);
            //GL.glVertex3f(width, height, zPosition);
            //GL.glVertex3f(width, topAndBottomHeight, zPosition);
            //GL.glVertex3f(-width, topAndBottomHeight, zPosition);
            //GL.glEnd();

            //// Draw bottom part of the face
            //GL.glBegin(GL.GL_QUADS);
            //GL.glVertex3f(-width, -height + topAndBottomHeight, zPosition);
            //GL.glVertex3f(width, -height + topAndBottomHeight, zPosition);
            //GL.glVertex3f(width, -height, zPosition);
            //GL.glVertex3f(-width, -height, zPosition);
            //GL.glEnd();
        }


        public void DrawWindows()
        {
            float spacingBetweenWindows = (cabWidth - windowWidth * numOfWindowsPerCab) / (numOfWindowsPerCab + 1);
            float currentXPosition = -cabWidth / 2 + spacingBetweenWindows + windowWidth / 2;

            for (int i = 0; i < numOfWindowsPerCab; i++)
            {
                // Calculate the Y position to center the window vertically on the cab
                float windowPosY = (cabHeight - windowHeight) / 2;

                // Assuming DrawCuboid can handle semi-transparency when isWindow is true
                GL.glPushMatrix();
                GL.glTranslatef(currentXPosition, 0.0f, cabDepth / 2 + windowDepth / 2); // Adjust Z to put windows slightly outside
                                                                                         // Enable blending for semi-transparent windows
                DrawCuboid(windowWidth, windowHeight, windowDepth, ColorName.Blue, alpha: 0.5f);

                GL.glPopMatrix();

                // Move to the next window position
                currentXPosition += windowWidth + spacingBetweenWindows;
            }
        }

        private void DrawCabinWithWindows()
        {
            // Draw the base of the cab
            GL.glPushMatrix();
            DrawCuboid(cabWidth, cabHeight, cabDepth, ColorName.Beige, isWithWindows: true);
            GL.glPopMatrix();

            // Draw Windows
            // Enable blending for semi-transparent windows
            GL.glPushMatrix();
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

            ColorUtil.SetColor(ColorName.Blue, 0.5f); // The second parameter is the alpha value
            DrawWindows();
            // Disable blending after drawing the window
            GL.glDisable(GL.GL_BLEND);
            GL.glPopMatrix(); // Restore the original state
        }


        private void DrawCabBase()
        {
            GL.glPushMatrix(); // Save the current state
            DrawCabinWithWindows();
            GL.glPopMatrix(); // Restore the original state

            // Draw the bottom base of the cab
            GL.glPushMatrix(); // Save the current state
            DrawCabBottomBase();
            GL.glPopMatrix(); // Restore the original state

            // Draw the bottom coupler of the cab
            DrawCabBottomCoupler();
            GL.glPopMatrix(); // Restore the original state
        }
        private void DrawCabBottomCoupler()
        {
            GL.glPushMatrix(); // Save the current state

            // Translate to position the coupler at the end of the bottom base and below the cab
            // Assuming the coupler is to be placed at the very end of the bottom base.
            float halfBottomBaseWidth = cabBottomBaseWidth / 2;
            float translateX = -cabBottomBaseWidth;
            float translateY = -(cabHeight / 2 + cabBottomBaseHeight * 2 + cabBottomCouplerHeight / 2); // Below the bottom base
            float translateZ = 0.0f; // Assuming the Z position is centered with respect to the cab's depth

            GL.glTranslatef(translateX, translateY, translateZ);

            DrawCuboid(cabBottomCouplerWidth, cabBottomCouplerHeight, cabBottomCouplerDepth, ColorName.Bronze);

            GL.glPopMatrix(); // Restore the original state
        }

    private void DrawCabBottomBase()
        {
            GL.glPushMatrix(); // Save the current state

            // Assuming cabWidth, cabHeight, and cabDepth have been defined elsewhere
            // Translate down to position the bottom base at the bottom of the cab
            float translateY = -cabHeight; //- (cabHeight / 2 + cabBottomBaseHeight / 2);
            GL.glTranslatef(0.0f, translateY, 0.0f); // No translation on X and Z axes


            DrawCuboid(cabBottomBaseWidth, cabBottomBaseHeight, cabDepth, ColorName.Bronze);

            GL.glPopMatrix(); // Restore the original state
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

        private void DrawWheel(float radius, float thickness, ColorName color)
        {
            ColorUtil.SetColor(color);
            // Parameters
            int numSides = 50; // Number of sides for the cylinder to make it appear circular

            // Wheel Hub
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, -thickness / 2);
            GLU.gluCylinder(obj, radius * 0.75, radius * 0.75, thickness, numSides, 1);
            GL.glPopMatrix();

            // Tire sides (hubcap)
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, -thickness / 2);
            GLU.gluDisk(obj, 0, radius, numSides, 1); // Bottom hubcap
            GL.glTranslatef(0.0f, 0.0f, thickness);
            GLU.gluDisk(obj, 0, radius, numSides, 1); // Top hubcap
            GL.glPopMatrix();

            // Adding radial lines (spokes)
            float angle = 2.0f * (float)Math.PI;
            float cosAngle = (float)Math.Cos(angle);
            float sinAngle = (float)Math.Sin(angle);

            GL.glBegin(GL.GL_LINES);
            // Inner point (near hub center)
            GL.glVertex3f(cosAngle * radius * 0.25f, sinAngle * radius * 0.25f, -thickness / 2);
            GL.glVertex3f(cosAngle * radius * 0.25f, sinAngle * radius * 0.25f, thickness / 2);
            // Outer point (edge of hubcap)
            GL.glVertex3f(cosAngle * radius, sinAngle * radius, -thickness / 2);
            GL.glVertex3f(cosAngle * radius, sinAngle * radius, thickness / 2);
            GL.glEnd();
        }

        private void DrawWheels()
        {
            int wheelsPerSide = numOfWheels / 2;

            float edgeMargin = cabBottomBaseWidth * 0.15f;
            float usableWidth = cabBottomBaseWidth - (2 * edgeMargin); // Reduce usable width by margins on both sides
            float wheelSpacing = (wheelsPerSide > 1) ? usableWidth / (wheelsPerSide - 1) : 0;

            float wheelOffsetY = -(wheelRadius + cabBottomBaseHeight) * 1.3f;
            float wheelOffsetZFront = cabDepth;
            float wheelOffsetZBack = -cabDepth;

            for (int i = 0; i < wheelsPerSide; i++)
            {
                float posX = -cabBottomBaseWidth / 2 + edgeMargin + (i * wheelSpacing);
                float posZ = (i % 2 == 0) ? wheelOffsetZFront : wheelOffsetZBack;

                GL.glPushMatrix();
                GL.glTranslatef(posX, wheelOffsetY, posZ);
                GL.glCallList(wheelList);
                GL.glPopMatrix();

                GL.glPushMatrix();
                GL.glTranslatef(-posX, wheelOffsetY, posZ);
                GL.glCallList(wheelList);
                GL.glPopMatrix();
            }
        }


        private void DrawCenterMarker(float radius)
        {
            GL.glBegin(GL.GL_LINE_LOOP); // Use GL_LINE_LOOP for an outline
            for (int i = 0; i <= 360; i++) // Full circle
            {
                float degInRad = (float)(i * Math.PI / 180.0f);
                GL.glVertex2f((float)(Math.Cos(degInRad) * radius), (float)(Math.Sin(degInRad) * radius));
            }
            GL.glEnd();
        }

        public void Update(float deltaTime)
        {
            // Update the wheel rotation based on delta time
            wheelRotation += deltaTime * 20.0f; // Adjust the speed as necessary
        }

    }

}

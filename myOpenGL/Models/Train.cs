﻿using OpenGL;
using System;
using static Utils.MaterialConfig;
using Utils;
using TextBox = System.Windows.Forms.TextBox;
using System.Drawing;

namespace Models
{
    public class Train
    {
        public bool isLocomotive = true;
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
        // Constants for the locomotive parts dimensions
        private readonly int numOfWheels;
        private readonly float wheelRadius;
        private readonly float wheelThickness;
        private readonly float cabWidth;
        private readonly float cabHeight;
        private readonly float cabDepth;
        private readonly float cabBottomBaseWidth;
        private readonly float cabBottomBaseHeight;
        private readonly float cabBottomCouplerWidth;
        private readonly float cabBottomCouplerHeight;
        private readonly float cabBottomCouplerDepth;
        private readonly float cabinWidth;
        private readonly float cabinHeight;
        private readonly float cabinDepth;
        private readonly float chimneyBaseRadius;
        private readonly float chimneyTopRadius;
        private readonly float chimneyHeight;

        GLUquadric obj;

        private float wheelRotation = 0.0f;
        private uint cabList, wheelList, locomotiveList;
        TextBox debugTextBox;

        public Locomotive(TextBox debugTextBox, float shininess = DefaultConfig.SHININESS)
        {
            cabWidth = 3.5f;
            cabHeight = 0.7f;
            cabDepth = 0.7f;
            cabinWidth = 0.7f;
            cabinHeight = cabHeight * 1.2f;
            cabinDepth = cabDepth;
            cabBottomBaseWidth = cabWidth * 1.2f + cabinWidth;
            cabBottomBaseHeight = 0.2f;
            cabBottomCouplerWidth = 0.7f;
            cabBottomCouplerHeight = 0.1f;
            cabBottomCouplerDepth = 0.2f;

            numOfWheels = 4;
            wheelRadius = 0.4f;
            wheelThickness = 0.2f;
            chimneyBaseRadius = 0.1f;
            chimneyTopRadius = 0.7f;
            chimneyHeight = 0.7f;
            obj = GLU.gluNewQuadric();
            this.debugTextBox = debugTextBox;
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
            DrawWheel(wheelRadius, wheelThickness, ColorName.Black);
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

            DrawWheels();
        }

        public void Draw()
        {
            GL.glPushMatrix(); // Save the current state
            SetMaterial();
            // Translate to set the new center
            //float centerTranslationX = cabinWidth / 2;
            //GL.glTranslatef(centerTranslationX, 0.0f, 0.0f); // Shift everything to the left

            //GL.glRotatef(wheelRotation, 0.0f, 0.0f, 1.0f); // This uses the wheelRotation, adjust as needed

            // Now draw the locomotive
            GL.glCallList(locomotiveList);

            GL.glPopMatrix(); // Restore the original state
        }

        private void DrawCuboid(float width, float height, float depth, ColorName color)
        {
            // Set the color for the cuboid
            ColorUtil.SetColor(color);

            // Front Face (pointing towards positive Z)
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(0.0f, 0.0f, 1.0f); // Normal pointing outwards the front face
            GL.glVertex3f(-width, -height, depth);
            GL.glVertex3f(width, -height, depth);
            GL.glVertex3f(width, height, depth);
            GL.glVertex3f(-width, height, depth);
            GL.glEnd();

            // Back Face (pointing towards negative Z)
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(0.0f, 0.0f, -1.0f); // Normal pointing outwards the back face
            GL.glVertex3f(-width, -height, -depth);
            GL.glVertex3f(width, -height, -depth);
            GL.glVertex3f(width, height, -depth);
            GL.glVertex3f(-width, height, -depth);
            GL.glEnd();

            // Top Face (pointing upwards)
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(0.0f, 1.0f, 0.0f); // Normal pointing upwards
            GL.glVertex3f(-width, height, -depth);
            GL.glVertex3f(width, height, -depth);
            GL.glVertex3f(width, height, depth);
            GL.glVertex3f(-width, height, depth);
            GL.glEnd();

            // Bottom Face (pointing downwards)
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(0.0f, -1.0f, 0.0f); // Normal pointing downwards
            GL.glVertex3f(-width, -height, -depth);
            GL.glVertex3f(width, -height, -depth);
            GL.glVertex3f(width, -height, depth);
            GL.glVertex3f(-width, -height, depth);
            GL.glEnd();

            // Right Face (pointing towards positive X)
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(1.0f, 0.0f, 0.0f); // Normal pointing to the right
            GL.glVertex3f(width, -height, -depth);
            GL.glVertex3f(width, height, -depth);
            GL.glVertex3f(width, height, depth);
            GL.glVertex3f(width, -height, depth);
            GL.glEnd();

            // Left Face (pointing towards negative X)
            GL.glBegin(GL.GL_QUADS);
            GL.glNormal3f(-1.0f, 0.0f, 0.0f); // Normal pointing to the left
            GL.glVertex3f(-width, -height, -depth);
            GL.glVertex3f(-width, height, -depth);
            GL.glVertex3f(-width, height, depth);
            GL.glVertex3f(-width, -height, depth);
            GL.glEnd();

        }

        private void SetMaterial()
        {
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT, MaterialConfig.Instance.MatAmbient);
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_DIFFUSE, MaterialConfig.Instance.MatDiffuse);
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_SPECULAR, MaterialConfig.Instance.MatSpecular);
            //GL.glMaterialf(GL.GL_FRONT_AND_BACK, GL.GL_SHININESS, MaterialConfig.Instance.Shininess);
        }

        private void DrawCabBase()
        {
            GL.glPushMatrix(); // Save the current state
            // Draw the base of the cab
            //float halfCabWidth = cabWidth / 2;
            //float halfCabHeight = cabHeight / 2;
            //float halfCabDepth = cabDepth / 2;
            DrawCuboid(cabWidth, cabHeight, cabDepth, ColorName.Red);
            GL.glPopMatrix(); // Restore the original state

            // Draw the cabin of the cab
            GL.glPushMatrix(); // Save the current state
            DrawCabin();
            GL.glPopMatrix(); // Restore the original state

            // Draw the bottom base of the cab
            GL.glPushMatrix(); // Save the current state
            DrawCabBottomBase();
            GL.glPopMatrix(); // Restore the original state

            // Draw the bottom coupler of the cab
            GL.glPushMatrix(); // Save the current state
            DrawCabBottomCoupler();
            GL.glPopMatrix(); // Restore the original state
        }
        private void DrawCabBottomCoupler()
        {
            // Assuming cabWidth, cabHeight, and cabDepth have been defined elsewhere
            float halfBottomBaseWidth = cabBottomBaseWidth / 2;
            //float halfWidth = cabBottomCouplerWidth / 2;
            //float halfHeight = cabBottomCouplerHeight / 2;
            //float halfDepth = cabBottomCouplerDepth / 2;
            // Translate down to position the bottom base at the bottom of the cab
            float translateY = -(cabHeight / 2 + cabBottomBaseHeight / 2);
            float translateX = -halfBottomBaseWidth * 1.1f;
            GL.glTranslatef(translateX, translateY, 0.0f); // No translation on X and Z axes
            DrawCuboid(cabBottomCouplerWidth, cabBottomCouplerHeight, cabBottomCouplerDepth, ColorName.Bronze);
        }
        private void DrawCabBottomBase()
        {
            // Assuming cabWidth, cabHeight, and cabDepth have been defined elsewhere
            //float halfWidth = cabBottomBaseWidth / 2;
            //float halfHeight = cabBottomBaseHeight / 2;
            //float halfDepth = cabDepth / 2;
            // Translate down to position the bottom base at the bottom of the cab
            float translateY = -cabHeight; //- (cabHeight / 2 + cabBottomBaseHeight / 2);
            float translateX = -cabinWidth / 2;
            GL.glTranslatef(translateX, translateY, 0.0f); // No translation on X and Z axes
            DrawCuboid(cabBottomBaseWidth, cabBottomBaseHeight, cabDepth, ColorName.DimGrey);
        }

        private void DrawCabin()
        {
            // Translate for the cabin drawing
            float cabinTranslateX = -(cabWidth + cabinWidth / 2); // Move it to the left of the cab base
            float cabinTranslateY = cabBottomBaseHeight; // Align the bottom of the cabin with the top of the cab bottom base
            GL.glTranslatef(cabinTranslateX, cabinTranslateY, 0.0f);

            DrawCuboid(cabinWidth, cabinHeight, cabinDepth, ColorName.Bronze); // Drawing the cabin
        }


        private void DrawChimney()
        {
            // Assuming you're using gluCylinder to draw the chimney
            GL.glPushMatrix(); // Save the current transformation state

            // Calculate the translation values before drawing the chimney
            float translateX = cabWidth * 0.75f;
            float translateY = (cabHeight / 2) * 1.2f; // Align the base of the chimney with the top of the cab
            //float translateY = cabHeight / 1.1f; // On top of the cab
            float translateZ = 0.0f; // Centered along the cab's depth


            GL.glTranslatef(translateX, translateY, translateZ); // Apply the calculated translation
            DrawCylinder(baseRadius: chimneyBaseRadius, topRadius: chimneyTopRadius, height: chimneyHeight, color: ColorName.Black);
            GL.glPopMatrix(); // Restore the previous transformation state
        }

        private void DrawCylinder(float baseRadius, float topRadius, float height, ColorName color, bool isRotateUpwards = true)
        {
            ColorUtil.SetColor(color);
            //SetMaterial(color);
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
            //SetMaterial(color);

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
            // Constants for wheel placement
            float wheelOffsetX = cabBottomBaseWidth * 0.7f; // 20% of the bottom base width from the center to each side
            float wheelOffsetY = -(wheelRadius + cabBottomBaseHeight) * 1.3f; // Just below the bottom base
            float wheelOffsetZFront = cabDepth;// * 0.8f; // 20% of the depth for front wheels
            float wheelOffsetZBack = -cabDepth;// * 0.8f; // 20% of the depth for back wheels


            // Place the wheels relative to the cab
            for (int i = 0; i < numOfWheels; i++)
            {
                GL.glPushMatrix();
                // Determine the X and Z positions based on the loop index
                float posX = (i % 2 == 0) ? -wheelOffsetX : wheelOffsetX; // Alternate sides
                float posZ = (i < 2) ? wheelOffsetZFront : wheelOffsetZBack; // Alternate front/back
                // Apply the transformation for wheel position
                GL.glTranslatef(posX, wheelOffsetY, posZ);
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

    public class Coach
    {
        public float shininess;
        public Coach(TextBox debugTextBox, float shininess = DefaultConfig.SHININESS)
        {

        }


        public void Draw()
        {

        }
    }
}

using OpenGL;
using System;
using System.Windows.Forms;
using Utils;

namespace Models
{
    public class Train
    {
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

            //locomotive.Draw();
            DrawCoaches();

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

        public Locomotive(TextBox debugTextBox)
        {
            cabWidth = 7.0f;
            cabHeight = 1.5f;
            cabDepth = 1.5f;
            cabinWidth = 1.5f;
            cabinHeight = cabHeight * 2f; 
            cabinDepth = cabDepth;
            cabBottomBaseWidth = cabWidth * 1.2f + cabinWidth;
            cabBottomBaseHeight = 0.5f;
            cabBottomCouplerWidth = 1.5f;
            cabBottomCouplerHeight = 0.2f;
            cabBottomCouplerDepth = 0.5f;

            numOfWheels = 4;
            wheelRadius = 0.8f;
            wheelThickness = 0.5f;
            chimneyBaseRadius = 0.3f;
            chimneyTopRadius = 0.8f;
            chimneyHeight = 1.5f;
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

            // Translate to set the new center
            float centerTranslationX = cabinWidth / 2;
            GL.glTranslatef(centerTranslationX, 0.0f, 0.0f); // Shift everything to the left

            //GL.glRotatef(wheelRotation, 0.0f, 0.0f, 1.0f); // This uses the wheelRotation, adjust as needed

            // Now draw the locomotive
            GL.glCallList(locomotiveList);

            GL.glPopMatrix(); // Restore the original state
        }

        private void DrawCuboid(float width, float height, float depth, ColorName color)
        {
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
        }

        private void DrawCabBase()
        {
            GL.glPushMatrix(); // Save the current state
            // Draw the base of the cab
            float halfCabWidth = cabWidth / 2;
            float halfCabHeight = cabHeight / 2;
            float halfCabDepth = cabDepth / 2;
            DrawCuboid(halfCabWidth, halfCabHeight, halfCabDepth, ColorName.Beige);
            GL.glPopMatrix(); // Restore the original state

            // Draw the cabin of the cab
            DrawCabin();

            // Draw the bottom base of the cab
            DrawCabBottomBase();
            GL.glPopMatrix(); // Restore the original state

            // Draw the bottom coupler of the cab
            DrawCabBottomCoupler();
            GL.glPopMatrix(); // Restore the original state
        }
        private void DrawCabBottomCoupler()
        {
            GL.glPushMatrix(); // Save the current state

            // Assuming cabWidth, cabHeight, and cabDepth have been defined elsewhere
            float halfBottomBaseWidth = cabBottomBaseWidth / 2;
            float halfWidth = cabBottomCouplerWidth / 2;
            float halfHeight = cabBottomCouplerHeight / 2;
            float halfDepth = cabBottomCouplerDepth / 2;
            // Translate down to position the bottom base at the bottom of the cab
            float translateY = -(cabHeight / 2 + cabBottomBaseHeight / 2);
            float translateX = - halfBottomBaseWidth * 1.1f;
            GL.glTranslatef(translateX, translateY, 0.0f); // No translation on X and Z axes
            
            DrawCuboid(halfWidth, halfHeight, halfDepth, ColorName.Bronze);
            
            GL.glPopMatrix(); // Restore the original state
        }
        private void DrawCabBottomBase()
        {
            GL.glPushMatrix(); // Save the current state
          
            // Assuming cabWidth, cabHeight, and cabDepth have been defined elsewhere
            float halfWidth = cabBottomBaseWidth / 2;
            float halfHeight = cabBottomBaseHeight / 2;
            float halfDepth = cabDepth / 2;
            // Translate down to position the bottom base at the bottom of the cab
            float translateY = -(cabHeight / 2 + cabBottomBaseHeight / 2);
            float translateX = -cabinWidth / 2;
            GL.glTranslatef(translateX, translateY, 0.0f); // No translation on X and Z axes
            
            DrawCuboid(halfWidth, halfHeight, halfDepth, ColorName.Bronze);
            
            GL.glPopMatrix(); // Restore the original state
        }

        private void DrawCabin()
        {
            GL.glPushMatrix(); // Save the current state for cabin drawing
            
            // Translate for the cabin drawing
            float cabinTranslateX = -(cabWidth / 2 + cabinWidth / 2); // Move it to the left of the cab base
            float cabinTranslateY = cabBottomBaseHeight/1.4f; // Align the bottom of the cabin with the top of the cab bottom base
            GL.glTranslatef(cabinTranslateX, cabinTranslateY, 0.0f);
            
            DrawCuboid(cabinWidth / 2, cabinHeight / 2, cabinDepth / 2, ColorName.Blue); // Drawing the cabin
            
            GL.glPopMatrix(); // Restore the original state after drawing the cabin
        }


        private void DrawChimney()
        {
            // Assuming you're using gluCylinder to draw the chimney
            GL.glPushMatrix(); // Save the current transformation state
            
            // Setup color and materials
            ColorUtil.SetColor(ColorName.Black);

            // Calculate the translation values before drawing the chimney
            float translateX = cabWidth * 0.25f; // 75% to the right, starting from the center
            float translateY = (cabHeight / 2); // Align the base of the chimney with the top of the cab
            //float translateY = cabHeight / 1.1f; // On top of the cab
            float translateZ = 0.0f; // Centered along the cab's depth


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
            // Constants for wheel placement
            float wheelOffsetX = cabBottomBaseWidth * 0.3f; // 20% of the bottom base width from the center to each side
            float wheelOffsetY = -(wheelRadius + cabBottomBaseHeight); // Just below the bottom base
            float wheelOffsetZFront = cabDepth * 0.5f; // 20% of the depth for front wheels
            float wheelOffsetZBack = -cabDepth * 0.5f; // 20% of the depth for back wheels


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
        // Constants for the coach parts dimensions
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

        GLUquadric obj;


        private float wheelRotation = 0.0f;
        private uint cabList, wheelList, coachList;
        TextBox debugTextBox;

        public Coach(TextBox debugTextBox)
        {
            cabWidth = 14.0f;
            cabHeight = 2.0f;
            cabDepth = 1.5f;
            cabBottomBaseWidth = cabWidth * 1.1f;
            cabBottomBaseHeight = 0.5f;
            cabBottomCouplerWidth = 1.5f;
            cabBottomCouplerHeight = 0.2f;
            cabBottomCouplerDepth = 0.5f;

            numOfWheels = 8;
            wheelRadius = 0.8f;
            wheelThickness = 0.5f;
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
            float centerTranslationX = cabinWidth / 2;
            GL.glTranslatef(centerTranslationX, 0.0f, 0.0f); // Shift everything to the left

            //GL.glRotatef(wheelRotation, 0.0f, 0.0f, 1.0f); // This uses the wheelRotation, adjust as needed

            // Now draw the coach
            GL.glCallList(coachList);

            GL.glPopMatrix(); // Restore the original state
        }

        private void DrawCuboid(float width, float height, float depth, ColorName color)
        {
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
        }

        private void DrawCabBase()
        {
            GL.glPushMatrix(); // Save the current state
            // Draw the base of the cab
            float halfCabWidth = cabWidth / 2;
            float halfCabHeight = cabHeight / 2;
            float halfCabDepth = cabDepth / 2;
            DrawCuboid(halfCabWidth, halfCabHeight, halfCabDepth, ColorName.Beige);
            GL.glPopMatrix(); // Restore the original state

            // Draw the cabin of the cab

            // Draw the bottom base of the cab
            DrawCabBottomBase();
            GL.glPopMatrix(); // Restore the original state

            // Draw the bottom coupler of the cab
            DrawCabBottomCoupler();
            GL.glPopMatrix(); // Restore the original state
        }
        private void DrawCabBottomCoupler()
        {
            GL.glPushMatrix(); // Save the current state

            // Assuming cabWidth, cabHeight, and cabDepth have been defined elsewhere
            float halfBottomBaseWidth = cabBottomBaseWidth / 2;
            float halfWidth = cabBottomCouplerWidth / 2;
            float halfHeight = cabBottomCouplerHeight / 2;
            float halfDepth = cabBottomCouplerDepth / 2;
            // Translate down to position the bottom base at the bottom of the cab
            float translateY = -(cabHeight / 2 + cabBottomBaseHeight / 2);
            float translateX = -halfBottomBaseWidth * 1.1f;
            GL.glTranslatef(translateX, translateY, 0.0f); // No translation on X and Z axes

            DrawCuboid(halfWidth, halfHeight, halfDepth, ColorName.Bronze);

            GL.glPopMatrix(); // Restore the original state
        }
        private void DrawCabBottomBase()
        {
            GL.glPushMatrix(); // Save the current state

            // Assuming cabWidth, cabHeight, and cabDepth have been defined elsewhere
            float halfWidth = cabBottomBaseWidth / 2;
            float halfHeight = cabBottomBaseHeight / 2;
            float halfDepth = cabDepth / 2;
            // Translate down to position the bottom base at the bottom of the cab
            float translateY = -(cabHeight / 2 + cabBottomBaseHeight / 2);
            GL.glTranslatef(0.0f, translateY, 0.0f); // No translation on X and Z axes

            DrawCuboid(halfWidth, halfHeight, halfDepth, ColorName.Bronze);

            GL.glPopMatrix(); // Restore the original state
        }

        private void DrawCabin()
        {
            GL.glPushMatrix(); // Save the current state for cabin drawing

            // Translate for the cabin drawing
            float cabinTranslateX = -(cabWidth / 2 + cabinWidth / 2); // Move it to the left of the cab base
            float cabinTranslateY = cabBottomBaseHeight / 1.4f; // Align the bottom of the cabin with the top of the cab bottom base
            GL.glTranslatef(cabinTranslateX, cabinTranslateY, 0.0f);

            DrawCuboid(cabinWidth / 2, cabinHeight / 2, cabinDepth / 2, ColorName.Blue); // Drawing the cabin

            GL.glPopMatrix(); // Restore the original state after drawing the cabin
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
            // Calculate the number of wheels on one side since they come in pairs
            int wheelsPerSide = numOfWheels / 2;

            // Determine the spacing between wheels along the X-axis
            float edgeMargin = cabBottomBaseWidth * 0.15f; // Let's use 15% for the margin
            float usableWidth = cabBottomBaseWidth - 2 * edgeMargin; // Width available for placing wheels
            float wheelSpacing = (wheelsPerSide > 1) ? usableWidth / (wheelsPerSide - 1) : 0; // Prevent division by zero

            float wheelOffsetY = -(wheelRadius + cabBottomBaseHeight); // Position Y just below the bottom base
            float wheelOffsetZFront = -cabDepth / 2 + cabDepth * 0.1f; // Front margin
            float wheelOffsetZBack = cabDepth / 2 - cabDepth * 0.1f; // Back margin


            for (int i = 0; i < wheelsPerSide; i++)
            {
                // Calculate X-position for each wheel on one side
                float posX = -cabBottomBaseWidth / 2 + edgeMargin + (i * wheelSpacing);
                float posZ = (i < 2) ? wheelOffsetZFront : wheelOffsetZBack; // Alternate front/back

                // Draw wheel on the left side
                GL.glPushMatrix();
                GL.glTranslatef(posX, wheelOffsetY, posZ);
                GL.glCallList(wheelList);
                GL.glPopMatrix();

                // Draw wheel on the right side (mirror position along the X-axis)
                GL.glPushMatrix();
                GL.glTranslatef(-posX, wheelOffsetY, posZ); // Use -posX for mirroring
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

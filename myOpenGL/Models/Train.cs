using OpenGL;
using System;
using static Utils.MaterialConfig;
using Utils;
using TextBox = System.Windows.Forms.TextBox;
using System.Drawing;
using GraphicProject.Utils.Math;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace Models
{
    public class Train
    {
        public bool isLocomotive = true;
        public bool isTextureEnabled = true;
        private Locomotive mainLocomotive;
        private Locomotive shadowLocomotive;
        private Coach[] coaches;
        public TextBox debugTextBox;

        public Train(TextBox debugTextBox, int numberOfCoaches, bool isTextureEnabled = true)
        {
            this.debugTextBox = debugTextBox;
            this.coaches = new Coach[numberOfCoaches];
            for (int i = 0; i < numberOfCoaches; i++)
            {
                // Initialize each coach instance here
                this.coaches[i] = new Coach(debugTextBox);
            }
            this.isTextureEnabled = isTextureEnabled;
            this.mainLocomotive = new Locomotive(debugTextBox, isShadowDrawing: false, isTextureEnabled: isTextureEnabled);
            this.shadowLocomotive = new Locomotive(debugTextBox, isShadowDrawing: true, isTextureEnabled: false);
        }

        public void Draw(bool isShadowDrawing = false)
        {
            GL.glPushMatrix(); // Save the current state

            if (isLocomotive)
            {
                if (!isShadowDrawing)
                {
                    mainLocomotive.Draw();
                }
                else
                {
                    shadowLocomotive.Draw();
                }
            }
            else
            {
                DrawCoaches(isShadowDrawing);
            }

            //locomotive.Draw();
            //DrawCoaches();

            GL.glPopMatrix(); // Restore the original state
        }

        public void Update(float deltaTime)
        {
            mainLocomotive.Update(deltaTime);
            shadowLocomotive.Update(deltaTime);
        }

        public void DrawCoaches(bool isShadowDrawing)
        {
            for (int i = 0; i < coaches.Length; i++)
            {
                coaches[i].Draw(isShadowDrawing);
            }
        }
    }

    public class Locomotive
    {
        // Constants for the locomotive parts dimensions
        private readonly int numOfWheels;
        private readonly float wheelRadius;
        private readonly float wheelThickness;
        private readonly float carriageWidth;
        private readonly float carriageHeight;
        private readonly float carriageDepth;
        private readonly float cabBottomBaseWidth;
        private readonly float cabBottomBaseHeight;
        private readonly float cabBottomCouplerWidth;
        private readonly float cabBottomCouplerHeight;
        private readonly float cabBottomCouplerDepth;
        private readonly float controlCabinWidth;
        private readonly float controlCabinHeight;
        private readonly float controlCabinDepth;
        private readonly float chimneyBaseRadius;
        private readonly float chimneyTopRadius;
        private readonly float chimneyHeight;


        private readonly Color textureColor = Color.White;
        private readonly Color chimneyColor = Color.DarkGray;
        private readonly Color controlCabinColor = Color.DarkGoldenrod;
        private readonly Color carriageColor = Color.SandyBrown;
        private readonly Color cabBottomColor = Color.DarkOliveGreen;
        private readonly Color WheelsColor = Color.DarkSlateGray;
        //private readonly Color chimneyColor = Color.White;
        //private readonly Color controlCabinColor = Color.White;
        //private readonly Color carriageColor = Color.White;
        //private readonly Color cabBottomColor = Color.White;
        //private readonly Color WheelsColor = Color.White;

        gluNewQuadric obj;

        private float wheelRotation = 0;
        private bool isWheelRotation = true;
        private uint cabList, smokeQuadDisplayList, locomotiveList;
        TextBox debugTextBox;
        private bool isShadowDrawing;
        private bool isTextureEnabled;
        private readonly Color shadowColor = Color.DarkGray;
        public uint[] Textures;
        public string[] imagesName;
        Dictionary<Orientation, TrainObject> carriageFaceDictionary;
        Dictionary<Orientation, TrainObject> controlCabinFaceDictionary;
        Dictionary<Orientation, TrainObject> cabBottomBaseFaceDictionary;
        Dictionary<Orientation, TrainObject> cabCouplerFaceDictionary;

        List<SmokeParticle> particles;
        Random random = new Random();
        private float particleGenerationTimer = 0.0f;
        private float particleGenerationInterval = 0.1f;
        private float chimneyTopX;
        private float chimneyTopY;
        private float chimneyTopZ;


        public Locomotive(TextBox debugTextBox,
            float shininess = DefaultConfig.MAT_SHININESS, bool isShadowDrawing = false, bool isTextureEnabled = true)
        {
            carriageWidth = 3.5f;
            carriageHeight = 0.7f;
            carriageDepth = 0.7f;
            controlCabinWidth = 0.7f;
            controlCabinHeight = carriageHeight * 1.2f;
            controlCabinDepth = carriageDepth;
            cabBottomBaseWidth = carriageWidth * 1.2f + controlCabinWidth;
            cabBottomBaseHeight = 0.2f;
            cabBottomCouplerWidth = 0.4f;
            cabBottomCouplerHeight = 0.1f;
            cabBottomCouplerDepth = 0.2f;

            numOfWheels = 4;
            wheelRadius = 0.4f;
            wheelThickness = 0.2f;
            chimneyBaseRadius = 0.2f;
            chimneyTopRadius = 0.3f;
            chimneyHeight = 1f;

            Textures = new uint[Enum.GetValues(typeof(TrainObject)).Length];
            imagesName = Enum.GetNames(typeof(TrainObject))
                     .Select(name => name.ToLower() + ".bmp")
                     .ToArray();
            for (int i = 0; i < Textures.Length; i++)
            {
                Textures[i] = (uint)i;
            }

            carriageFaceDictionary = new Dictionary<Orientation, TrainObject>
            {
                { Orientation.FRONT, TrainObject.CARRIAGE_FRONT },
                { Orientation.BACK, TrainObject.CARRIAGE_BACK },
                { Orientation.RIGHT, TrainObject.CARRIAGE_RIGHT },
                { Orientation.LEFT, TrainObject.CARRIAGE_LEFT },
                { Orientation.TOP, TrainObject.CARRIAGE_TOP },
                { Orientation.BOTTOM, TrainObject.CARRIAGE_BOTTOM },
            };
            controlCabinFaceDictionary = new Dictionary<Orientation, TrainObject>
            {
                { Orientation.FRONT, TrainObject.CONTROL_CABIN_FRONT},
                { Orientation.BACK, TrainObject.CONTROL_CABIN},
                { Orientation.RIGHT, TrainObject.CONTROL_CABIN},
                { Orientation.LEFT, TrainObject.CONTROL_CABIN},
                { Orientation.TOP, TrainObject.CONTROL_CABIN},
                { Orientation.BOTTOM, TrainObject.CONTROL_CABIN},
            };
            cabBottomBaseFaceDictionary = new Dictionary<Orientation, TrainObject>
            {
                { Orientation.FRONT, TrainObject.CAB_BOTTOM_BASE },
                { Orientation.BACK, TrainObject.CAB_BOTTOM_BASE },
                { Orientation.RIGHT, TrainObject.CAB_BOTTOM_BASE },
                { Orientation.LEFT, TrainObject.CAB_BOTTOM_BASE },
                { Orientation.TOP, TrainObject.CAB_BOTTOM_BASE },
                { Orientation.BOTTOM, TrainObject.CAB_BOTTOM_BASE },
            };
            cabCouplerFaceDictionary = new Dictionary<Orientation, TrainObject>
            {
                { Orientation.FRONT, TrainObject.CAB_COUPLER},
                { Orientation.BACK, TrainObject.CAB_COUPLER},
                { Orientation.RIGHT, TrainObject.CAB_COUPLER},
                { Orientation.LEFT, TrainObject.CAB_COUPLER},
                { Orientation.TOP, TrainObject.CAB_COUPLER},
                { Orientation.BOTTOM, TrainObject.CAB_COUPLER},
            };

            particles = new List<SmokeParticle>();

            this.isShadowDrawing = isShadowDrawing;
            this.isTextureEnabled = isTextureEnabled;
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
            //wheelList = locomotiveList + 2; // The third list is for the wheels
            smokeQuadDisplayList = locomotiveList + 2; // The third list is for the wheels


            // Define the cab
            GL.glNewList(cabList, GL.GL_COMPILE);
            DrawCabBase();
            EnableTexture(TrainObject.CHIMNEY);
            DrawChimney();
            DisableTexture();
            GL.glEndList();
            // Define the wheel
            //GL.glNewList(wheelList, GL.GL_COMPILE);
            //DrawWheel(wheelRadius, wheelThickness, WheelsColor);
            //DisableTexture();
            //GL.glEndList();

            // Combine into the locomotive
            GL.glNewList(locomotiveList, GL.GL_COMPILE);
            CreateLocomotiveList();
            GL.glEndList();
            

            // Define the smoke
            GL.glNewList(smokeQuadDisplayList, GL.GL_COMPILE);
            EnableTexture(TrainObject.SMOKE);
            DrawSmoke();
            DisableTexture();
            GL.glEndList();

        }

        private void DrawSmoke()
        {
            // Begin defining a quad
            GL.glBegin(GL.GL_QUADS);

            // Define the quad vertices. Adjust these to create the desired size and orientation.
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-0.5f, -0.5f, 0.0f); // Bottom Left
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(0.5f, -0.5f, 0.0f);  // Bottom Right
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(0.5f, 0.5f, 0.0f);   // Top Right
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-0.5f, 0.5f, 0.0f); // Top Left
            GL.glEnd();
            // End defining the quad
        }

        private void CreateLocomotiveList()
        {
            // Place the cab
            GL.glPushMatrix();
            GL.glCallList(cabList);
            GL.glPopMatrix();

            //DrawWheels();
        }

        public void Draw()
        {
            GL.glPushMatrix(); // Save the current state
            if (!isShadowDrawing)
            {
                SetMaterial();
                SetLighting();
                GenerateTextures();
            }

            // Now draw the locomotive
            GL.glCallList(locomotiveList);

            // Directl-call to support animation
            DrawWheels();
            DrawParticles();

            GL.glPopMatrix(); // Restore the original state
        }


        private void SetLighting()
        {
            //Console.WriteLine($" Ambient = {LightConfig.Instance.Ambient[0]}, {LightConfig.Instance.Ambient[1]}, {LightConfig.Instance.Ambient[2]}, {LightConfig.Instance.Ambient[3]} " +
            //$"Diffuse = {LightConfig.Instance.Diffuse[0]}, {LightConfig.Instance.Diffuse[1]}, {LightConfig.Instance.Diffuse[2]}, {LightConfig.Instance.Diffuse[3]} " +
            //$"Specular = {LightConfig.Instance.Specular[0]}, {LightConfig.Instance.Specular[1]}, {LightConfig.Instance.Specular[2]}, {LightConfig.Instance.Specular[3]} " +
            //$"Position = {LightConfig.Instance.Position[0]}, {LightConfig.Instance.Position[1]}, {LightConfig.Instance.Position[2]}, {LightConfig.Instance.Position[3]} ");
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_AMBIENT, LightConfig.Instance.Ambient);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_DIFFUSE, LightConfig.Instance.Diffuse);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_SPECULAR, LightConfig.Instance.Specular);
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, LightConfig.Instance.Position);

        }

        private void SetMaterial()
        {
            //Console.WriteLine($" MatAmbient = {MaterialConfig.Instance.MatAmbient[0]}, {MaterialConfig.Instance.MatAmbient[1]}, {MaterialConfig.Instance.MatAmbient[2]}, {MaterialConfig.Instance.MatAmbient[3]} " +
            //    $"MatDiffuse = {MaterialConfig.Instance.MatDiffuse[0]}, {MaterialConfig.Instance.MatDiffuse[1]}, {MaterialConfig.Instance.MatDiffuse[2]}, {MaterialConfig.Instance.MatDiffuse[3]} " +
            //    $"MatSpecular = {MaterialConfig.Instance.MatSpecular[0]}, {MaterialConfig.Instance.MatSpecular[1]}, {MaterialConfig.Instance.MatSpecular[2]}, {MaterialConfig.Instance.MatSpecular[3]} " +
            //    $"Shininess = { MaterialConfig.Instance.Shininess}");
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT, MaterialConfig.Instance.MatAmbient);
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_DIFFUSE, MaterialConfig.Instance.MatDiffuse);
            GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_SPECULAR, MaterialConfig.Instance.MatSpecular);
            GL.glMaterialf(GL.GL_FRONT_AND_BACK, GL.GL_SHININESS, MaterialConfig.Instance.Shininess);
        }

        private void EnableTexture(TrainObject trainObject, Color? color = null)
        {
            color = GetAdjustedColor(); // Will return the appropriate color based on the flags or the default Color
            if (isTextureEnabled && !isShadowDrawing)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[(int)trainObject]);
                switch (trainObject)
                {
                    case TrainObject.CARRIAGE_FRONT:
                    case TrainObject.CARRIAGE_BACK:
                    case TrainObject.CARRIAGE_TOP:
                    case TrainObject.CARRIAGE_BOTTOM:
                    case TrainObject.CARRIAGE_RIGHT:
                    case TrainObject.CARRIAGE_LEFT:
                    case TrainObject.CONTROL_CABIN:
                    case TrainObject.CAB_BOTTOM_BASE:
                        // For Cuboids, use object linear or another suitable mapping.
                        GL.glDisable(GL.GL_TEXTURE_GEN_S);
                        GL.glDisable(GL.GL_TEXTURE_GEN_T);
                        // Setup texture coordinates in your drawing function for these.
                        break;
                    case TrainObject.WHEEL_FRONT_BACK:
                    case TrainObject.WHEEL_TOP_BOTTOM:
                        // Wheels might use a different mapping if they look better with spherical.
                        //GL.glEnable(GL.GL_TEXTURE_GEN_S);
                        //GL.glEnable(GL.GL_TEXTURE_GEN_T);
                        GL.glDisable(GL.GL_TEXTURE_GEN_S);
                        GL.glDisable(GL.GL_TEXTURE_GEN_T);
                        GL.glTexGeni(GL.GL_S, GL.GL_TEXTURE_GEN_MODE, (int)GL.GL_SPHERE_MAP);
                        GL.glTexGeni(GL.GL_T, GL.GL_TEXTURE_GEN_MODE, (int)GL.GL_SPHERE_MAP);
                        break;
                    case TrainObject.CHIMNEY:
                        // Assuming the Chimney, being cylindrical, benefits from spherical mapping.
                        //GL.glEnable(GL.GL_TEXTURE_GEN_S);
                        //GL.glEnable(GL.GL_TEXTURE_GEN_T);
                        GL.glDisable(GL.GL_TEXTURE_GEN_S);
                        GL.glDisable(GL.GL_TEXTURE_GEN_T);
                        GL.glTexGeni(GL.GL_S, GL.GL_TEXTURE_GEN_MODE, (int)GL.GL_SPHERE_MAP);
                        GL.glTexGeni(GL.GL_T, GL.GL_TEXTURE_GEN_MODE, (int)GL.GL_SPHERE_MAP);
                        break;
                }
            }
            ColorUtil.SetColor((Color)color);
        }

        private Color GetAdjustedColor(Color? color = null)
        {
            if (isShadowDrawing)
            {
                return Color.FromArgb((int)(0.5f * 255), shadowColor.R, shadowColor.G, shadowColor.B);
            }
            else if (isTextureEnabled)
            {
                return textureColor; // Assuming textureColor is a predefined Color
            }
            else if (color.HasValue)
            {
                return color.Value;
            }
            else
            {
                return Color.White; // Default color
            }
        }

        private void DisableTexture()
        {
            GL.glDisable(GL.GL_TEXTURE_GEN_S);
            GL.glDisable(GL.GL_TEXTURE_GEN_T);
            GL.glDisable(GL.GL_TEXTURE_2D);
        }

        void GenerateTextures()
        {
            for (int i = 0; i < Textures.Length; i++)
            {
                if (File.Exists(imagesName[i]))
                {

                    GL.glGenTextures(1, new uint[] { Textures[i] });
                    Bitmap textureImage = new Bitmap(imagesName[i]);
                    textureImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    System.Drawing.Imaging.BitmapData bitmapdata;
                    Rectangle rect = new Rectangle(0, 0, textureImage.Width, textureImage.Height);

                    bitmapdata = textureImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[i]);
                    GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, textureImage.Width, textureImage.Height,
                                                                    0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);
                    GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                    GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                    textureImage.UnlockBits(bitmapdata);
                    textureImage.Dispose();
                }    
            }
        }

        private void DrawCabBase()
        {
            // Draw the carriage
            GL.glPushMatrix(); // Save the current state
            DrawCuboid(carriageWidth, carriageHeight, carriageDepth, carriageFaceDictionary, carriageColor);
            GL.glPopMatrix(); // Restore the original state

            // Draw the cabin of the cab
            EnableTexture(TrainObject.CONTROL_CABIN);
            GL.glPushMatrix(); // Save the current state
            DrawControlCabin();
            GL.glPopMatrix(); // Restore the original state
            DisableTexture();

            // Draw the bottom base of the cab
            EnableTexture(TrainObject.CAB_BOTTOM_BASE);
            GL.glPushMatrix(); // Save the current state
            DrawCabBottomBase();
            GL.glPopMatrix(); // Restore the original state

            // Draw the bottom couplers of the cab
            GL.glPushMatrix(); // Save the current state
            DrawCabBottomCouplers();
            GL.glPopMatrix(); // Restore the original state
            DisableTexture();
        }
        private void DrawCabBottomCouplers()
        {
            // FrontCoupler
            GL.glPushMatrix();
            float translateY = -(carriageHeight / 2 + cabBottomBaseHeight*1.7f);
            float translateX = -(cabBottomBaseWidth + cabBottomCouplerWidth / 2) * 1.05f;
            GL.glTranslatef(translateX, translateY, 0.0f);
            DrawCuboid(cabBottomCouplerWidth, cabBottomCouplerHeight, cabBottomCouplerDepth, cabCouplerFaceDictionary, cabBottomColor);
            GL.glPopMatrix();

            // Back Coupler
            GL.glPushMatrix();
            translateX = (cabBottomBaseWidth + cabBottomCouplerWidth / 2) * 0.9f; // halfBottomBaseWidth * 2f;
            GL.glTranslatef(translateX, translateY, 0.0f);
            DrawCuboid(cabBottomCouplerWidth, cabBottomCouplerHeight, cabBottomCouplerDepth, cabCouplerFaceDictionary, cabBottomColor);
            GL.glPopMatrix();

        }
        private void DrawCabBottomBase()
        {
            // Translate down to position the bottom base at the bottom of the cab
            float translateY = -carriageHeight; //- (cabHeight / 2 + cabBottomBaseHeight / 2);
            float translateX = -controlCabinWidth / 2;
            GL.glTranslatef(translateX, translateY, 0.0f); // No translation on X and Z axes
            DrawCuboid(cabBottomBaseWidth, cabBottomBaseHeight, carriageDepth, cabBottomBaseFaceDictionary, cabBottomColor);
        }

        private void DrawControlCabin()
        {
            // Translate for the cabin drawing
            float cabinTranslateX = -(carriageWidth + controlCabinWidth / 2); // Move it to the left of the cab base
            float cabinTranslateY = cabBottomBaseHeight; // Align the bottom of the cabin with the top of the cab bottom base
            GL.glTranslatef(cabinTranslateX, cabinTranslateY, 0.0f);

            DrawCuboid(controlCabinWidth, controlCabinHeight, controlCabinDepth, controlCabinFaceDictionary, controlCabinColor); // Drawing the cabin
        }


        private void DrawChimney()
        {
            // Assuming you're using gluCylinder to draw the chimney
            GL.glPushMatrix(); // Save the current transformation state

            // Calculate the translation values before drawing the chimney
            float translateX = carriageWidth * 0.75f;
            float translateY = (carriageHeight / 2) * 1.2f; // Align the base of the chimney with the top of the cab
            float translateZ = 0.0f; // Centered along the cab's depth

            GL.glTranslatef(translateX, translateY, translateZ); // Apply the calculated translation

            chimneyTopX = translateX;
            chimneyTopY = translateY + chimneyHeight; // Add the chimney's height to get the top position
            chimneyTopZ = translateZ;

            DrawCylinder(chimneyBaseRadius, chimneyTopRadius, chimneyHeight, chimneyColor);
            GL.glPopMatrix(); // Restore the previous transformation state
        }


        private void DrawWheel(float radius, float thickness, Color color)
        {
            if (isShadowDrawing)
            {
                color = shadowColor;
            }
            ColorUtil.SetColor(color);

            int numSides = 50; // Number of sides for the disc
            float cylinderRadius = radius * 0.8f; // Reduced radius for the cylinder to create an edge effect

            // Begin wheel rotation
            GL.glPushMatrix();
            EnableTexture(TrainObject.WHEEL_FRONT_BACK);
            // Draw the bottom solid disc with texture
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, -thickness / 2);
            DrawDiscWithTexture(0, radius, numSides, true); // Use 0 for inner radius to make it solid
            GL.glPopMatrix();

            // Draw the top solid disc with texture
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, thickness / 2); // Adjust position to draw on the other end
            DrawDiscWithTexture(0, radius, numSides, true); // Use 0 for inner radius to make it solid
            GL.glPopMatrix();
            EnableTexture(TrainObject.WHEEL_TOP_BOTTOM);
            // Draw the cylinder between the two discs
            GL.glPushMatrix();
            GL.glTranslatef(0.0f, 0.0f, -thickness / 2);
            DrawCylinder(cylinderRadius, cylinderRadius, thickness, color, isRotateUpwards: false, isTextureOn: true);
            GL.glPopMatrix();

            DisableTexture();
            GL.glPopMatrix(); // End wheel rotation
        }


        private void DrawWheels()
        {
            // Constants for wheel placement
            float wheelOffsetX = cabBottomBaseWidth * 0.7f; // 20% of the bottom base width from the center to each side
            float wheelOffsetY = -(wheelRadius + cabBottomBaseHeight) * 1.3f; // Just below the bottom base
            float wheelOffsetZFront = carriageDepth * 1.10f;// * 0.8f; // 20% of the depth for front wheels
            float wheelOffsetZBack = -carriageDepth * 1.10f;// * 0.8f; // 20% of the depth for back wheels


            // Place the wheels relative to the cab
            for (int i = 0; i < numOfWheels; i++)
            {
                GL.glPushMatrix();
                // Determine the X and Z positions based on the loop index
                float posX = (i % 2 == 0) ? -wheelOffsetX : wheelOffsetX; // Alternate sides
                float posZ = (i < 2) ? wheelOffsetZFront : wheelOffsetZBack; // Alternate front/back
                // Apply the transformation for wheel position
                GL.glTranslatef(posX, wheelOffsetY, posZ);
                if (isWheelRotation)
                {
                    GL.glRotatef(wheelRotation, 0.0f, 0.0f, 1.0f); // Rotate around the Z-axis
                }
                //GL.glCallList(wheelList);
                DrawWheel(wheelRadius, wheelThickness, WheelsColor); // Adapt this call to your actual wheel drawing method

                GL.glPopMatrix();
            }
        }

        void GenerateParticle()
        {
            Vector3 position = new Vector3(chimneyTopX, chimneyTopY, chimneyTopZ);
            float randX = (float)random.NextDouble() * 0.2f - 0.1f;
            float randZ = (float)random.NextDouble() * 0.2f - 0.1f;
            Vector3 velocity = new Vector3(randX, 0.3f, randZ);

            float scale = 0.5f;
            float life = 2.0f; // Particle will live for 2 seconds

            particles.Add(new SmokeParticle(position, velocity, scale, life));
        }

        private void UpdateParticles(float deltaTime)
        {
            particleGenerationTimer -= deltaTime;

            if (particleGenerationTimer <= 0)
            {
                GenerateParticle();
                particleGenerationTimer = particleGenerationInterval; // Reset the timer
            }

            // Update existing particles
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                var particle = particles[i];
                particle.Position += particle.Velocity * deltaTime;
                particle.Life -= deltaTime;

                if (particle.Life <= 0)
                {
                    particles.RemoveAt(i);
                }
            }
        }

        void DrawParticles()
        {
            foreach (SmokeParticle p in particles)
            {
                GL.glPushMatrix(); // Save the current transformation state

                // Apply particle transformations here
                GL.glTranslated(p.Position.X, p.Position.Y, p.Position.Z);
                GL.glScalef(p.Scale, p.Scale, p.Scale); // Example scaling

                // Draw the quad by executing the display list
                GL.glCallList(smokeQuadDisplayList);

                GL.glPopMatrix(); // Restore the previous transformation state
            }
        }

        private void DrawDiscWithTexture(float innerRadius, float outerRadius, int numSides, bool bottom)
        {
            float angleStep = 360.0f / numSides;
            GL.glBegin(GL.GL_TRIANGLE_FAN);
            GL.glTexCoord2f(0.5f, 0.5f); // Center of the disc in texture coordinates
            GL.glVertex3f(0.0f, 0.0f, 0.0f); // Center vertex for a solid disc
            for (int i = 0; i <= numSides; i++)
            {
                float angle = i * angleStep * (float)Math.PI / 180.0f;
                float x = (float)Math.Cos(angle) * outerRadius;
                float y = (float)Math.Sin(angle) * outerRadius;
                GL.glTexCoord2f((float)Math.Cos(angle) * 0.5f + 0.5f, (float)Math.Sin(angle) * 0.5f + 0.5f); // Mapping texture
                GL.glVertex3f(x, y, 0.0f);
            }
            GL.glEnd();
        }

        
        private void DrawCylinder(float baseRadius, float topRadius, float height, Color color, bool isRotateUpwards = true, bool isTextureOn = true)
        {
            if (isShadowDrawing)
            {
                color = shadowColor;
            }
            ColorUtil.SetColor(color);

            if (isRotateUpwards)
            {
                // Rotate -90 degrees around the x-axis to make the chimney's top face upwards
                GL.glRotatef(-90.0f, 1.0f, 0.0f, 0.0f);
            }

            int slices = 32; // Number of slices around the chimney
            float stackHeight = height; // Height of the chimney
            float angleStep = (float)(2.0 * Math.PI / slices); // Angle between each slice

            for (int i = 0; i < slices; i++)
            {
                float angle = i * angleStep;
                float nextAngle = (i + 1) * angleStep;

                // Calculate lower and upper points for two adjacent slices
                float[] lower1 = { baseRadius * (float)Math.Cos(angle), baseRadius * (float)Math.Sin(angle), 0 };
                float[] upper1 = { topRadius * (float)Math.Cos(angle), topRadius * (float)Math.Sin(angle), stackHeight };
                float[] lower2 = { baseRadius * (float)Math.Cos(nextAngle), baseRadius * (float)Math.Sin(nextAngle), 0 };
                float[] upper2 = { topRadius * (float)Math.Cos(nextAngle), topRadius * (float)Math.Sin(nextAngle), stackHeight };

                GL.glBegin(GL.GL_QUADS);
                // Map the texture onto the quad
                if (isTextureOn)
                {
                    GL.glTexCoord2f((float)i / slices, 0.0f); GL.glVertex3fv(lower1);
                    GL.glTexCoord2f((float)i / slices, 1.0f); GL.glVertex3fv(upper1);
                    GL.glTexCoord2f((float)(i + 1) / slices, 1.0f); GL.glVertex3fv(upper2);
                    GL.glTexCoord2f((float)(i + 1) / slices, 0.0f); GL.glVertex3fv(lower2);
                }
                else
                {
                    GL.glVertex3fv(lower1);
                    GL.glVertex3fv(upper1);
                    GL.glVertex3fv(upper2);
                    GL.glVertex3fv(lower2);
                }
                GL.glEnd();
            }
        }

        private bool ShouldDrawTexture(Dictionary<Orientation, TrainObject> faceTextures, Orientation orientation)
        {
            return faceTextures.ContainsKey(orientation);
        }

        private void SetTexCoordIfNeeded(float x, float y, bool condition)
        {
            if (condition)
            {
                GL.glTexCoord2f(x, y);
            }
        }

        private void DrawCuboid(float width, float height, float depth, Dictionary<Orientation, TrainObject> faceTextures, Color color)
        {
            color = color == null ? Color.White : color;
            foreach (var face in faceTextures)
            {
                // Determine if we should draw texture for this face
                bool shouldDrawTexture = ShouldDrawTexture(faceTextures, face.Key);
                if (shouldDrawTexture)
                {
                    EnableTexture(face.Value, color);
                }
                else
                {
                    // Set the color for the cuboid
                    if (isShadowDrawing)
                    {
                        color = shadowColor;
                    }
                    ColorUtil.SetColor(color); // Use fallback color if texture is missing
                }

                // Drawing each face with conditional texture coordinates
                switch (face.Key)
                {
                    case Orientation.FRONT:
                        // Front Face (pointing towards positive Z)
                        GL.glBegin(GL.GL_QUADS);
                        GL.glNormal3f(0.0f, 0.0f, 1.0f); // Normal pointing outwards the front face

                        SetTexCoordIfNeeded(0.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(-width, -height, depth);
                        SetTexCoordIfNeeded(1.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(width, -height, depth);
                        SetTexCoordIfNeeded(1.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(width, height, depth);
                        SetTexCoordIfNeeded(0.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(-width, height, depth);
                        GL.glEnd();
                        break;
                    case Orientation.BACK:
                        // Back Face (pointing towards negative Z)
                        GL.glBegin(GL.GL_QUADS);
                        GL.glNormal3f(0.0f, 0.0f, -1.0f); // Normal pointing outwards the back face

                        SetTexCoordIfNeeded(0.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(-width, -height, -depth);
                        SetTexCoordIfNeeded(1.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(width, -height, -depth);
                        SetTexCoordIfNeeded(1.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(width, height, -depth);
                        SetTexCoordIfNeeded(0.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(-width, height, -depth);
                        GL.glEnd();
                        break;
                    case Orientation.TOP:
                        // Top Face (pointing upwards)
                        GL.glBegin(GL.GL_QUADS);
                        GL.glNormal3f(0.0f, 1.0f, 0.0f); // Normal pointing upwards

                        SetTexCoordIfNeeded(0.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(-width, height, -depth);
                        SetTexCoordIfNeeded(1.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(width, height, -depth);
                        SetTexCoordIfNeeded(1.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(width, height, depth);
                        SetTexCoordIfNeeded(0.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(-width, height, depth);
                        GL.glEnd();
                        break;
                    case Orientation.BOTTOM:
                        // Bottom Face (pointing downwards)
                        GL.glBegin(GL.GL_QUADS);
                        GL.glNormal3f(0.0f, -1.0f, 0.0f); // Normal pointing downwards

                        SetTexCoordIfNeeded(0.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(-width, -height, -depth);
                        SetTexCoordIfNeeded(1.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(width, -height, -depth);
                        SetTexCoordIfNeeded(1.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(width, -height, depth);
                        SetTexCoordIfNeeded(0.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(-width, -height, depth);
                        GL.glEnd();
                        break;
                    case Orientation.RIGHT:
                        // Right Face (pointing towards positive X)
                        GL.glBegin(GL.GL_QUADS);
                        GL.glNormal3f(1.0f, 0.0f, 0.0f); // Normal pointing to the right

                        SetTexCoordIfNeeded(0.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(width, -height, -depth);
                        SetTexCoordIfNeeded(1.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(width, height, -depth);
                        SetTexCoordIfNeeded(1.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(width, height, depth);
                        SetTexCoordIfNeeded(0.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(width, -height, depth);
                        GL.glEnd();
                        break;
                    case Orientation.LEFT:
                        // Left Face (pointing towards negative X)
                        GL.glBegin(GL.GL_QUADS);
                        GL.glNormal3f(-1.0f, 0.0f, 0.0f); // Normal pointing to the left

                        SetTexCoordIfNeeded(0.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(-width, -height, -depth);
                        SetTexCoordIfNeeded(1.0f, 0.0f, shouldDrawTexture); GL.glVertex3f(-width, height, -depth);
                        SetTexCoordIfNeeded(1.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(-width, height, depth);
                        SetTexCoordIfNeeded(0.0f, 1.0f, shouldDrawTexture); GL.glVertex3f(-width, -height, depth);
                        GL.glEnd();
                        break;
                }

                if (shouldDrawTexture)
                {
                    DisableTexture();
                }
            }
        }

        public void Update(float deltaTime)
        {
            // Update the wheel rotation based on delta time
            // Translate the locomotive, if necessary
            // Update wheel rotation
            if (isWheelRotation)
            {
                wheelRotation += deltaTime * 200.0f; // Adjust the speed as necessary
            }

            Draw();
            // Update particle system
            UpdateParticles(deltaTime);
        }

    }

    public class Coach
    {
        public float shininess;
        public Coach(TextBox debugTextBox, float shininess = DefaultConfig.MAT_SHININESS)
        {

        }


        public void Draw(bool isShadowDrawing)
        {

        }
    }

    class SmokeParticle
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public float Scale;
        public float Life;

        public SmokeParticle(Vector3 position, Vector3 velocity, float scale, float life)
        {
            Position = position;
            Velocity = velocity;
            Scale = scale;
            Life = life;
        }
    }

}

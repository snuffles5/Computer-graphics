using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Models
{
    internal class Rail
    {
        private static readonly ColorName shadowColor = ColorName.LightGrey;
        private static readonly float railLength = 100.0f;
        private static readonly float railWidth = 0.7f;
        private static readonly float railThickness = 0.05f; // Thickness of the rail
        private static readonly float railHeight = 0.05f;
        private static readonly ColorName railColor = ColorName.DimGrey;
        private static readonly ColorName tieColor = ColorName.DarkBronze;
        private static readonly float tieWidth = 1f;
        private static readonly float tieHeight = 0.1f;
        private static readonly float tieLength = 0.1f;
        private static readonly float tieSpacing = 0.5f;
        private uint displayListId;
        public uint[] Textures;
        public string[] imagesName;
        public bool isShadowDrawing;
        Dictionary<Orientation, RailObject> railFaceDictionary;
        Dictionary<Orientation, RailObject> TiesFaceDictionary;

        public Rail(float trainWheelGauge, bool isShadowDrawing = false)
        {
            railFaceDictionary = new Dictionary<Orientation, RailObject>
            {
                { Orientation.FRONT, RailObject.RAIL },
                { Orientation.BACK, RailObject.RAIL },
                { Orientation.RIGHT, RailObject.RAIL },
                { Orientation.LEFT, RailObject.RAIL },
                { Orientation.TOP, RailObject.RAIL },
                { Orientation.BOTTOM, RailObject.RAIL },
            };
            TiesFaceDictionary = new Dictionary<Orientation, RailObject>
            {
                { Orientation.FRONT, RailObject.TIES },
                { Orientation.BACK, RailObject.TIES },
                { Orientation.RIGHT, RailObject.TIES },
                { Orientation.LEFT, RailObject.TIES },
                { Orientation.TOP, RailObject.TIES },
                { Orientation.BOTTOM, RailObject.TIES },
            };
            Textures = new uint[Enum.GetValues(typeof(RailObject)).Length];
            imagesName = Enum.GetNames(typeof(RailObject))
                     .Select(name => name.ToLower() + ".bmp")
                     .ToArray();
            for (int i = 0; i < Textures.Length; i++)
            {
                Textures[i] = (uint)i;
            }

            PrepareList(trainWheelGauge);
            this.isShadowDrawing = isShadowDrawing;
        }

        private void PrepareList(float trainWheelGauge)
        {
            displayListId = GL.glGenLists(1);
            GL.glNewList(displayListId, GL.GL_COMPILE);


            // Draw two rails, positioned to match the wheels' gauge
            for (int i = -1; i <= 1; i += 2)
            {
                GL.glPushMatrix();
                GL.glRotatef(90.0f, 0.0f, 1.0f, 0.0f); // Rotate to lay it along the Z axis
                GL.glTranslatef(i * trainWheelGauge, 0.0f, 0.0f); // Use the train wheel gauge for positioning
                DrawCuboid(railHeight, railThickness, railLength, railFaceDictionary, railColor); // railHeight is now the horizontal dimension
                GL.glPopMatrix();
            }

            //Draw ties
            for (float z = -railLength / 2; z < railLength / 2; z += tieSpacing)
            {
                GL.glPushMatrix();
                GL.glRotatef(90.0f, 0.0f, 1.0f, 0.0f);
                GL.glTranslatef(0, -0.3f, 0.0f); // Position the rails
                GL.glTranslatef(0.0f, railHeight / 2 + tieHeight / 2, z); // Position the ties
                DrawCuboid(tieWidth, tieHeight, tieLength, TiesFaceDictionary, tieColor); 
                GL.glPopMatrix();
            }
            GL.glEndList();
        }

        public void Draw()
        {
            GenerateTextures();
            GL.glCallList(displayListId);
        }

        private void EnableTexture(RailObject railObjects, ColorName color = ColorName.White)
        {
            GL.glEnable(GL.GL_TEXTURE_2D);
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[(int)railObjects]);
            switch (railObjects)
            {
                case RailObject.TIES:
                case RailObject.RAIL:
                    // For Cuboids, use object linear or another suitable mapping.
                    GL.glDisable(GL.GL_TEXTURE_GEN_S);
                    GL.glDisable(GL.GL_TEXTURE_GEN_T);
                    // Setup texture coordinates in your drawing function for these.
                    break;
            }
            ColorUtil.SetColor(color);
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
                else
                {
                    Textures[i] = 0; // Assign 0 to indicate no texture
                }
            }
        }

        private bool ShouldDrawTexture(Dictionary<Orientation, RailObject> faceTextures, Orientation orientation)
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

        private void DrawCuboid(float width, float height, float depth, Dictionary<Orientation, RailObject> faceTextures, ColorName color = ColorName.White)
        {
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
    }
}

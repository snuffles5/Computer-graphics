using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Models
{
    internal class Rail
    {
        private readonly ColorName shadowColor = ColorName.LightGrey;
        private readonly float railLength = 100.0f;
        private readonly float railWidth = 0.7f * 1.5f;
        private readonly float railHeight = 0.05f;
        private readonly ColorName railColor = ColorName.DimGrey;
        private readonly ColorName tieColor = ColorName.Brown;
        private readonly float tieWidth = 0.2f;
        private readonly float tieHeight = 0.1f;
        private readonly float tieLength = 0.1f;
        private readonly float tieSpacing = 2.0f;
        private uint displayListId;
        public uint[] Textures;
        public string[] imagesName;
        public bool isShadowDrawing;
        Dictionary<Orientation, RailObject> railFaceDictionary;
        Dictionary<Orientation, RailObject> TiesFaceDictionary;

        public Rail(bool isShadowDrawing = false)
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

            PrepareList();
            this.isShadowDrawing = isShadowDrawing;
        }

        private void PrepareList()
        {
            displayListId = GL.glGenLists(1);
            GL.glNewList(displayListId, GL.GL_COMPILE);


            // Draw two rails
            for (int i = -1; i <= 1; i += 2)
            {
                GL.glPushMatrix();
                GL.glTranslatef(i * tieLength / 2, 0.0f, 0.0f); // Position the rails
                GL.glRotatef(90.0f, 0.0f, 1.0f, 0.0f);
                DrawCuboid(railWidth, railHeight, railLength, railFaceDictionary, railColor);
                GL.glPopMatrix();
            }

            //Draw ties
            for (float z = -railLength / 2; z < railLength / 2; z += tieSpacing)
            {
                GL.glPushMatrix();
                GL.glRotatef(90.0f, 0.0f, 1.0f, 0.0f);
                GL.glTranslatef(0.0f, railHeight / 2 + tieHeight / 2, z); // Position the ties
                DrawCuboid(tieWidth, tieHeight, tieLength, TiesFaceDictionary, tieColor);
                GL.glPopMatrix();
            }
            GL.glEndList();
        }

        public void Draw()
        {
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

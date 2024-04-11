using OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ColorUtil
    {
        public static void SetColor(ColorName colorName)
        {
            Color color = ColorEnum.GetColor(colorName);
            GL.glColor3f(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
        }
        public static void SetColor(Color color)
        {
            GL.glColor3f(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
        }

        public static void SetColor(ColorName colorName, float alpha)
        {
            Color color = ColorEnum.GetColor(colorName);
            GL.glColor4f(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, alpha);
        }
        public static void SetColor(Color color, float alpha)
        {
            GL.glColor4f(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, alpha);
        }

        public static float[] GetMaterialProperties(ColorName colorName, bool forSpecular = false)
        {
            Color color = ColorEnum.GetColor(colorName);
            // Specular properties
            if (forSpecular)
            {
                return new float[] 
                {
                    Math.Min(1.0f, color.R / 255.0f + 0.2f),
                    Math.Min(1.0f, color.G / 255.0f + 0.2f),
                    Math.Min(1.0f, color.B / 255.0f + 0.2f),
                    color.A / 255.0f
                };
            }
            else
            {
                // Ambient and Diffuse color
                return new float[] 
                {
                    color.R / 255.0f,
                    color.G / 255.0f,
                    color.B / 255.0f,
                    color.A / 255.0f
                };
            }
        }
        public static float[] GetMaterialProperties(Color color, bool forSpecular = false)
        {
            // Specular properties
            if (forSpecular)
            {
                return new float[] 
                {
                    Math.Min(1.0f, color.R / 255.0f + 0.2f),
                    Math.Min(1.0f, color.G / 255.0f + 0.2f),
                    Math.Min(1.0f, color.B / 255.0f + 0.2f),
                    color.A / 255.0f
                };
            }
            else
            {
                // Ambient and Diffuse color
                return new float[] 
                {
                    color.R / 255.0f,
                    color.G / 255.0f,
                    color.B / 255.0f,
                    color.A / 255.0f
                };
            }
        }

    }

}

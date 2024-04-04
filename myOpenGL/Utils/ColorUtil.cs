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

        public static void SetColor(ColorName colorName, float alpha)
        {
            Color color = ColorEnum.GetColor(colorName);
            GL.glColor4f(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, alpha);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public enum ColorName
    {
        Red,
        Green,
        Blue,
        Yellow,
        Orange,
        Purple,
        Cyan,
        Magenta,
        Lime,
        Pink,
        Teal,
        Lavender,
        Brown,
        Beige,
        Maroon,
        Mint,
        Olive,
        Coral,
        Navy,
        Grey,
        White,
        Black,
        Gold,
        Silver,
        Bronze,
        Violet,
        Indigo,
        Chartreuse,
        Aquamarine,
        Azure
    }

    public static class ColorEnum
    {
        public static Color GetColor(ColorName colorName)
        {
            // Match the enum values to corresponding Colors
            switch (colorName)
            {
                case ColorName.Red:
                    return Color.FromArgb(255, 0, 0);
                case ColorName.Green:
                    return Color.FromArgb(0, 255, 0);
                case ColorName.Blue:
                    return Color.FromArgb(0, 0, 255);
                case ColorName.Yellow:
                    return Color.FromArgb(255, 255, 0);
                case ColorName.Orange:
                    return Color.FromArgb(255, 165, 0);
                case ColorName.Purple:
                    return Color.FromArgb(128, 0, 128);
                case ColorName.Cyan:
                    return Color.FromArgb(0, 255, 255);
                case ColorName.Magenta:
                    return Color.FromArgb(255, 0, 255);
                case ColorName.Lime:
                    return Color.FromArgb(50, 205, 50);
                case ColorName.Pink:
                    return Color.FromArgb(255, 192, 203);
                case ColorName.Teal:
                    return Color.FromArgb(0, 128, 128);
                case ColorName.Lavender:
                    return Color.FromArgb(230, 230, 250);
                case ColorName.Brown:
                    return Color.FromArgb(165, 42, 42);
                case ColorName.Beige:
                    return Color.FromArgb(245, 245, 220);
                case ColorName.Maroon:
                    return Color.FromArgb(128, 0, 0);
                case ColorName.Mint:
                    return Color.FromArgb(189, 252, 201);
                case ColorName.Olive:
                    return Color.FromArgb(128, 128, 0);
                case ColorName.Coral:
                    return Color.FromArgb(255, 127, 80);
                case ColorName.Navy:
                    return Color.FromArgb(0, 0, 128);
                case ColorName.Grey:
                    return Color.FromArgb(128, 128, 128);
                case ColorName.White:
                    return Color.FromArgb(255, 255, 255);
                case ColorName.Black:
                    return Color.FromArgb(0, 0, 0);
                case ColorName.Gold:
                    return Color.FromArgb(255, 215, 0);
                case ColorName.Silver:
                    return Color.FromArgb(192, 192, 192);
                case ColorName.Bronze:
                    return Color.FromArgb(205, 127, 50);
                case ColorName.Violet:
                    return Color.FromArgb(238, 130, 238);
                case ColorName.Indigo:
                    return Color.FromArgb(75, 0, 130);
                case ColorName.Chartreuse:
                    return Color.FromArgb(127, 255, 0);
                case ColorName.Aquamarine:
                    return Color.FromArgb(127, 255, 212);
                case ColorName.Azure:
                    return Color.FromArgb(240, 255, 255);
                default:
                    return Color.Black;
            }
        }
    }


}

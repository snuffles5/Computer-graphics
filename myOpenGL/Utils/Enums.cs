using Milkshape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{

    public static class DefaultConfig 
    {
        public const float SHININESS = 128.0f;
        public static readonly float[] MAT_AMBIENT = { 0.7f, 0.7f, 0.7f, 1.0f };
        public static readonly float[] MAT_DIFFUSE = { 0.8f, 0.8f, 0.8f, 1.0f };
        public static readonly float[] MAT_SPECULAR = { 0.1f, 0.1f, 0.1f, 1.0f };
        public static readonly float[] LIGHT_AMBIENT = { 0.2f, 0.2f, 0.2f, 1.0f };
        public static readonly float[] LIGHT_DIFFUSE = { 0.8f, 0.8f, 0.8f, 1.0f };
        public static readonly float[] LIGHT_SPECULAR = { 1.0f, 1.0f, 1.0f, 1.0f };
        public static readonly float[] LIGHT_POSITION = { 1.0f, 1.0f, 5.0f, 0.0f };
    }

    public enum ColorName
    {
        Aquamarine,
        Azure,
        Beige,
        Black,
        Blue,
        Bronze,
        Brown,
        Chartreuse,
        Chocolate,
        Coral,
        Crimson,
        Cyan,
        DarkRed,
        Emerald,
        ForestGreen,
        Gold,
        Green,
        Grey,
        DimGrey,
        Honeydew,
        Indigo,
        Ivory,
        Lavender,
        Lime,
        LimeGreen,
        Magenta,
        Maroon,
        MidnightBlue,
        Mint,
        Moccasin,
        Navy,
        Olive,
        OliveDrab,
        Orange,
        Orchid,
        Peach,
        Periwinkle,
        Pink,
        Plum,
        PowderBlue,
        Purple,
        Red,
        Rose,
        RoyalBlue,
        Salmon,
        SeaGreen,
        Sienna,
        Silver,
        SkyBlue,
        SlateGray,
        Tan,
        Teal,
        Turquoise,
        Violet,
        White,
        Yellow
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
                case ColorName.DimGrey:
                    return Color.FromArgb(105, 105, 105);
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
                case ColorName.Emerald:
                    return Color.FromArgb(80, 200, 120);
                case ColorName.Turquoise:
                    return Color.FromArgb(64, 224, 208);
                case ColorName.Peach:
                    return Color.FromArgb(255, 229, 180);
                case ColorName.Rose:
                    return Color.FromArgb(255, 0, 127);
                case ColorName.Chocolate:
                    return Color.FromArgb(210, 105, 30);
                case ColorName.Tan:
                    return Color.FromArgb(210, 180, 140);
                case ColorName.ForestGreen:
                    return Color.FromArgb(34, 139, 34);
                case ColorName.SkyBlue:
                    return Color.FromArgb(135, 206, 235);
                case ColorName.Plum:
                    return Color.FromArgb(221, 160, 221);
                case ColorName.Orchid:
                    return Color.FromArgb(218, 112, 214);
                case ColorName.Salmon:
                    return Color.FromArgb(250, 128, 114);
                case ColorName.Ivory:
                    return Color.FromArgb(255, 255, 240);
                case ColorName.MidnightBlue:
                    return Color.FromArgb(25, 25, 112);
                case ColorName.OliveDrab:
                    return Color.FromArgb(107, 142, 35);
                case ColorName.Sienna:
                    return Color.FromArgb(160, 82, 45);
                case ColorName.PowderBlue:
                    return Color.FromArgb(176, 224, 230);
                case ColorName.Periwinkle:
                    return Color.FromArgb(204, 204, 255);
                case ColorName.LimeGreen:
                    return Color.FromArgb(50, 205, 50);
                case ColorName.SeaGreen:
                    return Color.FromArgb(46, 139, 87);
                case ColorName.Honeydew:
                    return Color.FromArgb(240, 255, 240);
                case ColorName.SlateGray:
                    return Color.FromArgb(112, 128, 144);
                case ColorName.DarkRed:
                    return Color.FromArgb(139, 0, 0);
                case ColorName.RoyalBlue:
                    return Color.FromArgb(65, 105, 225);
                case ColorName.Moccasin:
                    return Color.FromArgb(255, 228, 181);
                case ColorName.Crimson:
                    return Color.FromArgb(220, 20, 60);
                default:
                    return Color.Black;
            }
        }
    }

    public class MaterialConfig
    {
        private static MaterialConfig _instance;
        public static MaterialConfig Instance => _instance ?? (_instance = new MaterialConfig());

        public float[] MatAmbient { get; set; } = DefaultConfig.MAT_AMBIENT;
        public float[] MatDiffuse { get; set; } = DefaultConfig.MAT_DIFFUSE;
        public float[] MatSpecular { get; set; } = DefaultConfig.MAT_SPECULAR;
        public float Shininess { get; set; } = DefaultConfig.SHININESS;

        public MaterialProperty? GetMaterialByString(string materialPropertyString)
        {
            // Convert the input string to lowercase for case-insensitive comparison
            string lowerCasePropertyString = materialPropertyString.ToLower();

            MaterialProperty property;
            switch (lowerCasePropertyString)
            {
                case "shininess": property = MaterialProperty.SHININESS; break;
                case "ambient": property = MaterialProperty.AMBIENT; break;
                case "diffuse": property = MaterialProperty.DIFFUSE; break;
                case "specular": property = MaterialProperty.SPECULAR; break;
                default:
                    return null; // Indicate that no valid MaterialProperty was found
            }
            return property;
        }

        public void SetMaterialProperty(MaterialProperty? property, float[] values = null, float value = float.NaN)
        {
            if (property == null)
            {
                Console.WriteLine("Invalid material property name.");
                return;
            }


            switch (property)
            {
                case MaterialProperty.AMBIENT:
                    MatAmbient = values;
                    break;
                case MaterialProperty.DIFFUSE:
                    MatDiffuse = values;
                    break;
                case MaterialProperty.SPECULAR:
                    MatSpecular = values;
                    break;
                case MaterialProperty.SHININESS:
                    Shininess = value;
                    break;
                default:
                    throw new ArgumentException("Unhandled material property.");
            }
        }

    }

    public enum MaterialProperty
    {
        SPECULAR,
        SHININESS,
        AMBIENT,
        DIFFUSE
    }

    public class MaterialPropertyUpdateKeyAndValue
    {
        public MaterialProperty? Key { get; set; }
        public float NewValue { get; set; }
        public float[] NewValues { get; set; }
    }

    public class LightConfig
    {
        private static LightConfig _instance;
        public static LightConfig Instance => _instance ?? (_instance = new LightConfig());

        public float[] Ambient = DefaultConfig.LIGHT_AMBIENT;
        public float[] Diffuse = DefaultConfig.LIGHT_DIFFUSE;
        public float[] Specular= DefaultConfig.LIGHT_SPECULAR;
        public float[] Position= DefaultConfig.LIGHT_POSITION;

        public LightProperty? GetLightByString(string lightPropertyString)
        {
            // Convert the input string to lowercase for case-insensitive comparison
            string lowerCasePropertyString = lightPropertyString.ToLower();

            LightProperty property;
            switch (lowerCasePropertyString)
            {
                case "position": property = LightProperty.POSITION; break;
                case "ambient": property = LightProperty.AMBIENT; break;
                case "diffuse": property = LightProperty.DIFFUSE; break;
                case "specular": property = LightProperty.SPECULAR; break;
                default:
                    return null; // Indicate that no valid LightProperty was found
            }
            return property;
        }

        public void SetLightProperty(LightProperty? property, float[] values = null)
        {
            if (property == null)
            {
                Console.WriteLine("Invalid light property name.");
                return;
            }


            switch (property)
            {
                case LightProperty.AMBIENT:
                    Ambient = values;
                    break;
                case LightProperty.DIFFUSE:
                    Diffuse = values;
                    break;
                case LightProperty.SPECULAR:
                    Specular = values;
                    break;
                case LightProperty.POSITION:
                    Position = values;
                    break;
                default:
                    throw new ArgumentException("Unhandled light property.");
            }
        }

    }

    public enum LightProperty
    {
        AMBIENT,
        DIFFUSE,
        SPECULAR,
        POSITION
    }

    public class LightPropertyUpdateKeyAndValue
    {
        public LightProperty? Key { get; set; }
        public float NewValue { get; set; }
        public float[] NewValues { get; set; }
    }
}

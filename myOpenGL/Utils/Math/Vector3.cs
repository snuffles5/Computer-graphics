using MathEX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicProject.Utils.Math
{
    public struct Vector3
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3(double X = 0, double Y = 0, double Z = 0)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public Vector3 CrossProduct(Vector3 v)
        {
            return new Vector3(Y * v.Z - Z * v.Y,
                    Z * v.X - X * v.Z,
                    X * v.Y - Y * v.X);
        }

        public double DotProduct(Vector3 v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        public Vector3 Normalize()
        {
            double d = System.Math.Sqrt(X * X + Y * Y + Z * Z);

            if (d == 0) d = 1;

            return this / d;
        }

        public override string ToString()
        {
            return String.Format("X: {0} Y: {1} Z: {2}", X, Y, Z);
        }

        public static Vector3 operator + (Vector3 v1, Vector3 v2)
        {
            Vector3 vr = new Vector3();

            vr.X = v1.X + v2.X;
            vr.Y = v1.Y + v2.Y;
            vr.Z = v1.Z + v2.Z;

            return vr;
        }

        public static Vector3 operator / (Vector3 v1, double s)
        {
            Vector3 vr;

            vr.X = v1.X / s;
            vr.Y = v1.Y / s;
            vr.Z = v1.Z / s;

            return vr;
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            Vector3 vr;

            vr.X = v1.X - v2.X;
            vr.Y = v1.Y - v2.Y;
            vr.Z = v1.Z - v2.Z;

            return vr;
        }
    }
}

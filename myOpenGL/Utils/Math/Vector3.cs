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

        // Define the * operator to multiply a vector by a scalar
        public static Vector3 operator *(Vector3 vector, float scalar)
        {
            return new Vector3(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        // Define the * operator to multiply a scalar by a vector
        public static Vector3 operator *(float scalar, Vector3 vector)
        {
            return new Vector3(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        // Define the subtraction operator
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

    }

    public struct Vector3WithAngle
    {
        public Vector3 Position { get; set; }
        public double Angle; // Angle in degrees or radians, depending on your use case

        public Vector3WithAngle(double x = 0, double y = 0, double z = 0, double angle = 0) : this()
        {
            Position = new Vector3(x, y, z);
            Angle = angle;
        }

        // Delegate CrossProduct to Vector3
        public Vector3WithAngle CrossProduct(Vector3WithAngle v)
        {
            Vector3 crossProduct = Position.CrossProduct(v.Position);
            return new Vector3WithAngle(crossProduct.X, crossProduct.Y, crossProduct.Z, Angle); // Angle handling can be adjusted based on use case
        }

        // Delegate DotProduct to Vector3
        public double DotProduct(Vector3WithAngle v)
        {
            return Position.DotProduct(v.Position);
        }

        // Delegate Normalize to Vector3
        public Vector3WithAngle Normalize()
        {
            Vector3 normalized = Position.Normalize();
            return new Vector3WithAngle(normalized.X, normalized.Y, normalized.Z, Angle); // Angle remains unchanged
        }

        // Override ToString to include the angle
        public override string ToString()
        {
            return $"{Position.ToString()} Angle: {Angle}";
        }

        // Define operators by delegating to Vector3 operators and handling angles appropriately
        public static Vector3WithAngle operator +(Vector3WithAngle v1, Vector3WithAngle v2)
        {
            double newX = v1.Position.X + v2.Position.X;
            double newY = v1.Position.Y + v2.Position.Y;
            double newZ = v1.Position.Z + v2.Position.Z;
            double newAngle = (v1.Angle + v2.Angle) / 2; 

            return new Vector3WithAngle(newX, newY, newZ, newAngle);
        }

    }


}

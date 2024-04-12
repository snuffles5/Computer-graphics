using MathEX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicProject.Utils.Math
{
    public class Matrix4x4
    {
        public double[,] Data { get; private set; }

        public Matrix4x4()
        {
            Data = new double[4, 4];
        }

        public double this[int row, int col]
        {
            get { return Data[row, col]; }
            set { Data[row, col] = value; }
        }

        public static Matrix4x4 operator +(Matrix4x4 m1, Matrix4x4 m2)
        {
            var result = new Matrix4x4();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return result;
        }

        // Additional matrix operations (multiplication, subtraction, etc.) can be added here as needed

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sb.Append(String.Format("{0,6:0.00} ", Data[i, j]));
                }
                if (i != 3) sb.AppendLine();
            }
            return sb.ToString();
        }
    }

}

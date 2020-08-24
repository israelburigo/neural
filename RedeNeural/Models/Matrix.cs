using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedeNeural.Models
{
    public class Matrix
    {
        private readonly float[,] _data;
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public float this[int row, int col]
        {
            get { return _data[row, col]; }
            set { _data[row, col] = value; }
        }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            _data = new float[Rows, Cols];
        }

        public bool SameType(Matrix a)
        {
            return Rows == a.Rows && Cols == a.Cols;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Cols != b.Rows)
                throw new Exception("Matrix A can't multiply with B");

            var c = new Matrix(a.Cols, b.Rows);

            for (var i = 0; i < a.Rows; i++)
                for (var j = 0; j < b.Cols; j++)
                    for (var k = 0; k < a.Cols; k++)
                        c[i, j] += a[i, k] * b[k, j];
            return c;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (!a.SameType(b))
                throw new Exception("Matrix A isn't the same type of B");

            var c = new Matrix(a.Rows, a.Cols);

            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    c[i, j] = a[i, j] + b[i, j];

            return c;
        }
    }
}

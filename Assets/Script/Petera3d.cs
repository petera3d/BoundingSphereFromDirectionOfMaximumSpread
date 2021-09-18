using UnityEngine;
using System;
using Unity.Mathematics;

namespace Petera3d
{

    #region Structs

    [Serializable]
    public struct Point
    {
        public float x, y, z;

        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 ToVector3() => new Vector3(x, y, z);

        public override string ToString()
        {
            return x + " , " + y + " , " + z;
        }

        public static Point operator *(Point point, float f) => new Point(point.x * f, point.y * f, point.z * f);
        public static Point operator +(Point a, Point b) => new Point(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    [Serializable]
    public struct AABB
    {
        public Point Center;
        public Point Size;
        public Point Extents => Size * 0.5f;

        public AABB(Point center, Point size)
        {
            Center = center;
            Size = size;
        }

        public bool IsCollide(AABB b)
        {
            if (Mathf.Abs(Center.x - b.Center.x) > Extents.x + b.Extents.x) return false;
            if (Mathf.Abs(Center.y - b.Center.y) > Extents.y + b.Extents.y) return false;
            if (Mathf.Abs(Center.z - b.Center.z) > Extents.z + b.Extents.z) return false;
            return true;
        }
    }

    #endregion

    #region Extentions

    public static class Vector3Ext
    {
        public static Point ToPoint(this Vector3 v3)
        {
            return new Point(v3.x, v3.y, v3.z);
        }
    }

    #endregion

    #region Matrix 3x3 class
    
    [Serializable]
    public struct Matrix3x3
    {
        private readonly float[,] _maxtrix3x3;

        public float this[int r, int c]
        {
            get => _maxtrix3x3[r, c];
            set => _maxtrix3x3[r, c] = value;
        }

        public readonly Matrix3x3 Identity =>
            new Matrix3x3(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));

        public readonly Matrix3x3 Zero =>
            new Matrix3x3(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        public readonly Vector3 Column1 => new Vector3(_maxtrix3x3[0, 0], _maxtrix3x3[1, 0], _maxtrix3x3[0, 2]);
        public readonly Vector3 Column2 => new Vector3(_maxtrix3x3[0, 1], _maxtrix3x3[1, 1], _maxtrix3x3[2, 1]);
        public readonly Vector3 Column3 => new Vector3(_maxtrix3x3[0, 2], _maxtrix3x3[1, 2], _maxtrix3x3[2, 2]);

        public Matrix3x3(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            _maxtrix3x3 = new float[3, 3];
            _maxtrix3x3[0, 0] = v1.x;
            _maxtrix3x3[1, 0] = v1.y;
            _maxtrix3x3[2, 0] = v1.z;

            _maxtrix3x3[0, 1] = v2.x;
            _maxtrix3x3[1, 1] = v2.y;
            _maxtrix3x3[2, 1] = v2.z;

            _maxtrix3x3[0, 2] = v3.x;
            _maxtrix3x3[1, 2] = v3.y;
            _maxtrix3x3[2, 2] = v3.z;
        }

        public Matrix3x3(float e00, float e01, float e02,
            float e10, float e11, float e12,
            float e20, float e21, float e22)
        {
            _maxtrix3x3 = new float[3, 3];
            _maxtrix3x3[0, 0] = e00;
            _maxtrix3x3[1, 0] = e10;
            _maxtrix3x3[2, 0] = e20;
            _maxtrix3x3[0, 1] = e01;
            _maxtrix3x3[1, 1] = e11;
            _maxtrix3x3[2, 1] = e21;
            _maxtrix3x3[0, 2] = e02;
            _maxtrix3x3[1, 2] = e12;
            _maxtrix3x3[2, 2] = e22;
        }

        public Matrix3x3 Transpose()
        {
            Matrix3x3 m = Zero;
            m[0, 0] = _maxtrix3x3[0, 0];
            m[0, 1] = _maxtrix3x3[1, 0];
            m[0, 2] = _maxtrix3x3[2, 0];
            m[1, 0] = _maxtrix3x3[0, 1];
            m[1, 1] = _maxtrix3x3[1, 1];
            m[1, 2] = _maxtrix3x3[2, 1];
            m[2, 0] = _maxtrix3x3[0, 2];
            m[2, 1] = _maxtrix3x3[1, 2];
            m[2, 2] = _maxtrix3x3[2, 2];
            return m;
        }

        public override string ToString()
        {
            return Mathf.RoundToInt(_maxtrix3x3[0, 0]) + " " + _maxtrix3x3[0, 1] + " " +
                   Mathf.RoundToInt(_maxtrix3x3[0, 2]) + "\n" +
                   Mathf.RoundToInt(_maxtrix3x3[1, 0]) + " " + _maxtrix3x3[1, 1] + " " +
                   Mathf.RoundToInt(_maxtrix3x3[1, 2]) + "\n" +
                   Mathf.RoundToInt(_maxtrix3x3[2, 0]) + " " + _maxtrix3x3[2, 1] +
                   " " + Mathf.RoundToInt(_maxtrix3x3[2, 2]);
        }

        public Vector3 MultiplyBasisByVector(Vector3 v)
        {
            float x = _maxtrix3x3[0, 0] * v.x + _maxtrix3x3[0, 1] * v.y + _maxtrix3x3[0, 2] * v.z;
            float y = _maxtrix3x3[1, 0] * v.x + _maxtrix3x3[1, 1] * v.y + _maxtrix3x3[1, 2] * v.z;
            float z = _maxtrix3x3[2, 0] * v.x + _maxtrix3x3[2, 1] * v.y + _maxtrix3x3[2, 2] * v.z;
            return new Vector3(x, y, z);
        }

        public void MaxOffDiagonialElementValueAndPosition(out int p, out int q)
        {
            p = 0;
            q = 1; //if all off diagonial elements are zero then assume that value on [0,1] position is the max (zero). This privents to assign value on diagonial pos 0.0  
            // in the case math.abs(_maxtrix3x3[i, j]) > math.abs(maxValue) is never true
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == j) continue;
                    if (math.abs(_maxtrix3x3[i, j]) > math.abs(_maxtrix3x3[p, q]))
                    {
                        p = i;
                        q = j;
                        //Debug.Log("Max " +  _maxtrix3x3[p, q]);
                    }
                }
            }
        }

        #region Operators

        public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
        {
            Matrix3x3 m = new Matrix3x3(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        m[r, c] += a[r, k] * b[k, c];
                    }
                }
            }

            return m;
        }

        #endregion

    }
    #endregion
    
    public class Plane
    {
        public Vector3 Normal;
        public float Distance;

        public Plane(Vector3 a, Vector3 b, Vector3 c)
        {
            Normal = Vector3.Normalize(Vector3.Cross(a - b, a - c));
            Distance = Vector3.Dot(a, Normal);
        }
    }


    public static class Probability
    {
        public static float Variance(float[] points)
        {
            float u = 0;
            int l = points.Length;
            for (int i = 0; i < l; i++)
            {
                u += points[i];
            }

            u /= l;
            float s2 = 0;
            for (int i = 0; i < l; i++)
            {
                s2 += (points[i] - u) * (points[i] - u);
            }

            return s2 / l;
        }

        public static Matrix3x3 CovarianceMatrix(Point[] points)
        {
            Point meanPoint = new Point(0, 0, 0);
            float l = points.Length;
            float n = 1f / l;
            float e00 = 0, e11 = 0, e22 = 0, e01 = 0, e02 = 0, e12 = 0;
            for (int i = 0; i < l; i++)
            {
                meanPoint += points[i];
            }

            meanPoint *= n;
            for (int i = 0; i < l; i++)
            {
                Point p = points[i] - meanPoint;
                e00 += p.x * p.x;
                e11 += p.y * p.y;
                e22 += p.z * p.z;
                e01 += p.x * p.y;
                e02 += p.x * p.z;
                e12 += p.y * p.z;
            }

            return new Matrix3x3(e00 * n, e01 * n, e02 * n, e01 * n, e11 * n, e12 * n, e02 * n, e12 * n, e22 * n);
        }
    }
}
        

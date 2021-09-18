using Petera3d;
using Unity.Mathematics;

public class Jacobi
{
    private Matrix3x3 _a;

    public Jacobi(Matrix3x3 a)
    {
        this._a = a;
    }

    //Symmetric Schur Decomposition 2 by 2 Matrix. This function among other helps us in Jacobi rotation algorithm to find sin,cos values.
    private void SymShurDec(int p, int q, out float sin, out float cos)
    {
        if (math.abs(_a[p, q]) > 0.0001f)
        {
            float r = (_a[q, q] - _a[p, p]) / 2 * _a[p, q]; //Get theta
            //Get t (tan)
            float t = r > 0 ? 1f / (r + math.sqrt(1 + r * r)) : 1 / (r - math.sqrt(1 + r * r));
            //Out sin, cos
            cos = 1f / math.sqrt(1 + t * t);
            sin = t * cos;
        }
        else
        {
            sin = 0;
            cos = 1;
        }
    }

    //JacobiTest Rotation Algorithm - Compute eigenvalues & eigenvectors from symetric Matrix
    public void JacobiRotation(out Matrix3x3 eigenVectors, out Matrix3x3 eigenValues)
    {
        Matrix3x3 v = new Matrix3x3().Identity; //This matrix will contain eigenvalues
        const int MAX_ITERATIONS = 1000;
        float prevOff = 0;
        for (int n = 0; n < MAX_ITERATIONS; n++)
        {
            // Get largest off-diagonal absolute value _a[p][q]
            _a.MaxOffDiagonialElementValueAndPosition(out int p, out int q);
            //Run SymShurDec to get sin,cos values for Jacobi matrix
            SymShurDec(p, q, out float sin, out float cos);
            //Create and initialize Jacobi inentity matrix on every iteration and fill it with new sin,cos values from SymShurDec
            Matrix3x3 J = new Matrix3x3().Identity;
            J[p, p] = J[q, q] = cos;
            J[p, q] = sin;
            J[q, p] = -sin;
            //Apply Jacobi matrix to matrix v that will contain eigenvectors (Multiply this matrix with new jacobi computed matrix on every iteration)
            v *= J;
            _a = J.Transpose() * _a * J;
            // Compute F norm of off-diagonal elements 
            float off = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == j) continue;
                    off += _a[i, j] * _a[i, j];
                }
            }

            //Stop iterations when F norm is no longer desreasing and interations > 2
            if (n > 2 && off >= prevOff) break;
            prevOff = off;
        }

        eigenVectors = v;
        eigenValues = _a;
    }
}

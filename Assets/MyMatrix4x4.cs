using System;
using UnityEngine;

public class MyMatrix4x4 : IEquatable<MyMatrix4x4>, IFormattable
{
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MyMatrix4x4)obj);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public float M00;
    public float M01;
    public float M02;
    public float M03;
    public float M10;
    public float M11;
    public float M12;
    public float M13;
    public float M20;
    public float M21;
    public float M22;
    public float M23;
    public float M30;
    public float M31;
    public float M32;
    public float M33;

    public MyMatrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
    {
        M00 = column0[0];
        M10 = column0[1];
        M20 = column0[2];
        M30 = column0[3];

        M01 = column1[0];
        M11 = column1[1];
        M21 = column1[2];
        M31 = column1[3];

        M02 = column2[0];
        M12 = column2[1];
        M22 = column2[2];
        M32 = column2[3];

        M03 = column3[0];
        M13 = column3[1];
        M23 = column3[2];
        M33 = column3[3];
    }

    public MyMatrix4x4()
    {
        M00 = 0;
        M10 = 0;
        M20 = 0;
        M30 = 0;

        M01 = 0;
        M11 = 0;
        M21 = 0;
        M31 = 0;

        M02 = 0;
        M12 = 0;
        M22 = 0;
        M32 = 0;

        M03 = 0;
        M13 = 0;
        M23 = 0;
        M33 = 0;
    }

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0: return M00;
                case 1: return M10;
                case 2: return M20;
                case 3: return M30;
                case 4: return M01;
                case 5: return M11;
                case 6: return M21;
                case 7: return M31;
                case 8: return M02;
                case 9: return M12;
                case 10: return M22;
                case 11: return M32;
                case 12: return M03;
                case 13: return M13;
                case 14: return M23;
                case 15: return M33;
                default:
                    throw new IndexOutOfRangeException("Invalid matrix index!");
            }
        }

        set
        {
            switch (index)
            {
                case 0:
                    M00 = value;
                    break;
                case 1:
                    M10 = value;
                    break;
                case 2:
                    M20 = value;
                    break;
                case 3:
                    M30 = value;
                    break;
                case 4:
                    M01 = value;
                    break;
                case 5:
                    M11 = value;
                    break;
                case 6:
                    M21 = value;
                    break;
                case 7:
                    M31 = value;
                    break;
                case 8:
                    M02 = value;
                    break;
                case 9:
                    M12 = value;
                    break;
                case 10:
                    M22 = value;
                    break;
                case 11:
                    M32 = value;
                    break;
                case 12:
                    M03 = value;
                    break;
                case 13:
                    M13 = value;
                    break;
                case 14:
                    M23 = value;
                    break;
                case 15:
                    M33 = value;
                    break;

                default:
                    throw new IndexOutOfRangeException("Invalid matrix index!");
            }
        }
    }

    public float this[int row, int column]
    {
        get { return this[row + column * 4]; }

        set { this[row + column * 4] = value; }
    }

    public static MyMatrix4x4 Zero
    {
        get
        {
            return new MyMatrix4x4(
                new Vector4(0, 0, 0, 0),
                new Vector4(0, 0, 0, 0),
                new Vector4(0, 0, 0, 0),
                new Vector4(0, 0, 0, 0)
            );
        }
    }

    public static MyMatrix4x4 Identity
    {
        get
        {
            return new MyMatrix4x4(
                new Vector4(1, 0, 0, 0),
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(0, 0, 0, 1)
            );
        }
    }

    public MyQuaternion Rotation
    {
        get
        {
            MyMatrix4x4 m = this;
            MyQuaternion q = MyQuaternion.Identity;

            //Toma la diagonal (m00, m11, m22) que es la escala y en base a eso aplica en cada uno de los ejes su respectiva escala
            //Toma en cuenta la escala para poder ajustar bien los valores ya que la escala y rotacion comparten valores

            q.W = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) * 0.5f; //Devuelve la raiz de un número que debe ser al menos 0.
            q.X = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) * 0.5f;
            q.Y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) * 0.5f;
            q.Z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) * 0.5f;

            q.X *= Mathf.Sign(q.X * (m[2, 1] - m[1, 2]));
            q.Y *= Mathf.Sign(q.Y * (m[0, 2] - m[2, 0])); //Tiene en cuenta los senos de cada eje para saber que signo deben tener
            q.Z *= Mathf.Sign(q.Z * (m[1, 0] - m[0, 1]));

            return q;
        }
    }

    public Vector3 LossyScale =>
        new(
            new Vector3(M00, M10, M20).magnitude,
            new Vector3(M01, M11, M21).magnitude,
            new Vector3(M02, M12, M22).magnitude
        );

    public bool IsIdentity
    {
        get
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i == j && Mathf.Approximately(this[i, j], 1))
                    {
                        return false;
                    }
                    else if (this[i, j] != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
    
    public float determinant =>
        M03 * M12 * M21 * M30 - M02 * M13 * M21 * M30 -
        M03 * M11 * M22 * M30 + M01 * M13 * M22 * M30 +
        M02 * M11 * M23 * M30 - M01 * M12 * M23 * M30 -
        M03 * M12 * M20 * M31 + M02 * M13 * M20 * M31 +
        M03 * M10 * M22 * M31 - M00 * M13 * M22 * M31 -
        M02 * M10 * M23 * M31 + M00 * M12 * M23 * M31 +
        M03 * M11 * M20 * M32 - M01 * M13 * M20 * M32 -
        M03 * M10 * M21 * M32 + M00 * M13 * M21 * M32 +
        M01 * M10 * M23 * M32 - M00 * M11 * M23 * M32 -
        M02 * M11 * M20 * M33 + M01 * M12 * M20 * M33 +
        M02 * M10 * M21 * M33 - M00 * M12 * M21 * M33 -
        M01 * M10 * M22 * M33 + M00 * M11 * M22 * M33;


    public MyMatrix4x4 transpose =>
        new(
            new Vector4(M00, M10, M20, M30),
            new Vector4(M01, M11, M21, M31),
            new Vector4(M02, M12, M22, M32),
            new Vector4(M03, M13, M23, M33)
        );

    public MyMatrix4x4 inverse
    {
        get
        {
            float newM00 = M12 * M23 * M31 - M13 * M22 * M31 + M13 * M21 * M32 - M11 * M23 * M32 - M12 * M21 * M33 +
                           M11 * M22 * M33;

            float newM01 = M03 * M22 * M31 - M02 * M23 * M31 - M03 * M21 * M32 + M01 * M23 * M32 + M02 * M21 * M33 -
                           M01 * M22 * M33;

            float newM02 = M02 * M13 * M31 - M03 * M12 * M31 + M03 * M11 * M32 - M01 * M13 * M32 - M02 * M11 * M33 +
                           M01 * M12 * M33;

            float newM03 = M03 * M12 * M21 - M02 * M13 * M21 - M03 * M11 * M22 + M01 * M13 * M22 + M02 * M11 * M23 -
                           M01 * M12 * M23;

            float newM10 = M13 * M22 * M30 - M12 * M23 * M30 - M13 * M20 * M32 + M10 * M23 * M32 + M12 * M20 * M33 -
                           M10 * M22 * M33;

            float newM11 = M02 * M23 * M30 - M03 * M22 * M30 + M03 * M20 * M32 - M00 * M23 * M32 - M02 * M20 * M33 +
                           M00 * M22 * M33;

            float newM12 = M03 * M12 * M30 - M02 * M13 * M30 - M03 * M10 * M32 + M00 * M13 * M32 + M02 * M10 * M33 -
                           M00 * M12 * M33;

            float newM13 = M02 * M13 * M20 - M03 * M12 * M20 + M03 * M10 * M22 - M00 * M13 * M22 - M02 * M10 * M23 +
                           M00 * M12 * M23;

            float newM20 = M11 * M23 * M30 - M13 * M21 * M30 + M13 * M20 * M31 - M10 * M23 * M31 - M11 * M20 * M33 +
                           M10 * M21 * M33;

            float newM21 = M03 * M21 * M30 - M01 * M23 * M30 - M03 * M20 * M31 + M00 * M23 * M31 + M01 * M20 * M33 -
                           M00 * M21 * M33;

            float newM22 = M01 * M13 * M30 - M03 * M11 * M30 + M03 * M10 * M31 - M00 * M13 * M31 - M01 * M10 * M33 +
                           M00 * M11 * M33;

            float newM23 = M03 * M11 * M20 - M01 * M13 * M20 - M03 * M10 * M21 + M00 * M13 * M21 + M01 * M10 * M23 -
                           M00 * M11 * M23;

            float newM30 = M12 * M21 * M30 - M11 * M22 * M30 - M12 * M20 * M31 + M10 * M22 * M31 + M11 * M20 * M32 -
                           M10 * M21 * M32;

            float newM31 = M01 * M22 * M30 - M02 * M21 * M30 + M02 * M20 * M31 - M00 * M22 * M31 - M01 * M20 * M32 +
                           M00 * M21 * M32;

            float newM32 = M02 * M11 * M30 - M01 * M12 * M30 - M02 * M10 * M31 + M00 * M12 * M31 + M01 * M10 * M32 -
                           M00 * M11 * M32;

            float newM33 = M01 * M12 * M20 - M02 * M11 * M20 + M02 * M10 * M21 - M00 * M12 * M21 - M01 * M10 * M22 +
                           M00 * M11 * M22;

            return new MyMatrix4x4(
                new Vector4(newM00, newM01, newM02, newM03),
                new Vector4(newM10, newM11, newM12, newM13),
                new Vector4(newM20, newM21, newM22, newM23),
                new Vector4(newM30, newM31, newM32, newM33)
            );
        }
    }

    public static float Determinant(MyMatrix4x4 m)
    {
        return m.determinant;
    }

    public static MyMatrix4x4 Inverse(MyMatrix4x4 m)
    {
        return m.inverse;
    }

    public static MyMatrix4x4 LookAt(Vector3 from, Vector3 to, Vector3 up)
    {
        return TRS(from, MyQuaternion.LookRotation(to - from, up), new Vector3(1f, 1f, 1f));
    }

    public static MyMatrix4x4 Rotate(MyQuaternion q)
    {
        float x = q.X * 2.0F;
        float y = q.Y * 2.0F;
        float z = q.Z * 2.0F;
        float xx = q.X * x;
        float yy = q.Y * y;
        float zz = q.Z * z;
        float xy = q.X * y;
        float xz = q.X * z;
        float yz = q.Y * z;
        float wx = q.W * x;
        float wy = q.W * y;
        float wz = q.W * z;

        MyMatrix4x4 resultMatrix = new MyMatrix4x4();
        resultMatrix.M00 = 1.0f - (yy + zz);
        resultMatrix.M10 = xy + wz;
        resultMatrix.M20 = xz - wy;
        resultMatrix.M30 = 0.0F;
        resultMatrix.M01 = xy - wz;
        resultMatrix.M11 = 1.0f - (xx + zz);
        resultMatrix.M21 = yz + wx;
        resultMatrix.M31 = 0.0F;
        resultMatrix.M02 = xz + wy;
        resultMatrix.M12 = yz - wx;
        resultMatrix.M22 = 1.0f - (xx + yy);
        resultMatrix.M32 = 0.0F;
        resultMatrix.M03 = 0.0F;
        resultMatrix.M13 = 0.0F;
        resultMatrix.M23 = 0.0F;
        resultMatrix.M33 = 1.0F;
        return resultMatrix;
    }

    public static MyMatrix4x4 Scale(Vector3 vector)
    {
        return new MyMatrix4x4(
            new Vector4(vector.x, 0, 0, 0),
            new Vector4(0, vector.y, 0, 0),
            new Vector4(0, 0, vector.z, 0),
            new Vector4(0, 0, 0, 1)
        );
    }

    public static MyMatrix4x4 Translate(Vector3 vector)
    {
        return new MyMatrix4x4(
            new Vector4(1, 0, 0, vector.x),
            new Vector4(0, 1, 0, vector.y),
            new Vector4(0, 0, 1, vector.z),
            new Vector4(0, 0, 0, 1)
        );
    }

    public static MyMatrix4x4 Transpose(MyMatrix4x4 m)
    {
        return m.transpose;
    }

    public static MyMatrix4x4 TRS(Vector3 pos, MyQuaternion q, Vector3 s)
    {
        return Translate(pos) * Rotate(q) * Scale(s);
    }

    public bool Equals(MyMatrix4x4 other)
    {
        return other != null && Mathf.Approximately(M00, other.M00) &&
               Mathf.Approximately(M01, other.M01) &&
               Mathf.Approximately(M02, other.M02) &&
               Mathf.Approximately(M03, other.M03) &&
               Mathf.Approximately(M10, other.M10) &&
               Mathf.Approximately(M11, other.M11) &&
               Mathf.Approximately(M12, other.M12) &&
               Mathf.Approximately(M13, other.M13) &&
               Mathf.Approximately(M20, other.M20) &&
               Mathf.Approximately(M21, other.M21) &&
               Mathf.Approximately(M22, other.M22) &&
               Mathf.Approximately(M23, other.M23) &&
               Mathf.Approximately(M30, other.M30) &&
               Mathf.Approximately(M31, other.M31) &&
               Mathf.Approximately(M32, other.M32) &&
               Mathf.Approximately(M33, other.M33);
    }

    public Vector4 GetColumn(int index)
    {
        switch (index)
        {
            case 0:
                return new Vector4(M00, M10, M20, M30);
            case 1:
                return new Vector4(M01, M11, M21, M31);
            case 2:
                return new Vector4(M02, M12, M22, M32);
            case 3:
                return new Vector4(M03, M13, M23, M33);
            default:
                throw new IndexOutOfRangeException("Not in range!");
        }
    }

    public Vector3 GetPosition()
    {
        return new Vector3(M03, M13, M23);
    }

    public Vector4 GetRow(int index)
    {
        switch (index)
        {
            case 0:
                return new Vector4(M00, M01, M02, M03);
            case 1:
                return new Vector4(M10, M11, M12, M13);
            case 2:
                return new Vector4(M20, M21, M22, M23);
            case 3:
                return new Vector4(M30, M31, M32, M33);
            default:
                throw new IndexOutOfRangeException("Not in range!");
        }
    }

    public Vector3 MultiplyPoint(Vector3 point)
    {
        Vector3 vector3 = MultiplyPoint3x4(point);

        float num = 1f / ((float)((double)this.M30 * (double)point.x + (double)this.M31 * (double)point.y +
                                  (double)this.M32 * (double)point.z) + this.M33);
        vector3.x *= num;
        vector3.y *= num;
        vector3.z *= num;

        return vector3;
    }

    public Vector3 MultiplyPoint3x4(Vector3 point)
    {
        return new Vector3(
            (this.M00 * point.x + this.M01 * point.y + this.M02 * point.z) + this.M03,
            (this.M10 * point.x + this.M11 * point.y + this.M12 * point.z) + this.M13,
            (this.M20 * point.x + this.M21 * point.y + this.M22 * point.z) + this.M23
        );
    }

    public Vector3 MultiplyVector(Vector3 vector)
    {
        return new Vector3(
            (this.M00 * vector.x + this.M01 * vector.y + this.M02 * vector.z),
            (this.M10 * vector.x + this.M11 * vector.y + this.M12 * vector.z),
            (this.M20 * vector.x + this.M21 * vector.y + this.M22 * vector.z)
        );
    }

    public void SetColumn(int index, Vector4 column)
    {
        switch (index)
        {
            case 0:
                M00 = column.x;
                M10 = column.y;
                M20 = column.z;
                M30 = column.w;
                break;
            case 1:
                M01 = column.x;
                M11 = column.y;
                M21 = column.z;
                M31 = column.w;
                break;
            case 2:
                M02 = column.x;
                M12 = column.y;
                M22 = column.z;
                M32 = column.w;
                break;
            case 3:
                M03 = column.x;
                M13 = column.y;
                M23 = column.z;
                M33 = column.w;
                break;
            default:
                throw new IndexOutOfRangeException("Not in range!");
        }
    }

    public void SetRow(int index, Vector4 row)
    {
        switch (index)
        {
            case 0:
                M00 = row.x;
                M01 = row.y;
                M02 = row.z;
                M03 = row.w;
                break;
            case 1:
                M10 = row.x;
                M11 = row.y;
                M12 = row.z;
                M13 = row.w;
                break;
            case 2:
                M20 = row.x;
                M21 = row.y;
                M22 = row.z;
                M23 = row.w;
                break;
            case 3:
                M30 = row.x;
                M31 = row.y;
                M32 = row.z;
                M33 = row.w;
                break;
            default:
                throw new IndexOutOfRangeException("Not in range!");
        }
    }

    public void SetTRS(Vector3 pos, MyQuaternion q, Vector3 s)
    {
        MyMatrix4x4 trsMatrix = TRS(pos, q, s);

        for (int i = 0; i < 4; i++)
        {
            Vector4 row = new Vector4(trsMatrix[i, 0], trsMatrix[i, 1], trsMatrix[i, 2], trsMatrix[i, 3]);
            SetRow(i, row);
        }
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"{M00} {M01} {M02} {M03}\n {M10} {M11} {M12} {M13}\n {M20} {M21} {M22} {M23}\n {M30} {M31} {M32} {M33}";
    }

    public bool ValidTRS()
    {
        return Vector3.Dot(new Vector3(M00, M10, M20), new Vector3(M01, M11, M21)) <= float.Epsilon &&
               Vector3.Dot(new Vector3(M01, M11, M21), new Vector3(M02, M12, M22)) <= float.Epsilon &&
               Vector3.Dot(new Vector3(M00, M10, M20), new Vector3(M02, M12, M22)) <= float.Epsilon;
    }

    public static MyMatrix4x4 operator *(MyMatrix4x4 a, MyMatrix4x4 b)
    {
        return new MyMatrix4x4(
            a * b.GetColumn(0),
            a * b.GetColumn(1),
            a * b.GetColumn(2),
            a * b.GetColumn(3)
        );
    }

    public static bool operator ==(MyMatrix4x4 a, MyMatrix4x4 b)
    {
        return a != null && a.Equals(b);
    }

    public static bool operator !=(MyMatrix4x4 a, MyMatrix4x4 b)
    {
        return !(a == b);
    }

    public static Vector4 operator *(MyMatrix4x4 a, Vector4 vector)
    {
        return new Vector4(
            a.M00 * vector.x + a.M01 * vector.y + a.M02 * vector.z + a.M03 * vector.w,
            a.M10 * vector.x + a.M11 * vector.y + a.M12 * vector.z + a.M13 * vector.w,
            a.M20 * vector.x + a.M21 * vector.y + a.M22 * vector.z + a.M23 * vector.w,
            a.M30 * vector.x + a.M31 * vector.y + a.M32 * vector.z + a.M33 * vector.w
        );
    }
}
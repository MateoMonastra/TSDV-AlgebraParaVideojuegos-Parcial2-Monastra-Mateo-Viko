using System;
using UnityEngine;

public class MyQuaternion : IEquatable<MyQuaternion>, IFormattable
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    private static MyQuaternion _identityQuaternion = new MyQuaternion(0.0f, 0.0f, 0.0f, 1f);
    public const float Epsilon = 1E-06f;

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return this.X;
                case 1:
                    return this.Y;
                case 2:
                    return this.Z;
                case 3:
                    return this.W;
                default:
                    throw new IndexOutOfRangeException("Invalid Quaternion index!");
            }
        }
        set
        {
            switch (index)
            {
                case 0:
                    this.X = value;
                    break;
                case 1:
                    this.Y = value;
                    break;
                case 2:
                    this.Z = value;
                    break;
                case 3:
                    this.W = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Invalid Quaternion index!");
            }
        }
    }

    public MyQuaternion(float x, float y, float z, float w)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
        this.W = w;
    }

    public void Set(float newX, float newY, float newZ, float newW)
    {
        this.X = newX;
        this.Y = newY;
        this.Z = newZ;
        this.W = newW;
    }
    
    private Vector3 NormalizeAngles(Vector3 someEulerAngles)
    {
        return new Vector3(NormalizeAngle(someEulerAngles.x), NormalizeAngle(someEulerAngles.y),
            NormalizeAngle(someEulerAngles.z));
    }

    private float NormalizeAngle(float angle)
    {
        float newAngle = angle;
        while (newAngle > 360.0f)
        {
            newAngle -= 360.0f;
        }

        while (newAngle < 0.0f)
        {
            newAngle += 360.0f;
        }

        return newAngle;
    }

    public static MyQuaternion Identity
    {
        get => _identityQuaternion;
    }

    public MyQuaternion Normalized
    {
        get => Normalize(this);
    }

    public static MyQuaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
        Vector3 axis = Vector3.Cross(fromDirection, toDirection);
        
        float angle = Vector3.Angle(fromDirection, toDirection);
        
        return AngleAxis(angle, axis);
    }

    public static MyQuaternion Slerp(MyQuaternion a, MyQuaternion b, float t)
    {
        return SlerpUnclamped(a, b, t < 0 ? 0 : (t > 1 ? 1 : t));
    }

    public static MyQuaternion SlerpUnclamped(MyQuaternion a, MyQuaternion b, float t)
    {
        MyQuaternion normA = a.Normalized;
        MyQuaternion normB = b.Normalized;

        float cosOmega = Dot(normA, normB);

        if (cosOmega < 0.0f)
        {
            cosOmega = -cosOmega;
        }

        float coeff1, coeff2;

        float omega = Mathf.Acos(cosOmega);

        coeff1 = Mathf.Sin((1 - t) * omega) / Mathf.Sin(omega);
        coeff2 = (cosOmega < 0.0f ? -1 : 1) * (Mathf.Sin(t * omega) / Mathf.Sin(omega));

        return new MyQuaternion(
            coeff1 * normA.X + coeff2 * normB.X,
            coeff1 * normA.Y + coeff2 * normB.Y,
            coeff1 * normA.Z + coeff2 * normB.Z,
            coeff1 * normA.W + coeff2 * normB.W
        );
    }

    public static MyQuaternion Lerp(MyQuaternion a, MyQuaternion b, float t)
    {
        return LerpUnclamped(a, b, t < 0 ? 0 : (t > 1 ? 1 : t));
    }

    public static MyQuaternion LerpUnclamped(MyQuaternion a, MyQuaternion b, float t)
    {
        MyQuaternion result = Identity;

        if(Dot(a, b) >= float.Epsilon)
        {
            result.X = a.X + (b.X - a.X) * t;
            result.Y = a.Y + (b.Y - a.Y) * t;
            result.Z = a.Z + (b.Z - a.Z) * t;
            result.W = a.W + (b.W - a.W) * t;
        } else
        {
            result.X = a.X - (b.X - a.X) * t;
            result.Y = a.Y - (b.Y - a.Y) * t;
            result.Z = a.Z - (b.Z - a.Z) * t;
            result.W = a.W - (b.W - a.W) * t;
        }

        return result;
    }

    public static MyQuaternion AngleAxis(float angle, Vector3 axis)
    {
        Vector3 normalizedAxis = axis.normalized;
        
        normalizedAxis *= Mathf.Sin(angle * Mathf.Deg2Rad * 0.5f);

        return new MyQuaternion(normalizedAxis.x, normalizedAxis.y, normalizedAxis.z, Mathf.Cos(angle * Mathf.Deg2Rad * 0.5f));
    }

    public static MyQuaternion LookRotation(Vector3 forward, Vector3 upwards)
    {
            if (forward.magnitude <= Epsilon) return Identity;
            
            Vector3 forwardToUse = forward.normalized;
            Vector3 rightToUse = Vector3.Cross(upwards, forward).normalized;
            Vector3 upToUse = upwards.normalized;
            
            
            float m00 = rightToUse.x;
            float m01 = rightToUse.y;
            float m02 = rightToUse.z;

            float m10 = upToUse.x;
            float m11 = upToUse.y;
            float m12 = upToUse.z;

            float m20 = forwardToUse.x;
            float m21 = forwardToUse.y;
            float m22 = forwardToUse.z;
            
            MyQuaternion result;
            float factor;
            
            if (m22 < 0)
            {
                if (m00 > m11)
                {
                    factor = 1 + m00 - m11 - m22;
                    
                    result = new MyQuaternion(factor, m10 + m01, m20 + m02, m12 - m21);
                }
                else
                {
                    factor = 1 - m00 + m11 - m22;

                    result = new MyQuaternion(m01 + m10, factor, m12 + m21, m20 - m02);
                }
            }
            else
            {
                if (m00 < -m11)
                {
                    factor = 1 - m00 - m11 + m22;

                    result = new MyQuaternion(m20 + m02, m12 + m21, factor, m01 - m10);
                }
                else
                {
                    factor = 1 + m00 + m11 + m22;

                    result = new MyQuaternion(m12 - m21, m20 - m02, m01 - m10, factor);
                }
            }

            result *= 0.5f / Mathf.Sqrt(factor);

            return result;
    }

    public static MyQuaternion LookRotation(Vector3 forward)
    {
        return LookRotation(forward, Vector3.up);
    }

    public void SetLookRotation(Vector3 view, Vector3 up)
    {
        MyQuaternion lookRotationQuaternion = LookRotation(view, up);

        this.X = lookRotationQuaternion.X;
        this.Y = lookRotationQuaternion.Y;
        this.Z = lookRotationQuaternion.Z;
    }

    public void SetLookRotation(Vector3 view)
    {
        Vector3 up = Vector3.up;
        SetLookRotation(view, up);
    }

    public static float Angle(MyQuaternion a, MyQuaternion b)
    {
        float dotValue = Dot(a.Normalized, b.Normalized);
        float dotAbsValue = Mathf.Abs(dotValue);
        
        return IsEqualUsingDot(dotValue) ? 0.0f : Mathf.Acos(dotAbsValue) * 2.0f * Mathf.Rad2Deg; 
    }

    public static bool IsEqualUsingDot(float dotValue)
    {
        return dotValue > 1 - float.Epsilon && dotValue < 1 + float.Epsilon;
    }

    public static MyQuaternion Euler(float x, float y, float z)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion Euler(Vector3 euler)
    {
        throw new NotImplementedException();
    }

    public void ToAngleAxis(out float angle, out Vector3 axis)
    {
        throw new NotImplementedException();
    }

    public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
        MyQuaternion newQuaternion = FromToRotation(fromDirection, toDirection).Normalized;

        this.X = newQuaternion.X;
        this.Y = newQuaternion.Y;
        this.Z = newQuaternion.Z;
        this.W = newQuaternion.W;
    }

    public static MyQuaternion RotateTowards(MyQuaternion from, MyQuaternion to, float maxDegreesDelta)
    {
        float angle = Angle(from, to);
        if (angle == 0.0f) return to;
        return SlerpUnclamped(from, to, Mathf.Min(1.0f, maxDegreesDelta / angle));
    }

    public float SquaredMagnitude()
    {
        return this.X * this.X + this.Y * this.Y +
               this.Z * this.Z + this.W * this.W;
    }

    public static MyQuaternion Conjugated(MyQuaternion q)
    {
        return new MyQuaternion(-q.X, -q.Y, -q.Z, q.W);
    }

    public static MyQuaternion Inverse(MyQuaternion q)
    {
        return Conjugated(q) / q.SquaredMagnitude();
    }

    public static MyQuaternion Normalize(MyQuaternion q)
    {
        float mag = Mathf.Sqrt(Dot(q, q));

        if (mag < Mathf.Epsilon)
            return Identity;

        return new MyQuaternion(q.X / mag, q.Y / mag, q.Z / mag, q.W / mag);
    }

    public static float Dot(MyQuaternion a, MyQuaternion b)
    {
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
    }

    public static MyQuaternion operator *(MyQuaternion a, MyQuaternion b)
    {
        return new MyQuaternion(
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
            a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X,
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z);
    }

    public static Vector3 operator *(MyQuaternion rotation, Vector3 point)
    {
        MyQuaternion pureVectorQuaternion = new MyQuaternion(point.x, point.y, point.z, 0);
        MyQuaternion appliedPureQuaternion = rotation * pureVectorQuaternion * Conjugated(rotation);

        return new Vector3(appliedPureQuaternion.X, appliedPureQuaternion.Y, appliedPureQuaternion.Z);
    }

    public static MyQuaternion operator *(MyQuaternion q, float value)
    {
        return new MyQuaternion(
            q.X * value,
            q.Y * value,
            q.Z * value,
            q.W * value
        );
    }
    public static bool operator ==(MyQuaternion a, MyQuaternion b
    )
    {
        return Dot(a, b) > 1 - Epsilon;
    }

    public static MyQuaternion operator /(MyQuaternion q, float value)
    {
        return new MyQuaternion(
            q.X / value,
            q.Y / value,
            q.Z / value,
            q.W / value
        );
    }

    public static bool operator !=(MyQuaternion a, MyQuaternion b
    )
    {
        return !(a == b);
    }
    
    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"( X = ${this.X}, Y = ${this.Y}, Z = ${this.Z}, W = ${this.W})";
    }

    public bool Equals(MyQuaternion other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && W.Equals(other.W);
    }
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((MyQuaternion)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z, W);
    }
}


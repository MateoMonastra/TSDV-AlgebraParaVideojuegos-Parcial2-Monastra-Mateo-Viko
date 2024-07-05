using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyQuaternion : IEquatable<MyQuaternion>, IFormattable
{
    public float x;
    public float y;
    public float z;
    public float w;

    private static MyQuaternion _identityQuaternion = new MyQuaternion(0.0f, 0.0f, 0.0f, 1f);
    public const float Epsilon = 1E-06f;

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return this.x;
                case 1:
                    return this.y;
                case 2:
                    return this.z;
                case 3:
                    return this.w;
                default:
                    throw new IndexOutOfRangeException("Invalid Quaternion index!");
            }
        }
        set
        {
            switch (index)
            {
                case 0:
                    this.x = value;
                    break;
                case 1:
                    this.y = value;
                    break;
                case 2:
                    this.z = value;
                    break;
                case 3:
                    this.w = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Invalid Quaternion index!");
            }
        }
    }

    public MyQuaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public void Set(float newX, float newY, float newZ, float newW)
    {
        this.x = newX;
        this.y = newY;
        this.z = newZ;
        this.w = newW;
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
        get => MyQuaternion._identityQuaternion;
    }

    public MyQuaternion Normalized
    {
        get => MyQuaternion.Normalize(this);
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
            coeff1 * normA.x + coeff2 * normB.x,
            coeff1 * normA.y + coeff2 * normB.y,
            coeff1 * normA.z + coeff2 * normB.z,
            coeff1 * normA.w + coeff2 * normB.w
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
            result.x = a.x + (b.x - a.x) * t;
            result.y = a.y + (b.y - a.y) * t;
            result.z = a.z + (b.z - a.z) * t;
            result.w = a.w + (b.w - a.w) * t;
        } else
        {
            result.x = a.x - (b.x - a.x) * t;
            result.y = a.y - (b.y - a.y) * t;
            result.z = a.z - (b.z - a.z) * t;
            result.w = a.w - (b.w - a.w) * t;
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
        throw new NotImplementedException();
    }

    public static MyQuaternion LookRotation(Vector3 forward)
    {
        throw new NotImplementedException();
    }

    public void SetLookRotation(Vector3 view, Vector3 up)
    {
        this = LookRotation(view, up);
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
        this = FromToRotation(fromDirection, toDirection);
    }

    public static MyQuaternion RotateTowards(MyQuaternion from, MyQuaternion to, float maxDegreesDelta)
    {
        float angle = MyQuaternion.Angle(from, to);
        if (angle == 0.0f) return to;
        return SlerpUnclamped(from, to, Mathf.Min(1.0f, maxDegreesDelta / angle));
    }

    public float SquaredMagnitude()
    {
        return this.x * this.x + this.y * this.y +
               this.z * this.z + this.w * this.w;
    }

    public static MyQuaternion Conjugated(MyQuaternion q)
    {
        return new MyQuaternion(-q.x, -q.y, -q.z, q.w);
    }

    public static MyQuaternion Inverse(MyQuaternion q)
    {
        return MyQuaternion.Conjugated(q) / q.SquaredMagnitude();
    }

    public static MyQuaternion Normalize(MyQuaternion q)
    {
        float mag = Mathf.Sqrt(Dot(q, q));

        if (mag < Mathf.Epsilon)
            return MyQuaternion.Identity;

        return new MyQuaternion(q.x / mag, q.y / mag, q.z / mag, q.w / mag);
    }

    public static float Dot(MyQuaternion a, MyQuaternion b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
    }

    public static MyQuaternion operator *(MyQuaternion a, MyQuaternion b)
    {
        return new MyQuaternion(
            a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y,
            a.w * b.y + a.y * b.w + a.z * b.x - a.x * b.z,
            a.w * b.z + a.z * b.w + a.x * b.y - a.y * b.x,
            a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z);
    }

    public static Vector3 operator *(MyQuaternion rotation, Vector3 point)
    {
        MyQuaternion pureVectorQuaternion = new MyQuaternion(point.x, point.y, point.z, 0);
        MyQuaternion appliedPureQuaternion = rotation * pureVectorQuaternion * MyQuaternion.Conjugated(rotation);

        return new Vector3(appliedPureQuaternion.x, appliedPureQuaternion.y, appliedPureQuaternion.z);
    }

    public static bool operator ==(MyQuaternion a, MyQuaternion b
    )
    {
        return MyQuaternion.Dot(a, b) > 1 - Epsilon;
    }

    public static MyQuaternion operator /(MyQuaternion q, float value)
    {
        return new MyQuaternion(
            q.x / value,
            q.y / value,
            q.z / value,
            q.w / value
        );
    }

    public static bool operator !=(MyQuaternion a, MyQuaternion b
    )
    {
        return !(a == b);
    }
    
    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"( X = ${this.x}, Y = ${this.y}, Z = ${this.z}, W = ${this.w})";
    }

    public bool Equals(MyQuaternion other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z, w);
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyQuaternion : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public float w;

    private static readonly MyQuaternion IdentityQuaternion = new MyQuaternion(0.0f, 0.0f, 0.0f, 1f);
    public const float KEpsilon = 1E-06f;

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
        throw new NotImplementedException();
    }

    private float NormalizeAngle(float angle)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion Identity
    {
        get => MyQuaternion.IdentityQuaternion;
    }

    public MyQuaternion normalized
    {
        get => MyQuaternion.Normalize(this);
    }

    public static MyQuaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion Slerp(MyQuaternion a, MyQuaternion b, float t)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion SlerpUnclamped(MyQuaternion a, MyQuaternion b, float t)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion Lerp(MyQuaternion a, MyQuaternion b, float t)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion LerpUnclamped(MyQuaternion a, MyQuaternion b, float t)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion AngleAxis(float angle, Vector3 axis)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public void SetLookRotation(Vector3 view)
    {
        throw new NotImplementedException();
    }

    public static float Angle(MyQuaternion a, MyQuaternion b)
    {
        throw new NotImplementedException();
    }

    public static bool IsEqualUsingDot(float dotValue)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public static MyQuaternion RotateTowards(MyQuaternion from, MyQuaternion to, float maxDegreesDelta)
    {
        throw new NotImplementedException();
    }

    public float SquaredMagnitude()
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion Conjugated(MyQuaternion q)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion Inverse(MyQuaternion q)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion Normalize(MyQuaternion q)
    {
        throw new NotImplementedException();
    }

    public static float Dot(MyQuaternion lhs, MyQuaternion rhs)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion operator *(MyQuaternion lhs, MyQuaternion rhs)
    {
        throw new NotImplementedException();
    }

    public static Vector3 operator *(MyQuaternion rotation, Vector3 point)
    {
        throw new NotImplementedException();
    }

    public static bool operator ==(MyQuaternion lhs, MyQuaternion rhs)
    {
        throw new NotImplementedException();
    }

    public static MyQuaternion operator /(MyQuaternion q, float value)
    {
        throw new NotImplementedException();
    }

    public static bool operator !=(MyQuaternion lhs, MyQuaternion rhs)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object other)
    {
        throw new NotImplementedException();
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"( X = ${this.x}, Y = ${this.y}, Z = ${this.z}, W = ${this.w})";
    }

    public bool Equals(MyQuaternion other)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z, w);
    }
}


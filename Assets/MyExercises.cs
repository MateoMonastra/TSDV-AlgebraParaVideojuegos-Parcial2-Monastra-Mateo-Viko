using System.Collections.Generic;
using MathDebbuger.Debuggers;
using UnityEngine;

public enum QuatExercices
{
    One = 1,
    Two = 2,
    Three = 3,
}

public class MyExercises : MonoBehaviour
{
    [SerializeField] private QuatExercices exercise;

    private Vector3 _a = new Vector3();
    private Vector3 _b = new Vector3();
    private Vector3 _c = new Vector3();
    private Vector3 _d = new Vector3();

    private int _lastUsed;


    [SerializeField] private float angle;

    private List<string> _vectorsId = new List<string>();


    void Start()
    {
        _vectorsId.Add("Vec A");
        _vectorsId.Add("Vec B");
        _vectorsId.Add("Vec C");
        _vectorsId.Add("Vec D");

        Vector3Debugger.AddVector(_a, Color.black, _vectorsId[0]);
        Vector3Debugger.AddVector(_b, Color.black, _vectorsId[1]);
        Vector3Debugger.AddVector(_b, _c, Color.black, _vectorsId[2]);
        Vector3Debugger.AddVector(_c, _d, Color.black, _vectorsId[3]);

        SetInitValues();

        _lastUsed = (int)exercise;
    }

    void FixedUpdate()
    {
        UpdateExercises();
    }

    private void Exercise1()
    {
        TurnOfVectors();
        TurnOnVectors(1);

        MyQuaternion rotation = MyQuaternion.AngleAxis(angle, Vector3.up);
        _a = rotation * _a;
    }

    private void Exercise2()
    {
        TurnOfVectors();
        TurnOnVectors(3);

        MyQuaternion rotation = MyQuaternion.AngleAxis(angle, Vector3.up);
        _a = rotation * _a;
        _b = rotation * _b;
        _c = rotation * _c;
    }

    private void Exercise3()
    {
        MyQuaternion rotation = MyQuaternion.AngleAxis(angle, _b);

        _a = rotation * _a;

        rotation = MyQuaternion.Inverse(MyQuaternion.AngleAxis(angle, _b));

        _c = rotation * _c;
    }

    private void UpdateExercises()
    {
        if (_lastUsed != (int)exercise)
        {
            SetInitValues();
            _lastUsed = (int)exercise;
        }

        switch ((int)exercise)
        {
            case 1:
                {
                    Exercise1();
                    break;
                }
            case 2:
                {
                    Exercise2();
                    break;
                }
            case 3:
                {
                    Exercise3();
                    break;
                }
        }

        UpdateVectors();
    }

    private void SetInitValues()
    {
        _a = Vector3.zero;
        _b = Vector3.zero;
        _c = Vector3.zero;
        _d = Vector3.zero;

        _a.x = 10;
        _b.x = 10;
        _b.y = 10;
        _c.x = 20;
        _c.y = 10;
        _d.x = 20;
        _d.y = 20;

    }

    private void TurnOnVectors(int qnty)
    {
        for (int i = 0; i < qnty; i ++)
        {
            Vector3Debugger.TurnOnVector(_vectorsId[i]);
        }
    }

    private void UpdateVectors()
    {
        Vector3Debugger.UpdatePosition(_vectorsId[0], _a);
        Vector3Debugger.UpdatePosition(_vectorsId[1], _a, _b);
        Vector3Debugger.UpdatePosition(_vectorsId[2], _b, _c);
        Vector3Debugger.UpdatePosition(_vectorsId[3], _c, _d);
    }

    private void TurnOfVectors()
    {
        foreach (string id in _vectorsId)
        {
            Vector3Debugger.TurnOffVector(id);
        }
    }
}
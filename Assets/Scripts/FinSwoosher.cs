using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinSwoosher : MonoBehaviour //code borrowed from https://forum.unity.com/threads/rotate-object-back-and-forth-x-degrees.375899/
{
    private Vector3 _from = new Vector3(0, 30, 0);
    private Vector3 _to = new Vector3(0, 120, 0);
    private float _frequency = 1;
    
    void Update()
    {
        Quaternion lFrom = Quaternion.Euler(_from);
        Quaternion lTo = Quaternion.Euler(_to);
        float lerp = 0.5f * (1.0f + Mathf.Sin(Mathf.PI * Time.deltaTime) * _frequency);
        
       transform.localRotation = Quaternion.Lerp(lFrom, lTo, lerp);
    }
}

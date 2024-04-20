using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WingsAnim : MonoBehaviour
{
    private GameObject _wing1, _wing2, _wing3, _wing4;//extra wings to give illusion of fast movement
    private Vector3 _basePosition;
    public float speed, rotateSpeed;
    private float _currentLerpTime, _lerpTime;
    public float upAndDown;
    public float upAndDown2;
    public float upAndDownRotate;
    private bool _positive, _positive2;
    void Start()
    {
        _wing1 = transform.GetChild(0).gameObject;
        _wing2 = transform.GetChild(1).gameObject;
        _wing3 = transform.GetChild(2).gameObject;
        _wing4 = transform.GetChild(3).gameObject;
        //_basePosition = transform.position;
        upAndDownRotate += Time.deltaTime * rotateSpeed;
        upAndDown2 = .5f;
        _positive = true;
    }

   
    void Update()
    {
        switch (upAndDown) //I KNOW THESE ARE REALLY STUPID I COULDN'T FIGURE OUT A SINE WAVE
        {
            case >= .5f:
                _positive = false;
                break;
            case <= 0:
                _positive = true;
                break;
        }
        switch (_positive)
        {
            case true:
                upAndDown += Time.deltaTime * speed;
                break;
            case false:
                upAndDown -= Time.deltaTime * speed;
                break;
        }
        
        switch (upAndDown2) //I KNOW THESE ARE REALLY STUPID I COULDN'T FIGURE OUT A SINE WAVE
        {
            case >= .5f:
                _positive2 = false;
                break;
            case <= 0:
                _positive2 = true;
                break;
        }
        switch (_positive2)
        {
            case true:
                upAndDown2 += Time.deltaTime * speed;
                break;
            case false:
                upAndDown2 -= Time.deltaTime * speed;
                break;
        }
        
        switch (upAndDownRotate)
        {
            case >= 20f:
                _positive = false;
                break;
            case <= -20:
                _positive = true;
                break;
        }

        upAndDown = Mathf.Clamp(upAndDown,0, .6f);
        upAndDown2 = Mathf.Clamp(upAndDown2,0, .6f);

        Debug.Log(upAndDownRotate);
        
        _wing1.transform.localPosition = new Vector3(0.75f, upAndDown, 0);
        _wing2.transform.localPosition = new Vector3(-0.75f, upAndDown, 0);
        _wing3.transform.localPosition = new Vector3(0.75f, upAndDown2, 0);
        _wing4.transform.localPosition = new Vector3(-0.75f, upAndDown2, 0);

    }
}

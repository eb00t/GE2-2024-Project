using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EyeHandler : MonoBehaviour
{
    private GameObject _eyeBall, _player;
    public bool end = true;
    public int speed = 1;
    private Animator _eyeAnims;
    private static readonly int Interested = Animator.StringToHash("Interested");

    void Start()
    {
        _eyeBall = GameObject.Find("Eyeball");
        _eyeAnims = GetComponent<Animator>();
        _player = GameObject.FindWithTag("Player");
        _eyeAnims.SetBool(Interested, true);
    }
    
    void FixedUpdate()
    {
        Vector3 direction = _eyeBall.transform.position - _player.transform.position;
        if (end)
        {
            Quaternion ToRotation = Quaternion.LookRotation(direction, Vector3.up);
            _eyeBall.transform.rotation =
                Quaternion.Lerp(_eyeBall.transform.rotation, ToRotation, speed * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenClose : MonoBehaviour
{
    private Animator _animator;
    private bool _openMe;
    private static readonly int Open = Animator.StringToHash("Open");

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenOrClose()
    {
        _openMe = !_openMe;
        _animator.SetBool(Open,_openMe);
    }
}

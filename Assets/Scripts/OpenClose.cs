using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenClose : MonoBehaviour
{
    private Animator _animator;
    private bool _openMe = false;
    private static readonly int Open = Animator.StringToHash("Open");
    private Button _button;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _button = GetComponent<Button>();
    }

    public void OpenOrClose()
    {
        _openMe = !_openMe;
        _animator.SetBool(Open,_openMe);
    }

    public void Close()
    {
        _openMe = false;
        _animator.SetBool(Open, _openMe);
    }

    public void ReactivateButton()
    {
        _button.interactable = true;
        _openMe = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomText : MonoBehaviour
{
    private TMP_Text _tmpText;
    public List<string> _messages;
    public int _message;
    void Start()
    {
        _tmpText = GetComponent<TMP_Text>();
        _messages = new List<string>()
        {
            "X_X",
            "H_LP",
            "_0NE",
            "L0_T",
            "ST_CK",
            "0H-N_",
            "AL0_E",
            "...",
            "0000",
        };
        _message = Random.Range(0, _messages.Count);
        switch (_message)
        {
            case 0:
                _tmpText.fontSize = 4;
                break;
            default:
                _tmpText.fontSize = 3;
                break;
        }
        _tmpText.text = _messages[_message];
    }
    

    private void Update()
    {
        _tmpText.text = _messages[_message];
    }
}

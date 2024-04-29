using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowMyVariable : MonoBehaviour
{
    private GlobalVariables _globalVariables;
    private TMP_Text _text;
    private string _message;
    public enum VariableToShow
    {
        BugCount,
    }
    public VariableToShow variable;
    void Start()
    {
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        _text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (variable)
        {
            case VariableToShow.BugCount:
            _message = _globalVariables.totalBugsAllowed.ToString();
            break;
        }
        _text.text = _message;
    }
}

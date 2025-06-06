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
        BugsAlive,
        OrangeFishCount,
        SchoolFishCount,
        PreyFishAlive,
        PredatorCount,
        PredatorFishAlive,
        SchoolFishLeaderCount
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
            case VariableToShow.BugsAlive:
                _message = _globalVariables.bugCount.ToString();
                break;
            case VariableToShow.OrangeFishCount:
                _message = _globalVariables.totalOrangeFishAllowed.ToString();
                break;
            case VariableToShow.SchoolFishCount:
                _message = _globalVariables.totalSchoolFishAllowed.ToString();
                break;
            case VariableToShow.PreyFishAlive: 
                _message = _globalVariables.preyFishCount.ToString();
                break;
            case VariableToShow.PredatorCount:
                _message = _globalVariables.totalPredatorFishAllowed.ToString();
                break;
            case VariableToShow.PredatorFishAlive:
                _message = _globalVariables.predatorFishCount.ToString();
            break;
            case VariableToShow.SchoolFishLeaderCount:
                _message = _globalVariables.totalSchoolFishLeadersAllowed.ToString();
                break;
        }
        _text.text = _message;
    }
}

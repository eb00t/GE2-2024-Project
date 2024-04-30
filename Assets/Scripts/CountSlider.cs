using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountSlider : MonoBehaviour
{
    private GlobalVariables _globalVariables;
    private Slider _slider;
    public enum ShowWhat
    {
        TotalBugs,
        TotalOrangeFish,
        TotalSchoolFish,
        TotalPredatorFish
    }

    public ShowWhat WhatToShow;
    void Start()
    {
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        _slider = GetComponent<Slider>();
        switch (WhatToShow)
        {
            case ShowWhat.TotalBugs:
                _slider.value = _globalVariables.totalBugsAllowed;
                break;
            case ShowWhat.TotalOrangeFish:
                _slider.value = _globalVariables.totalOrangeFishAllowed;
                break;
            case ShowWhat.TotalPredatorFish:
                _slider.value = _globalVariables.totalPredatorFishAllowed;
                break;
            case ShowWhat.TotalSchoolFish:
                _slider.value = _globalVariables.totalOrangeFishAllowed;
                break;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        switch (WhatToShow)
        {
            case ShowWhat.TotalBugs:
                _globalVariables.totalBugsAllowed = (int)_slider.value;
                break;
            case ShowWhat.TotalOrangeFish:
                _globalVariables.totalOrangeFishAllowed = (int)_slider.value;
                break;
            case ShowWhat.TotalPredatorFish:
                _globalVariables.totalPredatorFishAllowed = (int)_slider.value;
                break;
            case ShowWhat.TotalSchoolFish:
                _globalVariables.totalOrangeFishAllowed = (int)_slider.value;
                break;
        }
        
    }
}

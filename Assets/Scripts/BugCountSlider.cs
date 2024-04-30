using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BugCountSlider : MonoBehaviour
{
    private GlobalVariables _globalVariables;
    private Slider _slider;
    void Start()
    {
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        _slider = GetComponent<Slider>();
        _slider.value = _globalVariables.totalBugsAllowed;
    }

    // Update is called once per frame
    void Update()
    {
        _globalVariables.totalBugsAllowed = (int)_slider.value;
    }
}

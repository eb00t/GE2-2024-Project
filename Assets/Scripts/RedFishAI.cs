using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFishAI : MonoBehaviour
{
    private float _hunger = 100;
    private Pursue _pursue;
    private NoiseWander _noiseWander;
    private GlobalVariables _globalVariables;
    public enum AIStates
    {
        Wandering,
        Chasing
    }

    public AIStates states;
    void Start()
    {
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        _noiseWander = GetComponent<NoiseWander>();
        _pursue = GetComponent<Pursue>();
    }
    
    void Update()
    {
        switch (states)
        {
            case AIStates.Wandering:
                _noiseWander.enabled = true;
                _pursue.enabled = false;
                break;
            case AIStates.Chasing:
                _noiseWander.enabled = false;
                _pursue.enabled = true;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera fishCam;
    private CinemachineVirtualCamera _playerCam;
    private GameObject _fishCamLayer;
    private GlobalVariables _globalVariables;
    void Start()
    {
        fishCam = GameObject.FindWithTag("FishCam").GetComponent<CinemachineVirtualCamera>();
        _fishCamLayer = GameObject.Find("FishCamLayer");

        fishCam.Priority = 1;
        _fishCamLayer.SetActive(false);
    }

    public void PredatorCamActivate()
    {
        fishCam.Priority = 12;
        fishCam.Follow = _globalVariables.allPredatorFish[RngPredators("pred")].gameObject.transform;
        fishCam.LookAt = fishCam.Follow.transform;
    }

    public void PreyCamActivate()
    {
        fishCam.Priority = 12;
        fishCam.Follow = _globalVariables.allPreyFish[RngPredators("prey")].gameObject.transform;
        fishCam.LookAt = fishCam.Follow.transform;
    }

    private int RngPredators(string predOrPrey)
    {
        int genNum = 0;
        switch (predOrPrey)
        {
            case "pred":
              genNum = Random.Range(0, _globalVariables.allPredatorFish.Count);
                break;
            case "prey":
                genNum = Random.Range(0, _globalVariables.allPreyFish.Count);
                break;
        }
        return genNum;
    }
}

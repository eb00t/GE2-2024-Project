using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera fishCam;
    private GameObject _player;
    private GameObject _fishCamLayer;
    private GlobalVariables _globalVariables;
    private int _camNumber;
    private bool _preyCam;
    private GameObject _backToRobotButton;

    void Start()
    {
        fishCam = GameObject.FindWithTag("FishCam").GetComponent<CinemachineVirtualCamera>();
        _fishCamLayer = GameObject.Find("FishCamLayer");
        _player = GameObject.FindWithTag("Player");
        _globalVariables = GameObject.FindWithTag("GlobalVariables").GetComponent<GlobalVariables>();
        _backToRobotButton = GameObject.Find("BackToRobotButton");
        _backToRobotButton.SetActive(false);

        fishCam.Priority = 1;
        _fishCamLayer.SetActive(false);
        _preyCam = false;
        _camNumber = RngFish("pred");
    }

    public void PredatorCamActivate()
    {
        StartCoroutine(SwitchCamPred());
    }

    public void PreyCamActivate()
    {
        StartCoroutine(SwitchCamPrey());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            switch (_preyCam)
            {
                case false:
                    if (_camNumber > _globalVariables.allPredatorFish.Count)
                    {
                        _camNumber = 0;
                    }
                    else if (_camNumber < 0)
                    {
                        _camNumber = _globalVariables.allPredatorFish.Count;
                    }
                    else
                    {
                        _camNumber++;
                    }

                    break;
                case true:
                    if (_camNumber > _globalVariables.allPreyFish.Count)
                    {
                        _camNumber = 0;
                    }
                    else if (_camNumber < 0)
                    {
                        _camNumber = _globalVariables.allPreyFish.Count;
                    }
                    else
                    {
                        _camNumber++;
                    }

                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch (_preyCam)
            {
                case false:
                    if (_camNumber > _globalVariables.allPredatorFish.Count)
                    {
                        _camNumber = 0;
                    }
                    else if (_camNumber < 0)
                    {
                        _camNumber = _globalVariables.allPredatorFish.Count;
                    }
                    else
                    {
                        _camNumber--;
                    }

                    break;
                case true:
                    if (_camNumber > _globalVariables.allPreyFish.Count)
                    {
                        _camNumber = 0;
                    }
                    else if (_camNumber < 0)
                    {
                        _camNumber = _globalVariables.allPreyFish.Count;
                    }
                    else
                    {
                        _camNumber--;
                    }

                    break;
            }
        }

        switch (_preyCam)
        {
            case false:
                fishCam.Follow = _globalVariables.allPredatorFish[_camNumber].gameObject.transform;
                fishCam.LookAt = fishCam.Follow.transform;
                break;
            case true:
                fishCam.Follow = _globalVariables.allPreyFish[_camNumber].gameObject.transform;
                fishCam.LookAt = fishCam.Follow.transform;
                break;
        }

        Debug.Log(_camNumber);
    }


    public void DeactivateFishCam()
    {
        StartCoroutine(SwitchCamRobot());
    }

    private int RngFish(string predOrPrey)
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

    IEnumerator SwitchCamPred()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _preyCam = false;
        _player.GetComponent<PlayerController>().controlsEnabled = false;
        fishCam.Priority = 12;
        _camNumber = RngFish("pred");
        _backToRobotButton.SetActive(true);
    }

    IEnumerator SwitchCamPrey()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _preyCam = true;
        _player.GetComponent<PlayerController>().controlsEnabled = false;
        fishCam.Priority = 12;
        _camNumber = RngFish("prey");
        _backToRobotButton.SetActive(true);
    }

    IEnumerator SwitchCamRobot()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _player.GetComponent<PlayerController>().controlsEnabled = true;
        fishCam.Priority = 1;
        _backToRobotButton.SetActive(false);
    }
}

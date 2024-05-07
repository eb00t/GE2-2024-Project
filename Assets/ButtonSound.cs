using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayButtonSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Button, transform.position);
    }
    public void PlayGoodButtonSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.GoodButton, transform.position);
    }
    public void PlayBadButtonSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.BadButton, transform.position);
    }
}

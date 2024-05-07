using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskSound : MonoBehaviour
{
    
    private float _soundCd = 0;
    
    private void Update()
    {
        _soundCd -= Time.deltaTime;
        _soundCd = Mathf.Clamp(_soundCd, 0, .25f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player") || !other.gameObject.CompareTag("InHand"))
        {
            if (_soundCd == 0)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.WoodThud, transform.position);
                _soundCd = 0.25f;
            }
        }
    }
}

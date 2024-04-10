using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowFog : MonoBehaviour
{
    private Color _lerpedColour = Color.red;
    //private Color _colourA, _colourB;
    private float _r, _g, _b;
    private bool _rNeg, _gNeg, _bNeg;
    void Start()
    {
        _r = Random.Range(0f, 1f);
        _g = Random.Range(0f, 1f);
        _b = Random.Range(0f, 1f);
        Debug.Log("r: " + _r + " g: " + _g + " b: " + _b);
        /*_colourA = Random.ColorHSV();
        _colourB = Random.ColorHSV();*/
    }

    
    void Update()
    {
        /*_lerpedColour = Color.Lerp(_colourA, _colourB, 5f * Time.deltaTime);
        if (_lerpedColour == _colourB)
        {
            _colourB = Random.ColorHSV();
            _colourA = Random.ColorHSV();
            Debug.Log("randomising");
        }
        Debug.Log(_lerpedColour);*/

        switch (_rNeg)
        {
            case true:
                _r -= 0.0001f;
                break;
            case false:
                _r += 0.0001f;
                break;
        }
        switch (_gNeg)
        {
            case true:
                _g -= 0.0001f;
                break;
            case false:
                _g += 0.0001f;
                break;
        }
        switch (_bNeg)
        {
            case true:
                _b -= 0.0001f;
                break;
            case false:
                _b += 0.0001f;
                break;
        }
       
        
        if (_r >= 1 && _rNeg == false)
        {
            _rNeg = true;
        }
        if (_g >= 1 && _gNeg == false)
        {
            _gNeg = true;
        }
        if (_b >= 1 && _bNeg == false)
        {
            _bNeg = true;
        }
        
        if (_r <= 0 && _rNeg)
        {
            _rNeg = false;
        }
        if (_g <= 0 && _gNeg)
        {
            _gNeg = false;
        }
        if (_b <= 0 && _bNeg)
        {
            _bNeg = false;
        }
        RenderSettings.fogColor = new Color(_r, _g, _b);
        
        
        
    }
}

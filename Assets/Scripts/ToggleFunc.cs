using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFunc : MonoBehaviour
{
   private Image _image;

   private void Start()
   {
      _image = GetComponent<Image>();
   }

   public void Toggle()
   {
      _image.enabled = !_image.enabled;
   }
}

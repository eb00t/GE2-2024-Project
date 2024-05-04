using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToggleFunc : MonoBehaviour
{
   private TextMeshProUGUI _textMeshProUGUI;

   private void Start()
   {
      _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
   }

   public void Toggle()
   {
      _textMeshProUGUI.enabled = !_textMeshProUGUI.enabled;
   }
}

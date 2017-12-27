using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TouchButton : Button {
  new public bool IsPressed() {
    return base.IsPressed();
  }
}

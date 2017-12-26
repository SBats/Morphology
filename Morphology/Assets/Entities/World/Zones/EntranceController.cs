using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceController : HubController {
  public void OnPlayerEnter() {
    GetComponentInParent<ZoneController>().OnPlayerEnter();
  }
}

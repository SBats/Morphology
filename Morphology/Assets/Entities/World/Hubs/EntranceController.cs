using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceController : HubController {
  private ZoneController zoneController;

  private void Awake() {
    zoneController = GetComponentInParent<ZoneController>();
  }
  public void OnPlayerEnter() {
    zoneController.OnPlayerEnter();
  }
}

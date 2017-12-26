using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Morphology;

public class ZoneController : MonoBehaviour {
  public ZONE_TYPES type = ZONE_TYPES.Earth;
  public HUB_POSITIONS entrancePosition = HUB_POSITIONS.Left;
  public HUB_POSITIONS exitPosition = HUB_POSITIONS.Right;
  public ZoneGenerator generator;

  private bool entered;

  public Bounds GetBounds() {
    TilemapCollider2D tileMap = GetComponentInChildren<TilemapCollider2D>(true);
    return tileMap.bounds;
  }

  public Bounds GetEntranceBounds() {
    EntranceController entrance = GetComponentInChildren<EntranceController>(true);
    return entrance.getBounds();
  }

  public Bounds GetExitBounds() {
    ExitController exit = GetComponentInChildren<ExitController>(true);
    return exit.getBounds();
  }

  public void OnPlayerEnter() {
    if (!entered) {
      entered = true;
      generator.OnZoneEntered(this);
    }
  }
}

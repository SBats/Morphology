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

  private TilemapCollider2D tilemap;
  private EntranceController entrance;
  private ExitController exit;

  private bool entered;

  private void Awake() {
    tilemap = GetComponentInChildren<TilemapCollider2D>(true);
    entrance = GetComponentInChildren<EntranceController>(true);
    exit = GetComponentInChildren<ExitController>(true);
  }

  public Bounds GetBounds() {
    return tilemap.bounds;
  }

  public Bounds GetEntranceBounds() {
    return entrance.getBounds();
  }

  public Vector3 GetEntranceLocalPosition() {
    return entrance.transform.localPosition;
  }

  public Bounds GetExitBounds() {
    return exit.getBounds();
  }

  public Vector3 GetExitLocalPosition() {
    return exit.transform.localPosition;
  }

  public void OnPlayerEnter() {
    if (!entered) {
      entered = true;
      generator.OnZoneEntered(this);
    }
  }
}

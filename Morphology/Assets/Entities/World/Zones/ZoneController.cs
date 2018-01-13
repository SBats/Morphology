using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Morphology;

public class ZoneController : MonoBehaviour {
  public ZONE_TYPES type = ZONE_TYPES.Earth;
  public HUB_POSITIONS entrancePosition = HUB_POSITIONS.Left;
  public HUB_POSITIONS exitPosition = HUB_POSITIONS.Right;
  public GameObject mainTilemap;
  [HideInInspector]
  public ZoneGenerator generator;
  [HideInInspector]
  public Vector3 entranceLocalPosition;
  [HideInInspector]
  public Vector3 exitLocalPosition;

  private TilemapCollider2D tilemap;
  private EntranceController entrance;
  private ExitController exit;

  private bool entered;

  private void Awake() {
    tilemap = mainTilemap.GetComponentInChildren<TilemapCollider2D>();
    entrance = GetComponentInChildren<EntranceController>();
    entranceLocalPosition = entrance.transform.localPosition;
    exit = GetComponentInChildren<ExitController>();
    exitLocalPosition = exit.transform.localPosition;
  }

  public Bounds GetBounds() {
    return tilemap.bounds;
  }

  public Bounds GetEntranceBounds() {
    return entrance.getBounds();
  }

  public Bounds GetExitBounds() {
    return exit.getBounds();
  }

  public void OnPlayerEnter() {
    if (!entered) {
      entered = true;
      generator.OnZoneEntered(this);
    }
  }

  public void ResetZone() {
    entered = false;
  }
}

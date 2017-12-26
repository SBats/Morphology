using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Morphology;

public class ZoneController : MonoBehaviour {
  public ZONE_TYPES type = ZONE_TYPES.Earth;

  public Bounds getEntranceBounds() {
    EntranceController entrance = GetComponentInChildren<EntranceController>(true);
    return entrance.GetComponent<BoxCollider2D>().bounds;
  }

  public Bounds getExitBounds() {
    ExitController exit = GetComponentInChildren<ExitController>(true);
    return exit.GetComponent<BoxCollider2D>().bounds;
  }

  public HUB_POSITIONS getEntrancePosition() {
    EntranceController entrance = GetComponentInChildren<EntranceController>(true);
    return getHubPosition(entrance);
  }

  public HUB_POSITIONS getExitPosition() {
    ExitController exit = GetComponentInChildren<ExitController>(true);
    return getHubPosition(exit);
  }

  private HUB_POSITIONS getHubPosition(HubController hub) {
    TilemapCollider2D tilemapCollider = GetComponentInChildren<TilemapCollider2D>(true);
    if (isRightSide(hub, tilemapCollider)) return HUB_POSITIONS.Right;
    if (isLeftSide(hub, tilemapCollider)) return HUB_POSITIONS.Left;
    if (isTopSide(hub, tilemapCollider)) return HUB_POSITIONS.Top;
    if (isBottomSide(hub, tilemapCollider)) return HUB_POSITIONS.Bottom;
    return HUB_POSITIONS.Middle;
  }

  private bool isRightSide(HubController hub, TilemapCollider2D tilemapCollider) {
    var hubBounds = hub.getBounds();
    var zoneBounds = tilemapCollider.bounds;
    return hubBounds.max.x == zoneBounds.max.x && hubBounds.size.x < hubBounds.size.y;
  }

  private bool isLeftSide(HubController hub, TilemapCollider2D tilemapCollider) {
    var hubBounds = hub.getBounds();
    var zoneBounds = tilemapCollider.bounds;
    return hubBounds.min.x == zoneBounds.min.x && hubBounds.size.x < hubBounds.size.y;
  }

  private bool isTopSide(HubController hub, TilemapCollider2D tilemapCollider) {
    var hubBounds = hub.getBounds();
    var zoneBounds = tilemapCollider.bounds;
    return hubBounds.max.y == zoneBounds.max.y && hubBounds.size.x > hubBounds.size.y;
  }

  private bool isBottomSide(HubController hub, TilemapCollider2D tilemapCollider) {
    var hubBounds = hub.getBounds();
    var zoneBounds = tilemapCollider.bounds;
    return hubBounds.min.y == zoneBounds.min.y && hubBounds.size.x > hubBounds.size.y;
  }
}

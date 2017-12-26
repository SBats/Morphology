using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Morphology;

public class ZoneGenerator : MonoBehaviour {
  public List<GameObject> prefabs;

  public List<ZoneController> zonesList = new List<ZoneController>();
  private int currentZone;

  private void Awake() {
    zonesList.Add(GenerateStartZone().GetComponent<ZoneController>());
    currentZone = 0;
    zonesList.Add(GenerateZoneFromExit(zonesList[0]).GetComponent<ZoneController>());
    zonesList.Add(GenerateZoneFromExit(zonesList[1]).GetComponent<ZoneController>());
  }

  private GameObject GenerateZoneFromExit(ZoneController zone) {
    HUB_POSITIONS exitPosition = zone.exitPosition;
    List<GameObject> eligibleZones = prefabs.FindAll((GameObject zonePrefab) => ZoneMatchEntrance(zonePrefab, exitPosition));
    GameObject newZonePrefab = eligibleZones[Random.Range(0, eligibleZones.Count)];
    GameObject newZone = GenerateZone(newZonePrefab);
    Vector3 newZonePosition = ComputeZonePositionFromHubs(zone, newZone.GetComponent<ZoneController>());
    PositionZone(newZone, newZonePosition);
    return newZone;
  }

  private GameObject GenerateStartZone() {
    List<GameObject> eligibleZones = prefabs.FindAll((GameObject zonePrefab) => ZoneMatchEntrance(zonePrefab, HUB_POSITIONS.Bottom));
    GameObject newZonePrefab = eligibleZones[Random.Range(0, eligibleZones.Count)];
    GameObject newZone = GenerateZone(newZonePrefab);
    PositionZone(newZone, new Vector3(0, 0, 1));
    return newZone;
  }

  private GameObject GenerateZone(GameObject prefab) {
    return Instantiate(prefab, new Vector3(-1000, -1000, -1000), Quaternion.identity);
  }

  private void PositionZone(GameObject zone, Vector3 position) {
    zone.transform.position = position;
  }

  private static bool ZoneMatchEntrance(GameObject zonePrefab, HUB_POSITIONS exitPosition) {
    var zone = zonePrefab.GetComponent<ZoneController>();
    if (exitPosition == HUB_POSITIONS.Bottom) return zone.entrancePosition == HUB_POSITIONS.Top;
    if (exitPosition == HUB_POSITIONS.Top) return zone.entrancePosition == HUB_POSITIONS.Bottom;
    if (exitPosition == HUB_POSITIONS.Left) return zone.entrancePosition == HUB_POSITIONS.Right;
    if (exitPosition == HUB_POSITIONS.Right) return zone.entrancePosition == HUB_POSITIONS.Left;
    return false;
  }

  private Vector3 ComputeZonePositionFromHubs(ZoneController zoneA, ZoneController zoneB) {
    Bounds zoneABounds = zoneA.GetBounds();
    Bounds zoneAExitBounds = zoneA.GetExitBounds();
    Bounds zoneBBounds = zoneB.GetBounds();
    Bounds zoneBEntranceBounds = zoneB.GetEntranceBounds();
    if (zoneA.exitPosition == HUB_POSITIONS.Right) {
      return new Vector3(
        zoneAExitBounds.max.x + zoneBBounds.extents.x - 1,
        zoneAExitBounds.max.y - (zoneBBounds.extents.y - (zoneBBounds.max.y - zoneBEntranceBounds.max.y)) - 1,
        zoneABounds.max.z
      );
    }
    if (zoneA.exitPosition == HUB_POSITIONS.Left) {
      return new Vector3(
        zoneAExitBounds.max.x - zoneBBounds.extents.x - 1,
        zoneAExitBounds.max.y - (zoneBBounds.extents.y - (zoneBBounds.max.y - zoneBEntranceBounds.max.y)) - 1,
        zoneABounds.max.z
      );
    }
    if (zoneA.exitPosition == HUB_POSITIONS.Bottom) {
      return new Vector3(
        zoneAExitBounds.min.x + (zoneBBounds.extents.x - (zoneBEntranceBounds.min.x - zoneBBounds.min.x)) - 1,
        zoneAExitBounds.min.y - zoneBBounds.extents.y - 1,
        zoneABounds.max.z
      );
    }
    if (zoneA.exitPosition == HUB_POSITIONS.Top) {
      return new Vector3(
        zoneAExitBounds.min.x + (zoneBBounds.extents.x - (zoneBEntranceBounds.min.x - zoneBBounds.min.x)) - 1,
        zoneAExitBounds.max.y + zoneBBounds.extents.y - 1,
        zoneABounds.max.z
      );
    }
    return new Vector3(
      zoneABounds.max.x + zoneBBounds.extents.x - 1,
      zoneABounds.max.y - zoneBBounds.extents.y - 1,
      zoneABounds.max.z
    );
  }
}

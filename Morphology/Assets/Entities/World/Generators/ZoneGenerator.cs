using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Morphology;

public class ZoneGenerator : MonoBehaviour {

  public List<ZoneController> zonesList = new List<ZoneController>();

  private List<ZoneController> availableZones = new List<ZoneController>();
  private int currentZone;

  private void Awake() {
    foreach (GameObject zoneObject in GameObject.FindGameObjectsWithTag("Zone")) {
      zoneObject.SetActive(false);
      ZoneController zone = zoneObject.GetComponent<ZoneController>();
      zone.generator = this;
      availableZones.Add(zone);
    }
    GenerateStartZone();
    currentZone = 0;
    GenerateZoneFromExit(zonesList[0]);
    GenerateZoneFromExit(zonesList[1]);
  }

  public void OnZoneEntered(ZoneController zone) {
    GenerateZoneFromExit(zonesList[zonesList.Count-1]);
    int zoneIndex = zonesList.FindIndex(listElement => listElement.GetInstanceID() == zone.GetInstanceID());
    if (zoneIndex > 2) {
      for (int i = 0; i < zoneIndex - 2; i++) {
        zonesList[0].gameObject.SetActive(false);
        zonesList[0].ResetZone();
        zonesList.RemoveAt(0);
        availableZones.Add(zonesList[0]);
      }
    }
  }

  private void GenerateZoneFromExit(ZoneController previousZone) {
    HUB_POSITIONS exitPosition = previousZone.exitPosition;
    List<ZoneController> eligibleZones = availableZones.FindAll((ZoneController zone) => ZoneMatchEntrance(zone, exitPosition));
    ZoneController newZone = eligibleZones[Random.Range(0, eligibleZones.Count)];
    Vector3 newZonePosition = ComputeZonePositionFromHubs(previousZone, newZone.GetComponent<ZoneController>());
    newZone.transform.position = newZonePosition;
    newZone.gameObject.SetActive(true);
    availableZones.Remove(newZone);
    zonesList.Add(newZone);
  }

  private void GenerateStartZone() {
    List<ZoneController> eligibleZones = availableZones.FindAll((ZoneController zone) => ZoneMatchEntrance(zone, HUB_POSITIONS.Bottom));
    ZoneController newZone = eligibleZones[Random.Range(0, eligibleZones.Count)];
    newZone.transform.position = new Vector3(0, 0, 1);
    newZone.gameObject.SetActive(true);
    availableZones.Remove(newZone);
    zonesList.Add(newZone);
  }

  private static bool ZoneMatchEntrance(ZoneController zone, HUB_POSITIONS exitPosition) {
    if (exitPosition == HUB_POSITIONS.Bottom) return zone.entrancePosition == HUB_POSITIONS.Top;
    if (exitPosition == HUB_POSITIONS.Top) return zone.entrancePosition == HUB_POSITIONS.Bottom;
    if (exitPosition == HUB_POSITIONS.Left) return zone.entrancePosition == HUB_POSITIONS.Right;
    if (exitPosition == HUB_POSITIONS.Right) return zone.entrancePosition == HUB_POSITIONS.Left;
    return false;
  }

  private Vector3 ComputeZonePositionFromHubs(ZoneController zoneA, ZoneController zoneB) {
    Bounds zoneABounds = zoneA.GetBounds();
    Vector3 zoneAExitPosition = zoneA.exitLocalPosition;
    Bounds zoneBBounds = zoneB.GetBounds();
    Vector3 zoneBEntrancePosition = zoneB.entranceLocalPosition;
    if (zoneA.exitPosition == HUB_POSITIONS.Right) {
      return new Vector3(
        zoneA.transform.position.x + zoneAExitPosition.x - zoneBEntrancePosition.x + 1,
        zoneA.transform.position.y + zoneAExitPosition.y - zoneBEntrancePosition.y,
        1
      );
    }
    if (zoneA.exitPosition == HUB_POSITIONS.Left) {
      return new Vector3(
        zoneA.transform.position.x + zoneAExitPosition.x + zoneBEntrancePosition.x - 1,
        zoneA.transform.position.y + zoneAExitPosition.y - zoneBEntrancePosition.y,
        1
      );
    }
    if (zoneA.exitPosition == HUB_POSITIONS.Bottom) {
      return new Vector3(
        zoneA.transform.position.x + zoneAExitPosition.x - zoneBEntrancePosition.x,
        zoneA.transform.position.y + zoneAExitPosition.y - zoneBEntrancePosition.y - 1,
        1
      );
    }
    if (zoneA.exitPosition == HUB_POSITIONS.Top) {
      return new Vector3(
        zoneA.transform.position.x + zoneAExitPosition.x - zoneBEntrancePosition.x,
        zoneA.transform.position.y + zoneAExitPosition.y - zoneBEntrancePosition.y + 1,
        1
      );
    }
    return new Vector3(
      zoneABounds.max.x + zoneBBounds.extents.x - 1,
      zoneABounds.max.y - zoneBBounds.extents.y - 1,
      1
    );
  }
}

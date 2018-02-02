using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Morphology;

public class ZoneGenerator : MonoBehaviour {

  public List<ZoneController> zonesList = new List<ZoneController>();

  private List<ZoneController> availableZones = new List<ZoneController>();
  private int currentZone;
  private List<ZoneController> zonesToDelete = new List<ZoneController>();
  private int zonesToCreate = 0;

  private void FreeZone(ZoneController zone) {
    int zoneIndex = zonesToDelete.FindIndex(listElement => listElement.GetInstanceID() == zone.GetInstanceID());
    zone.ResetZone();
    zonesToDelete.RemoveAt(0);
    availableZones.Add(zone);
  }

  private void PlaceZone(ZoneController newZone, ZoneController previousZone) {
    Vector3 newZonePosition = previousZone ? ComputeZonePositionFromHubs(previousZone, newZone.GetComponent<ZoneController>()) : new Vector3(0, 0, 1);
    newZone.transform.position = newZonePosition;
    zonesList.Add(newZone);
  }

  private IEnumerator CreateZones() {
    while (true) {
      if (zonesToCreate > 0) {
        zonesToCreate--;
        GenerateZone(zonesList[zonesList.Count-1]);
      }
      yield return new WaitForSeconds(.1f);
    }
  }

  private IEnumerator DeleteZones() {
    while (true) {
      if (zonesToDelete.Count > 0) {
        FreeZone(zonesToDelete[0]);
      }
      yield return new WaitForSeconds(.1f);
    }
  }

  private void Awake() {
    foreach (GameObject zoneObject in GameObject.FindGameObjectsWithTag("Zone")) {
      ZoneController zone = zoneObject.GetComponent<ZoneController>();
      zone.generator = this;
      availableZones.Add(zone);
    }
    GenerateZone();
    currentZone = 0;
    GenerateZone(zonesList[0]);
    GenerateZone(zonesList[1]);
    StartCoroutine("CreateZones");
    StartCoroutine("DeleteZones");
  }

  public void OnZoneEntered(ZoneController zone) {
    int zoneIndex = zonesList.FindIndex(listElement => listElement.GetInstanceID() == zone.GetInstanceID());
    zonesToCreate++;
    currentZone = zoneIndex;
    if (zoneIndex > 2) {
      zonesToDelete.Add(zonesList[0]);
      zonesList.RemoveAt(0);
    }
  }

  private void GenerateZone(ZoneController previousZone = null) {
    HUB_POSITIONS exitPosition = GetExitPosition(previousZone);
    List<ZoneController> eligibleZones = GetEligibleZones(exitPosition);
    ZoneController newZone = GetZoneFromList(eligibleZones);
    PlaceZone(newZone, previousZone);
    newZone.EnableTilemapCollider();
  }

  private List<ZoneController> GetEligibleZones(HUB_POSITIONS exitPosition) {
    return availableZones.FindAll((ZoneController zone) => ZoneMatchEntrance(zone, exitPosition));
  }

  private HUB_POSITIONS GetExitPosition(ZoneController previousZone) {
    return previousZone ? previousZone.exitPosition : HUB_POSITIONS.Bottom;
  }

  private ZoneController GetZoneFromList(List<ZoneController> list) {
    ZoneController zone = list[Random.Range(0, list.Count)];
    availableZones.Remove(zone);
    return zone;
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

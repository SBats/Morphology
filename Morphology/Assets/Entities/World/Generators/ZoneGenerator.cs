using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Morphology;

public class ZoneGenerator : MonoBehaviour {
  public List<GameObject> prefabs;

  private List<ZoneController> zonesList = new List<ZoneController>();
  private int currentZone;

  private void Awake() {
    zonesList.Add(GenerateRandomZone().GetComponent<ZoneController>());
    currentZone = 0;
    zonesList.Add(GenerateZoneFromExit(zonesList[0]).GetComponent<ZoneController>());
    zonesList.Add(GenerateZoneFromExit(zonesList[1]).GetComponent<ZoneController>());
  }

  private GameObject GenerateZoneFromExit(ZoneController zone) {
    HUB_POSITIONS exitPosition = zone.getExitPosition();
    List<GameObject> eligibleZones = prefabs.FindAll((GameObject zonePrefab) => ZoneMatchEntrance(zonePrefab, exitPosition));
    GameObject newZonePrefab = eligibleZones[Random.Range(0, eligibleZones.Count)];
    Vector3 newZonePosition = new Vector3(0, 0, 1);
    return GenerateZone(newZonePrefab, newZonePosition);
  }

  private GameObject GenerateRandomZone() {
    GameObject newZonePrefab = prefabs[Random.Range(0, prefabs.Count)];
    return GenerateZone(newZonePrefab, new Vector3(0, 0, 1));
  }

  private GameObject GenerateZone(GameObject prefab, Vector3 position) {
    return Instantiate(prefab, position, Quaternion.identity);
  }

  private static bool ZoneMatchEntrance(GameObject zonePrefab, HUB_POSITIONS exitPosition) {
    var zone = zonePrefab.GetComponent<ZoneController>();
    return zone.getEntrancePosition() == exitPosition;
  }

  // private Vector3 getPositionFromZones(ZoneController previousZone, ZoneController newZone) {

  // }
}

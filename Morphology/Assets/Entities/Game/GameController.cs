using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Morphology;

public class GameController : MonoBehaviour {

  public GameObject uiController;
  public GameObject zoneGeneratorPrefab;

  private ZoneGenerator zoneGenerator;
  private PlayerController player;


  private void Awake() {
    Time.timeScale = 0.0f;
    player = GameObject.Find("Player").GetComponent<PlayerController>();
    LoadZones();
    zoneGenerator = Instantiate(zoneGeneratorPrefab, new Vector3(0, 0, 1), Quaternion.identity).GetComponent<ZoneGenerator>();
    positionPlayer(zoneGenerator.zonesList[0]);
    Time.timeScale = 1.0f;
  }

  public void positionPlayer(ZoneController zone) {
    Vector3 position = zone.GetEntranceBounds().center;
    player.transform.position = position;
  }

  public void OnDeath() {
    uiController.GetComponent<UIController>().EndGame();
  }

  private void LoadZones() {
    GameObject grid = GameObject.Find("Grid");
    Vector3 defaultPosition = new Vector3(-100, -100, -100);
    Object[] zonesfiles = Resources.LoadAll("Earth", typeof(GameObject));
    foreach (GameObject prefab in zonesfiles) {
      for (int i = 0; i < 3; i++) {
        GameObject zone = Instantiate(prefab, defaultPosition, Quaternion.identity);
        zone.transform.parent = grid.gameObject.transform;
      }
    }
  }

}

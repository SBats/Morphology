using UnityEngine;
using System.Collections;

public class KillWhenFarAway : MonoBehaviour {

  private GameObject _gameCamera;
  private float _halfWidth;
  private float _cameraHalfWidth;

  private void Awake() {
    _gameCamera = GameObject.Find("Main Camera");
    _halfWidth = gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
    _cameraHalfWidth = _gameCamera.GetComponent<Camera>().orthographicSize;
  }

  private void Update() {
    Vector3 cameraPosition = _gameCamera.transform.position;
    Vector3 position = gameObject.transform.position;
    if (position.x + _halfWidth*2 < cameraPosition.x - _cameraHalfWidth) {
      Debug.Log("passed!");
      Destroy(gameObject);
    }
  }

}

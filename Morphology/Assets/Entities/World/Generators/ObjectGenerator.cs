using UnityEngine;
using System.Collections;

public class ObjectGenerator : MonoBehaviour {
  public GameObject prefab;
  public float generationTime;

  private float _generationTimer;
  private float _halfSize;
  private Vector3 _originalPosition;
  private Vector3 _lastPosition;

  private void Awake() {
    _originalPosition = gameObject.transform.position;
    _lastPosition = _originalPosition;
    _GenerateElement();
    _halfSize = prefab.GetComponent<SpriteRenderer>().bounds.extents.x;
  }

  private void Update() {
    _generationTimer -= Time.deltaTime;
     if ( _generationTimer < 0 ) {
        _GenerateElement();
        _generationTimer = generationTime;
     }
  }

  void _GenerateElement() {
    Vector3 newPosition = new Vector3(
      _lastPosition.x + _halfSize,
      _lastPosition.y,
      _lastPosition.z
    );
    Instantiate(prefab, newPosition, Quaternion.identity);
    _lastPosition = newPosition;
  }

  public void Reset() {
    ResetPosition();
    KillAllChildInstances();
  }

  private void KillAllChildInstances() {
    GameObject[] generatedChild = GameObject.FindGameObjectsWithTag("Generated");
    for (int i = 0; i < generatedChild.Length - 1; i++) {
      Destroy(generatedChild[i]);
    }
  }

  private void ResetPosition() {
    _lastPosition = new Vector3(
      _originalPosition.x - _halfSize*2,
      _originalPosition.y,
      _originalPosition.z
    );
  }
}

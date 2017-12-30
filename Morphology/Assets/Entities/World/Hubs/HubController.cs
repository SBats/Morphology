using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubController : MonoBehaviour {
	public Bounds getBounds() {
		return gameObject.GetComponent<BoxCollider2D>().bounds;
	}
}

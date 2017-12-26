using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

	public GameObject targetObject;
	public Transform startPoint;
	public Transform endPoint;
	public float moveSpeed;

	private Vector3 currentTarget;

	// Use this for initialization
	void Start () {
		currentTarget = endPoint.position;
	}

	// Update is called once per frame
	void Update () {
		targetObject.transform.position = Vector3.MoveTowards(
			targetObject.transform.position,
			currentTarget,
			moveSpeed * Time.deltaTime
		);

		if (targetObject.transform.position == endPoint.position) {
			currentTarget = startPoint.position;
		}
		if (targetObject.transform.position == startPoint.position) {
			currentTarget = endPoint.position;
		}
	}
}

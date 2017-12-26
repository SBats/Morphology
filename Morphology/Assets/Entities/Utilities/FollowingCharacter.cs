using UnityEngine;
using System.Collections;
using Prime31;


public class FollowingCharacter : MonoBehaviour {
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 offset;
	public bool useFixedUpdate = false;
	public bool fixedX = false;
	public bool fixedY = false;

	private CharacterController2D _playerController;
	private Vector3 _smoothDampVelocity;


	void Awake()
	{
		transform = gameObject.transform;
		_playerController = target.GetComponent<CharacterController2D>();
	}


	void LateUpdate()
	{
		if( !useFixedUpdate )
			updatePosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updatePosition();
	}


	void updatePosition()
	{
		Vector3 targetPosition = new Vector3(
			fixedX ? transform.position.x : target.position.x - (Mathf.Abs(_playerController.velocity.x) * offset.x),
			fixedY ? transform.position.y : target.position.y - (Mathf.Abs(_playerController.velocity.x) * offset.y),
			transform.position.z
		);
		if( _playerController == null )
		{
			transform.position = Vector3.SmoothDamp( transform.position, targetPosition, ref _smoothDampVelocity, smoothDampTime );
			return;
		}

		if( _playerController.velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, targetPosition, ref _smoothDampVelocity, smoothDampTime );
		}
	}

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Prime31;
using Morphology;


public class PlayerController : MonoBehaviour {
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public float jumpCancelFactor = 3f;
	public Sprite fireSprite;
	public Sprite waterSprite;
	public Sprite earthSprite;
	public GameObject gameController;
	public TouchButton actionButton;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	private Vector3 _spawnPosition;
	private FORMS currentForm;
	private CharacterController2D _controller;
	private BoxCollider2D _collider;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private CameraController _cameraController;
	private Button _fireButton;
	private Button _waterButton;
	private Button _earthButton;
	private bool jumpRequested = false;


	void Awake () {
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		_collider = GetComponent<BoxCollider2D>();
		_cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
		_velocity = _controller.velocity;
		_fireButton = GameObject.Find("Fire").GetComponent<Button>();
		_waterButton = GameObject.Find("Water").GetComponent<Button>();
		_earthButton = GameObject.Find("Earth").GetComponent<Button>();

		// listen to some events for illustration purposes
		// _controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		// _controller.onTriggerExitEvent += onTriggerExitEvent;
	}

	void Start () {
		_spawnPosition = transform.position;
		SetFireForm();
	}

	void Update ()
	{
		Move ();
		Render ();
	}

	#region Event Listeners

	// void onControllerCollider (RaycastHit2D hit)
	// {
	// 	// bail out on plain old ground hits cause they arent very interesting
	// 	if (hit.normal.y == 1f)
	// 		return;

	// 	// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
	// 	//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	// }


	void onTriggerEnterEvent (Collider2D collider)
	{
		// Debug.Log ("onTriggerEnterEvent: " + collider.name);
		if (collider.tag == "KillPlane") {
			Die();
		}
		if (collider.tag == "Entrance") {
			_spawnPosition = transform.position;
			collider.GetComponent<EntranceController>().OnPlayerEnter();
		}
		if (collider.tag == "FireGate" && currentForm != FORMS.Fire) {
			Die();
		}
		if (collider.tag == "WaterGate" && currentForm != FORMS.Water) {
			Die();
		}
		if (collider.tag == "EarthGate" && currentForm != FORMS.Earth) {
			Die();
		}
	}


	// void onTriggerExitEvent (Collider2D col)
	// {
		// Debug.Log ("onTriggerExitEvent: " + col.gameObject.name);
	// }

	#endregion

	#region Forms

	public void SetFireForm() {
		ChangeForm(FORMS.Fire);
		_fireButton.interactable = false;
		_waterButton.interactable = true;
		_earthButton.interactable = true;
	}

	public void SetWaterForm() {
		ChangeForm(FORMS.Water);
		_fireButton.interactable = true;
		_waterButton.interactable = false;
		_earthButton.interactable = true;
	}

	public void SetEarthForm() {
		ChangeForm(FORMS.Earth);
		_fireButton.interactable = true;
		_waterButton.interactable = true;
		_earthButton.interactable = false;
	}

	public void ChangeForm (FORMS form) {
		currentForm = form;
		gameObject.GetComponent<SpriteRenderer>().sprite = GetSpriteFromForm(form);
	}

	private Sprite GetSpriteFromForm(FORMS form) {
		if (form == FORMS.Water) return waterSprite;
		if (form == FORMS.Earth) return earthSprite;
		return fireSprite;
	}

	#endregion

	#region Movements

	public void RequestJump() {
		if (_controller.isGrounded) jumpRequested = true;
	}

	private void Move ()
	{
		if (_controller.isGrounded) _velocity.y = 0;

		var horizontalInput = GetHorizontalInput ();
		var horizontalSpeed = SmoothHorizontalSpeed (horizontalInput);
		_velocity.x = horizontalSpeed;

		var jump = GetJump ();
		if (jump > 0) _velocity.y = jump;

		var gravity = GetGravity ();
		if (!getActionInput() && _velocity.y > 0) {
			gravity *= jumpCancelFactor;
		}

		_velocity.y += gravity;

		_controller.move (_velocity * Time.deltaTime);
		_velocity = _controller.velocity;
		jumpRequested = false;
	}

	private float GetHorizontalInput ()
	{
		return 1;
	}

	private float GetJump () {
		if (!_controller.isGrounded || !getActionInputDown()) return 0;
		return Mathf.Sqrt (2f * jumpHeight * -gravity);
	}

	private bool getActionInputDown() {
		return (jumpRequested || Input.GetButtonDown ("Jump"));
	}

	private bool getActionInput() {
		return (actionButton.IsPressed() || Input.GetButton ("Jump"));
	}

	private float GetGravity ()
	{
		return gravity * Time.deltaTime;
	}

	private float SmoothHorizontalSpeed (float speed)
	{
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		var finalSpeed = speed * runSpeed;
		return Mathf.Lerp (_velocity.x, finalSpeed, Time.deltaTime * smoothedMovementFactor);
	}

	private void HandleOneWayPlatforms ()
	{
		if (_controller.isGrounded && Input.GetKey (KeyCode.DownArrow)) {
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}
	}

	#endregion


	#region Rendering

	private void Render ()
	{
		if (Mathf.Abs (_velocity.x) > 0.01f) {
			transform.localScale = new Vector3 (
			    Mathf.Sign (_velocity.x) * Mathf.Abs (transform.localScale.x),
			    transform.localScale.y,
			    transform.localScale.z
			);
		}
		updateAnimatorParameters ();
	}

	private void updateAnimatorParameters ()
	{
		_animator.SetBool ("grounded", _controller.isGrounded);
		_animator.SetFloat ("x_velocity", _velocity.x);
		_animator.SetFloat ("y_velocity", _velocity.y);
		_animator.SetFloat ("absolute_x_velocity", Mathf.Abs (_velocity.x));
	}

	#endregion

	public void Respawn ()
	{
		transform.position = _spawnPosition;
		GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");
 		for (int i = 0; i < generators.Length - 1; i++) {
			 generators[i].GetComponent<ObjectGenerator>().Reset();
		}
	}

	public void Die() {
		gameController.GetComponent<GameController>().OnDeath();
		gameObject.SetActive(false);
	}

}

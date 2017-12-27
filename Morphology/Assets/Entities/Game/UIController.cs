using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	public GameObject InGameCanvas;
	public GameObject PauseMenuCanvas;
	public GameObject DeathMenuCanvas;

	public GameObject GameController;

	private void Awake() {
		initUI();
	}

	public void Pause() {
		PauseMenuCanvas.SetActive(true);
		InGameCanvas.SetActive(false);
		Time.timeScale = 0.0f;
	}

	public void Unpause() {
		InGameCanvas.SetActive(true);
		PauseMenuCanvas.SetActive(false);
		Time.timeScale = 1.0f;
	}

	public void EndGame() {
		InGameCanvas.SetActive(false);
		DeathMenuCanvas.SetActive(true);
	}

	public void initUI() {
		DeathMenuCanvas.SetActive(false);
		InGameCanvas.SetActive(true);
	}
}

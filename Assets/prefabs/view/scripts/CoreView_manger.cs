using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <Summary>
/// Handles  the core view control system, main camera functions
/// Default light
/// </Summary>
public class CoreView_manger : MonoBehaviour
{
	[Header("Core view configuration component")]
	/// <summary>
	/// Exposed main camera reference
	/// </summary>
	public Camera mainCamera = null;

	public GameObject pauseMenu = null;

	public GameObject gameOverMenu = null;

	public win_panel win_panel = null;

	public Image shadowPanel = null;


	/// <summary> Gets the current audio Listener </summary>
	public AudioListener GetAudioListener(){
		return mainCamera.GetComponent<AudioListener>();
	}


	private void Awake() {
		Helpers.PrintDebug(GameTypes.kVIEW, "View behavior awaken");
	}


	private void Start() 
	{

	}

}
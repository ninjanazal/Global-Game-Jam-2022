using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole_Behavior : MonoBehaviour {	

	/// <Summary> Reference to the message elements </summary>
	[SerializeField] private MessageHandler[] messages;


	/// <Summary> Reference to the player trigger </summary>
	[SerializeField] private mole_trigger _trigger;

	/// <Summary> Marks if the player is near </Summary>
	private bool _isPlayerNear = false;


	private void Start() {
		_trigger.Init(this);
		ChangeMessageState(false);
		
	}

	/// <Summary> Change the messages state </summary>
	public void ChangeMessageState(bool enable){
		_isPlayerNear = enable;

		for (int i = 0; i < messages.Length; i++) {
			messages[i].ToggleCanvas(enable);
		}

		if(_isPlayerNear){
			GameCore.Instance.player.SetNearHole(this.gameObject);
		}
		else {
			GameCore.Instance.player.ClearNearHole();
		}
	}
}

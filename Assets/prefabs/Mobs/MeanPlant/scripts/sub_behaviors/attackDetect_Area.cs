using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackDetect_Area : MonoBehaviour
{
	/// <summary> internal reference for the player </summary>
	private CharacterMovement _playerRef;

	/// <Summary> Reference to the target mob behavior
	private Mob_base_behavior _mob_behavior;



	/// <Summary> Defines a reference to the target mob behavior </Summary>
	public void DefineMob(Mob_base_behavior mob){
		_mob_behavior = mob;
	}

	/// <summary> On Attack callback funtion </summary>
	public void OnAttackCall(){
		if(_playerRef){
			_playerRef.ApplyForce(new Vector3(
				50f * (_mob_behavior.isOnLeft() ? 1f : -1f), 0f, 0f));
		};
	}

	/// <summary> Used to clear internal player references </summary>
	public void clearPlayerReference(){
		if(_playerRef){
			_playerRef = null;
		}
	}


	/// <summary> On Trigger Enter callback </summary>
	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Player"){
			_playerRef = other.GetComponent<CharacterMovement>();
		}
	}

	/// <summary> On Trigger Exit callback funtion </summary>
	private void OnTriggerExit(Collider other) {
		if(other.tag == "Player" && _playerRef){
			_playerRef = null;
		}
	}

}

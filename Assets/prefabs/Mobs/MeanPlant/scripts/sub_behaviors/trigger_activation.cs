using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_activation : MonoBehaviour
{

	/// <Summary> Reference to the target mob behavior
	private Mob_base_behavior _mob_behavior;


	/// <Summary> Defines a reference to the target mob behavior </Summary>
	public void DefineMob(Mob_base_behavior mob){
		_mob_behavior = mob;
	}


	/// <summary> On Trigger Enter callback </summary>
	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Player" && _mob_behavior){
			_mob_behavior.OnPlayerdetectionChanged(true);
		}
	}
	
	
	/// <summary> On Trigger Exit callback funtion </summary>
	private void OnTriggerExit(Collider other) {
		if(other.tag == "Player" && _mob_behavior){
			_mob_behavior.OnPlayerdetectionChanged(false);
		}
	}
}

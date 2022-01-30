using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mole_trigger : MonoBehaviour
{

	private Mole_Behavior _mole_behavior;

	
	/// <summary> Regist a reference for the mole behavior </summary>
	public void Init(Mole_Behavior define_value)=> _mole_behavior = define_value;


    private void OnTriggerEnter(Collider other) {
		if(other.tag == "Player"){
			_mole_behavior.ChangeMessageState(true);
		}
	}


	private void OnTriggerExit(Collider other) {
		if(other.tag == "Player"){
			_mole_behavior.ChangeMessageState(false);
		}
	}
}

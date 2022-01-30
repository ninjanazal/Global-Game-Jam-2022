using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Helpers
{
	/// <summary>
	/// Prints to console with the logic type enum stirng
	/// </summary>
	/// <param name="l_type">Enum value for the logic division enum</param>
	/// <param name="msg"> String with the defined message</param>
	public static void PrintDebug(GameTypes l_type, string msg){
		Debug.Log($"--> {l_type.ToString()} ::::: {msg}");
	}

}
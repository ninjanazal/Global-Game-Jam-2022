using System;
using System.Collections.Generic;


/// <summary>
/// Finite state transition class, specified by the stateType
/// </summary>
public class FsmTransition<T>{

	private T[] _from; // Valid from states array
	private T _to; // Destination array value

	Action _onTransitionEnded = null; // Callback to be called after the transition happens


	// - - - - - - - - - - - - - - - - - - - - //
	//                 PUBLIC                  //
	// - - - - - - - - - - - - - - - - - - - - //


	/// <summary> Transition constructor</summary>
	/// <param name="from"> Array containing all the valid from states </param>
	/// <param name="to"> Result state if the transition happens </param>
	/// <param name="onTransitionCallback"> Function called after the transition happens </param>
	public FsmTransition(T[] from, T to, Action onTransitionCallback = null){
		this._from = from;
		this._to = to;
		this._onTransitionEnded = onTransitionCallback;
	}


	/// <summary> Checks if the current state is presented on the valid from state array </summary>
	/// <param name="curr_state"> Current state machine state </param>
	/// <return> Gets if is a valid from state to the transtion </return>
	public bool ValidateCurrent(T curr_state){
		return Array.Exists<T>(_from,
			state => {
				return EqualityComparer<T>.Default.Equals(state, curr_state);
			});	
	}


	/// <summary>
	/// Calls the on Transition callback funtion
	/// The Callback return value will be discarded
	/// </summary>
	public void OnTransitionCallback(){
		if(_onTransitionEnded != null){
			_onTransitionEnded();
		}
	}


	/// <summary> Gets the result state for the transition </summary>
	public T GetResultState(){ return _to; }
}

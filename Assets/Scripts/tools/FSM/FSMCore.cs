using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Finite state core class, specified by the stateType
/// </summary>
public class FSMCore<T>{

	private bool __active__; // Holds if the current machine is enabled
	private string _finiteName; // Finite name
	private T _previousState; // Previous state value
	private T _currentState; // Finite current State value

	// Finite Transitions dicionary
	private Dictionary<string, FsmTransition<T>> _transitions;


	// - - - - - - - - - - - - - - - - - - - - //
	//                 PUBLIC                  //
	// - - - - - - - - - - - - - - - - - - - - //


	/// <summary> Finite State Constructor</summary>
	/// <param name="fsmName"> Finite name</param>
	/// <param name="transitions"> Dictionary with the finite transitions</param>
	public FSMCore(string fsmName, Dictionary<string, FsmTransition<T>> transitions){
		this._finiteName = fsmName;
		this._transitions = transitions;

		this.__active__ = false;
		Helpers.PrintDebug(
			GameTypes.kFSMAKINA,
			$"{_finiteName} has been started with {_transitions.Count} transitions"
		);
	}


	/// <summary> Starts the current finite state with a defined state</summary>
	/// <param name="initialState"> Initial defined state</param>
	public void StartFsmOn (T initialState){
		if(this.__active__) return;

		_currentState = _previousState = initialState;
		this.__active__ = true;

		Helpers.PrintDebug(GameTypes.kFSMAKINA,
			$"Makina enabled with the state {_currentState.ToString()}"
		);
	}


	/// </summary> Trys to attend a a request transition by name<summary>
	/// <param name="transitionName"> Request transition name</param>
	/// <return> Returns if was able to attend the transition</return>
	public bool AttendTransition(string transitionName){
		Helpers.PrintDebug(GameTypes.kFSMAKINA,
			$"Attending -- {transitionName} -- transition");
		
		if(_transitions.ContainsKey(transitionName)){
			FsmTransition<T> _t = _transitions[transitionName];
			
			if(!_t.ValidateCurrent(_currentState)) 
				return false;

			_t.OnTransitionCallback();
			_previousState = _currentState;
			_currentState = _t.GetResultState();
			Helpers.PrintDebug(
				GameTypes.kFSMAKINA,
				$"Valid Transition --> Current state {_currentState.ToString()}"
			);
			return true;
		}
		return false;
	}
}

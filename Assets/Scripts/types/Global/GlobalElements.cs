using System;
using System.Collections.Generic;

/// <summary>
/// Static class holding global variables
/// </summary>
static class GlobalElements
{

	/// <summary> CoreTransitions for the main FSM </summary>
	public static Dictionary<string, FsmTransition<CoreStates>> coreTransitionBundle =
		new Dictionary<string, FsmTransition<CoreStates>>() {
			{
				"gotoMainMenu",
				new FsmTransition<CoreStates>(
					new CoreStates[]{CoreStates.kINTRO, CoreStates.kPAUSEMENU},
					 CoreStates.kMAINMENU /*,
					new Action(() => Helpers.PrintDebug(GameTypes.kFSMAKINA, "Test Callback"))*/
				)
			},
			{
				"gotoPauseMenu",
				new FsmTransition<CoreStates>(
					new CoreStates[] {CoreStates.kGAMEPLAY, CoreStates.kWORLDCONTROL},
					 CoreStates.kMAINMENU
				)
			},
			{
				"gotoGamePlay",
				new FsmTransition<CoreStates>(
					new CoreStates[] {CoreStates.kMAINMENU, CoreStates.kWORLDCONTROL, CoreStates.kPAUSEMENU},
					CoreStates.kGAMEPLAY
				)
			},
			{
				"gotoWorldControl",
				new FsmTransition<CoreStates>(
					new CoreStates[] {CoreStates.kGAMEPLAY, CoreStates.kPAUSEMENU},
					CoreStates.kWORLDCONTROL
				)
			}
		};

}
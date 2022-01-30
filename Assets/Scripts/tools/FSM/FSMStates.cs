
/// <summary>
/// Enum holding the core FSM game states
/// </summary>
public enum CoreStates {
	kINTRO,
	kMAINMENU,
	kPAUSEMENU,

	kGAMEPLAY,
	kWORLDCONTROL
};


/// <summary>
/// Enum holding the main player fsm state
/// </summary>
public enum PlayerStates {
	kIDLE,
	kJUMP,
	kMOVE,
	kGRAB,
	kSWITCH
}
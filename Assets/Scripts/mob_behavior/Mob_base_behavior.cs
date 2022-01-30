using UnityEngine;


/// <summary> Base Class fro mobs behaviors </summary>
public abstract class Mob_base_behavior : MonoBehaviour
{
	/// <summary> On Attack callback funtion </summary>
	public abstract void OnPlayerdetectionChanged(bool onArea);

	/// <summary> On Attack callback funtion </summary>
	public abstract void TriggerAttack();


	/// <summary> Reference to the bo animatior </summary>
	public abstract void OnDeath();


	/// <summary> On Take dagame function </summary>
	public abstract void OnTakeDamage(float amount);

	/// <Summary>Gets if the player is on the left side of the mob </Summary>
	public abstract bool isOnLeft();


}
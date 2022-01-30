using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeanPlant_behavior : Mob_base_behavior
{
	/// <Summary> Mob Internal states </Summary>
	enum MobStates{ SLEEP, AWAKE, IDLE, ATTACK, DEAD };


	[Header ("Mob Properties")]
	/// <Summary> Max </Summary>
	public float MaxHealth = 100.0f;


	/// <Summary> Attack interval seconds</Summary>
	public float AttackInterval = 5.0f;


	[Header ("Reference internal areas")]	
	/// <Summary> Activation area reference </Summary>
	[SerializeField] private trigger_activation _activation_area;


	/// <Summary> Attack area refernce </Summary>
	[SerializeField] private attackDetect_Area _attack_area;


	[Space(2f)]
	[Header("Message references")]
	/// <Summary> Attack area refernce </Summary>
	[SerializeField] private ParticleSystem particle_system;


	/// <Summary> Reference to the messageHandler </Summary>
	[SerializeField] private MessageHandler _msg_handle;


	[SerializeField] private ParticleSystem dmg_particle;


	/// <Summary> On Awake message </Summary>
	[TextArea(15,20)]
	public string OnAwakeMsg;


	[SerializeField] GrabbableItem drop_on_death;


	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


	/// <Summary> Animator reference</Summary>
	private Animator _mob_animator;

	/// <Summary> Reference to the current audio player
	private AudioSource _mob_audioPlayer;


	/// <Summary> Marks the mob current state</Summary>
	private MobStates _currState = MobStates.SLEEP;


	/// <Summary> Holds if the current mob is enabled </Summary>
	private bool _enabled = false;


	/// <Summary> Mob Status, current health </Summary>
	private float _current_hp;


	/// <Summary> Mob Status, Inner timer value</Summary>
	private float _inner_timer = 0.0f;

	/// <Summary> Mob Status,Marks if the player is on the left side</Summary>
	private bool _on_left = true ;




	#region Public
	/// <Summary>Gets if the player is on the left side of the mob </Summary>
	public override bool isOnLeft(){
		return _on_left;
	}

	/// <Summary> On Player detection changed </Summary>
	public override void OnPlayerdetectionChanged(bool onArea){
		_enabled = onArea;
		}

	/// <Summary> Trigger attack particle system action </Summary>
	public void TriggerAttackParticles(){
		particle_system.Play();
		if(_mob_audioPlayer)
			_mob_audioPlayer.Play();
	}


	/// <Summary> Trigger a attack area validation sequence </Summary>
	public override void TriggerAttack() =>_attack_area.OnAttackCall();


	/// <Summary> On Death callback funtion</Summary>
	public override void OnDeath(){	
		_mob_audioPlayer.Stop();
		Destroy(_mob_audioPlayer);
		_mob_animator.SetTrigger("OnDeath");
		_currState = MobStates.DEAD;
		GameCore.Instance.player.RemoveMobInRange(this);

		if(drop_on_death){
			GameObject temp_item = Instantiate(
				drop_on_death.gameObject, transform.parent);

			temp_item.transform.position += Vector3.back * 0.1f;
		}
	}


	/// <Summary> On Take Damage callback function</Summary>
	public override void OnTakeDamage(float amount){
		dmg_particle.Play();
		_current_hp -= amount;
		if(_current_hp <= 0.0){
			_current_hp *= 0.0f;
			OnDeath();
			return;
		}
		TriggerAttackParticles();
		TriggerAttack();

	}

	/// <Summary> On Enter Idle callback function</Summary>
	public void OnEnterIdle(){
		_currState = MobStates.IDLE;
		_inner_timer = 0.0f;
		_mob_audioPlayer.Stop();
	}
	# endregion


	# region Private
	/// <Summary> On Awake Unity API Message </Summary>
	private void Awake() {
		_mob_animator = GetComponent<Animator>();
		_activation_area.DefineMob(this);
		_attack_area.DefineMob(this);
		_mob_animator.gameObject.SetActive(true);
	}


	/// <Summary> On Start Unity API Message </Summary>
	private void Start() {
		_current_hp = MaxHealth;
		_enabled = false;

		_inner_timer = 0.0f;
		_mob_audioPlayer = GetComponent<AudioSource>();
		
		_on_left = GameCore.Instance.player.transform.position.x < transform.position.y;
		_msg_handle.ToggleCanvas(false);
	}


	/// <Sumarry> On Update Unit API Message</Summary>
	private void Update() {
		switch (_currState)
		{
			case MobStates.SLEEP:
				if(_enabled){
					_msg_handle.ToggleCanvas(true);
					_msg_handle.SetMessage(OnAwakeMsg, 20.0f);

					_mob_animator.SetTrigger("Start_Awake");
					_currState = MobStates.AWAKE;
				}

				break;

			case MobStates.IDLE:
				if(_enabled){
					_inner_timer += Time.deltaTime;

					if(_inner_timer >= AttackInterval){
						_currState = MobStates.ATTACK;
						_mob_animator.SetTrigger("Attack_trigger");
					}
				}
				break;
		}

		if(_currState != MobStates.SLEEP && _currState != MobStates.DEAD){
			if(_on_left != 
					(GameCore.Instance.player.transform.position.x > transform.position.x)){
				_on_left = !_on_left;
				transform.localScale = new Vector3(
					-transform.localScale.x,transform.localScale.y, transform.localScale.z
				);
				RectTransform temp_transform = _msg_handle.GetComponent<RectTransform>();
				temp_transform.localScale = new Vector3(
					-temp_transform.localScale.x, temp_transform.localScale.y,
					temp_transform.localScale.z
				);
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if(_currState == MobStates.DEAD)
			return;
		
		if(other.tag == "Player"){
			GameCore.Instance.player.RegistMobInRange(this);
		}
	}

	private void OnTriggerExit(Collider other) {
		if(_currState == MobStates.DEAD)
			return;

		if(other.tag == "Player"){
			GameCore.Instance.player.RemoveMobInRange(this);
		}
	}
	#endregion
}

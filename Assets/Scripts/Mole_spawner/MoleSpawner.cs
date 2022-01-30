using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleSpawner : MonoBehaviour
{

	enum SpawnState
	{
		SPAWNED, REST
	}

	/// <Summary> Time to update spawn position </summary>
	public float RespawnInterval = 30f;

	/// <Summary> Time before spawn a new hole </summary>
	public float rest_interval = 3f;

	/// <Summary> Reference to the spawner area </summary>
	[SerializeField] private GameObject Mole_prefab;


	/// <Summary> Reference to the spawner area </summary>
	private BoxCollider _spawnerArea;

	/// <Summary> internal timer</Summary>
	private float _inner_timer = 0f;

	/// <Summary> Reference to the current spawned hole </Summary>
	private GameObject _current_hole = null;


	/// <Summary> Current spawner state </Summary>
	private SpawnState _curr_state = SpawnState.REST;


	/// <Summary> Marker, Pauses the spawn progress </Summary>
	private bool _paused = false;


	/// <Summary> Pauses the spawn progress </Summary>
	public void PauseSpawner()=> _paused = true;


	/// <Summary> Continue the spawner progress </Summary>
	public void ReleaseSpawner() => _paused = false;

    // Start is called before the first frame update
    void Start()
    {
        _spawnerArea = GetComponent<BoxCollider>();
		_inner_timer = 0f;
    }

    // Update Message, Unity API
	private void Update() {
		if(_paused)
			return;
		
		switch (_curr_state)
		{
			case SpawnState.SPAWNED:
				_inner_timer += Time.deltaTime;

				if(_inner_timer >= RespawnInterval){
					_curr_state = SpawnState.REST;
					Destroy(_current_hole);
					_current_hole = null;

					_inner_timer = 0.0f;
				}
				break;

			case SpawnState.REST:
				_inner_timer += Time.deltaTime;

				if(_inner_timer >= rest_interval){
					_curr_state = SpawnState.SPAWNED;
					_current_hole = Instantiate(Mole_prefab);
					_current_hole.transform.position = RandomPosition();

					_inner_timer = 0.0f;
				}
				break;
		}
	}


    /// <Summary> Gets a random position inside the collider, respecting the game dynamics </Summary>
	private Vector3 RandomPosition(){
		Vector3 temp_pos = new Vector3(
			Random.Range(_spawnerArea.bounds.min.x, _spawnerArea.bounds.max.x),
			0f, Random.Range(0, 2) + 0.1f
		);


		return temp_pos;
	}


}

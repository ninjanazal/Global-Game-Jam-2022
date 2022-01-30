using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class GameCore : MonoBehaviour
{

	[SerializeField] private levelsInfos _level_info;

	/// <summary> Internal reference for the afiliation </summary>
	private static GameCore _instance;

	/// <summary> Used to get the instance for the core behavior</summary>
	/// <return> Gets a reference for the current(unique) GameCore instance</return>
	public static GameCore Instance{
		get { return _instance; }
	}

	/// <Summary> References the game core view object </summary>
	public CoreView_manger viewCore;


	/// <Summary> References the current player object </summary>
	public CharacterMovement player;

	/// <Summary> References the current mole spawner </summary>
	public MoleSpawner moleSpawner;

	/// <summary>  Time of playtime in minutes
	public float levelTime = 2f;
	private float levelTimer = 0.0f;
	private bool over = false;


	/// <summary> Main Fsm object</summary>
	private FSMCore<CoreStates> main_fsm;


	/// <summary> Defines the current view side
	private bool _on_brighter_side = true;


	/// <summary>
	private bool isGamePaused = false;



	// - - - - - - - - - - - - - - - - - - - - //
	//                 PUBLIC                  //
	// - - - - - - - - - - - - - - - - - - - - //

	/// <summary> Defines a side switch </Summary>
	public void ChangeViewSide(bool brighter) => _on_brighter_side = brighter;


	/// <summary> Gets if is on the brighter side </Summary>
	public bool isOnBrighter(){
		return _on_brighter_side;
	}


	/// <summary>Unity Api awaken override </summary>
	private void Awake(){
		if (_instance != null && _instance != this){
			Destroy(this);
		}else{
			_instance = this;
		}

		/*
		this.main_fsm = new FSMCore<CoreStates>(
			"Main Machine", GlobalElements.coreTransitionBundle);
		*/
		Helpers.PrintDebug(GameTypes.kCORE, "Core behavior awaken!");

	}


	/// <summary>Unity Api start override </summary>
	private void Start(){
		//this.main_fsm.StartFsmOn(CoreStates.kINTRO);
		levelTimer = levelTime * 60f;
	}


	private void Update()
    {
		if ( Input.GetKeyDown(KeyCode.Escape) )
        {
			if ( isGamePaused )
            {
				Resume();
            }
			else
            {
				Pause();
            }
        }

		if ( !isGamePaused )
        {
			levelTimer -= Time.deltaTime;

			viewCore.shadowPanel.color = new Color(0f, 0f, 0f, (1f - (levelTimer / (levelTime * 60))) );

			if (levelTimer <= 0f)
			{
				//game over
				GameOver();
			}
		}
    }


	public void Pause()
    {
		Time.timeScale = 0f;
		isGamePaused = true;
		viewCore.pauseMenu.SetActive(isGamePaused);
	}

	public void Resume()
    {
		Time.timeScale = 1f;
		isGamePaused = false;
		viewCore.pauseMenu.SetActive(isGamePaused);
		if ( over )
        {
			viewCore.gameOverMenu.SetActive(isGamePaused);
			over = false;
			levelTimer = levelTime * 60f;
		}
		
	}

	public void RestartLEvel()
    {
		//reload scene
		Resume();
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadSceneAsync(scene.buildIndex);
    }

	public void GameOver()
    {
		Time.timeScale = 0f;
		isGamePaused = true;
		over = true;
		viewCore.gameOverMenu.SetActive(over);
	}

	public void QuitToMenu()
    {
		Resume();
		//to main menu
		SceneManager.LoadSceneAsync(0);
    }

	public void MarkLevelCompleted(){
		_level_info.level_one = 1;
		QuitToMenu();
	}

}
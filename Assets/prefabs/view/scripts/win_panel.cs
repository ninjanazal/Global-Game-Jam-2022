using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class win_panel : MonoBehaviour
{
   private Animator _win_animation;
   [SerializeField] private Canvas _canvas;


   private void Start() {
	   _win_animation = GetComponent<Animator>();
	   _canvas.enabled = false;
   }



	public void StartEndAnimation(){
		_canvas.enabled = true;
		_win_animation.SetTrigger("exit");

	}


	public void OnAnimationEnd(){
		_canvas.enabled = false;
		GameCore.Instance.MarkLevelCompleted();
	}
}

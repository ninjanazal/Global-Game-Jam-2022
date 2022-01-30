using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Parameters
    [SerializeField] private float cameraSpeed = 1.7f;
    [SerializeField] private float characterOffSet = 2.0f;

	public Vector2 verticalPositions;

    public GameObject character;
    public GameObject floor;

    public Vector2 maxPosition = new Vector2(-10f, 10f);
    private Vector3 originalPosition;

    private float minX = 0.0f;
    private float maxX = 0.0f;
    private bool movingCamera = false;

    private bool enteringHole = false;
    private bool exitHole = false;
    private GameObject focusHole = null;


	private CameraViewEffects _camera_effects;

    // Start is called before the first frame update
    void Start()
    {
		_camera_effects = GetComponent<CameraViewEffects>();
		_camera_effects.InvertColors = !GameCore.Instance.isOnBrighter();

        if ( !floor )
        {
            minX = (maxPosition.x) + 5;
            maxX = (maxPosition.y) - 5;
        }
        else
        {
            minX = (floor.transform.position.x - floor.transform.localScale.x * 0.5f) + 5;
            maxX = (floor.transform.position.x + floor.transform.localScale.x * 0.5f) - 5;
        }
    }

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if ( character && !enteringHole)
        {
            float playerDirection = character.transform.position.x - transform.position.x;

            if (character.transform.position.x > minX && character.transform.position.x < maxX)
            {
                if (Mathf.Abs(playerDirection) > characterOffSet && !movingCamera)
                {
                    movingCamera = true;
                }
                else if (movingCamera)
                {
                    float moveX = playerDirection * cameraSpeed * Time.deltaTime;
                    transform.position += new Vector3(moveX, 0.0f, 0.0f);

                    if (Mathf.Abs(moveX) < 0.005f)
                    {
                        movingCamera = false;
                    }
                }
            }
            else if (movingCamera)
            {
                float moveX = 0.0f;
                if (character.transform.position.x <= minX)
                {
                    moveX = minX - transform.position.x;
                }
                else if (character.transform.position.x >= maxX)
                {
                    moveX = maxX - transform.position.x;
                }
                moveX *= cameraSpeed * Time.deltaTime;
                transform.position += new Vector3(moveX, 0.0f, 0.0f);

                if (Mathf.Abs(moveX) < 0.0001f)
                {
                    movingCamera = false;
                }
            }
        }

        if ( enteringHole)
        {
            AnimateCamera();
        }
    }

    private void AnimateCamera()
    {
        if ( !exitHole )
        {
            Vector3 dir = focusHole.transform.position - this.transform.position;
            dir = dir * cameraSpeed * Time.deltaTime;

            transform.position += dir;

            if ( Mathf.Abs(dir.z) <= 0.01f )
            {
                //change position and shader here
				GameCore.Instance.ChangeViewSide(!GameCore.Instance.isOnBrighter());
				_camera_effects.InvertColors = !GameCore.Instance.isOnBrighter();

				if(GameCore.Instance.isOnBrighter()){
					originalPosition.y = verticalPositions.x;
					transform.eulerAngles = new Vector3(
						transform.eulerAngles.x, transform.eulerAngles.y, 0.0f
					);
				}else {
					originalPosition.y = verticalPositions.y;
					transform.eulerAngles = new Vector3(
						transform.eulerAngles.x, transform.eulerAngles.y, -180.0f
					);
				}

				GameCore.Instance.player.switchView();

                exitHole = true;
            }
        }
        else
        {
            Vector3 dir = originalPosition - this.transform.position;
            dir = dir * cameraSpeed * Time.deltaTime;

            transform.position += dir;

            if ( Mathf.Abs(dir.z) <= 0.01f )
            {
                //Completed
                transform.position = originalPosition;
                exitHole = false;
                enteringHole = false;

				GameCore.Instance.moleSpawner.ReleaseSpawner();
                GameCore.Instance.player.SetExitHoleFinish();
            }
        }
    }

    public void EnterHole(GameObject hole)
    {
        focusHole = hole;
        enteringHole = true;
        originalPosition = transform.position;

		GameCore.Instance.moleSpawner.PauseSpawner();
    }

    public void StartRotation()
    {

    }

}

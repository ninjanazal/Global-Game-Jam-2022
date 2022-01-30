using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleHandler : MonoBehaviour
{
    // Parameters
    [SerializeField] float detectionRange = 3f;
    // Cached
    private MessageHandler messageHandler;
    private Collider[] playerDetector;

    // Start is called before the first frame update
    void Start()
    {
        messageHandler = gameObject.GetComponentInChildren<MessageHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        playerDetector = Physics.OverlapSphere(transform.position, detectionRange);

        if(playerDetector.Length != 0)
        {
            bool playerFound = false;
            foreach(Collider collider in playerDetector)
            {
                if (collider.gameObject.tag == "Player")
                {
                    playerFound = true;
                    messageHandler.ToggleCanvas(true);
                }
            }
            if (!playerFound) messageHandler.ToggleCanvas(false);
        }
    }
}

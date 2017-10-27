using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private GameObject player;
    [SerializeField] private float moveSpeed = 0.1f; // How fast the camer follows the player
    [SerializeField] private float lookAheadAmount = 0.2f; // How much to pan camera based on mouse position

    void Update()
    {
        if (player != null)
        {
            // Follow player
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, offset.x + player.transform.position.x, Time.deltaTime * moveSpeed),
                                             Mathf.Lerp(transform.position.y, offset.y + player.transform.position.y, Time.deltaTime * moveSpeed),
                                             Mathf.Lerp(transform.position.z, offset.z + player.transform.position.z, Time.deltaTime * moveSpeed));

            //Debug.Log(transform.position);

            // Look ahead based on mouse position

            // TODO: Decide if we actually want this
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lookAheadDir = mousePosition - player.transform.position;
            Vector3 newPos = transform.position + lookAheadDir.normalized * lookAheadAmount;

            transform.position = new Vector3(Mathf.Lerp(transform.position.x, newPos.x, Time.deltaTime),
                                             Mathf.Lerp(transform.position.y, newPos.y, Time.deltaTime),
                                             Mathf.Lerp(transform.position.z, newPos.z, Time.deltaTime));
        }
    }
}
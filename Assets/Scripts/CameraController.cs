using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float panSpeed = 30.0f;
    private float panMargin = 10f;
    private float scrollSpeed = 5.0f;
    private float minY = 5.0f;
    private float maxY = 25.0f;
    private float minX = -2f;
    private float maxX = 15.0f;
    private float minZ = -5.0f;
    private float MaxZ = 7.0f;
    
    
    private bool disableMovement = false;
    public bool useMouseToMove = false;
    public Transform centerPoint;
    public float rotateSpeed = 30.0f;
    // public bool followPlayer = true;
    // public Transform player;
    // public Vector3 offset;
    // public float smoothness = 0.5f;
    
    // private void FixedUpdate() {
        // if (followPlayer) {
        //     Vector3 pos = player.position + offset;
        //     transform.position = Vector3.Lerp(transform.position, pos, smoothness);
            // transform.LookAt(player);
    //     }
    // }
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameOver) {
            this.enabled = false;
        }
        // disable camera movement when press tab key
        if (Input.GetKeyDown(KeyCode.Tab)) {
            disableMovement = !disableMovement;
        }
        if (disableMovement) return;
        
        // float verticalInput = Input.GetAxis("Vertical");
        // float horizontalInput = Input.GetAxis("Horizontal");
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        // when press wsad key or when mouse move to the side of the screen
        // move the camera to that direction
        if (Input.GetKey(KeyCode.W) || (useMouseToMove && Input.mousePosition.y >= Screen.height - panMargin)) {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S) || (useMouseToMove && Input.mousePosition.y <= panMargin)) {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D) || (useMouseToMove && Input.mousePosition.x >= Screen.width - panMargin)) {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A) || (useMouseToMove && Input.mousePosition.x <= panMargin)) {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        // rotate around center point
        if (Input.GetKey(KeyCode.Q)) {
            transform.RotateAround(centerPoint.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E)) {
            transform.RotateAround(centerPoint.position, Vector3.up, -rotateSpeed * Time.deltaTime);
        }
        
        Vector3 pos = transform.position;
        pos.y -= scrollSpeed * scrollInput * Time.deltaTime * 1000;
        // clamp the axis so the camera not move too far away
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, MaxZ);
        
        transform.position = pos;
    }
}

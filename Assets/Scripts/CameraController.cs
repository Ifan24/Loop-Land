using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 30.0f;
    public float panMargin = 10f;
    public float scrollSpeed = 5.0f;
    public float minY = 5.0f;
    public float maxY = 25.0f;
    public float minX = -2f;
    public float maxX = 15.0f;
    public float zRange = 5.0f;
    
    private bool disableMovement = true;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameOver) {
            this.enabled = false;
        }
        // disable camera movement when press escape key
        if (Input.GetKeyDown(KeyCode.Escape)) {
            disableMovement = !disableMovement;
        }
        if (disableMovement) return;
        
        // float verticalInput = Input.GetAxis("Vertical");
        // float horizontalInput = Input.GetAxis("Horizontal");
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        // when press wsad key or when mouse move to the side of the screen
        // move the camera to that direction
        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - panMargin) {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panMargin) {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panMargin) {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= panMargin) {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        
        Vector3 pos = transform.position;
        pos.y -= scrollSpeed * scrollInput * Time.deltaTime * 1000;
        // clamp the axis
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, -zRange, zRange);
        
        transform.position = pos;
    }
}

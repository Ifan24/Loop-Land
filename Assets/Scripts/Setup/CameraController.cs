using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject followPlayerVirtualCam;
    [SerializeField] private GameObject wholeMapVirtualCam;
    
    private float panSpeed = 30.0f;
    private float panMargin = 10f;
    private float scrollSpeed = 5.0f;
    private float minY = 5.0f;
    private float maxY = 25.0f;
    private float minX = -2f;
    private float maxX = 15.0f;
    private float minZ = -5.0f;
    private float MaxZ = 7.0f;
    
    
    private bool disableMovement = true;
    public bool useMouseToMove = false;
    public Transform centerPoint;
    public float rotateSpeed = 30.0f;
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameOver) {
            this.enabled = false;
        }
        // disable camera movement when press tab key
        if (Input.GetKeyDown(KeyCode.Tab)) {
            disableMovement = !disableMovement;
            followPlayerVirtualCam.SetActive(!followPlayerVirtualCam.activeSelf);
        }
        
        if (disableMovement || followPlayerVirtualCam.activeSelf) return;
        
        // float verticalInput = Input.GetAxis("Vertical");
        // float horizontalInput = Input.GetAxis("Horizontal");
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        // when press wsad key or when mouse move to the side of the screen
        // move the camera to that direction
        if (Input.GetKey(KeyCode.W) || (useMouseToMove && Input.mousePosition.y >= Screen.height - panMargin)) {
            wholeMapVirtualCam.transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S) || (useMouseToMove && Input.mousePosition.y <= panMargin)) {
            wholeMapVirtualCam.transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.D) || (useMouseToMove && Input.mousePosition.x >= Screen.width - panMargin)) {
            wholeMapVirtualCam.transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.A) || (useMouseToMove && Input.mousePosition.x <= panMargin)) {
            wholeMapVirtualCam.transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        // rotate around center point
        if (Input.GetKey(KeyCode.Q)) {
            wholeMapVirtualCam.transform.RotateAround(centerPoint.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E)) {
            wholeMapVirtualCam.transform.RotateAround(centerPoint.position, Vector3.up, -rotateSpeed * Time.deltaTime);
        }
        
        Vector3 pos = wholeMapVirtualCam.transform.position;
        pos.y -= scrollSpeed * scrollInput * Time.deltaTime * 1000;
        // clamp the axis so the camera not move too far away
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, MaxZ);
        
        wholeMapVirtualCam.transform.position = pos;
    }
}

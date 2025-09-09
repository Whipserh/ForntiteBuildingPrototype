using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleMoveAround : MonoBehaviour
{

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();   
    }

    private float[] movePlayer = new float[3];/*0 - horizontal movement, 1 - forward/backward movement, 2 - vertical movement*/
    public float elevationSpeed = 2;
    public float movementSpeed = 5;
    public float mouseSensitivity = 1;
    private Vector2 turnCamera;
    // Update is called once per frame
    void Update()
    {

        movePlayer[0] = Input.GetAxis("Horizontal");
        movePlayer[1] = Input.GetAxis("Vertical");


        //focus on the vertical movement
        if (Input.GetKeyDown(KeyCode.Q)) //move down vertically
        {
            movePlayer[2] = -1;
        }
        else if (Input.GetKeyDown(KeyCode.E))//move up vertically
        {
            movePlayer[2] = 1;
        }
        else//stop moving vertically
        {
            movePlayer[2] = 0;
        }

        //rotate the camera
        turnCamera = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity;

        //look up and down
        cameraVerticalRotation -= Mathf.Clamp(turnCamera.y, -45, 90);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        //look around
        transform.Rotate(Vector3.up * turnCamera.x);
        
    }

    float cameraVerticalRotation;
    
    private void FixedUpdate()
    {
        rb.linearVelocity = (movementSpeed * new Vector3(movePlayer[0], movePlayer[1], 0)) + (elevationSpeed * new Vector3(0, 0, 1));
    }

}

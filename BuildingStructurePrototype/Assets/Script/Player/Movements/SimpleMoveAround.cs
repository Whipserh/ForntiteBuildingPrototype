using JetBrains.Annotations;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleMoveAround : MonoBehaviour
{

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Vector2.zero; 
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
        parentTransform.Rotate(Vector3.up * turnCamera.x);
        
    }

    public Transform parentTransform;
    float cameraVerticalRotation;



    [SerializeField] private float acceleration = 1;
    public float linearDrag = 1;
    public float maxSpeed = 10;
    private Vector3 speed;
    private void FixedUpdate()
    {


        //since we can't have a moveable rotatable body we'll have to improvise

        //we'll start by adding plane drag where if the player is not using wasd the camera will start to slow down
        //we don't need to worry about the vertical movement as that will just be purely based on the players pricision movements...
        //                                                         i just don't want vertical drag
        if (movePlayer[0] == 0 && movePlayer[1] == 0)//we don't move
        {
            speed = speed.normalized * (speed.magnitude-(linearDrag * Time.fixedDeltaTime));
        }else//we are moving
        {
            
            Vector3 horizontalDirection = new Vector3(1,0,0) * movePlayer[0];
            Vector3 forwardDirection = new Vector3(0, 0, 1) * movePlayer[1]; 

            Vector3 direction = (horizontalDirection + forwardDirection).normalized;
            
            speed += direction * acceleration * Time.fixedDeltaTime;
        }

        speed = speed.normalized * Mathf.Clamp(speed.magnitude, 0, maxSpeed);
        
        parentTransform.Translate(speed * Time.deltaTime);
    }

}

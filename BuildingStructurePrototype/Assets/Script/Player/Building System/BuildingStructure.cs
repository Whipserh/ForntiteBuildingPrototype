using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildingStructure : MonoBehaviour
{
    [SerializeField]
    private Transform buildingTransform;
    
    [SerializeField]
    private float maxPlacementDistance = 5;
    [SerializeField]
    private float buildingScale = 3;
    private RaycastHit hit;
    
    private bool buildMode = false;

    private GameObject structureGuidelines;
    private int structureIndex;

    [SerializeField]
    private GameObject [] structurePrefabs = new GameObject[2];
    private bool rotatableStructure = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        structureIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //toggle buildmode
        if (Input.GetKeyDown(KeyCode.C)){toggeBuildMode();}

        
        if (!buildMode) return;

        //controls for rotating through structures
        if (Input.GetKeyDown(KeyCode.F)) { rotateSelectedStructure(); }

        //update buildmode
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxPlacementDistance)){
            Debug.Log(hit.point);

            Vector3 raycastHit = hit.point/buildingScale;
            Vector3 floorposition = Vector3.zero;
            
            switch (structureIndex)
            {
                case 0://wall
                    raycastHit = hit.point / (buildingScale/2);
                    floorposition = new Vector3(Mathf.RoundToInt(raycastHit.x), Mathf.RoundToInt(raycastHit.y), Mathf.RoundToInt(raycastHit.z)) * (buildingScale/2);
                    if (floorposition.x % 3 == floorposition.z % 3)
                        structureGuidelines.transform.position = floorposition + new Vector3(1.5f,0,0);
                    break;
                case 1://floor
                    raycastHit = hit.point / buildingScale;
                    floorposition = new Vector3(Mathf.RoundToInt(raycastHit.x), Mathf.RoundToInt(raycastHit.y), Mathf.RoundToInt(raycastHit.z)) * buildingScale;
                    structureGuidelines.transform.position = floorposition;
                    break;
            }

            

        }

        //rotate the stairs
        if (structureIndex == 2 && Input.GetKeyDown(KeyCode.R))
        {
            structureGuidelines.transform.Rotate(Vector3.up, 90);
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            placeStructure();
        }

    }
    private void rotateSelectedStructure()
    {
        //turn off current guildline
        Destroy(structureGuidelines);

        //switch to new guideline
        structureIndex = ++structureIndex % structurePrefabs.Length;
        structureGuidelines = Instantiate(structurePrefabs[structureIndex], buildingTransform);

        //if the object is stairs/rotatable
        rotatableStructure = structureIndex == 2;//2 is the stairs index
    }

    //TODO
    private void placeStructure()
    {
        Debug.Log("Structure created");

        //create the object at the given structure
        structureGuidelines = Instantiate(structurePrefabs[structureIndex], buildingTransform);
    }


    /// <summary>
    ///changes the gamemode that the player is in
    /// turns the builder guidelines on and off
    /// </summary>
    private void toggeBuildMode()
    {
        Debug.Log(-3%3);
        Debug.Log("toggled buildmode");
        buildMode = !buildMode;
        //toggle the visibility of the build guideline from the player
        if(buildMode)
            structureGuidelines = Instantiate(structurePrefabs[structureIndex], buildingTransform);
        else
            Destroy(structureGuidelines);

    }

}

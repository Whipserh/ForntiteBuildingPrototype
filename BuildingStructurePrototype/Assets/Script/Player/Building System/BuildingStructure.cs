using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildingStructure : MonoBehaviour
{

    [SerializeField]
    private Transform floor;
    [SerializeField]
    private float maxPlacementDistance = 5;
    [SerializeField]
    private float buildingScale = 3;
    private RaycastHit hit;
    
    private bool buildMode = true;

    [SerializeField]
    private GameObject[] structureGuidelines = new GameObject[2];
    private int structureIndex;


    public GameObject wall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            Vector3 floorposition = new Vector3(Mathf.RoundToInt(raycastHit.x), Mathf.RoundToInt(raycastHit.y), Mathf.RoundToInt(raycastHit.z))*buildingScale;

            structureGuidelines[structureIndex].transform.position = floorposition;
        }

        
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Structure created");
            placeStructure();
        }

    }
    private void rotateSelectedStructure()
    {
        //turn off current guildline
        structureGuidelines[structureIndex].SetActive(false);

        //switch to new guideline
        structureIndex = ++structureIndex % structureGuidelines.Length;
        structureGuidelines[structureIndex].SetActive(true);
    }

    //TODO
    private void placeStructure()
    {
        
    }

    private void toggeBuildMode()
    {
        buildMode = !buildMode;
        //hide the build guideline from the player
        structureGuidelines[structureIndex].SetActive(buildMode);
    }

}

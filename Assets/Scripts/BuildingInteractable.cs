using UnityEngine;

public class BuildingInteractable : MonoBehaviour
{
    [SerializeField] 
    private BuildingInfo buildingInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        CanvasManager.instance.ShowWarning(30, ChangeBuilding);
    }

    public void ChangeBuilding()
    {
        CameraMovement.instance.ChangeMove(false);
        CameraMovement.instance.SnapPosition(new Vector2(transform.position.x, transform.position.z));
        
    }
}

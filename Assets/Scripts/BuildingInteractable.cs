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
        float dist = (buildingInfo.pos - GameManager.instance.GetCurrentLocation().pos).magnitude;
        int time = Mathf.RoundToInt(30f * dist / 8.5f);
        time = Mathf.RoundToInt(time / 30f) * 30;
        CanvasManager.instance.ShowWarning(time, ChangeBuilding);
    }

    public void SnapToBuilding()
    {
        CameraMovement.instance.ChangeMove(false);
        CameraMovement.instance.SetRotate(true);
        CameraMovement.instance.SnapPosition(new Vector2(transform.position.x, transform.position.z));
    }
    
    
    public void ChangeBuilding()
    {
        CameraMovement.instance.ChangeMove(false);
        CameraMovement.instance.SetRotate(true);
        CameraMovement.instance.SnapPosition(new Vector2(transform.position.x, transform.position.z));
        //GameManager.instance.DecreaseTimer(); CALCULATE HERE TIME between buildings
        if (GameManager.instance.GetCurrentLocation() != null)
        {
            float dist = (buildingInfo.pos - GameManager.instance.GetCurrentLocation().pos).magnitude;
            int time = Mathf.RoundToInt(30f * dist / 8.5f);
            time = Mathf.RoundToInt(time / 30f) * 30;
            GameManager.instance.DecreaseTimer(time);
        }
        GameManager.instance.SetCurrentBuilding(buildingInfo);
        GameManager.instance.UseTool();
    }

    public void ChangeBuilding(bool calc)
    {
        CameraMovement.instance.ChangeMove(false);
        CameraMovement.instance.SetRotate(true);
        CameraMovement.instance.SnapPosition(new Vector2(transform.position.x, transform.position.z));
        //GameManager.instance.DecreaseTimer(); CALCULATE HERE TIME between buildings
        if (GameManager.instance.GetCurrentLocation() != null && calc)
        {
            float dist = (buildingInfo.pos - GameManager.instance.GetCurrentLocation().pos).magnitude;
            int time = Mathf.RoundToInt(30f * dist / 8.5f);
            time = Mathf.RoundToInt(time / 30f) * 30;
            GameManager.instance.DecreaseTimer(time);
        }
        GameManager.instance.SetCurrentBuilding(buildingInfo);
        GameManager.instance.UseTool();
    }

    public string GetBuildingName()
    {
        return buildingInfo.buildingName;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName="Building", menuName = "Buildings/Building", order=1)]
public class BuildingInfo : ScriptableObject
{
    public string buildingName;
    public string buildingSmallDesc;
    [TextArea(15,20)]
    public string buildingDesctription;
    //general info? vector of strings
}

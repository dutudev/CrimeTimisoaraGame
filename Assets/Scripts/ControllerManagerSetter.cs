using System.Collections.Generic;
using UnityEngine;

public class ControllerManagerSetter : MonoBehaviour
{
    [SerializeField] private List<BuildingSerial> buildings;

    public List<BuildingSerial> GetList()
    {
        return buildings;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class ControllerManager : MonoBehaviour
{
    
    public ControllerManager instance { private set; get; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopMsg : MonoBehaviour
{
    private void OnEnable()
    {
        if (SimulatorManager.Instance != null)
        {
            SimulatorManager.Instance.PlayingSimulator += SimulatorMessageLogic;
        }
    }

    private void OnDisable()
    {
        if (SimulatorManager.Instance != null)
        {
            SimulatorManager.Instance.PlayingSimulator -= SimulatorMessageLogic;
        }
    }

    public void SimulatorMessageLogic(Transform loc)
    {
        foreach (Transform child in loc)
        { 
            if (child.childCount > 0)
            {
                Check chk = child.GetComponentInChildren<Check>();
                Renderer rdr = chk.GetComponent<Renderer>();

                if (chk && !chk.CheckEmpty && transform.GetChild(1).name == "Plane")
                {
                    var copyMaterial = rdr.material;

                    GameObject originObj = transform.GetChild(1).gameObject;

                    if (originObj != null)
                    {
                        originObj.SetActive(true);
                        originObj.GetComponent<Renderer>().material = copyMaterial;
                    }
                    chk.CheckEmpty = true;                    
                    break;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

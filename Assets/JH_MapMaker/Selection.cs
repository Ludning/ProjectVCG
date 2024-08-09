using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Selection : MonoBehaviour
{
    private MapMakerManager _MapMakerManager;
    [SerializeField] private GameObject SelectedObj;
    [SerializeField] private TextMeshProUGUI ObjNameText;


    private void Start()
    {
        _MapMakerManager = GameObject.Find("BuildingManager").GetComponent<MapMakerManager>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //         
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.gameObject.CompareTag("tileObj"))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    Select(hit.collider.gameObject);
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            DeSelect();
        }
    }

    private void Select(GameObject obj)
    {
        if (obj == SelectedObj) return;
        if (SelectedObj != null) DeSelect();
        Outline outline = obj.GetComponent<Outline>();
        if (outline == null) obj.AddComponent<Outline>();
        else outline.enabled = true;
        ObjNameText.text = obj.name;
        SelectedObj = obj;
    }

    private void DeSelect()
    {
        SelectedObj.GetComponent<Outline>().enabled = false;
        SelectedObj = null;
    }

    public void Move()
    {
        _MapMakerManager.PendingObject = SelectedObj;
    }

    public void Delete()
    {
        GameObject objDestroy = SelectedObj;
        DeSelect();
        Destroy(objDestroy);
    }
}

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                // hit.collider가 null이 아닌지 확인
                if (hit.collider != null)
                {
                    // hit.collider.gameObject가 null이 아닌지 확인
                    if (hit.collider.gameObject != null)
                    {
                        // hit.collider.gameObject.transform.parent가 null이 아닌지 확인
                        if (hit.collider.gameObject.transform.parent != null)
                        {
                            // hit.collider.gameObject.transform.parent.gameObject가 null이 아닌지 확인
                            if (hit.collider.gameObject.transform.parent.gameObject != null)
                            {
                                // 타겟이 tileObj 태그를 가진 부모 오브젝트인지 확인
                                if (hit.collider.gameObject.transform.parent.gameObject.CompareTag("tileObj"))
                                {
                                    Debug.Log(hit.collider.gameObject.name);
                                    Select(hit.collider.gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }
        if (_MapMakerManager != null && Input.GetMouseButtonDown(1))
        {
            if(!_MapMakerManager.PendingObject && SelectedObj) DeSelect();
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

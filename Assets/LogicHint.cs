using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicHint : MonoBehaviour
{
    [SerializeField] private Image Image;
    private Camera _camera;
    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        transform.LookAt(_camera.transform);
    }

    public void SetLogicImage(Sprite iconImage)
    {
        Image.sprite = iconImage;
    }
    public void HideLogicHint()
    {
        gameObject.SetActive(false);
    }
}

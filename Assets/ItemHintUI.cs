using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHintUI : MonoBehaviour
{
    [SerializeField] private Transform _context;
    
    private Stack<Image> _images = new Stack<Image>();
    private Camera _camera;
    
    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        transform.LookAt(_camera.transform);
        transform.Rotate(0, 180, 0);
    }
    
    public void PushImage(ItemType type)
    {
        FoodData data = DataManager.Instance.GetGameData<FoodData>(((int)type).ToString());
        Sprite sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(data.Icon);
        GameObject itemImagePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ItemImage");
        GameObject itemImage = Instantiate(itemImagePrefab, _context, false);
        Image image = itemImage.GetComponent<Image>();
        image.sprite = sprite;
        _images.Push(image);
    }
    public void RemoveImage()
    {
        if (_images.Count == 0)
            return;
        var image = _images.Pop();
        Destroy(image.gameObject);
    }

    public void Clear()
    {
        RemoveImage();
        Destroy(gameObject);
    }
}

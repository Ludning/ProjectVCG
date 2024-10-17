using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour, IUIBase
{
    [SerializeField] private Transform Context;
    private List<Image> ItemList = new List<Image>();


    public void Init()
    {
        Clear();
        PcData pcData = DataManager.Instance.GetGameData<PcData>("0");
        
        for (int i = 0; i < pcData.InventoryMax; i++)
        {
            // 새로운 빈 게임 오브젝트 생성
            Type[] componentArray = new[] { typeof(RectTransform), typeof(Image) };
            GameObject itemImage = new GameObject("ItemImage", componentArray);

            // 부모를 설정 (주로 Canvas가 될 것입니다)
            itemImage.transform.SetParent(Context, false);
            
            // RectTransform 컴포넌트를 추가하고 설정
            RectTransform rectTransform = itemImage.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100, 100); // 크기 설정
            rectTransform.localScale = Vector3.one; // 스케일을 1로 설정

            // Image 컴포넌트를 추가
            Image image = itemImage.GetComponent<Image>();
            image.color = Color.white; // 기본 색상 설정 (원하는 색으로 변경 가능)
            ItemList.Add(image);
        }
        
    }

    public void Clear()
    {
        foreach (var image in ItemList)
        {
            image.sprite = null;
            Destroy(image.gameObject);
        }
        ItemList.Clear();
    }
    
    public void AddItem(ItemType type)
    {
        FoodData foodData = DataManager.Instance.GetGameData<FoodData>(((int)type).ToString());
        Sprite foodSprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(foodData.Icon);
        
        // 리스트가 비어 있지 않을 경우 기존 이미지들을 뒤로 이동
        if (ItemList.Count > 0)
        {
            for (int i = ItemList.Count - 1; i > 0; i--)
            {
                ItemList[i].sprite = ItemList[i - 1].sprite;
            }
        }

        // 첫 번째 이미지에 새로운 아이템의 스프라이트를 설정
        if (ItemList.Count > 0)
        {
            ItemList[0].sprite = foodSprite;
        }
    }

    public void RemoveItem()
    {
        // 첫 번째 이미지를 제거하고, 나머지 이미지를 앞으로 이동
        for (int i = 0; i < ItemList.Count - 1; i++)
        {
            ItemList[i].sprite = ItemList[i + 1].sprite;
        }

        // 마지막 아이템은 제거된 상태로 남겨둠 (null로 설정)
        if (ItemList.Count > 0)
        {
            ItemList[ItemList.Count - 1].sprite = null;
        }
    }
}

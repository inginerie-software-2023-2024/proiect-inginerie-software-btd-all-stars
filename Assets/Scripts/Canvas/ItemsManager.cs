using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemsManager : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Image itemPrefab;
    private void OnEnable()
    {
        var list = inventory.upgrades;
        foreach(IUpgrade item in list)
        {
            var instance = Instantiate(itemPrefab,gameObject.transform);
            instance.sprite = item.Sprite;
            instance.rectTransform.sizeDelta = new Vector2(35, 35);
        }
        
        if(list.Count > 6)
        {
            RectTransform _rect = GetComponent<RectTransform>();
            _rect.sizeDelta = new Vector2((list.Count * 40 - 250), _rect.sizeDelta.y);
        }
    }
    private void OnDisable()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

}

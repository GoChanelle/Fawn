using UnityEngine;
using System.Collections.Generic;

public class InsideInventoryController : MonoBehaviour
{
    public static InsideInventoryController instance { get; private set; }

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    private List<Slot> slots = new List<Slot>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            slots.Add(slot);

            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Slot slot in slots)
        {
            if (slot.currentItem == null)
            {
                GameObject item = Instantiate(itemPrefab, slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
                return true;
            }
        }
        return false; // inventory full
    }
}
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

    private static List<GameObject> savedItems = new List<GameObject>();
    private static bool initialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        savedItems.Clear();
        initialized = false;
    }

    public static void ResetInventory()
    {
        savedItems.Clear();
        initialized = false;
    }

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
        }

        if (!initialized)
        {
            initialized = true;
            for (int i = 0; i < itemPrefabs.Length && i < slotCount; i++)
                savedItems.Add(itemPrefabs[i]);
        }

        for (int i = 0; i < savedItems.Count && i < slots.Count; i++)
            SpawnItemInSlot(savedItems[i], slots[i]);
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Slot slot in slots)
        {
            if (slot.currentItem == null)
            {
                SpawnItemInSlot(itemPrefab, slot);
                savedItems.Add(itemPrefab);
                return true;
            }
        }
        return false;
    }

    private void SpawnItemInSlot(GameObject prefab, Slot slot)
    {
        GameObject item = Instantiate(prefab, slot.transform);
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        slot.currentItem = item;
    }

    public static bool HasItem(string prefabName)
    {
        foreach (GameObject item in savedItems)
            if (string.Equals(item.name, prefabName, System.StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }

    public static void ConsumeItem(string prefabName)
    {
        for (int i = 0; i < savedItems.Count; i++)
        {
            if (string.Equals(savedItems[i].name, prefabName, System.StringComparison.OrdinalIgnoreCase))
            {
                savedItems.RemoveAt(i);
                return;
            }
        }
    }
}
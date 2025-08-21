using UnityEngine;
using UnityEngine.UI;

public class QuickslotInventory : MonoBehaviour
{
    public Transform quickslotParent;
    public InventoryManager inventoryManager;
    public int currentQuickslotID = 0;
    public Sprite selectedSprite;
    public Sprite notSelectedSprite;
    public Player player;

    void Update()
    {
        InventorySlot slot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();
        float mw = Input.GetAxis("Mouse ScrollWheel");

        if (mw > 0.1)
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            if (currentQuickslotID >= quickslotParent.childCount - 1)
            {
                currentQuickslotID = 0;
            }
            else
            {
                currentQuickslotID++;
            }
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;

        }
        if (mw < -0.1)
        {
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
            if (currentQuickslotID <= 0)
            {
                currentQuickslotID = quickslotParent.childCount - 1;
            }
            else
            {
                currentQuickslotID--;
            }
            quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
        }
        for (int i = 0; i < quickslotParent.childCount; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                if (currentQuickslotID == i)
                {
                    if (quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == notSelectedSprite)
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                    }
                    else
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                    }
                }
                else
                {
                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = notSelectedSprite;
                    currentQuickslotID = i;
                    quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite = selectedSprite;
                }
            }
        }
        if (slot.item != null)
        {
            if (slot.item.canBeHeld)
            {
                EquipItem(slot.item);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (slot.item.isConsumable && !inventoryManager.isOpened && quickslotParent.GetChild(currentQuickslotID).GetComponent<Image>().sprite == selectedSprite)
                {
                    Consume(slot.item);

                    if (slot.amount <= 1)
                    {
                        quickslotParent.GetChild(currentQuickslotID).GetComponentInChildren<DragAndDropItem>().NullifySlotData();
                    }
                    else
                    {
                        slot.amount--;
                        slot.amountText.text = slot.amount.ToString();
                    }
                }
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0 || Input.anyKeyDown)
        {
            InventorySlot currentSlot = quickslotParent.GetChild(currentQuickslotID).GetComponent<InventorySlot>();

            if (currentSlot.isEmpty)
            {
                if (player.currentItem != null)
                {
                    Destroy(player.currentItem);
                    player.currentItem = null;
                }
            }
        }

    }

    private void Consume(ItemScriptableObject item)
    {
        if (item.isConsumable)
        {
            player.HP += item.changeHealth;
            player.hunger += item.changeHunger;
        }
    }

    private void EquipItem(ItemScriptableObject item)
    {
        if (player.currentItem != null)
        {
            Destroy(player.currentItem);
        }

        if (item.canBeHeld && item.itemPrefab != null)
        {
            GameObject newItem = Instantiate(item.itemPrefab, player.itemHolder);
            newItem.transform.localPosition = Vector3.zero;
            newItem.transform.localRotation = Quaternion.identity;
            player.currentItem = newItem;
            player.ItemPrefab = item;
        }
    }

    public int[] GetQuickslotItemIDs()
    {
        int count = quickslotParent.childCount;
        int[] ids = new int[count];
        for (int i = 0; i < count; i++)
        {
            InventorySlot slot = quickslotParent.GetChild(i).GetComponent<InventorySlot>();
            ids[i] = (slot.item != null) ? slot.item.ID : -1;
        }
        return ids;
    }

    public int[] GetQuickslotAmounts()
    {
        int count = quickslotParent.childCount;
        int[] amounts = new int[count];
        for (int i = 0; i < count; i++)
        {
            InventorySlot slot = quickslotParent.GetChild(i).GetComponent<InventorySlot>();
            amounts[i] = (slot.item != null) ? slot.amount : 0;
        }
        return amounts;
    }

    public void LoadQuickslots(int[] ids, int[] amounts, ItemDatabase itemDatabase)
    {
        for (int i = 0; i < quickslotParent.childCount; i++)
        {
            InventorySlot slot = quickslotParent.GetChild(i).GetComponent<InventorySlot>();
            if (ids[i] != -1)
            {
                slot.item = itemDatabase.GetItemByID(ids[i]);
                slot.amount = amounts[i];
                slot.SetIcon(slot.item.icon);
                slot.isEmpty = false;
            }
            else
            {
                slot.NullifySlotData();
            }
        }
    }
}


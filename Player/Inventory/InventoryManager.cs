using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public Transform inventoryPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isOpened;

    private void Start() {
        for (int i = 0; i < inventoryPanel.childCount; i++) {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null) {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
    }

    public void AddItemToInventory(ItemScriptableObject _item, int _amount) {
        foreach (InventorySlot slot in slots) {
            if (slot.item == _item) {
                if (slot.amount + _amount <= _item.maxAmount) {
                    slot.amount += _amount;
                    slot.amountText.text = slot.amount.ToString();
                    return;
                }
                break;
            }
        }
        foreach (InventorySlot slot in slots) {
            if (slot.isEmpty) {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.amountText.text += _amount.ToString();
                return;
            }
        }
    }
}
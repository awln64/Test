using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Items/New Item")]
class ItemCreator : ItemScriptableObject {
    public float healAmount;

    void Start() {
        itemType = ItemType.CONSUMABLE;
    }
}
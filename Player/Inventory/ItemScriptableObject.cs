using UnityEngine;

public enum ItemType { CONSUMABLE, ARMOR, WEAPON, INSTRUMENT }
public class ItemScriptableObject : ScriptableObject {
    public int ID;
    public ItemType itemType;
    public string itemName;
    public GameObject itemPrefab;
    public Sprite icon;
    public int maxAmount;
    public string description;
    public bool isConsumable;
    public bool canBeHeld;
    public float damage;

    [Header("Consumable Characteristics")]
    public float changeHealth;
    public float changeHunger;
}

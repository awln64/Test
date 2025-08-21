using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float posX, posY, posZ;
    public float HP;
    public float hunger;
    public int[] inventoryItemIDs;
    public int[] inventoryAmounts;
    public int[] quickslotItemIDs;
    public int[] quickslotAmounts;
}

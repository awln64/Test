using System.IO;
using UnityEngine;

public class MenuScripts : MonoBehaviour
{
    public Player player;
    public InventoryManager inventoryManager;
    public QuickslotInventory quickslotInventory;
    public ItemDatabase itemDatabase;

    private string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
    }

    public void SaveGame()
    {
        PlayerData data = new PlayerData();
        data.posX = player.transform.position.x;
        data.posY = player.transform.position.y;
        data.posZ = player.transform.position.z;
        data.HP = player.HP;
        data.hunger = player.hunger;

        int count = inventoryManager.slots.Count;
        data.inventoryItemIDs = new int[count];
        data.inventoryAmounts = new int[count];
        for (int i = 0; i < count; i++)
        {
            if (inventoryManager.slots[i].item != null)
            {
                data.inventoryItemIDs[i] = inventoryManager.slots[i].item.ID;
                data.inventoryAmounts[i] = inventoryManager.slots[i].amount;
            }
            else
            {
                data.inventoryItemIDs[i] = -1;
                data.inventoryAmounts[i] = 0;
            }
        }

        data.quickslotItemIDs = quickslotInventory.GetQuickslotItemIDs();
        data.quickslotAmounts = quickslotInventory.GetQuickslotAmounts();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            CharacterController cc = player.GetComponent<CharacterController>();

            cc.enabled = false;
            player.transform.position = new Vector3(data.posX, data.posY, data.posZ);
            cc.enabled = true;

            player.HP = data.HP;
            player.hunger = data.hunger;

            for (int i = 0; i < inventoryManager.slots.Count; i++)
            {
                if (data.inventoryItemIDs[i] != -1)
                {
                    var item = itemDatabase.GetItemByID(data.inventoryItemIDs[i]);
                    inventoryManager.slots[i].item = item;
                    inventoryManager.slots[i].amount = data.inventoryAmounts[i];
                    inventoryManager.slots[i].SetIcon(item.icon);
                    inventoryManager.slots[i].isEmpty = false;
                    inventoryManager.slots[i].amountText.text = data.inventoryAmounts[i].ToString();
                }
                else
                {
                    inventoryManager.slots[i].NullifySlotData();
                }
            }

            quickslotInventory.LoadQuickslots(data.quickslotItemIDs, data.quickslotAmounts, itemDatabase);
        }
        else
        {
            Debug.LogWarning("No save file found!");
        }
    }

    public void Exit()
    {
        Application.Quit(1);
    }
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemScriptableObject> items;

    public ItemScriptableObject GetItemByID(int id)
    {
        if (id >= 0 && id < items.Count)
        {
            return items[id];
        }
        return null;
    }

    public int GetID(ItemScriptableObject item)
    {
        return items.IndexOf(item);
    }
}

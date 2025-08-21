using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    public ItemScriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGO;
    public TMP_Text amountText;

    public void NullifySlotData()
    {
        item = null;
        amount = 0;
        isEmpty = true;

        if (amountText != null)
        {
            amountText.text = "";
        }

        SetIcon(null);
    }

    private void Awake() {
        Transform dragObject = transform.GetChild(0);

        iconGO = dragObject.GetChild(0).gameObject;
        amountText = dragObject.GetChild(1).GetComponent<TMP_Text>();
    }

    public void SetIcon(Sprite sprite) {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGO.GetComponent<Image>().sprite = sprite;
    }
}

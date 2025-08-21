using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Player : MonoBehaviour {
    public Transform itemHolder;

    public GameObject currentItem;
    public ItemScriptableObject ItemPrefab;

    private Camera mainCamera;
    public InventoryManager invManager;

    public Image HPBar;
    public Image HungerBar;
    public Image StaminaBar;
    public GameObject background;
    public GameObject deathText;
    public GameObject hungerText;
    public GameObject inventory;
    public GameObject menu;

    public bool isAlive;
    public bool isInventory;
    public bool isMenu;

    public float minFallHeight = 3.0f;
    public float damageMultiplier = 7.5f;
    private float startFallHeight;
    private bool isFalling;

    public float maxHP = 100f;
    public float HP = 100f;
    public float HPRegen = 1f;

    public float maxHunger = 100f;
    public float hunger = 100f;
    public float hungerDrain = 2f;

    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaDrain = 15f;
    public float staminaRegen = 10f;

    public float damage;
    public float reachDistance = 2.5f;

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpDowngrade = 1f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning;

    void Start() {
        inventory.SetActive(true);
        isMenu = true;
        menu.SetActive(true);
        isInventory = true;
        mainCamera = Camera.main;
        isAlive = true;
        controller = GetComponent<CharacterController>();
        GetComponent<MeshRenderer>().enabled = false;
        HPBar.color = new Color(HPBar.color.r, HPBar.color.g, HPBar.color.b, 255);
        HungerBar.color = new Color(HungerBar.color.r, HungerBar.color.g, HungerBar.color.b, 255);
        StaminaBar.color = new Color(StaminaBar.color.r, StaminaBar.color.g, StaminaBar.color.b, 255);
        inventory.SetActive(false);
        isInventory = false;
        isMenu = false;
        menu.SetActive(false);
    }

    void Update() {
        if (isAlive) {
            if (!isInventory) {
                float moveX = Input.GetAxis("Horizontal");
                float moveZ = Input.GetAxis("Vertical");
                Vector3 move = transform.right * moveX + transform.forward * moveZ;

                isRunning = Input.GetKey(KeyCode.LeftShift) && stamina > 0 && hunger > maxHunger * 0.5f && (moveX != 0 || moveZ != 0);

                float currentSpeed = isRunning ? runSpeed : walkSpeed;
                controller.Move(move * currentSpeed * Time.deltaTime);

                if (Input.GetButtonDown("Jump") && isGrounded) {
                    velocity.y = Mathf.Sqrt(-2f * gravity) - jumpDowngrade;
                }

                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime);

                if (Input.GetKeyDown(KeyCode.E)) {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, reachDistance)) {

                        if (hit.collider.gameObject.GetComponent<Item>() != null) {
                            invManager.AddItemToInventory(hit.collider.gameObject.GetComponent<Item>().item, hit.collider.gameObject.GetComponent<Item>().amount);
                            Destroy(hit.collider.gameObject);
                        }
                    }
                }
            }

            if (isRunning)
            {
                stamina -= staminaDrain * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
            }
            else
            {
                stamina += staminaRegen * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
            }

            if (Input.GetKeyDown(KeyCode.Escape) && isInventory) {
                inventory.SetActive(false);
                isInventory = false;
                invManager.isOpened = false;
                background.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !isInventory && !isMenu)
            {
                background.SetActive(true);
                isMenu = true;
                menu.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Time.timeScale = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && !isInventory && isMenu)
            {
                background.SetActive(false);
                isMenu = false;
                menu.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                Time.timeScale = 1f;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isInventory)
                {
                    inventory.SetActive(false);
                    isInventory = false;
                    invManager.isOpened = false;
                    background.SetActive(false);

                }
                else
                {
                    inventory.SetActive(true);
                    isInventory = true;
                    invManager.isOpened = true;
                    background.SetActive(true);
                }
            }

            if (HP <= 0) {
                isAlive = false;
                Time.timeScale = 1;
                deathText.SetActive(true);

            }

            if (HP < maxHP) {
                HP += HPRegen * Time.deltaTime;
                HP = Mathf.Clamp(HP, 0, maxHP);
            }

            if(hunger > 0)
            {
                hunger -= hungerDrain * Time.deltaTime;
                hunger = Mathf.Clamp(hunger, 0, maxHunger);
            }

            if (hunger < maxHunger * 0.5f)
            {
                hungerText.SetActive(true);
            }
            else
            {
                hungerText.SetActive(false);
            }
        }

        isGrounded = controller.isGrounded;
        if (isGrounded) {
            if (isFalling) {
                isFalling = false;
                float fallDistance = startFallHeight - transform.position.y;
                if (fallDistance > minFallHeight) {
                    float damage = (fallDistance - minFallHeight) * damageMultiplier;
                    HP -= damage;
                }
            }
        } else {
            if (!isFalling) {
                isFalling = true;
                startFallHeight = transform.position.y;
            }
        }

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        HPBar.fillAmount = HP / maxHP;
        HungerBar.fillAmount = hunger / maxHunger;
        StaminaBar.fillAmount = stamina / maxStamina;
    }
}
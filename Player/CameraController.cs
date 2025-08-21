using UnityEngine;

public class CameraController : MonoBehaviour {
    private Player player;
    public float sensativity = 2.0f;
    public float maxYAngle = 80.0f;

    private float rotationX = 90.0f;

    private void Start() {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) {
            player = playerObj.GetComponent<Player>();
        }
    }
    public void Update() {
        if (player.isAlive) {
            if (!player.isInventory && !player.isMenu) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                transform.parent.Rotate(Vector3.up * mouseX * sensativity);

                rotationX -= mouseY * sensativity;
                rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);

                transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);

            } else {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}

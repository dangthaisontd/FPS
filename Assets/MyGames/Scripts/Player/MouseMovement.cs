using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("DangSon/MouseMovement")]
public class MouseMovement : MonoBehaviour
{
    public float mouseSensivity = 500f;
    public float xRotation;
    public float yRotation;
    public float topClamp = -90f;
    public float bottomClamp = 90f;
    //private Player player;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
      //  player = GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
      //  if (player != null& player.isPlayerDead) return;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") *mouseSensivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        yRotation += mouseX;
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}

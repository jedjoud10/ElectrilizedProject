using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject cameraVar;
    public bool canMove;
    public float sensivityCamera;
    private float XAxis;
    private Vector3 rot;
    private CharacterController cr;
    public float Speed;
    public float Gravity;
    public float JumpForce;
    private Vector3 mov;
    public float effort;
    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        cr = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        XAxis += Input.GetAxis("Mouse Y") * sensivityCamera;
        if (XAxis > 90)
        {
            XAxis = 90;
        }
        if (XAxis < -90)
        {
            XAxis = -90;
        }
        rot = cameraVar.transform.eulerAngles;
        rot.x = -XAxis;
        if (canMove)
        {
            cameraVar.transform.eulerAngles = rot;
            gameObject.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensivityCamera, 0));
        }
        if (cr.isGrounded)
        {
            if (canMove)
            {
                mov = gameObject.transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal") * Speed, 0, Input.GetAxis("Vertical") * Speed));
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    mov.y = JumpForce;
                }
                effort = mov.magnitude;
            }
            else
            {
                mov.x = 0;
                mov.z = 0;
            }
        }
        mov.y -= Gravity * Time.deltaTime;
        cr.Move(mov * Time.deltaTime);
    }
}

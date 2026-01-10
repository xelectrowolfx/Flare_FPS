using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    Vector3 moveDir;
    Vector3 playerVel;

    int JumpCount;
    int HPOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
    }
    void movement()
    {
        if (controller.isGrounded)
        {
            JumpCount = 0;
        }
        else
        {
            playerVel.y -= gravity * Time.deltaTime;
        }
            moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);
        jump();
        controller.Move(playerVel * Time.deltaTime);
       
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && JumpCount < jumpMax)
        {
            playerVel.y = jumpSpeed;
            JumpCount++;

        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }
}

using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("       Components      ")]
    [SerializeField] CharacterController controller;

    [Header("       Stats      ")]
    [Range(1,10)] [SerializeField] int HP;
    [Range(1, 10)][SerializeField] int speed;
    [Range(2, 5)][SerializeField] int sprintMod;
    [Range(1, 20)][SerializeField] int jumpSpeed;
    [Range(1, 3)][SerializeField] int jumpMax;

    [Header("       Physics      ")]
    [Range(15, 40)][SerializeField] int gravity;

    [Header("       Gun      ")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] int shootRate;

    Vector3 moveDir;
    Vector3 playerVel;

    float shootTimer;

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
        shootTimer += Time.deltaTime;

        if (controller.isGrounded)
        {
            JumpCount = 0;
        }
        else
        {
            playerVel.y -= gravity * Time.deltaTime;
        }
        if (Input.GetButton("Fire1") && shootTimer >= shootRate)
        {
            shoot();
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

    void shoot()
    {
        shootTimer = 0;

        RaycastHit Hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out Hit, shootDist))
        {
            Debug.Log(Hit.collider.name);
            IDamage dmg = Hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }

        }
        
   

    }
}

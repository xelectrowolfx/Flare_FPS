using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;
using Color = UnityEngine.Color;

public class enemyAI : MonoBehaviour, IDamage

{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    
    [SerializeField] GameObject bullet;

    [SerializeField] int HP;
    [SerializeField] float shootRate;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] LayerMask IgnoreLayer;

    [SerializeField] GameObject dropItem;


    Color colorOrig;
    
    float shootTimer;
    float angleToPlayer;

    Vector3 playerdir;

    bool playerinTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        GameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (playerinTrigger && CanSeePlayer())
        {
            
        }
        
    }
    bool CanSeePlayer()
    {
        playerdir = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerdir, transform.forward);
        Debug.DrawRay(headPos.position, playerdir);
        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerdir, out hit, float.MaxValue, ~IgnoreLayer))
        {
            if (angleToPlayer <= FOV && hit.collider.CompareTag("Player"))
            {
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (shootTimer >= shootRate)
                {
                    shoot();
                }


                return true;
            }
        }

     
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinTrigger = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerinTrigger = false;
        }
    }
    void shoot()
    {
        shootTimer = 0;

        Instantiate(bullet, shootPos.position, transform.rotation);

    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerdir.x, transform.position.y, playerdir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
    public void takeDamage(int amount)
    {
        HP -= amount;

        if(HP <= 0)
        {
            GameManager.instance.updateGameGoal(-1);
            if (dropItem != null)
            {
                Instantiate(dropItem, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashRed());
           
            
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;

    }
}

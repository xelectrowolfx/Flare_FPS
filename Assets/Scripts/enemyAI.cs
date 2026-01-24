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

    [SerializeField] int RoamDist;
    [SerializeField] int RoamPauseTime;

    [SerializeField] GameObject dropItem;


    Color colorOrig;

    float RoamTimer;
    float stoppingDistOrig;

    float shootTimer;
    float angleToPlayer;

    Vector3 playerdir;
    Vector3 startingPos;

    bool playerinTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
       // GameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
    }
    void Roam()
    {
        RoamTimer = 0;
        agent.stoppingDistance = 0;
        Vector3 ranPos = Random.insideUnitSphere * RoamDist;
        ranPos += startingPos;
        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, RoamDist, 1);
        agent.SetDestination(hit.position);


    }
    void checkRoam()
    {
        if(agent.remainingDistance < 0.01f && RoamTimer >= RoamPauseTime)
        {
            Roam();

        }
    }
 
    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if(agent.remainingDistance < 0.1f)
        {
            RoamTimer += Time.deltaTime;
        }
        
        if (playerinTrigger && !CanSeePlayer())
        {
           
            checkRoam();
        }
        else if (!playerinTrigger)
        {
           
            checkRoam();
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

                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }
        }

        agent.stoppingDistance = 0;
     
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
            agent.stoppingDistance = 0;
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

using UnityEngine;

public class pickupGuns : MonoBehaviour
{
    [SerializeField] GunStats gun;


    private void OnTriggerEnter(Collider other)
    {
        IPickup pik = other.GetComponent<IPickup>();

        if(pik != null)
        {
            pik.getGunSTats(gun);
            Destroy(gameObject);

        }
    }
}



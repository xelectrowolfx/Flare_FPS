using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Transform Door;
    [SerializeField] float Width;

    Vector3 Position;
       // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Position = Door.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenDoor()
    {
       // Door.localPosition = Door.localPosition.Set(Position.x += Width, Position.y, Position.z) ;
    }

    void CloseDoor()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) { return; }

            if (other.gameObject.CompareTag("Player"))
        {
            OpenDoor();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) { return; }
        if (other.gameObject.CompareTag("Player"))
        {
            CloseDoor();
        }
    }
}

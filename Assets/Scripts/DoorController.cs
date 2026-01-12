using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorController : MonoBehaviour
{
    [SerializeField] Transform Door;
    [SerializeField] float Width;
    [SerializeField] int OpenTime;
    [SerializeField] LayerField Layer;

    Vector3 Position;
    float doortimer;
       // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Position = Door.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        doortimer += Time.deltaTime;
    }

    void OpenDoor()
    { 
       Door.localPosition = new Vector3(Width, Position.y, Position.z);
    }

    void CloseDoor()
    {
        Door.localPosition = Position;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) { return; }

            if (other.gameObject.CompareTag("Player"))
        {
            OpenDoor();
            Debug.Log("Door Opening.");
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) { return; }
        if (other.gameObject.CompareTag("Player"))
        {
            CloseDoor();
            Debug.Log("Door Closing.");
        }
    }
}

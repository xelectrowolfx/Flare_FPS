using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("       Menu        ")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [Header("       Game State      ")]
    public bool isPaused;

    [Header("       Player Variable      ")]
    public GameObject player;
    public PlayerController playerScript;

    float timeScaleOrig;

    public static GameManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);

            }
            else if (menuActive == menuPause)
            {
                stateUnPause();
            }
        }
    }

    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void stateUnPause()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }
}

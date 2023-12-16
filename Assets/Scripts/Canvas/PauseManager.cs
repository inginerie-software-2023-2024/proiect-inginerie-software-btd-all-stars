using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool isPaused;
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public GameObject inventoryPanel;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Pause and unpause the game
        if(Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
            if(isPaused)
            {
                pausePanel.SetActive(true);
                player.SetActive(false);
                Time.timeScale = 0f;
                InventoryOpen();
            }
            else
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1f;
                player.SetActive(true);
            }
        }
    }

    //Close the pause menu
    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        player.SetActive(true);
    }

    //Switch from inventory panel to options panel
    public void OptionsOpen()
    {
        inventoryPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    
    //Switch from options panel to inventory panel
    public void InventoryOpen()
    {
        optionsPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }
}


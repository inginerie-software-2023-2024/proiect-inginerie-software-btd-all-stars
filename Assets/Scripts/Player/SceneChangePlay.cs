using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangePlay : MonoBehaviour
{
    public void Start()
    {
        SetRatio(4, 3);   
    }

    //Tries to set the resolution to the best 4:3 available from screen specs
    void SetRatio(float w, float h)
    {
        if ((((float)Screen.width) / ((float)Screen.height)) > w / h)
        {
            Screen.SetResolution((int)(((float)Screen.height) * (w / h)), Screen.height, true);
        }
        else
        {
            Screen.SetResolution(Screen.width, (int)(((float)Screen.width) * (h / w)), true);
        }
    }
    public void LoadScene(string sceneName)
    {
	    SceneManager.LoadScene(sceneName);
    }

    //Quiting the game
    public void Quit()
    {
        Application.Quit();
    }
}

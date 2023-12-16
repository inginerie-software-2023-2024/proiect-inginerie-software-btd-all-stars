using UnityEngine;

public class DialogPopUp : MonoBehaviour
{
    public GameObject window;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        window.SetActive(true);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        window.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DisableContinue : MonoBehaviour
{
    [SerializeField]
    private SaveManager _saveManager;

    [SerializeField]
    private Button _button;

    private void Awake()
    {
        _button.interactable = false;
        if (_saveManager.SaveExists)
        {
            _button.interactable = true;
        }
    }

}

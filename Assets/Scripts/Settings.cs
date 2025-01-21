using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Toggle zoomToggle;
    [SerializeField] GameObject removeSaveConfirmation;
    [SerializeField] Slider autosaveSlider;
    [SerializeField] TMP_Text autosaveTimerText;
    [SerializeField] TMP_Text UID;
    FirebaseUser user;

    [Header("Private variables")]
    [HideInInspector] public bool allowZoom;
    static public Settings instance = null;

    public void Startup()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        UID.text = user.UserId;
    }

    public void UpdateAutosaveSlider()
    {
        Saving.instance.timeBetweenAutosaves = (int)autosaveSlider.value;
        autosaveTimerText.text = autosaveSlider.value + "s";
    }

    public void UpdateAllowZoom()
    {
        allowZoom = zoomToggle.isOn;
    }

    public void UpdateUIToNewValues()
    {
        autosaveSlider.value = FindObjectOfType<Saving>().timeBetweenAutosaves;
        autosaveTimerText.text = autosaveSlider.value + "s";

        zoomToggle.isOn = allowZoom;
    }

    public void DeleteSaveConfirmation(bool value)
    {
        removeSaveConfirmation.SetActive(value);
    }

    public void CopyUID()
    {
        GUIUtility.systemCopyBuffer = user.UserId;
    }

    public void GoToPoolParty()
    {
        Saving.instance.SaveGame();
        SceneManager.LoadScene(2);
    }
}

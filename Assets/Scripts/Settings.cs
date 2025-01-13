using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Toggle zoomToggle;
    [SerializeField] GameObject removeSaveConfirmation;
    [SerializeField] Slider autosaveSlider;
    [SerializeField] TMP_Text autosaveTimerText;

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
}

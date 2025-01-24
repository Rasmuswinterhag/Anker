using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccountButton : MonoBehaviour
{
    [SerializeField] TMP_Text displayNameText;
    [SerializeField] TMP_Text UidText;
    [SerializeField] TMP_Text levelText;
    [HideInInspector] public TMP_InputField visitInputField;
    public string displayName;
    public string UID;
    public int level;

    public void SetData(PlayerData buttonIn)
    {
        displayName = buttonIn.displayName;
        UID = buttonIn.UID;
        level = buttonIn.level;
        SetTexts();
    }

    void SetTexts()
    {
        displayNameText.text = displayName;
        UidText.text = UID;
        levelText.text = "Level: " + level.ToString();
    }

    public void SetVisitId()
    {
        visitInputField.text = UID;
    }
}

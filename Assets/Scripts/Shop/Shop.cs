using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField, TextArea(3, 5)] string[] greetings;
    [SerializeField] AudioClip[] quacks;
    [SerializeField] TMP_Text shopKeeperField;

    public static Shop Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    void OnEnable()
    {
        ShopkeeperSay(greetings[Random.Range(0, greetings.Length)]);
    }

    public void ShopkeeperSay(string whatToSay)
    {
        shopKeeperField.text = whatToSay;
        AudioSource.PlayClipAtPoint(quacks[Random.Range(0, quacks.Length)], Camera.main.transform.position);
    }
}
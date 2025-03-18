using UnityEngine;
using TMPro; // F�r UI-Text mit TextMeshPro

public class WallWeaponBuy : MonoBehaviour
{
    public int weaponPrice = 1000; // Preis der Waffe
    public PlayerMoney playerMoney; // Referenz auf das Geldsystem
    public GameObject weaponPrefab; // Die Waffe, die gekauft wird
    public Transform spawnPoint; // Wo die Waffe erscheint
    public TextMeshProUGUI messageText; // TMP-Textfeld f�r Nachrichten

    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F)) // "F" dr�cken, um zu kaufen
        {
            TryPurchaseWeapon();
        }
    }

    private void TryPurchaseWeapon()
    {
        if (playerMoney == null) return; // Falls kein Geldsystem gefunden wurde

        if (playerMoney.money >= weaponPrice) // Hat der Spieler genug Geld?
        {
            playerMoney.money -= weaponPrice; // Geld abziehen
            Instantiate(weaponPrefab, spawnPoint.position, spawnPoint.rotation); // Waffe spawnen
            messageText.text = "Waffe gekauft!";
        }
        else
        {
            messageText.text = "Nicht genug Geld!";
        }

        StartCoroutine(ClearMessage());
    }

    System.Collections.IEnumerator ClearMessage()
    {
        yield return new WaitForSeconds(2f);
        messageText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Spieler betritt Kaufbereich
        {
            isPlayerNear = true;
            messageText.text = $"Dr�cke [Interact], um die Waffe f�r {weaponPrice}$ zu kaufen.";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Spieler verl�sst Kaufbereich
        {
            isPlayerNear = false;
            messageText.text = "";
        }
    }
}

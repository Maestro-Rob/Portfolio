using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoubleTapMachine : MonoBehaviour
{
    public int doubleTapPrice = 2000; // Preis für Double Tap
    public PlayerMoney playerMoney; // Referenz zum Geldsystem
    private Player player; // Referenz zum Spieler-Skript
    public TextMeshProUGUI messageText; // UI-Text für Nachrichten

    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            TryPurchaseDoubleTap();
        }
    }

    private void TryPurchaseDoubleTap()
    {
        if (player == null || playerMoney == null) return; // Sicherheitscheck

        if (playerMoney.money >= doubleTapPrice && !player.hasDoubleTap)
        {
            playerMoney.money -= doubleTapPrice;
            player.BuyDoubleTap();
            messageText.text = "Double Tap gekauft! Doppelter Schaden!";
        }
        else if (player.hasDoubleTap)
        {
            messageText.text = "Du hast bereits Double Tap!";
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
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            player = other.GetComponent<Player>(); // Spieler-Referenz setzen
            messageText.text = "Drücke [Interact], um Double Tap zu kaufen.";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            player = null; // Referenz zurücksetzen
            messageText.text = "";
        }
    }
}
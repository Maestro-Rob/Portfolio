using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JuggernogMachine : MonoBehaviour
{
    public int juggernogPrice = 2500;
    public PlayerMoney playerMoney; // Referenz zum Geldsystem

    public TextMeshProUGUI messageText; // UI-Text für Nachrichten

    private Player player; // Referenz zum Spieler-Skript

    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            TryPurchaseJuggernog();
        }
    }

    private void TryPurchaseJuggernog()
    {
        if (player == null || playerMoney == null) return; // Sicherheitscheck

        if (playerMoney.money >= juggernogPrice && !player.hasJuggernog)
        {
            playerMoney.money -= juggernogPrice;
            player.BuyJuggernog();
            messageText.text = "Juggernog gekauft! ";
        }
        else if (player.hasJuggernog)
        {
            messageText.text = "Du hast bereits Juggernog!";
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
            messageText.text = "Drücke [Interact], um Juggernog zu kaufen.";
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

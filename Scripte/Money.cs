using UnityEngine;
using TMPro;



public class PlayerMoney : MonoBehaviour
{
    public int money = 3000; // Startgeld
    public TextMeshProUGUI MoneyText;

    public void Update()
    {
        if (MoneyText != null)
        {
            MoneyText.text = $"Money:{money}";
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        MoneyText.text = $"Money:{money}";
        Debug.Log($"Spieler hat {amount} Geld erhalten! Neues Geld: {money}");
    }

}

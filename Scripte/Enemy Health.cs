using JetBrains.Annotations;
using System;
using UnityEngine;


public class Zombie : MonoBehaviour
{
    public int health = 100;
    public int damage = 10;
    public int damage2 = 10;


    private PlayerMoney playerMoney;  // Referenz zum PlayerMoney-Skript
    private PlayerMoney2 playerMoney2;

    public event Action OnDeath; // Event, das beim Tod des Zombies ausgelöst wird
    

    private void Awake()
{
    playerMoney = FindFirstObjectByType<PlayerMoney>(); // Holt das PlayerMoney-Skript aus der Szene
    if (playerMoney == null)
    {
        Debug.LogError("Kein PlayerMoney-Skript in der Szene gefunden!");
    }

    playerMoney2 = FindFirstObjectByType<PlayerMoney2>(); // Holt das PlayerMoney-Skript aus der Szene
    if (playerMoney2 == null)
    {
            Debug.LogError("Kein PlayerMoney-Skript in der Szene gefunden!");
    }
}

    
    
        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();
            Player2 player2 = other.GetComponent<Player2>();

            if (player)
            {
                player.TakeZomDamage(damage);
                Debug.Log($"Zombie hat den Spieler getroffen! Spieler hat jetzt {player.Health} HP.");
            }

            if (player2)
            {
                player2.TakeZomDamage2(damage2);
                Debug.Log($"Zombie hat Spieler 2 getroffen! Spieler 2 hat jetzt {player2.Health2} HP.");
            }
        }


    

    public void SetStats(int roundNumber)
    {
        health = 100 + (roundNumber - 1) * 50;
        damage = 10 + (roundNumber / 5) * 5;
    }


    public void TakeDamage(int damage)
    {
        
        


            health -= damage;



            if (health <= 0)
            {
                health = 0;


                Geld();

                OnDeath?.Invoke();
                Debug.Log("Der Gegner ist gestorben!");
                Destroy(gameObject);

            }
            else
            {
                MiniGeld();
                Debug.Log($"Der Gegner hat noch {health} HP.");

            }
        

        
    }
    public void TakeDamage2(int damage2)
    {



            health -= damage2;



            if (health <= 0)
            {
                health = 0;


                Geld2();

                OnDeath?.Invoke();
                Debug.Log("Der Gegner ist gestorben!");
                Destroy(gameObject);

            }
            else
            {
                MiniGeld2();
                Debug.Log($"Der Gegner hat noch {health} HP.");

            }
        
    }
    private void Geld () 
    {
        
        
        playerMoney.AddMoney(50); // Geld hinzufügen
            
        
    }
    private void MiniGeld()
    {


        playerMoney.AddMoney(10); // Geld hinzufügen


    }

    private void Geld2()
    {


        playerMoney2.AddMoney2(50); // Geld hinzufügen


    }
    private void MiniGeld2()
    {


        playerMoney2.AddMoney2(10); // Geld hinzufügen


    }
}


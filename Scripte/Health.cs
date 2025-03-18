using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
class Player: MonoBehaviour
{
    public int Health = 100;
    private int MaxHealth = 100;
    private const int BaseMaxHealth = 100;
    private const int JuggernogMaxHealth = 200;
    public int damageMultiplier = 1;
    private const int MaxWeapons = 3; // Maximal 3 Waffen gleichzeitig
    public List<PickupableWeapon> weapons = new List<PickupableWeapon>(); // Waffenliste
    private const int HealAmount = 5;
    private const int HealDelay = 5000; // 5 Sekunden Verzögerung nach Schaden
    private const int HealInterval = 1000; // 1 Sekunde pro Heilungsschritt
    private Timer healTimer;

    public Slider healthBar; // Health-Bar UI

    public bool hasJuggernog = false; // Überprüft, ob Juggernog aktiv ist
    public bool hasDoubleTap = false; // Überprüft, ob Doubletap aktiv ist


    private void Start()
    {
        healTimer = new Timer(AutoHeal, null, Timeout.Infinite, HealInterval);

        if (healthBar != null)
        {
            healthBar.maxValue = MaxHealth;
            healthBar.value = Health;
            UpdateHealthBar();
        }

    }


    public void TakeZomDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
            Debug.Log("Der Spieler ist gestorben!");
            Application.Quit();
            LoseJuggernog(); // Effekt verlieren beim Tod
            LoseDoubleTap();
        }
        else
        {
            Debug.Log($"Der Spieler hat noch {Health} HP.");
            RestartHealTimer();
        }
        UpdateHealthBar();
    }

    public bool HasWeapon(PickupableWeapon weapon) => weapons.Contains(weapon);

    public bool CanCarryMoreWeapons() => weapons.Count < MaxWeapons;

    public void AddWeapon(PickupableWeapon weapon)
    {
        if (weapons.Count < MaxWeapons)
        {
            weapons.Add(weapon);

        }

        
    }

    public void RemoveWeapon(PickupableWeapon weapon)
    {
        if (weapons.Contains(weapon))
        {
            weapons.Remove(weapon);
        }
    }

    public int GetWeaponCount() => weapons.Count;

    public PickupableWeapon GetWeaponByIndex(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            return weapons[index];
        }
        return null;
    }


private void RestartHealTimer()
    {
        if (healTimer != null)
        {
            healTimer.Change(HealDelay, HealInterval);
        }
    }

    private void AutoHeal(object state)
    {
        if (Health < MaxHealth)
        {
            Health += HealAmount;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            Debug.Log($"Automatische Heilung: Spieler hat nun {Health} HP.");
            UpdateHealthBar();
        }
        else
        {
            healTimer?.Change(Timeout.Infinite, HealInterval);
        }
    }

    public void BuyJuggernog()
    {
        if (!hasJuggernog)
        {
            hasJuggernog = true;
            MaxHealth = JuggernogMaxHealth;
            Health = JuggernogMaxHealth; // HP sofort erhöhen
            Debug.Log("Juggernog aktiviert! Max HP verdoppelt.");
        }
        else
        {
            Debug.Log("Du hast bereits Juggernog!");
        }
    }

    private void LoseJuggernog()
    {
        if (hasJuggernog)
        {
            hasJuggernog = false;
            MaxHealth = BaseMaxHealth; // Zurück auf normales Maximum
            Health = Mathf.Min(Health, MaxHealth); // Falls über 100, auf 100 reduzieren
            Debug.Log("Juggernog verloren! Max HP zurückgesetzt.");
        }
    }

    public void BuyDoubleTap()
    {
        if (!hasDoubleTap)
        {
            hasDoubleTap = true;
            damageMultiplier = 2; // Schaden verdoppeln
            Debug.Log("Double Tap aktiviert! Schaden verdoppelt.");
        }
        else
        {
            Debug.Log("Du hast bereits Double Tap!");
        }
    }

    private void LoseDoubleTap()
    {
        if (hasJuggernog)
        {
            hasDoubleTap = true;
            damageMultiplier = 1; // Schaden halbieren
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = Health;
        }
    }
}

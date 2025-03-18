using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private Player player; // Verweis auf den Player
    private PickupableWeapon currentWeapon;

    private void Start()
    {
        player = GetComponent<Player>(); // Player-Komponente holen
        if (player == null)
        {
            Debug.LogError("Kein Player-Skript gefunden!");
        }
    }

    private void Update()
    {
        if (player == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) EquipWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) EquipWeapon(2);
    }

    
    public void AddWeapon(PickupableWeapon newWeapon)
    {
        Debug.Log("Waffe wird hinzugefügt...");
        if (player.HasWeapon(newWeapon)) return; // Doppelte Waffen verhindern

        if (player.CanCarryMoreWeapons())
        {
            player.AddWeapon(newWeapon);
            newWeapon.SetEquipped(false);

            if (player.GetWeaponCount() == 1) EquipWeapon(0); // Erste Waffe sofort ausrüsten
        }

        if (currentWeapon != null) currentWeapon.SetEquipped(false);
    }

    public void EquipWeapon(int index)
    {
        PickupableWeapon weapon = player.GetWeaponByIndex(index);
        if (weapon == null) return;

        if (currentWeapon != null) currentWeapon.SetEquipped(false);
            
        currentWeapon = weapon;
        currentWeapon.SetEquipped(true);

        Debug.Log($"Waffe gewechselt zu: {currentWeapon.name}");
    }

    public void DropCurrentWeapon()
    {
        if (currentWeapon == null) return;

        player.RemoveWeapon(currentWeapon);
        
        currentWeapon = null;

        if (player.GetWeaponCount() > 0)
        {
            EquipWeapon(0);
        }
        else
        {
            Debug.Log("Keine Waffen mehr vorhanden.");
        }
    }
}

using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int baseDamage = 10; // Basis-Schaden der Kugel
    [SerializeField]
    private Player player; // Referenz zum Spieler
    int damage;

    public bool hasPenetration = false;



    private void OnTriggerEnter(Collider other)
    {
        Zombie zombie = other.GetComponent<Zombie>();
        if (zombie != null)
        {
            damage = baseDamage;

            // Wenn der Spieler Double Tap gekauft hat, den Schaden erh�hen
            if (player != null && player.hasDoubleTap)
            {

                damage *= player.damageMultiplier; // Schaden verdoppeln
                Debug.Log("player.damageMultiplier");
                Debug.Log("damage");
            }



            zombie.TakeDamage(damage); // Schaden an den Zombie anwenden

            if (!hasPenetration) 
            { 
                 Destroy(gameObject); // Zerst�re die Kugel nach Treffer
            }
        }
    }
}

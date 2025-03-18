using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;  // Prefab f�r den Zombie
    public Transform[] spawnPoints;  // Orte, an denen Zombies spawnen k�nnen

    public int roundNumber = 1;  // Aktuelle Runde
    public int baseZombieCount = 20; // Startanzahl der Zombies
    private List<GameObject> activeZombies = new List<GameObject>(); // Liste der aktiven Zombies

    void Start()
    {
        StartCoroutine(StartNextRound());
    }

    IEnumerator StartNextRound()
    {
        Debug.Log($"Runde {roundNumber} beginnt!");
        int zombieCount = baseZombieCount + Mathf.RoundToInt(roundNumber * 1.5f);

        for (int i = 0; i < zombieCount; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(0.5f); // Verz�gerung zwischen Spawns
        }

        // Warte, bis alle Zombies besiegt sind
        yield return new WaitUntil(() => activeZombies.Count == 0);

        roundNumber++;
        yield return new WaitForSeconds(3f); // Kleine Pause vor der n�chsten Runde
        StartCoroutine(StartNextRound());
    }

    void SpawnZombie()
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
        activeZombies.Add(zombie); // Zombie zur Liste hinzuf�gen

        // Statistiken f�r die aktuelle Runde setzen
        Zombie zombieScript = zombie.GetComponent<Zombie>();
        if (zombieScript != null)
        {
            zombieScript.SetStats(roundNumber);
            zombieScript.OnDeath += () => RemoveZombie(zombie); // Event f�r das Entfernen des Zombies
        }
    }

    void RemoveZombie(GameObject zombie)
    {
        activeZombies.Remove(zombie);
        Destroy(zombie);
    }
}

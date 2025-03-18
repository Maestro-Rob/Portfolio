using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAtandChase : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float MinDist;


    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FindNearestTarget();

        Vector3 LookAtOwnYpos = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
        this.transform.LookAt(LookAtOwnYpos);

        if (Vector3.Distance(this.transform.position, target.transform.position) >= MinDist)
        {
            this.transform.position += transform.forward * speed * Time.deltaTime;

        }

       
        // Bewege den Gegner nur, wenn er nicht bereits nah genug ist
        float distance = Vector3.Distance(this.transform.position, target.transform.position);
        if (distance > MinDist)
        {
            this.transform.position += transform.forward * speed * Time.deltaTime;
        }
        

    }

    void FindNearestTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            Debug.LogWarning("Keine Spieler im Spiel!");
            target = null;
            return;
        }

        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(this.transform.position, player.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPlayer = player;
            }
        }

        target = nearestPlayer;
    }

   
}

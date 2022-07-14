using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [SerializeField] GameObject aiPrefab;
    [SerializeField] Transform spawnPoint;

    public List<DestinationPoint> destinationPoint = new List<DestinationPoint>();
    [SerializeField] float delayToSpawn = 5f;

    public List<string> tags = new List<string>();

    public AIPlaces aiPlaces;
    public int countBots;
    //public bool onesBording;

    //float startDelay;

    private void Start()
    {
        //startDelay = delayToSpawn;
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            var value = aiPlaces.mainPlace.Count;
            if (countBots < aiPlaces.allPlace.Count + value)
            {
                int rand = Random.Range(0, destinationPoint.Count);

                while (destinationPoint[rand].CurrentQueuePosition() == Vector3.zero)
                {
                    rand = Random.Range(0, destinationPoint.Count);
                }

                GameObject ai = Instantiate(aiPrefab, spawnPoint.position, spawnPoint.rotation);
                AIController aiController = ai.GetComponent<AIController>();
                aiPlaces.bots.Add(aiController);

                aiController.SetDestination(destinationPoint[rand].CurrentQueuePosition());
                aiController.queue = destinationPoint[rand];
                destinationPoint[rand].AddToTheQueue(aiController);
                countBots++;
            }
            yield return new WaitForSeconds(delayToSpawn);
        }
    }

    public void AddCashPay(DestinationPoint cashPay)
    {
        destinationPoint.Add(cashPay);
    }

    public void RefreshTags()
    {
        tags.RemoveRange(1, tags.Count-1);
    }

    public bool CheckTags(string tag)
    {
        foreach (var i in tags)
            if (i == tag) return false;
        return true;
    }
}

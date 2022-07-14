using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestinationPoint : MonoBehaviour
{
    [SerializeField] Vector3 offsetDelay;
    List<Vector3> queue = new List<Vector3>();
    List<AIController> agents = new List<AIController>();
    [SerializeField] int maxQueueCount = 8;

    [HideInInspector] public AIController firstInTheQueue;

    [Space]
    [Space]

    [SerializeField] private Sprite background;
    [SerializeField] private Sprite redImg;
    [SerializeField] private Sprite blueImg;
    [SerializeField] private Sprite greenImg;

    public SpriteRenderer []neededImage;
    public SpriteRenderer []backgroundImg;
    public TextMeshPro []neededText;

    private void Awake()
    {
        Vector3 startPos = transform.position;

        for (int i = 0; i < 100; i++)
        {
            queue.Add(transform.position);
            transform.position += offsetDelay;
        }

        transform.position = startPos;
    }

    public void AddToTheQueue(AIController ai)
    {
        agents.Add(ai);

        if (agents.Count == 1)
        {
            firstInTheQueue = ai;
        }

        //if (agents.Count > maxQueueCount) lvlManager.GameOver();
    }

    public void RemoveFromTheQueue()
    {
        agents.RemoveAt(0);

        //for (int i = 0; i < agents.Count; i++)
        //{
        //    agents[i].SetDestination(queue[i]);
        //}
        if(agents.Count > 0)
        {
            firstInTheQueue = agents[0];
        }
        else
        {
            //UpdateDisplay("null", "",);
        }
    }
    public void UpdateDisplay(string neededSprite, string neededCount,int numberTable)
    {
        switch (neededSprite)
        {
            case "RED":
                neededImage[numberTable].sprite = redImg;
                backgroundImg[numberTable].sprite = background;
                break;
            case "BLUE":
                neededImage[numberTable].sprite = blueImg;
                backgroundImg[numberTable].sprite = background;
                break;
            case "GREEN":
                neededImage[numberTable].sprite = greenImg;
                backgroundImg[numberTable].sprite = background;
                break;
            case "null":
                neededImage[numberTable].sprite = null;
                backgroundImg[numberTable].sprite = null;
                break;
            default:
                break;
        }

        neededText[numberTable].text = neededCount;
    }
    public Vector3 CurrentQueuePosition()
    {
        if (agents.Count >= queue.Count)
        {
            return Vector3.zero;
        }
        else
        {
            return queue[agents.Count];
        }
    }
}

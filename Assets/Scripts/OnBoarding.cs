using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoarding : MonoBehaviour
{
    public static OnBoarding onBoarding;

    public ObjectsBoarding[] onObjectsBoardings;
    public GameObject[] offObjectsBoardings;
    GameObject instruction8;

    private void Start()
    {
        onBoarding = this;
        instruction8 = FindObjectOfType<PlayerMove>().instruction8;
    }
    public void Boarding(int next)
    {
        for (int i = 0; i < offObjectsBoardings.Length; i++)
        {
            if (offObjectsBoardings[next] != null)
                offObjectsBoardings[next].gameObject.SetActive(false);
        }
        for (int i = 0; i < onObjectsBoardings[next].objects.Length; i++)
        {
            if (onObjectsBoardings[next] != null)
                onObjectsBoardings[next].objects[i].gameObject.SetActive(true);
        }
        instruction8.SetActive(false);
    }
}

[System.Serializable]
public class ObjectsBoarding
{
    public GameObject[] objects;
}

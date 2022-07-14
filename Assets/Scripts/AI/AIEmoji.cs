using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEmoji : MonoBehaviour
{
    [SerializeField] ParticleSystem[] smileEmoji;
    [SerializeField] ParticleSystem[] sadEmoji;
    [SerializeField] float speedMove;
    int isSmile;
    int randEmoji;

    private Vector3 vectortar;
    private Vector3 vectortar1;
    private float float_shrink = 1;

    private void Start()
    {
        
        vectortar = (StaticObject.instance.targetForEmoji.position);
    }

    public void Smile()
    {
        int rand = Random.Range(0, smileEmoji.Length);
        randEmoji = rand;
        smileEmoji[rand].Play();
        isSmile = 1;
    }

    public void Sad()
    {
        int rand = Random.Range(0, sadEmoji.Length);
        randEmoji = rand;
        sadEmoji[rand].Play();
        isSmile = 2;
    }

    void Update()
    {
        if (isSmile == 1)
        {
            StartCoroutine("Ieniter");
        }
    }

    IEnumerator Ieniter()
    {
        while (true)
        {
            vectortar1 = Camera.main.WorldToScreenPoint(smileEmoji[0].transform.position);

            if (Vector3.Distance(vectortar, vectortar1) > 60f)
            {
                float_shrink -= Time.deltaTime;
                vectortar1 = Vector3.MoveTowards(vectortar1, vectortar, speedMove);
                smileEmoji[0].transform.position = Camera.main.ScreenToWorldPoint(vectortar1);
                smileEmoji[0].transform.localScale = new Vector3(float_shrink, float_shrink, float_shrink);
                if (float_shrink <= 0)
                {
                    smileEmoji[0].transform.localScale = Vector3.zero;
                }
            }
            else
            {
                smileEmoji[0].transform.localScale = Vector3.zero;
                StopCoroutine("Ieniter");
                smileEmoji[0].gameObject.SetActive(false);
            }
            yield return null;
        }

    }

}

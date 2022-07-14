using System.Collections.Generic;
using UnityEngine;

public class AIHolder : MonoBehaviour
{
    private Animator anim;
    private AIController aiController;

    [HideInInspector]public List<SmoothLerp> objects = new List<SmoothLerp>();
    [SerializeField] Transform holderPoint;
    [SerializeField] AIRecieveTrigger neededItems;

    int neededCount = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
        aiController = GetComponent<AIController>();

        neededCount = neededItems.count;
    }

    void UpdateObjects()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].parentObject = (i == 0) ? holderPoint : objects[i - 1].transform;
            objects[i].lerpTime = 50;
        }
    }

    public void RecieveObject(SmoothLerp obj,string pillType,Holder holder)
    {
        SmoothLerp objectMoveController = obj;

        objectMoveController.activeMove = true;

        objects.Add(objectMoveController);
        UpdateObjects();
        anim.SetBool("isEmpty", false);

        neededCount--;
        aiController.UpdateDisplayNum(neededCount,aiController.currentPlace.id-1);

        if (neededCount <= 0)
        {
            if (pillType == "RED")
                holder.AddPopular(20);
            if (pillType == "BLUE")
                holder.AddPopular(25);
            else if (pillType == "Green")
                holder.AddPopular(30);

            aiController.GoBack();
            aiController.queue.RemoveFromTheQueue();
            neededItems.GetComponent<Collider>().enabled = false;
            neededItems.aiSpawner.countBots--;
            neededItems.aiSpawner.aiPlaces.bots.Remove(this.aiController);
            StaticObject.instance.botsGetRes.Add(this.aiController.gameObject);
            if (LevelManager.lvl == 0 && StaticObject.instance.onesBording ==false)
            {
                StaticObject.instance.onesBording = true;
                OnBoarding.onBoarding.Boarding(5);
            }
            aiController.currentPlace.busy = false;
            foreach (var i in aiController.aiPlaces.bots)
                if (!i.isSitMain)
                {
                    i.isSit = 1;
                    i.Go();
                    return;
                }
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if(objects[i] != null) objects[i].gameObject.SetActive(false);
        }
        objects.Clear();
    }
}

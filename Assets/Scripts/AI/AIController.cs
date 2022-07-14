using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public AIPlaces aiPlaces;
    AIEmoji emoji;
    NavMeshAgent agent;
    Animator anim;
    ObjectPooling objectPooling;
    PlayerMove player;
    Vector3 startPos;

    public DestinationPoint queue;

    [SerializeField] AIRecieveTrigger neededItems;
    [SerializeField] Collider coll;
    [SerializeField] GameObject[] skins;

    [HideInInspector] public AIPlace currentPlace;

    public int isSit;
    public bool isSitMain;
    bool isStopped = true;
    bool goBack = false;

    [SerializeField] float timingSmileEmoji = 10f;

    private void Awake()
    {
        aiPlaces = FindObjectOfType<AIPlaces>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        emoji = GetComponent<AIEmoji>();
        player = FindObjectOfType<PlayerMove>();
        objectPooling = ObjectPooling.Instance;
    }

    private void Start()
    {
        startPos = transform.position;

        foreach (GameObject skin in skins)
        {
            skin.SetActive(false);
        }
        int rand = Random.Range(0, skins.Length);
        skins[rand].SetActive(true);
    }

    private void Update()
    {
        if (isSit==0)
        {
            var place = aiPlaces.FreeMainPlace();

            for (int i = 0; i < aiPlaces.mainPlace.Count; i++)
                if (aiPlaces.mainPlace[i].isSitting == false)
                {
                    if (place != null)
                    {
                        anim.SetFloat("Speed", 1.4f);
                        anim.SetBool("Sitting", false);
                        agent.SetDestination(place.transform.GetChild(0).position);
                        place.busy = true;
                        isSit = -1;
                        isSitMain = true;
                        place.isSitting = true;
                        return;
                    }
                }

            if (agent.remainingDistance == 0)
                agent.SetDestination(aiPlaces.FindFreePlace(transform).position);
        }

        if (!isStopped)
        {
            float dist = agent.remainingDistance;
            if (dist != Mathf.Infinity && agent.remainingDistance == 0)
            {
                anim.Play("isEmpty");
                anim.SetFloat("Speed", 0);

                isStopped = true;
            }
        }

        if (goBack)
        {
            float dist = agent.remainingDistance;
            if (dist != Mathf.Infinity && agent.remainingDistance <= 2)
            {
                agent.enabled = false;
                Destroy(gameObject);
            }
        }
    }

    public void Go()
    {
        var place = aiPlaces.FreeMainPlace();
        if (place != null && isSit == 1)
        {
            anim.SetFloat("Speed", 1.4f);
            anim.SetBool("Sitting", false);
            agent.SetDestination(place.transform.GetChild(0).position);
            place.busy = true;
            isSit = -1;
            isSitMain = true;
        }
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(rotation.x, Mathf.Clamp(rotation.y, -20, 20), rotation.z, 2f), 2 * Time.deltaTime);
    }

    public void FirstInTheQueue()
    {
        coll.enabled = true;
        //queue.UpdateDisplay(neededItems.currentTag, neededItems.count.ToString());
    }

    public void UpdateDisplayNum(int num, int numberTable)
    {
        queue.UpdateDisplay(neededItems.currentTag, num.ToString(), numberTable);
    }

    public void SetDestination(Vector3 position)
    {
        coll.enabled = false;

        anim.SetFloat("Speed", 1.4f);
        anim.SetBool("Sitting", false);
        agent.SetDestination(position);
    }

    public void GoBack()
    {
        SetDestination(startPos);
        //queue.UpdateDisplay("null", "");
        goBack = true;

        emoji.Smile();
        //emoji.Sad();

        int count = neededItems.price;

        for (int i = 0; i < count; i++)
        {
            GameObject coin = objectPooling.SpawnFromPool("coin", new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
            CoinController coinController = coin.GetComponent<CoinController>();
            coinController.followedTarget = player.transform;
            coinController.fastFollow = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            isSit = -1;
            isStopped = false;
            anim.SetBool("Sitting", true);
        }
        if (other.gameObject.layer == 9)
        {
            var place = other.gameObject.GetComponent<AIPlace>();
            //if (place.aiController!=this) return;
            isSit = -1;
            isSitMain = true;
            isStopped = false;
            neededItems.GetComponent<Collider>().enabled = true;
            queue.backgroundImg[place.id - 1].gameObject.SetActive(true);
            UpdateDisplayNum(neededItems.count, place.id - 1);
            anim.SetBool("Sitting", true);
            currentPlace = place;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            other.gameObject.GetComponent<AIPlace>().isSitting = false;
            queue.backgroundImg[other.gameObject.GetComponent<AIPlace>().id - 1].gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var layer = other.gameObject;
        if (layer.layer == 8 || layer.layer==9)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, layer.transform.localEulerAngles.y, transform.eulerAngles.z);
    }
}

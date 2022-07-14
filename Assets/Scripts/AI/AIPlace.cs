using UnityEngine;

public class AIPlace : MonoBehaviour
{
    public int id;
    public bool busy,isSitting;
    public AIController aiController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AIController>(out AIController aIController))
        {
            busy = true;
            aiController = other.gameObject.GetComponent<AIController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<AIController>(out AIController aIController))
        {
            if(id==0)busy = false;
            aiController = null;
        }
    }
}

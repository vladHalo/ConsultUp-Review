using TMPro;
using UnityEngine;

public class PillsGenerator : MonoBehaviour
{
    public int resorceCount = 0;
    [SerializeField] TextMeshPro resDisplay;
    
    public void UpdDisplay()
    {
        resDisplay.text = resorceCount.ToString();
    }

    public void UpdDisplayFail(int l)
    {
        resDisplay.text = l.ToString();
    }
}

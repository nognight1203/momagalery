using UnityEngine;
using UnityEngine.EventSystems;

public class PointTrigger : MonoBehaviour, IPointerClickHandler
{
    public string pointID;

    public void OnPointerClick(PointerEventData eventData)
    {
        print(pointID);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

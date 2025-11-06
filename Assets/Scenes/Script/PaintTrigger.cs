using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PaintTrigger : MonoBehaviour, IPointerClickHandler
{
    public string paintID;

    public FirebaseWebRequest FirebaseWebRequest;

    public void OnPointerClick(PointerEventData eventData)
    {

        // PointTrigger.newPaint = this.gameObject;
        FirebaseWebRequest.DeletePaint(paintID);
        Destroy(this.gameObject);
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

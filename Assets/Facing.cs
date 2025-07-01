using UnityEngine;

public class Facing : MonoBehaviour
{
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 foraward = transform.forward;
        //Debug.DrawLine(transform.position, foraward * 2, Color.blue);
        //print(transform.forward);
    }
}

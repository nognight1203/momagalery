using UnityEngine;

public class Move : MonoBehaviour
{

    public GameObject selfObj;
    public float currentX;
    public float MaxX;
    public float MinX;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        selfObj.transform.position = new Vector3(currentX, 0, 0);

        currentX = currentX + speed;
        if(currentX >= MaxX || currentX <= MinX)
        {
            speed = speed * -1;
        }
            
    }
}

using UnityEngine;
using System.Collections.Generic;

public class SaveData : MonoBehaviour
{


    public AllThePaints allThePaints = new AllThePaints();

    public SetPaintings setPaintings;


    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            
        }
    }

    void PaintSetData()
    {
        Vector3 pantPosToSave = setPaintings.PaintPosToSave;

    }
    
}

[System.Serializable]
public class AllThePaints
{
    public Vector3[] PaintPos;

    public List<PaintData> paintDataList = new List<PaintData>();

}
[System.Serializable]
public class PaintData
{
    public Vector3 PaintPosition;
    public int PanintID;
}



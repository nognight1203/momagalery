using UnityEngine;

public class UIControler : MonoBehaviour
{
    public GameObject Setting;

    [Header("Chose Paint UI")]

    public GameObject UI;
    public GameObject PaintsScrollView;

    [Header("Add Mode")]
    public GameObject Add;
    public GameObject Done;
    public GameObject Cancel;
    

    public void ShowChosingPaintsUI()
    {
        PaintsScrollView.SetActive(true) ;
        Setting.SetActive(false);
    }

    public void CloseChosingPaintsUI()
    {
        PaintsScrollView.SetActive(false);
        Setting.SetActive(true);
    }

    //AddMode
    public void AddModeON()
    {
        Add.SetActive(false);
        Done.SetActive(true);
        Cancel.SetActive(true);
        PointTrigger.AddMode = true;
    }
    public void AddModeOFF()
    {
        Add.SetActive(true);
        Done.SetActive(false);
        Cancel.SetActive(false);
        PointTrigger.AddMode = false;
    }
    public void AddDone()
    {
        PointTrigger.PaintDataDic.Add(PointTrigger.SelectedPosID,PointTrigger.newPaint);
        PointTrigger.newPaint = null;
        PointTrigger.selectedPaint = null;
        foreach(var value in PointTrigger.PaintDataDic)
        {
            string textureName = value.Value.GetComponent<Renderer>().material.mainTexture.name.ToString();
            print($"ID:{value.Key} ,TexureName:{textureName}");
        }
        //print(PointTrigger.PaintDataDic.ToString());
    }
    public void AddCancel()
    {

    }



}

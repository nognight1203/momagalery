using UnityEngine;

public class UIControler : MonoBehaviour
{
    public GameObject Setting;

    [Header("Chose Paint UI")]

    public GameObject UI;
    public GameObject PaintsScrollView;
    public GameObject DeletButton;

    [Header("Add Mode")]
    public GameObject Add;
    public GameObject Done;
    public GameObject Cancel;
    public GameObject PaintScaleButtome;
    

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
        PointTrigger.SeletScale = (float)PaintScale.mid; PointTrigger.scalePaint = mid.ToString();
        Add.SetActive(false);
        Done.SetActive(true);
        Cancel.SetActive(true);
        PaintScaleButtome.SetActive(true);
        PointTrigger.AddMode = true;
    }
    public void AddModeOFF()
    {
        Add.SetActive(true);
        Done.SetActive(false);
        Cancel.SetActive(false);
        PaintScaleButtome.SetActive(false);
        PointTrigger.AddMode = false;
    }
    public void AddDone()
    {
        PointTrigger.PaintDataDic.Add(PointTrigger.SelectedPosID,PointTrigger.newPaint);
        //PointTrigger.newPaint = null;
        //PointTrigger.selectedPaint = null;
       /* foreach(var value in PointTrigger.PaintDataDic)
        {
            string textureName = value.Value.GetComponent<Renderer>().material.mainTexture.name.ToString();
            print($"ID:{value.Key} ,TexureName:{textureName}");
        }*/
        //print(PointTrigger.PaintDataDic.ToString());
    }
    public void AddCancel()
    {
        Destroy(PointTrigger.newPaint);
    }

    enum PaintScale
    {
        max = 4,
        mid = 2,
        min = 1
    }
    float max = (float)PaintScale.max;
    float mid = (float)PaintScale.mid;
    float min = (float)PaintScale.min;
    public void SelectScaleMax()
    {
        PointTrigger.SeletScale = (float)PaintScale.max; PointTrigger.scalePaint = max.ToString();
        PointTrigger.newPaint.transform.localScale = new Vector3(PointTrigger.SeletScale * (float)(PointTrigger.TextureTolerence), PointTrigger.SeletScale, 0.25f);
    }
    public void SelectScaleMid()
    {
        PointTrigger.SeletScale = (float)PaintScale.mid; PointTrigger.scalePaint = mid.ToString();
        PointTrigger.newPaint.transform.localScale = new Vector3(PointTrigger.SeletScale * (float)(PointTrigger.TextureTolerence), PointTrigger.SeletScale, 0.25f);
    }
    public void SelectScaleMin()
    {
        PointTrigger.SeletScale = (float)PaintScale.min; PointTrigger.scalePaint = min.ToString();
        PointTrigger.newPaint.transform.localScale = new Vector3(PointTrigger.SeletScale * (float)(PointTrigger.TextureTolerence), PointTrigger.SeletScale, 0.25f);
    }

    public void DeletePaint()
    {
        PaintTrigger.seletSetPaint.GetComponent<PaintTrigger>().Delete();
        DeletButton.SetActive(false);

    }


}

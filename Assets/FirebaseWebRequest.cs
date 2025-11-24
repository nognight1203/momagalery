using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;


public class FirebaseWebRequest : MonoBehaviour
{
    public string dbUrl;

    private string version = "1.0";

    public GameObject seterPanitPrefab;

    /* public void SavePaint(string paintID, string paintName)
     {
         StartCoroutine(SetValue(paintID, paintName));
     }*/

    public string dd;
    public string dd2;
    public string dd3;

    public GameObject DeleteButton;
    public void testsend()
    {
        
        StartCoroutine(SetValue(PointTrigger.SelectedPosID, PointTrigger.newPaint.GetComponent<Renderer>().material.mainTexture.name,PointTrigger.scalePaint));
        PointTrigger.newPaint.AddComponent<PaintTrigger>();
        PointTrigger.newPaint.GetComponent<PaintTrigger>().paintID = PointTrigger.SelectedPosID;
        PointTrigger.newPaint.GetComponent<PaintTrigger>().FirebaseWebRequest = this;
        PointTrigger.newPaint.GetComponent<PaintTrigger>().paintTextureName = PointTrigger.newPaint.GetComponent<Renderer>().material.mainTexture.name;
        PointTrigger.newPaint.GetComponent<PaintTrigger>().scale = float.Parse(PointTrigger.scalePaint);
        PointTrigger.newPaint.GetComponent<PaintTrigger>().DeleteButton = DeleteButton;
       // PointTrigger.newPaint = null;
       // PointTrigger.selectedPaint = null;
    }

    private void Start()
    {
        GetAll();
    }



    IEnumerator SetValue(string paintID, string paintName,string scale)
    {
        string url = $"{dbUrl}PaintID/{paintID}.json";
        PaintDatasFirebase Data = new PaintDatasFirebase {paintsName = paintName,paintsScale = scale,Version = version };
        //string json = $"\"{paintName}\""; // 或 $"\"{paintName}\""
        string json = JsonUtility.ToJson(Data);
        UnityWebRequest request = UnityWebRequest.Put(url, json);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
        if (request.result != UnityWebRequest.Result.Success)
#else
        if (request.isNetworkError || request.isHttpError)
#endif
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            Debug.Log($"✅ Saved paint: {paintName}");
        }
        PointTrigger.newPaint = null;
        PointTrigger.selectedPaint = null;
    }


    public void GetAll()
    {
        StartCoroutine(GetAllPaintData());
    }

    IEnumerator GetAllPaintData()
    {
        string url = $"{dbUrl}PaintID.json";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
        if (request.result != UnityWebRequest.Result.Success)
#else
        if (request.isNetworkError || request.isHttpError)
#endif
        {
            Debug.LogError($"❌ Error: {request.error}");
            Debug.LogError($"Response: {request.downloadHandler.text}");
        }
        else
        {
            string json = request.downloadHandler.text;
            Debug.Log($"📥 Raw JSON: {json}");

            // ✅ 使用 Newtonsoft.Json 解析成 Dictionary
            var paintDict = JsonConvert.DeserializeObject<Dictionary<string, PaintDatasFirebase>>(json);

            if (paintDict == null)
            {
                Debug.LogWarning("⚠️ 沒有資料或格式錯誤");
                yield break;
            }

            foreach (var kvp in paintDict)
            {
                string id = kvp.Key;
                PaintDatasFirebase data = kvp.Value;

                GameObject SetPaint = Instantiate(seterPanitPrefab);
                SetPaint.transform.position = PaintSetPointsSetter.PointsDic[id].transform.position;
                SetPaint.transform.rotation = PaintSetPointsSetter.PointsDic[id].transform.rotation;
               // Texture texture = SupabaseWebGLExample.TextureIamge[data.paintsName].GetComponent<Renderer>().material.mainTexture;
               // SetPaint.GetComponent<Renderer>().material.mainTexture = texture;
                SetPaint.AddComponent<PaintTrigger>();
                SetPaint.GetComponent<PaintTrigger>().paintID = id;
                SetPaint.GetComponent<PaintTrigger>().FirebaseWebRequest = this;
                SetPaint.GetComponent<PaintTrigger>().paintTextureName = data.paintsName;
                SetPaint.GetComponent<PaintTrigger>().scale = float.Parse(data.paintsScale);
                SetPaint.GetComponent<PaintTrigger>().DeleteButton = DeleteButton;

               // PaintTrigger.ActivePaints.Add(id, SetPaint);
                
                //SetPaint.transform.localScale = new Vector3(float.Parse(data.paintsScale) * (float)texture.width / (float)texture.height, float.Parse(data.paintsScale), 0.25f);


                Debug.Log($"🖼️ ID: {id} | Name: {data.paintsName} | Scale: {data.paintsScale}");
            }
        }
    }

    public void DeleteNoTexturePaint(string textureName)
    {
       foreach(GameObject gameObject in PaintTrigger.ActivePaints.Values)
        {
            print(gameObject.GetComponent<PaintTrigger>().paintTextureName);
            if(gameObject.GetComponent<PaintTrigger>().paintTextureName == textureName)
            {
                gameObject.GetComponent<PaintTrigger>().Delete();
            }
        }
    }
    enum PaintScale
    {
        max = 4,
        mid = 2,
        min = 1
    }

    public void DeletePaint(string paintID)
    {
        StartCoroutine(DeleteCoroutine(paintID));
    }

    IEnumerator DeleteCoroutine(string paintID)
    {
        string url = $"{dbUrl}PaintID/{paintID}.json";
        UnityWebRequest request = UnityWebRequest.Delete(url);

        yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
        if (request.result != UnityWebRequest.Result.Success)
#else
        if (request.isNetworkError || request.isHttpError)
#endif
        {
            Debug.LogError($"❌ 刪除失敗: {request.error}");
        }
        else
        {
            Debug.Log($"✅ 成功刪除 {paintID}");
        }
    }
}

public class PaintDatasFirebase
{
    //public  string paintsID ;
    public string paintsName;
    public string paintsScale;
    public string Version;
    //public  GameObject paintInstence;




}
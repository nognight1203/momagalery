using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FirebaseWebRequest : MonoBehaviour
{
    public string dbUrl;

    /* public void SavePaint(string paintID, string paintName)
     {
         StartCoroutine(SetValue(paintID, paintName));
     }*/

    public string dd;
    public string dd2;
    public void testsend()
    {
        StartCoroutine(SetValue(dd, dd2));
    }



    IEnumerator SetValue(string paintID, string paintName)
    {
        string url = $"{dbUrl}PaintID/{paintID}.json";
        string json = $"\"{paintName}\""; // 或 $"\"{paintName}\""

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
    }
}
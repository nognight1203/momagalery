using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaintTrigger : MonoBehaviour, IPointerClickHandler
{
    public string paintID;
    public string paintTextureName;
    public float scale;
    public static GameObject seletSetPaint;

    public GameObject DeleteButton;

    public FirebaseWebRequest FirebaseWebRequest;

    //public static Dictionary<string, GameObject> ActivePaints = new Dictionary<string, GameObject>();

     
    public void OnPointerClick(PointerEventData eventData)
    {

        // PointTrigger.newPaint = this.gameObject;
        seletSetPaint = this.gameObject;
        DeleteButton.SetActive(true);
    }

    public void Delete()
    {
        
        seletSetPaint = null;
        FirebaseWebRequest.DeletePaint(paintID);
        Destroy(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitForKey(paintTextureName));
       // ActivePaints.Add(paintID,this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitForKey(string key)
    {
        //Debug.Log($"🔍 開始等待 key = {key}");

        // 不斷檢查，直到字典中有這個 key 為止
        while (!SupabaseWebGLExample.TextureIamge.ContainsKey(key))
        {
            yield return null; // 每幀檢查一次
        }

        // Debug.Log($"✅ 找到 key: {key}");
        Texture texture = SupabaseWebGLExample.TextureIamge[key].GetComponent<Renderer>().material.mainTexture;
        this.GetComponent<Renderer>().material.mainTexture = texture;
        this.transform.localScale = new Vector3(scale * (float)texture.width / (float)texture.height, scale, 0.25f);

        // 可以在這裡對 obj 做事情
    }
}

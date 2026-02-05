using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;





public class SupabaseWebGLExample : MonoBehaviour
{
    [Header("Supabase Settings")]
    public string projectUrl = "https://ekxltzwemixjlndwqujw.supabase.co";
    public string anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImVreGx0endlbWl4amxuZHdxdWp3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTQ5NzUzNTQsImV4cCI6MjA3MDU1MTM1NH0.O2JDGPxwyimQC0eSCfj3fNMKK7BL54knWBYYx0HCaeY";
    public string bucketName = "galerypicture";

    [Header("UI")]
    public RawImage displayImage; // 顯示下載的圖片
    public GameObject Success;
    public GameObject Fail;

    // 範例圖片（你可以用任何 Texture2D）
    public Texture2D imageToUpload;

    public TextMeshProUGUI infor;

    public TextMeshProUGUI bucketListText;

    public FrostweepGames.Plugins.WebGLFileBrowser.Examples.LoadFileExample loadFileExample;

    public static string SelectGalleryName;

    [Header("UI Settings")]
    public RectTransform contentParent;
    public GameObject imageItemPrefab;
    [Header("Manerger Settings")]
    public GameObject manerger;
    public PointTrigger pointTrigger;

    public GameObject paintToSet;
    public RawImage SelectedPicture;

    [Header("All DownLoad Texture")]
    public static Dictionary<string,GameObject> TextureIamge = new Dictionary<string,GameObject>();
    public GameObject PaintTextureContainter;

    public FirebaseWebRequest fbWRequest;

    public void Start()
    {
        LoadGallery();
    }

    /// <summary>
    /// 上傳圖片
    /// </summary>
    public void UploadImage()
    {
         

        Texture texture =   loadFileExample.testIamge.transform.GetComponent<Renderer>().material.mainTexture;
        imageToUpload = texture as Texture2D;


        Texture2D readable = MakeTextureReadable(imageToUpload);
        StartCoroutine(UploadCoroutine(readable, imageToUpload.name + ".png"));
    }

    /// <summary>
    /// 從 Supabase 下載圖片
    /// </summary>
    public void DownloadImage()
    {
        StartCoroutine(DownloadCoroutine(imageToUpload.name + ".png", texture =>
        {
            displayImage.texture = texture;
        }));
    }

    public void DeletImage()
    {
         if (SelectGalleryName != null)
         {
             StartCoroutine(Deletecoroutinne(SelectGalleryName));
         }

        Destroy(GalleryItem.selectedGalleryItem.GetComponent<GalleryItem>().container);
        GalleryItem.selectedGalleryItem = null;

        fbWRequest.DeleteNoTexturePaint(SelectGalleryName);

    }

    public void LoadGallery()
    {
        StartCoroutine(ListAndShowIamges());
    }

    IEnumerator UploadCoroutine(Texture2D texture, string fileName)
    {

        byte[] pngData = texture.EncodeToPNG();

        string url = $"{projectUrl}/storage/v1/object/{bucketName}/{fileName}";

        UnityWebRequest request = UnityWebRequest.Put(url, pngData);
        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", $"Bearer {anonKey}");
        request.SetRequestHeader("Content-Type", "image/png");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ 上傳成功");
            
            //
            GameObject newItem = Instantiate(imageItemPrefab, contentParent);
            RawImage rawImage = newItem.GetComponentInChildren<RawImage>();
            LayoutElement layout = newItem.GetComponent<LayoutElement>();

            //貼圖存放位置
            TextureIamge.Add(fileName, Instantiate(PaintTextureContainter));
            GameObject texureImage = TextureIamge[fileName];
            texureImage.transform.GetComponent<Renderer>().material.mainTexture = texture;
            texureImage.transform.GetComponent<Renderer>().material.mainTexture.name = fileName;
            print(texureImage.transform.GetComponent<Renderer>().material.mainTexture.name);

            rawImage.texture = texture;
            RectTransform rt = rawImage.rectTransform;
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            GalleryItem item = rawImage.GetComponent<GalleryItem>();
            item.selectedPicture = SelectedPicture;

            // item.pointTrigger = pointTrigger;

            item.setPaintings = manerger.GetComponent<SetPaintings>();
            if (item != null)
            {
                item.Init(fileName, texture, paintToSet); // 或你的檔名
            }

            float fixedWidth = contentParent.rect.width;
            float aspect = (float)texture.height / texture.width;
            float targetHeight = fixedWidth * aspect;
            //AspectRatioFitter fitter = rawImage.GetComponent<AspectRatioFitter>();
            //fitter.aspectRatio = aspect;
            layout.preferredWidth = fixedWidth;
            layout.preferredHeight = targetHeight;

            Success.SetActive(true);
            Invoke("SuccesUpload", 2f);

        }
        else
        {
            Debug.LogError($"❌ 上傳失敗: {request.error}");
            Fail.SetActive(true);
            Invoke("FailUpload", 2f);
        }
    }

    void SuccesUpload()
    {
        Success.SetActive(false);
    }
    void FailUpload()
    {
        Fail.SetActive(false);
    }

    IEnumerator DownloadCoroutine(string fileName, Action<Texture2D> onDownloaded)
    {
        // 如果 bucket 是 public，就可以直接用 public URL
        string publicUrl = $"{projectUrl}/storage/v1/object/public/{bucketName}/{fileName}";

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(publicUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            onDownloaded?.Invoke(texture);
            Debug.Log("✅ 下載成功");
        }
        else
        {
            Debug.LogError($"❌ 下載失敗: {request.error}");
        }
    }

    

    public void ListBucketFiles()
    {
        StartCoroutine(ListBucketCoroutine());
    }

    IEnumerator ListBucketCoroutine()
    {
        string url = $"{projectUrl}/storage/v1/object/list/{bucketName}";

        // Supabase 需要 POST body
        string jsonBody = "{\"prefix\":\"\",\"limit\":100,\"offset\":0}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", $"Bearer {anonKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ 取得成功: " + request.downloadHandler.text);

            try
            {
                SupabaseFileInfo[] files = JsonConvert.DeserializeObject<SupabaseFileInfo[]>(request.downloadHandler.text);

                foreach (var file in files)
                {
                    Debug.Log($"📂 檔案: {file.name} ({file.size} bytes, 更新時間: {file.updated_at})");
                    infor.text += $" {file.name} ({file.size} bytes)\n";
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("❌ JSON 解析失敗: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("❌ 取得失敗: " + request.error + "\n" + request.downloadHandler.text);
        }
    }
    [SerializeField] int maxConcurrentDownloads = 1; // iPhone 建議 1，Android/iPad 可 2~4
    private readonly Queue<(string url, string fileName)> _downloadQueue = new();
    private int _activeDownloads = 0;
    IEnumerator ListAndShowIamges()
    {
        string url = $"{projectUrl}/storage/v1/object/list/{bucketName}";

        // Supabase 需要 POST body
        string jsonBody = "{\"prefix\":\"\",\"limit\":100,\"offset\":0}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", $"Bearer {anonKey}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ 取得成功: " + request.downloadHandler.text);

            try
            {
                SupabaseFileInfo[] files = JsonConvert.DeserializeObject<SupabaseFileInfo[]>(request.downloadHandler.text);

                foreach (var file in files)
                {
                    Debug.Log($"📂 檔案: {file.name} ({file.size} bytes, 更新時間: {file.updated_at})");
                    infor.text += $" {file.name} ({file.size} bytes)\n";

                    string fileName = file.name.ToString();
                    string publicUrl = $"{projectUrl}/storage/v1/object/public/{bucketName}/{fileName}";

                    //StartCoroutine(DownloadAndCreateItem(publicUrl,fileName));
                    _downloadQueue.Enqueue((publicUrl, fileName));
                }
                TryStartNextDownloads();
            }


            catch (System.Exception e)
            {
                Debug.LogError("❌ JSON 解析失敗: " + e.Message);
            }
        }
        else
        {
            Debug.LogError("❌ 取得失敗: " + request.error + "\n" + request.downloadHandler.text);
        }
    }

    private void TryStartNextDownloads()
    {
        while (_activeDownloads < maxConcurrentDownloads && _downloadQueue.Count > 0)
        {
            var job = _downloadQueue.Dequeue();
            _activeDownloads++;
            StartCoroutine(DownloadAndCreateItem_Queued(job.url, job.fileName));
        }
    }

    private IEnumerator DownloadAndCreateItem_Queued(string url, string fileName)
    {
        yield return StartCoroutine(DownloadAndCreateItem(url, fileName));

        _activeDownloads--;
        TryStartNextDownloads();
    }

    IEnumerator Deletecoroutinne(string fileName)
    {
        string url = $"{projectUrl}/storage/v1/object/{bucketName}/{fileName}";
        UnityWebRequest request = UnityWebRequest.Delete(url);
        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", $"Bearer {anonKey}");

        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.Success ||
                                request.responseCode == 200 ||
                                request.responseCode == 204)
        {
            Debug.Log($"已刪除檔案{fileName}");

        }
        else
        {
            Debug.Log($"{request.responseCode} {request.error} \n{request.downloadHandler.text}");
        }
        
    }

    IEnumerator DownloadAndCreateItem(string url,string fileName)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Debug.Log("✅ 下載成功");
            Canvas.ForceUpdateCanvases();

            GameObject newItem = Instantiate(imageItemPrefab, contentParent);
            RawImage rawImage = newItem.GetComponentInChildren<RawImage>();
            LayoutElement layout = newItem.GetComponent<LayoutElement>();

            //貼圖存放位置
            TextureIamge.Add(fileName,Instantiate(PaintTextureContainter));
            GameObject texureImage = TextureIamge[fileName];
            texureImage.transform.GetComponent<Renderer>().material.mainTexture = texture;
             texureImage.transform.GetComponent<Renderer>().material.mainTexture.name = fileName;
            print(texureImage.transform.GetComponent<Renderer>().material.mainTexture.name);

            rawImage.texture = texture;
            RectTransform rt = rawImage.rectTransform;
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            GalleryItem item = rawImage.GetComponent<GalleryItem>();
            item.selectedPicture = SelectedPicture;

           // item.pointTrigger = pointTrigger;

            item.setPaintings = manerger.GetComponent<SetPaintings>();
            if (item != null)
            {
                item.Init( fileName,texture,paintToSet); // 或你的檔名
            }

            float fixedWidth = contentParent.rect.width;
            float aspect = (float)texture.height / texture.width;
            float targetHeight = fixedWidth * aspect;
            //AspectRatioFitter fitter = rawImage.GetComponent<AspectRatioFitter>();
            //fitter.aspectRatio = aspect;
            layout.preferredWidth = fixedWidth;
            layout.preferredHeight = targetHeight;
            Debug.LogError($"preferredWidth：{aspect} preferredHeight：{texture.height}");
        }
        else
        {
            Debug.LogError($"圖片下載失敗：{request.error}");
        }
    }


    Texture2D MakeTextureReadable(Texture2D tex)
    {
        RenderTexture rt = RenderTexture.GetTemporary(
            tex.width,
            tex.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
        );

        Graphics.Blit(tex, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D readableTex = new Texture2D(tex.width, tex.height);
        readableTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTex.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readableTex;
    }



    
}

[System.Serializable]
public class SupabaseFileInfo
{
    public string name { get; set; }
    public int size { get; set; }
    public string id { get; set; }
    public string bucket_id { get; set; }
    public string created_at { get; set; }
    public string updated_at { get; set; }
    public string last_accessed_at { get; set; }
    public object  metadata { get; set; }
}





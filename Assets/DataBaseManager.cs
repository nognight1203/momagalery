using UnityEngine;
using Firebase;
using Firebase.Database;

public class DataBaseManager : MonoBehaviour
{
    private DatabaseReference dbReference;


    void Start()
    {
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        UpdatePaintData("A-1-1", "TestPaintName");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdatePaintData(string PaintID,string PaintName)
    {
       // string paintID = JsonUtility.ToJson(PaintID);
       // string paintName = JsonUtility.ToJson(PaintName);
        dbReference.Child("PaintID").Child(PaintID).SetValueAsync(PaintName);
        
        
    }
}

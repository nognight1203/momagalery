using UnityEngine;
using System;
using UnityEngine.UI;

namespace FrostweepGames.Plugins.WebGLFileBrowser.Examples
{



    public class LoadFileExample : MonoBehaviour
    {
        public Button openFileDialogButton;

        public Button saveOpenedFileButton;

        public Button cleanupButton;

        public InputField filterOfTypesField;

        public Text fileNameText,
                    fileInfoText;

        public GameObject testIamge;


        private void Start()
        {
            openFileDialogButton.onClick.AddListener(OpenFileDialogButtonOnClickHandler);
          //  saveOpenedFileButton.onClick.AddListener(SaveOpenedFileButtonOnClickHandler);
          //  cleanupButton.onClick.AddListener(CleanupButtonOnClickhandler);
            filterOfTypesField.onValueChanged.AddListener(FilterOfTypesFieldOnValueChangedHandler);

            WebGLFileBrowser.FilesWereOpenedEvent += FileWasOpenedEventHandler;

        }


     /*   private void SaveOpenedFileButtonOnClickHandler()
        {
            File file = new File()
            {
                fileInfo = new FileInfo()
                {
                    fullName = "Myfile.txt"
                },
                data = System.Text.Encoding.UTF8.GetBytes("my text content!")
            };
            WebGLFileBrowser.SaveFile(file);
        }*/

        private void OpenFileDialogButtonOnClickHandler()
        {
            WebGLFileBrowser.OpenFilePanelWithFilters(".jpg,.png");
           // WebGLFileBrowser.OpenFolderPanelWithFilters(".jpg");

            
        }

      /*  private void CleanupButtonOnClickhandler()
        {

        }*/

        private void FileWasOpenedEventHandler(File[] file)
        {
            fileNameText.text = file[0].fileInfo.name;
            fileInfoText.text = $"File name: {file[0].fileInfo.name}\n" +
                                $"File extension: {file[0].fileInfo.extension}\n" +
                                $"File size: {file[0].fileInfo.SizeToString()}\n" +
                                $"File path: {file[0].fileInfo.path}";

            if (file[0].IsImage())
            {
                testIamge.transform.GetComponent<Renderer>().material.mainTexture = file[0].ToTexture2D();
            }

        }

        private void FilterOfTypesFieldOnValueChangedHandler(string value)
        {

        }



        

    }
}

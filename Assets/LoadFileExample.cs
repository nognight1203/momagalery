using System;
using UnityEngine;
using UnityEngine.Rendering;
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
        public RawImage ShowIamge;

        

        private void Start()
        {
            openFileDialogButton.onClick.AddListener(OpenFileDialogButtonOnClickHandler);
          //  saveOpenedFileButton.onClick.AddListener(SaveOpenedFileButtonOnClickHandler);
          //  cleanupButton.onClick.AddListener(CleanupButtonOnClickhandler);
          //  filterOfTypesField.onValueChanged.AddListener(FilterOfTypesFieldOnValueChangedHandler);

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
            WebGLFileBrowser.OpenFilePanelWithFilters(WebGLFileBrowser.GetFilteredFileExtensions("jpg,png"));
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
                ShowIamge.texture = file[0].ToTexture2D();
                float fixedWidth = 100;
                float aspect = (float)ShowIamge.texture.height / ShowIamge.texture.width;
                float targetHeight = fixedWidth * aspect;
                ShowIamge.transform.localScale = new Vector3((float)1.822, (float)1.822 * aspect, (float)1.822);
            }

        }

        private void FilterOfTypesFieldOnValueChangedHandler(string value)
        {

        }



        

    }
}

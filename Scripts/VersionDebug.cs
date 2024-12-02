using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VersionDebug : MonoBehaviour
{
    public Text text;

    string WindowsPath;
    string MobilePath;
    
    public TextAsset textAsset;

    void Start()
    {
        //Directory.Exists(WindowsPath) : False     checa la direccion, no los archivos
        //File.Exists(WindowsPath)      : True      checa los archivos

        //string dataString = reader.;
        
        text.text = $"Version: {Application.version}";//    Res:{Screen.currentResolution}";
        //text.fontSize = 16;
    }
}

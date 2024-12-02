using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChargeBarManager : MonoBehaviour
{
    public Slider slider;
    private UnityWebRequest www;

    private void Update()
    {
        if (www != null) slider.value = www.downloadProgress;
    }

    public void TakeUnityWebRequest(UnityWebRequest wwww)
    {
        www = wwww;
        Debug.Log("  Took UnityWeb");
    }

    public void LeaveUnityWebRequest()
    {
        www = null;//new UnityWebRequest();
        slider.value = 0f;
    }

    public void DebugValue()
    {
        //Debug.Log($"\t {gameObject.name}.value: {slider.value}");
    }
}

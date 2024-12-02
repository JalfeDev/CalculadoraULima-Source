using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomIconSelector : MonoBehaviour
{
    public Button[] button;

    public GameObject loading;
    public GameObject panel;

    private void Start()
    {
        //button = new Button[transform.childCount];
        switch(SceneManager.GetActiveScene().name)
        {
            case "CalculadoraOneNote":
            case "SceneChoose":
                button[0].SendMessage("SelectRoomIcon");
            break;
            
            case "SceneSchedule":
                button[1].SendMessage("SelectRoomIcon");
            break;

            case "SceneExams":
                button[2].SendMessage("SelectRoomIcon");
            break;

            case "SceneAsync":
            case "SceneNotesManager":
                button[3].SendMessage("SelectRoomIcon");
            break;
        }
    }

    public void EnableLoadingScreen()
    {
        loading.SetActive(true);
        panel.SetActive(false);
    }
}

using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class roomGoTo : MonoBehaviour
{
    [Header("Esto para la SceneChoose")]
    public int indexClass = 0;
    public Text titleName;
    public Text noteText;
    public Text creditText;

    [Header("Esto para el panel de abajo")]
    public Image spotSelected;
    public TransitionType transitionOut;

    public void changeScene(string scene)
    {
        //FindFirstObjectByType<CameraTransition>().StartCoroutine("StartTransition", (int)transitionOut + scene);
        CameraTransition.Instance.StartCoroutine("StartTransition", (int)transitionOut + scene);
    }

    public void SetIndexCourse()
    {
        PlayerPrefs.SetInt("IndexCourse", indexClass);
    }

    public void SelectRoomIcon()
    {
        spotSelected.enabled = true;
    }

    public void SetCourseNames(string title, int index, string note, string credit)
    {
        titleName.text = title;
        indexClass = index;
        noteText.text = note;
        creditText.text = credit;
    }
}
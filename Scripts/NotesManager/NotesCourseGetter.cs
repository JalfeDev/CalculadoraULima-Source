using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesCourseGetter : MonoBehaviour
{
    public InputField inputName;
    public InputField inputCredit;

    public int indexCourse;
    public CourseListEditorManager courseEditor;

    private void Awake()
    {
        inputName = transform.GetChild(0).GetComponent<InputField>();
        inputCredit = transform.GetChild(1).GetComponent<InputField>();
        //No funciona courseEditor = transform.parent.GetComponent<CourseListEditorManager>();
    }

    public void SetNameAndCredit(string name, int credit)
    {
        //Si es en el Start se declaran los inputs, no dara tiempo
        inputName.text = name;
        inputCredit.text = credit.ToString();
    }

    public void SendDeleteMessage()
    {
        courseEditor.DisplayOpaquePanel(indexCourse);
        //SendMessageUpwards("DisplayOpaquePanel", indexCourse);
    }

    public void SendSaveMessageName(string texto)
    {
        courseEditor.SaveData(indexCourse, texto, "course");
    }
    public void SendSaveMessageCredit(string texto)
    {
        courseEditor.SaveData(indexCourse, texto, "credit");
    }
    public void SendSaveMessageDegree(string texto)
    {
        courseEditor.SaveData(-1, texto, "degree");
    }

    public void OnMyChange()
    {
        //Cuando empiezas, se activa esto
        //Se activa con cada letra que escribes
        Debug.Log(this.name + " is Value-Changed");
    }
    public void OnMyEndEdit()
    {
        //Si le das a espacio
        //Si seleccionas y das Enter
        //Conclusion: Se activa cuando das a Enter
        Debug.Log(this.name + " is Edit-Ended");
    }
    public void OnMySubmit()
    {
        //Si le das a espacio
        //Si seleccionas y clickeas fuera
        //Si seleccionas y das Enter
        //Conclusion: Se activa cuando es deseleccionado
        Debug.Log(this.name + " is Submitted");
    }
}
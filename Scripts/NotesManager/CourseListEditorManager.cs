using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CourseListEditorManager : MonoBehaviour
{
    NoteSelector notes;

    [Header("Extra")]
    public int indexSelectedCourse;
    public GameObject opaquePanel;
    public GameObject panelDelete;
    public Text textCourseName;
    public GameObject panelMaxAdd;
    public GameObject panelMaxCycles;
    public Text textAddNewCycle;
    public Text textDeleteLastCycle;

    private void Start()
    {
        notes = GetComponent<NoteSelector>();
    }

    public void SaveData(int indexCourse, string texto, string type)
    {
        bool isChanged = true;
        switch(type)
        {
            case "course":
                if (notes.curCourse[indexCourse].curso != texto) notes.curCourse[indexCourse].curso = texto;
                else isChanged = false;
            break;
        
            case "credit":
                if (notes.curCourse[indexCourse].credito != int.Parse(texto)) notes.curCourse[indexCourse].credito = int.Parse(texto);
                else isChanged = false;
            break;
        
            case "degree":
                if (notes.carrera.degreeName != texto) notes.carrera.degreeName = texto;
                else isChanged = false;
            break;
        }

        if (isChanged)
        {
            string stringDegreeJSON = JsonUtility.ToJson(notes.carrera);
            File.WriteAllText(CameraTransition.Instance.currentPath, stringDegreeJSON);
            Debug.Log("     Changed");
        }
        Debug.Log("Finished");
    }

    public void DisplayOpaquePanel(int index)
    {
        opaquePanel.SetActive(true);
        panelDelete.SetActive(true);
        textCourseName.text = notes.curCourse[index].curso;
        indexSelectedCourse = index;
    }

    public void DeleteCourseOfSelectedSemester()
    {
        notes.curCourse.RemoveAt(indexSelectedCourse);
        string stringDegreeJSON = JsonUtility.ToJson(notes.carrera);
        File.WriteAllText(CameraTransition.Instance.currentPath, stringDegreeJSON);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddCourseToSemester()
    {
        if (notes.curCourse.Count < 8)
        {
            List<int> foo = new List<int>(){15, 15, 20};
            notes.curCourse.Add(new CourseNotes("Lorem Ipsum", foo, foo, 9, 3));
            
            string stringDegreeJSON = JsonUtility.ToJson(notes.carrera);
            File.WriteAllText(CameraTransition.Instance.currentPath, stringDegreeJSON);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            opaquePanel.SetActive(true);
            panelMaxAdd.SetActive(true);
        }
    }

    public void SetCycleTexts()
    {
        textAddNewCycle.text = $"Ciclo {notes.carrera.ciclos.Count}";    //Este es del panel de pregunta de AddNewCycle
        textDeleteLastCycle.text = $"Ciclo {notes.carrera.ciclos.Count-1}";    //Este es del panel de pregunta de DeleteLastCycle
    }

    public void AddNewCycle()
    {
        int cantidadCiclos = notes.carrera.ciclos.Count;
        if (cantidadCiclos < 11)
        {
            List<int> foo = new List<int>(){15, 15, 20};
            List<CourseNotes> lorem = new List<CourseNotes>(){new CourseNotes("Lorem Ipsum", foo, foo, 9, 3)};
            Semester nuevo = new Semester(cantidadCiclos, 0f, lorem);
            notes.carrera.ciclos.Add(nuevo);

            string stringDegreeJSON = JsonUtility.ToJson(notes.carrera);
            File.WriteAllText(CameraTransition.Instance.currentPath, stringDegreeJSON);

            PlayerPrefs.SetInt("IndexSemester", cantidadCiclos);            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            opaquePanel.SetActive(true);
            panelMaxCycles.SetActive(true);
        }
    }

    public void DeleteLastCycle()
    {
        notes.carrera.ciclos.RemoveAt(notes.carrera.ciclos.Count-1);
        string stringDegreeJSON = JsonUtility.ToJson(notes.carrera);
        File.WriteAllText(CameraTransition.Instance.currentPath, stringDegreeJSON);

        PlayerPrefs.SetInt("IndexSemester", notes.carrera.ciclos.Count-1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
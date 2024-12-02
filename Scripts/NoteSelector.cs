using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.Collections;

public class NoteSelector : MonoBehaviour
{
    public enum CourseEnlister{GoTo, Editor}
    [Header("TODO")]
    public CourseEnlister type;
    public int indexCycle;
    //Clases de la carrera
    public Degree carrera;               //La carrera (todo)
    public List<CourseNotes> curCourse;  //Los cursos del semestre

    [Header("Textos")]
    public Text cycleText;  //Ciclo 1,2,3,3.5,4,4.5
    public Text finalNoteText;  //Opcional
    public InputField inputTitle;

    [Header("Lista de Cursos")]
    public Vector3 newPos;
    public GameObject prefabCourse;
    public int spaceMargin;
    
    [Header("Flechas")]
    public Button leftB;
    public Button rightB;

    //Crea una lista de los cursos de un ciclo
        //Nota: si lo pones en Awake, no funcionara al principio en Android, porque se crea antes del Singleton
    void Start()
    {
        //Clases de Json
        carrera = JsonUtility.FromJson<Degree>( File.ReadAllText(CameraTransition.Instance.currentPath) );
        indexCycle = PlayerPrefs.GetInt("IndexSemester");
        curCourse = carrera.ciclos[indexCycle].Courses;

        if (type == CourseEnlister.GoTo)
        {
            float sumaPonderada = 0f;
            int totalCreditos = 0;
            //Crear los botones de todos los cursos, y la nota final
            for(int i = 0; i < curCourse.Count; i++)
            {
                Vector3 newPosition = newPos - new Vector3(0f, spaceMargin*i, 0f);
                GameObject option = Instantiate(prefabCourse, newPosition, Quaternion.identity);
                option.transform.SetParent(transform, false);
                
                roomGoTo buttonGo = option.GetComponent<roomGoTo>();
                buttonGo.SetCourseNames(curCourse[i].curso, i, curCourse[i].notaFinal.ToString(), curCourse[i].credito.ToString());

                sumaPonderada += curCourse[i].notaFinal * curCourse[i].credito;
                totalCreditos += curCourse[i].credito;
            }
            finalNoteText.text = $"Promedio Final : {sumaPonderada/totalCreditos}";
        }
        else if (type == CourseEnlister.Editor)
        {
            inputTitle.text = carrera.degreeName;
            CourseListEditorManager thisCourse = GetComponent<CourseListEditorManager>();
            for (int i = 0; i < curCourse.Count; i++)
            {
                Vector3 newPosition = newPos - new Vector3(0f, spaceMargin*i, 0f);
                GameObject course = Instantiate(prefabCourse, newPosition, Quaternion.identity);
                course.transform.SetParent(transform, false);
                
                NotesCourseGetter courseScript = course.GetComponent<NotesCourseGetter>();
                courseScript.courseEditor = thisCourse;
                courseScript.indexCourse = i;
                courseScript.SetNameAndCredit(curCourse[i].curso, curCourse[i].credito);
            }
        }

        //Los textos
        cycleText.text = $"Ciclo { indexCycle }";

        //Actualizar Botones
        if (indexCycle == 1)
        {
            leftB.interactable = false;
            rightB.interactable = true;
        }
        else if (indexCycle == carrera.ciclos.Count - 1)
        {
            rightB.interactable = false;
            leftB.interactable = true;
        }
    }

    public void SwitchCycle(int add)
    {
        int nextSemester =  indexCycle + add;
        if (nextSemester > 0 && nextSemester < carrera.ciclos.Count) PlayerPrefs.SetInt("IndexSemester", nextSemester);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

/*
Degree
    Ciclo 1
         mate   [-]
         com    [-]
    Ciclo 2
         alg    [-]
         cal    [-]
    Ciclo 3
         arq    [-]
         fis    [-]
    [+]
*/
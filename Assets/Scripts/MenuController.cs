using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
public void ChangeScene(int index){
SceneManager.LoadScene(index);
}
    public void exitGame(){
        Application.Quit();
        Debug.Log("Гра закрилась");
    }
}
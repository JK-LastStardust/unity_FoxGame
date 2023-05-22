using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panel : MonoBehaviour
{
    private Animator anim;


    
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void PlayGameSC1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    void PlayGameSC2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+2);
    }

    void PlayGameSC3()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+3);
    }
}

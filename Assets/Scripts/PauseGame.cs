using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject MenuDePausa;
    public GameObject MenuDeNivel;
    public Button Resume;
    [SerializeField] Slider VolumeSlider; 
    public Cronometro cronometro;
    // Start is called before the first frame update

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {       
        if(!MenuDeNivel.activeSelf){
            if (Input.GetKeyDown(KeyCode.Escape)){
                Pause();
            }
        }   
    }

    public void Pause(){
        MenuDePausa.SetActive(true);
        Time.timeScale = 0;
        VolumeSlider.Select();
    }

    public void Despause(){
        MenuDePausa.SetActive(false);
        Time.timeScale = 1;
    }

    public void Janelinha(){
        Screen.fullScreen = !Screen.fullScreen;
   }
}
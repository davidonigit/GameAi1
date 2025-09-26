using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string cena;
    public GameObject MenuPanel;
    public GameObject OptionsPanel;
    public Button MenuStartButton;
    public Button MenuOptionsButton;
    public Button OptionsVolumeButton;
    [SerializeField] Slider VolumeSlider; 


    // Start is called before the first frame update
    void Start()
    {    
        MenuStartButton.Select();
    }

    // Update is called once per frame
    void Update()
    {   
    }
    
    
    /* Iniciar o jogo */
    public void StartGame(){
        PlayerPrefs.SetInt("level",1);
        PlayerPrefs.SetInt("max_xp",3);
        PlayerPrefs.SetInt("xp",0);
        PlayerPrefs.SetInt("vida",3);
        PlayerPrefs.SetInt("dano",1);
        PlayerPrefs.SetFloat("velocidade",3);
        PlayerPrefs.Save();

        SceneManager.LoadScene(cena);
    }


    /* Menu do jogo */
    
    /* Mostrar o menu ao apertar botao Opcoes */
    public void SettingsGame(){
        MenuPanel.SetActive(false);    
        OptionsPanel.SetActive(true);
        VolumeSlider.Select();
    }

    /* Alterar o volume do jogo */
    public void Volume(){
        AudioListener.volume = VolumeSlider.value;
    }

    /* Voltar para o menu principal */
    public void BackToMenu(){
        OptionsPanel.SetActive(false);
        MenuPanel.SetActive(true);  
        MenuOptionsButton.Select();  
    }


    public void QuitGame(){
        //Rodando na unity editor:
        //UnityEditor.EditorApplication.isPlaying = false;
        //jogo depois de compilado:
        Application.Quit();
    }
}

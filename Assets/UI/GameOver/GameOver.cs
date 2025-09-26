using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string cena1;
    public string cena2;
    public Button JogarNovamente;
    public Button MenuPrincipal;
    public Button Sair;
    
    // Start is called before the first frame update
    void Start()
    {
        JogarNovamente.Select();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartGameAgain(){
        SceneManager.LoadScene(cena1);
    }

    public void VoltarMenu(){
        SceneManager.LoadScene(cena2);
    }

    public void QuitGameOver(){
        //Rodando na unity editor:
        //UnityEditor.EditorApplication.isPlaying = false;
        //jogo depois de compilado:
        Application.Quit();
    }

}

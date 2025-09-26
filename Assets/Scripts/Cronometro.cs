using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cronometro : MonoBehaviour
{
    public TMP_Text TextSegundos;
    public TMP_Text TextMinutos;
    public int LimiteSegundos = 60;
    public float Segundos;
    public int  Minutos;
    

    // Update is called once per frame
    void FixedUpdate(){
        TextSegundos.text = Segundos.ToString("00");
        TextMinutos.text = Minutos.ToString("00");
        Segundos += Time.deltaTime;
        if(Segundos >= LimiteSegundos){
            Minutos++; 
            Segundos = 0;
        } 

    }

    public int GetMinutos(){
        return Minutos;
    }

    public float GetSeg(){
        return Segundos;
    }

}

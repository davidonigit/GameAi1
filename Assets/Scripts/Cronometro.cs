using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cronometro : MonoBehaviour
{
    [Header("Componentes de UI")]
    public TMP_Text TextSegundos;
    public TMP_Text TextMinutos;

    [Header("Controle de Tempo")]
    public float Segundos;
    public int   Minutos;
    public int LimiteSegundos = 60;
    
    [Header("Condição de Vitória")]
    [SerializeField] private int minutosParaVitoria = 2;
    [SerializeField] private int segundosParaVitoria = 0; 
    [SerializeField] private UIManager uiManager;

    private bool vitoriaAlcancada = false; // Trava para não chamar a vitória várias vezes

    void FixedUpdate()
    {
        // Se a vitória já foi alcançada, para de contar e de verificar
        if (vitoriaAlcancada) return;

        // --- Sua lógica de contar o tempo (continua igual) ---
        Segundos += Time.deltaTime;
        if(Segundos >= LimiteSegundos)
        {
            Minutos++; 
            Segundos = 0;
        }

        // --- Sua lógica de mostrar o tempo (continua igual) ---
        TextSegundos.text = Segundos.ToString("00");
        TextMinutos.text = Minutos.ToString("00");

        // --- LÓGICA DE VITÓRIA ADICIONADA ---
        // Verifica se o tempo atual é maior ou igual ao tempo de vitória
        if (Minutos >= minutosParaVitoria && Segundos >= segundosParaVitoria)
        {
            vitoriaAlcancada = true; // Ativa a trava
            if (uiManager != null)
            {
                uiManager.ShowVictoryPanel(); // Chama a função para mostrar a tela de vitória
            }
        }
    }

    public int GetMinutos()
    {
        return Minutos;
    }

    public float GetSeg()
    {
        return Segundos;
    }
}
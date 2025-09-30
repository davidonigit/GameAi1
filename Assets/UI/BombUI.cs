using UnityEngine;
using System.Collections.Generic;

public class BombUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> bombIcons = new List<GameObject>();
    public void UpdateBombIcons(int currentBombs)
    {
        // Passa por todos os ícones da lista
        for (int i = 0; i < bombIcons.Count; i++)
        {
            // Se o índice 'i' for menor que a quantidade de bombas, o ícone deve estar ativo.
            // Ex: Se o jogador tem 2 bombas, os ícones 0 e 1 devem estar ativos.
            if (i < currentBombs)
            {
                bombIcons[i].SetActive(true); // Mostra o ícone
            }
            else
            {
                bombIcons[i].SetActive(false); // Esconde o ícone
            }
        }
    }
}
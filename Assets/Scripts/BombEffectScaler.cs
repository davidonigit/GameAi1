using UnityEngine;

// Garante que este objeto tenha um SpriteRenderer para que o script não quebre.
[RequireComponent(typeof(SpriteRenderer))]
public class BombEffectScaler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float originalDiameter;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Calcula o diâmetro visual original do sprite em unidades do mundo.
        // Isso é crucial para sabermos o tamanho base da nossa animação.
        originalDiameter = spriteRenderer.bounds.size.x / transform.localScale.x;
    }

    /// <summary>
    /// Ajusta a escala do efeito para que seu diâmetro visual corresponda a 2x o raio.
    /// </summary>
    /// <param name="radius">O raio de dano da bomba.</param>
    public void SetScaleFromRadius(float radius)
    {
        if (originalDiameter <= 0)
        {
            Debug.LogWarning("O diâmetro original do efeito é zero. Não é possível ajustar a escala.");
            return;
        }

        // O diâmetro que queremos que a animação tenha é o dobro do raio de dano.
        float targetDiameter = radius * 2f;

        // A nova escala é o tamanho desejado dividido pelo tamanho original.
        float scaleMultiplier = targetDiameter / originalDiameter;

        // Aplica a nova escala uniformemente nos eixos X e Y.
        transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, 1f);
    }
}
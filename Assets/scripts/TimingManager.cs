using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimingManager : MonoBehaviour
{
    [Header("Slider")]
    public Slider timingSlider;  // ← Назначь Slider (child TimingBarPanel)!

    [Header("Зоны точности (0-1, лево=0 право=1)")]
    [Range(0f, 1f)] public float greenStart = 0.35f;
    [Range(0f, 1f)] public float greenEnd = 0.45f;

    [Header("Движение и scoring")]
    public float moveSpeed = 3f;  // Скорость ping-pong
    [Range(1f, 10f)] public float scoreFalloff = 2f;  // Падение точности: 2f = в зелёной ±100 очков (1000→900)

    private bool isActive = false;
    private Coroutine moveCoroutine;
    private void OnEnable()
    {
        moveSpeed += 0.6f;


    }
    public void StartTiming()
    {
        isActive = true;
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveMarker());
    }

    public void StopTiming()
    {
        isActive = false;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    IEnumerator MoveMarker()
    {
        float journey = 0f;
        while (isActive)
        {
            journey += Time.deltaTime * moveSpeed;
            float t = Mathf.PingPong(journey, 2f) / 2f;  // 0→1→0 ping-pong
            timingSlider.value = t;
            yield return null;
        }
    }

    public float GetAccuracy()
    {
        float pos = timingSlider.value;

        // Центр зелёной зоны (идеал)
        float perfectPos = (greenStart + greenEnd) / 2f;

        // Расстояние до идеала
        float distToPerfect = Mathf.Abs(pos - perfectPos);

        // Плавная точность: 1.0 в центре → 0.0 на краях/за пределами
        // falloff=2f: на краях зелёной (dist=0.05) acc=1-0.1=0.9 (900 очков)
        // дальше линейно падает: dist=0.1=800, 0.2=600, 0.4=200, 0.5=0
        float accuracy = Mathf.Clamp01(1f - distToPerfect * scoreFalloff);

        return accuracy;
    }
}
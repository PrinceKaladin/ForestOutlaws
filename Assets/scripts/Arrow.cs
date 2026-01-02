using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Sprite flyingSprite;
    public Sprite stuckSprite;

    private SpriteRenderer sr;
    private Vector2 target;
    private float speed;
    private float initialDist;
    private bool hit = false;
    private float accuracy;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = flyingSprite;
    }

    public void Launch(Vector2 _target, float _speed, float _accuracy)
    {
        target = _target;
        speed = _speed;
        accuracy = _accuracy;

        Vector2 startPos = transform.position;
        initialDist = Vector2.Distance(startPos, target);
    }

    void Update()
    {
        if (!hit)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Имитация 3D: уменьшение размера по мере приближения к цели
            float distToTarget = Vector2.Distance(transform.position, target);
            float scale = Mathf.Lerp(1f, 0.5f, 1f - (distToTarget / initialDist));
            transform.localScale = new Vector3(scale, scale, 1);

            if (distToTarget < 0.1f)
            {
                HitTarget();
            }
        }
    }

    void HitTarget()
    {
        hit = true;
        sr.sprite = stuckSprite;

        // Счёт на основе точности тайминга + небольшой бонус/штраф за финальное смещение
        Vector2 hitOffset = target - (Vector2)transform.position;
        float offsetMultiplier = 1f / (1f + hitOffset.magnitude * 10f);
        int score = Mathf.RoundToInt(1000f * accuracy * offsetMultiplier);

        FindObjectOfType<GameManager>().AddScore(score);
        FindObjectOfType<CameraController>().ResetCamera();
        FindObjectOfType<GameManager>().ShotComplete();

        Destroy(gameObject, 3f); // Удалить стрелку через 3 секунды
    }
}
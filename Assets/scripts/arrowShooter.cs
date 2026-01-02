using UnityEngine;

public class arrowShooter : MonoBehaviour
{
    [Header("Основные объекты")]
    public GameObject arrowPrefab;         // Префаб стрелки (с Arrow.cs)
    public Transform shootPoint;           // Точка вылета стрелы (кончик лука)
    public Vector2 targetPosition;         // Позиция центра мишени (Target)

    [Header("Параметры полёта")]
    [Tooltip("Базовая скорость полёта стрелы")]
    public float baseSpeed = 12f;

    [Tooltip("Максимальное смещение от центра мишени при accuracy = 0")]
    public float maxOffset = 1.8f;

    [Tooltip("Случайное направление смещения (true = рандом сторона, false = всегда одно)")]
    public bool randomDirection = true;

    /// <summary>
    /// Выстрел с учётом точности из TimingManager
    /// </summary>
    /// <param name="accuracy">0..1 (1 = идеально в центр зелёной зоны)</param>
    public void ShootWithAccuracy(float accuracy)
    {
        if (arrowPrefab == null || shootPoint == null)
        {
            Debug.LogError("ArrowShooter: arrowPrefab или shootPoint не назначены!");
            return;
        }

        // Направление от точки выстрела к центру мишени
        Vector2 directionToTarget = (targetPosition - (Vector2)shootPoint.position).normalized;

        // Перпендикулярное направление (для бокового смещения)
        Vector2 perpendicular = new Vector2(-directionToTarget.y, directionToTarget.x);

        // Вычисляем величину смещения (чем хуже accuracy → тем больше отклонение)
        float offsetAmount = (1f - accuracy) * maxOffset;

        // Выбираем сторону смещения
        Vector2 offsetDirection;
        if (randomDirection)
        {
            offsetDirection = (Random.value > 0.5f) ? perpendicular : -perpendicular;
        }
        else
        {
            offsetDirection = perpendicular; // всегда в одну сторону (можно изменить)
        }

        // Финальная точка попадания
        Vector2 finalTarget = targetPosition + offsetDirection * offsetAmount;

        // Создаём стрелу
        GameObject arrowObj = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);

        Arrow arrowScript = arrowObj.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.Launch(finalTarget, baseSpeed, accuracy);

        }
        else
        {
            Debug.LogError("На префабе Arrow отсутствует компонент Arrow!");
        }

        // Запускаем следование камеры за стрелой
        CameraController cameraCtrl = FindObjectOfType<CameraController>();
        if (cameraCtrl != null)
        {
            cameraCtrl.StartFollowing(arrowObj.transform);
        }
        else
        {
            Debug.LogWarning("CameraController не найден на сцене!");
        }
    }

    // Визуальная отладка в редакторе
    void OnDrawGizmosSelected()
    {
        if (shootPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(shootPoint.position, targetPosition);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetPosition, maxOffset);
        }
    }
}
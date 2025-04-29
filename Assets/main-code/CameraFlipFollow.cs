using UnityEngine;
using Unity.Cinemachine;

public class CameraFlipFollow : MonoBehaviour
{
    public PlayerFlipScript playerFlipScript;
    public CinemachineCamera virtualCamera;
    public float cameraOffset = 0.5f; // Добавляем настраиваемое смещение камеры

    private CinemachinePositionComposer composer;
    private bool previousFacingRight;

    // Длительность анимации поворота камеры
    public float flipDuration = 0.4f;

    void Start()
    {
        if (virtualCamera != null)
        {
            composer = virtualCamera.GetComponent<CinemachinePositionComposer>();
            // Устанавливаем начальное смещение
            var offset = composer.TargetOffset;
            offset.x = cameraOffset;
            composer.TargetOffset = offset;
        }

        if (playerFlipScript != null)
            previousFacingRight = playerFlipScript.IsFacingRight;
    }

    void Update()
    {
        if (playerFlipScript == null || composer == null) return;

        bool currentFacingRight = playerFlipScript.IsFacingRight;

        if (currentFacingRight != previousFacingRight)
        {
            // Плавно инвертируем заданное смещение
            float targetX = currentFacingRight ? cameraOffset : -cameraOffset;

            // Убедимся, что никакая предыдущая анимация не мешает
            LeanTween.cancel(gameObject);

            // Анимируем смещение
            LeanTween.value(gameObject, composer.TargetOffset.x, targetX, flipDuration)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnUpdate((float val) =>
                {
                    var offset = composer.TargetOffset;
                    offset.x = val;
                    composer.TargetOffset = offset;
                });

            previousFacingRight = currentFacingRight;
        }
    }
}

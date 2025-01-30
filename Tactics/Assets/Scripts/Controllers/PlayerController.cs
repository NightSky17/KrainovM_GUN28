using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    private bool isProcessingMove = false;
    private float moveAnimationDuration = 0.5f; // Длительность анимации перемещения
    
    [SerializeField] private BattleController battleController;

    public bool IsProcessingMove => isProcessingMove;

    public async Task ProcessMove(Unit unit, Cell targetCell)
    {
        if (isProcessingMove) return;

        isProcessingMove = true;
        
        // Сохраняем начальную позицию и клетку
        Vector3 startPosition = unit.transform.position;
        Vector3 targetPosition = targetCell.transform.position;
        Cell startCell = unit.CurrentCell;

        // Обновляем позицию юнита в логике игры
        startCell.RemoveUnit(); // Очищаем старую клетку
        targetCell.PlaceUnit(unit); // Устанавливаем юнит на новую клетку
        unit.MoveToCell(targetCell); // Обновляем текущую клетку юнита

        // Запускаем корутину для анимации
        StartCoroutine(AnimateMove(unit, startPosition, targetPosition));

        // Ждем завершения анимации
        await Task.Delay((int)(moveAnimationDuration * 1000));

        isProcessingMove = false;
    }

    private IEnumerator AnimateMove(Unit unit, Vector3 startPos, Vector3 targetPos)
    {
        float elapsedTime = 0;
        
        while (elapsedTime < moveAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveAnimationDuration;
            
            // Используем плавную интерполяцию для движения
            unit.transform.position = Vector3.Lerp(startPos, targetPos, Mathf.SmoothStep(0, 1, progress));
            
            yield return null;
        }

        // Убеждаемся, что юнит точно достиг целевой позиции
        unit.transform.position = targetPos;
    }

    public async Task ProcessCapture(Unit attackingUnit, Cell targetCell, Unit capturedUnit)
    {
        if (isProcessingMove) return;

        isProcessingMove = true;
        
        // Сохраняем начальные позиции
        Vector3 startPosition = attackingUnit.transform.position;
        Vector3 targetPosition = targetCell.transform.position;
        Cell startCell = attackingUnit.CurrentCell;

        // Обновляем позиции в логике игры
        startCell.UnitOnCell = null;
        capturedUnit.CurrentCell.UnitOnCell = null;
        targetCell.UnitOnCell = attackingUnit;
        attackingUnit.CurrentCell = targetCell;

        // Запускаем корутину для анимации движения
        StartCoroutine(AnimateMove(attackingUnit, startPosition, targetPosition));

        // Ждем половину времени анимации движения
        await Task.Delay((int)(moveAnimationDuration * 500));

        // Анимируем исчезновение взятой шашки
        StartCoroutine(AnimateCapture(capturedUnit));

        // Ждем оставшееся время
        await Task.Delay((int)(moveAnimationDuration * 500));

        // Уничтожаем взятую шашку
        GameObject.Destroy(capturedUnit.gameObject);

        isProcessingMove = false;
    }

    private IEnumerator AnimateCapture(Unit unit)
    {
        float elapsedTime = 0;
        Vector3 startScale = unit.transform.localScale;
        Vector3 endScale = Vector3.zero;
        
        while (elapsedTime < moveAnimationDuration * 0.5f)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / (moveAnimationDuration * 0.5f);
            
            // Уменьшаем размер шашки до нуля
            unit.transform.localScale = Vector3.Lerp(startScale, endScale, Mathf.SmoothStep(0, 1, progress));
            
            yield return null;
        }

        unit.transform.localScale = endScale;
    }

    // Метод для проверки, можно ли сейчас взаимодействовать с игрой
    public bool CanInteract()
    {
        return !isProcessingMove;
    }
}

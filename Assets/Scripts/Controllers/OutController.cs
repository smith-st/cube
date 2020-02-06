using TMPro;
using UnityEngine;

public class OutController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMoves;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private GameObject restartLevel;
    [SerializeField]
    private GameObject restartGame;

    public void CountMovesChanged(int value)
    {
        textMoves.text = $"Ходов: {value}";
    }

    public void LevelChanged(int value)
    {
        textLevel.text = $"Уровень: {value}";
    }

    public void LevelFail()
    {
        restartLevel.SetActive(true);
    }

    public void FinishGame()
    {
        restartGame.SetActive(true);
    }

    public void HideAllWindows()
    {
        restartGame.SetActive(false);
        restartLevel.SetActive(false);
    }
}

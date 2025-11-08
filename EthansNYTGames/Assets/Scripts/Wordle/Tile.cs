using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum TileState { Default, Absent, Present, Correct }
public class Tile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private Image background;

    [SerializeField] private Color defaultColor = new Color(233, 233, 233);
    [SerializeField] private Color absentColor = new Color(0.31f, 0.34f, 0.36f);
    [SerializeField] private Color presentColor = new Color(0.78f, 0.67f, 0.23f);
    [SerializeField] private Color correctColor = new Color(0.35f, 0.58f, 0.30f);

    public void SetLetter(char letter)
    {
        letterText.text = letter.ToString();
    }

    public void SetState(TileState state)
    {
        switch (state)
        {
            case TileState.Default:
                background.color = defaultColor;
                break;
            case TileState.Absent:
                background.color = absentColor;
                break;
            case TileState.Present:
                background.color = presentColor;
                break;
            case TileState.Correct:
                background.color = correctColor;
                break;
        }
    }
}
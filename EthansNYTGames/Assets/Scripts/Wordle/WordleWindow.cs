using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using System;

public class WordleWindow : Window
{
    [SerializeField] private Tile[] tiles;
    [SerializeField] private Toast toast;
    [SerializeField] private GameObject playAgainButton;
    private const int WordLength = 5;
    private int currentIndex = 0;
    private string currentGuess = "";
    private int guessRow = 0;
    private string answer = "";
    private bool gameOver = false;
    private bool showedAnswer = false;

    public override void Show()
    {
        base.Show();
        ClearBoard();
        currentIndex = 0;
        guessRow = 0;
        currentGuess = "";
        gameOver = false;
        WordBank.LoadIfNeeded();

        answer = WordBank.GetRandomAnswer();
    }
    
    private void OnEnable()
    {
        if (string.IsNullOrEmpty(answer) || answer.Length != WordLength)
        {
            ClearBoard();
            currentIndex = 0;
            guessRow = 0;
            currentGuess = "";
            gameOver = false;
            answer = WordBank.GetRandomAnswer();
        }
    }

    private void Update()
    {
        if (!showedAnswer)
        {
            Debug.Log("Current Answer: " + answer);
            showedAnswer = true;
        }
        if (gameOver)
        {
            playAgainButton.SetActive(true);
        }
        else
        {
            playAgainButton.SetActive(false);
        }

        foreach (char c in Input.inputString)
        {
            if (char.IsLetter(c))
            {
                AddLetter(char.ToUpper(c));
            }
            else if (c == '\b')
            {
                RemoveLetter();
            }
            else if (c == '\n' || c == '\r')
            {
                SubmitWord();
            }
        }
    }

    private void AddLetter(char letter)
    {
        int rowStart = guessRow * WordLength;
        int rowEnd = rowStart + WordLength;

        if (currentIndex >= tiles.Length || currentIndex >= rowEnd)
        {
            return;
        }

        currentGuess += letter;
        tiles[currentIndex].SetLetter(letter);
        currentIndex++;
    }

    private void RemoveLetter()
    {
        int rowStart = guessRow * WordLength;
        if (currentGuess.Length > 0 && currentIndex > rowStart)
        {
            currentIndex--;
            tiles[currentIndex].SetLetter(' ');
            currentGuess = currentGuess.Substring(0, currentGuess.Length - 1);
        }
    }

    private void SubmitWord()
    {
        if (currentGuess.Length != WordLength)
        {
            StartCoroutine(ShakeRow(guessRow));
            if (toast != null)
            {
                toast.ShowToast("Not enough letters");
            }
            return;
        }
        if (!WordBank.IsValid(currentGuess))
        {
            StartCoroutine(ShakeRow(guessRow));
            if (toast != null)
            {
                toast.ShowToast("Not in word list");
            }
            return;
        }

        CheckColorCurrentRow();

        if (currentGuess == answer)
        {
            gameOver = true;
            return;
        }
        guessRow++;
        currentGuess = "";

        int nextIndex = guessRow * WordLength;
        if (nextIndex >= tiles.Length)
        {
            gameOver = true;
            Debug.Log("Game Over! The correct word was: " + answer);
            toast.ShowToast(answer, Mathf.Infinity);
            return;
        }
        currentIndex = nextIndex;
    }

    private IEnumerator ShakeRow(int row, float duration = 0.25f, float strength = 15f, float freq = 40f)
    {
        int rowStart = row * WordLength;
        int rowEnd = rowStart + WordLength;

        int count = rowEnd - rowStart;
        var rts = new RectTransform[count];
        var start = new Vector2[count];
        if (!toast.isShowing)
        {
            for (int i = 0; i < count; i++)
            {
                rts[i] = tiles[rowStart + i].GetComponent<RectTransform>();
                start[i] = rts[i].anchoredPosition;
            }

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float damper = 1f - Mathf.Clamp01(t / duration);
                float offset = Mathf.Sin(t * freq) * strength * damper;
                for (int i = 0; i < count; i++)
                {
                    rts[i].anchoredPosition = start[i] + new Vector2(offset, 0f);
                }
                yield return null;
            }
            for (int i = 0; i < count; i++)
            {
                rts[i].anchoredPosition = start[i];
            }

        }
    }

    private void CheckColorCurrentRow()
    {
        if (string.IsNullOrEmpty(answer) || answer.Length != WordLength)
        {
            Debug.LogError("WordleWindow: Answer not initialized before evaluation.");
            return;
        }
        int rowStart = guessRow * WordLength;
        var states = new TileState[WordLength];

        int[] counts = new int[26];
        for (int i = 0; i < WordLength; i++)
        {
            counts[answer[i] - 'A']++;
        }

        // Check for th green letters
        for (int i = 0; i < WordLength; i++)
        {
            if (currentGuess[i] == answer[i])
            {
                states[i] = TileState.Correct;
                counts[currentGuess[i] - 'A']--;
            }
            else
            {
                states[i] = TileState.Absent;
            }
        }

        // Check for the yellow letters
        for (int i = 0; i < WordLength; i++)
        {
            if (states[i] == TileState.Correct)
            {
                continue;
            }
            int idx = currentGuess[i] - 'A';
            if (idx >= 0 && idx < 26 && counts[idx] > 0)
            {
                states[i] = TileState.Present;
                counts[idx]--;
            }
        }

        for (int i = 0; i < WordLength; i++)
        {
            tiles[rowStart + i].SetState(states[i]);
        }

    }
    public void ClearBoard()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].SetLetter(' ');
            tiles[i].SetState(TileState.Default);
        }
        currentIndex = 0;
        guessRow = 0;
        currentGuess = "";
        gameOver = false;
        WordBank.LoadIfNeeded();
        answer = WordBank.GetRandomAnswer();
        Debug.Log("New Answer: " + answer);
    }
}

using UnityEngine;
using TMPro;
//using System;

public class WordleWindow : Window
{
    [SerializeField] private Tile[] tiles;
    private const int WordLength = 5;
    private int currentIndex = 0;
    private string currentGuess = "";
    private int guessRow = 0;

    private string answer = "";
    private bool gameOver = false;

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
        if (!gameObject.activeInHierarchy || gameOver)
        {
            return;
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
            return;
        }
        if (!WordBank.IsValid(currentGuess))
        {
            Debug.Log("WordleWindow: Invalid word guessed: " + currentGuess);
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
            return;
        }
        currentIndex = nextIndex;
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
    private void ClearBoard()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].SetLetter(' ');
        }
    }
}

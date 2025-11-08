using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public static class WordBank
{
    private static string[] answers;
    private static HashSet<string> valid;

    public static void LoadIfNeeded()
    {
        if (valid != null) return;

        // Support both Resources/wordlists/* and Resources/* placement
        var answerAsset = Resources.Load<TextAsset>("wordlists/answers")
                           ?? Resources.Load<TextAsset>("answers");
        var guessAsset  = Resources.Load<TextAsset>("wordlists/guesses")
                           ?? Resources.Load<TextAsset>("guesses");

        var merged = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (answerAsset != null)
            foreach (var w in ParseLines(answerAsset.text)) merged.Add(w);

        if (guessAsset != null)
            foreach (var w in ParseLines(guessAsset.text)) merged.Add(w);

        valid = new HashSet<string>(merged.Select(w => w.ToUpperInvariant()));

        // If no dedicated answers, fall back to all valid words
        answers = (answerAsset != null
            ? ParseLines(answerAsset.text).Select(w => w.ToUpperInvariant())
            : valid).ToArray();

        if (answers.Length == 0)
            Debug.LogWarning("WordBank: No answers loaded. Using fallback APPLE.");
    }

    private static IEnumerable<string> ParseLines(string content)
    {
        foreach (var raw in content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
        {
            var s = raw.Trim();
            if (s.Length == 5 && s.All(char.IsLetter))
                yield return s.ToUpperInvariant();
        }
    }

    public static bool IsValid(string guess)
    {
        LoadIfNeeded();
        return valid.Contains(guess.ToUpperInvariant());
    }

    public static string GetRandomAnswer()
    {
        LoadIfNeeded();
        if (answers.Length == 0) return "APPLE";
        return answers[UnityEngine.Random.Range(0, answers.Length)];
    }
}

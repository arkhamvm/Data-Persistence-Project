using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class HighscoreManager : MonoBehaviour
{
    public TMP_Text highscoreText;
    private static HighscoreManager instance;
    private List<Tuple<string, int>> highscores = new List<Tuple<string, int>>();
    private string nickname;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighscores();
        highscores ??= new List<Tuple<string, int>>();
        if (highscores.Count == 0)
        {
            highscores.Add(new Tuple<string, int>("Anonymous", 0));
        }

        highscoreText.text = getHighscoresText();
    }

    public HighscoreManager getInstance()
    {
        return instance;
    }
    
    public void setNickname(string nickname)
    {
        this.nickname = nickname;
    }

    public string getNickname()
    {
        return nickname;
    }
    
    public void addHighscore(int score)
    {
        highscores.Add(new Tuple<string, int>(nickname, score));
        highscores.Sort((x, y) => -x.Item2.CompareTo(y.Item2));
        while (highscores.Count > 3)
        {
            highscores.RemoveAt(highscores.Count - 1);
        }
        
        SaveHighscores();
    }

    public string getHighscoresText()
    {
        return "Highscores:\n" + highscores.Aggregate("", (current, highscore) => current + (highscore.Item1 + ": " + highscore.Item2 + "\n"));
    }

    public string getBestScoreText()
    {
        return "Best Score: " + highscores[0].Item2 + " Name: " + highscores[0].Item1;
    }

    public void SaveHighscores()
    {
        var data = new SaveData();
        data.highscores = highscores;
        var json = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.persistentDataPath + "/highscores.json", json);
    }

    public void LoadHighscores()
    {
        string path = Application.persistentDataPath + "/highscores.json";
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<SaveData>(json);

            highscores = data.highscores;
        }
    }
}

[System.Serializable]
class SaveData
{
    public List<Tuple<string, int>> highscores;
}
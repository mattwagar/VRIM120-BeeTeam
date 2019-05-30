using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    Dictionary<string,int> score;


    void Start() {
        SetScore("Squad Size", 2);
        SetScore("Hero Flowers Hit", 10);
    }

    void Init()
    {
        if(score != null){
            return;
        }
        score = new Dictionary<string, int>();
    }


    public int GetScore(string scoretype)
    {
        Init ();

        if(score.ContainsKey(scoretype) == false){
            return 0;
        }
        return score[scoretype];
    }

    public void SetScore(string scoretype, int value)
    {
        Init ();
        score[scoretype] = value;
    }


    public void ChangeScore(string scoretype, int value){
        Init ();
        int currentScore = GetScore(scoretype);
        SetScore(scoretype, currentScore + value);
    }

    public string[] GetCategories(){
        Init ();
        return score.Keys.ToArray();
    }

}
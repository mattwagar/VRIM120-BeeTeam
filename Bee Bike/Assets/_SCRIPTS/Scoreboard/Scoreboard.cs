using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Scoreboard : MonoBehaviour
{
    public GameObject scoreprefab;
    ScoreManager scoreManager;

    void Start() {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
 
    }

    void Update() {
        if(scoreManager == null){
            Debug.LogError("No Score Manager");
            return;
        }

        while(this.transform.childCount > 0){
            Transform c = this.transform.GetChild(0);
            c.SetParent(null);
            Destroy(c.gameObject);
        }

        string[] categories = scoreManager.GetCategories();

        foreach(string category in categories){
            GameObject go = (GameObject)Instantiate(scoreprefab);
            go.transform.SetParent(this.transform);
            go.transform.Find("Category").GetComponent<Text>().text = category;
            go.transform.Find("Score").GetComponent<Text>().text = scoreManager.GetScore(category).ToString();
            Debug.Log(category);
        }
        
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartButtonScript : SingletonManager<GameStartButtonScript>
{
    public Button gameStartButton;

    protected override void Awake()
    {
        base.Awake();
        //gameStartButton에 연결해서 누르면 OnStart함수 호출됨
        gameStartButton.onClick.AddListener(OnStart);
    }

    public void OnStart()
    {
        print("OnStart눌림");
        SceneManager.LoadScene("GameScene");
    }
}

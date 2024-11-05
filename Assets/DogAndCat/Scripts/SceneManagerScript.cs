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
        //gameStartButton�� �����ؼ� ������ OnStart�Լ� ȣ���
        gameStartButton.onClick.AddListener(OnStart);
    }

    public void OnStart()
    {
        print("OnStart����");
        SceneManager.LoadScene("GameScene");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : SingletonManager<GameManager>
{
    public List<Cats> cats = new List<Cats>();
    public List<Dogs> dogs = new List<Dogs>();
    public List<Cat> cat = new List<Cat>();
    public List<Dog> dog = new List<Dog>();
    public Enemy enemy;
    public Player player;

    protected override void Awake()
    {
        base.Awake();
    }
    public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
}

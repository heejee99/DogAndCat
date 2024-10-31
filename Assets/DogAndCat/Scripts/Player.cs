using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public float money = 0;
    public int level = 1;
    public int exp = 0;
    public float hp;

    //public GameObject BabyPoodle;
    //public GameObject BabyHound;
    //public GameObject BigPitbull;
    //public GameObject BigHound;
    //public GameObject BigRottweiler;

    //GameObject dogPrefab;
    //public bool isTrue;

    //public UnityEvent spawnHound;

    private void Awake()
    {
        //if (spawnHound == null)
        //{
        //    //dogPrefab  = Resources.Load<GameObject>("2D Cute Domestic Animal Pack V.2/Prefabs/");
        //    spawnHound = new UnityEvent();
        //}
        GameManager.Instance.player = this;
    }


    private void Start()
    {
    }

    private void Update()
    {
        float DeltaTime = Time.deltaTime;
        print($"{DeltaTime}");
    }

    public void SpawnButton(int id)
    {
        ResourceManager.Instance.SpawnDog(id);
    }

    //public void SpawnButton1()
    //{
    //    Instantiate(BabyPoodle, transform.position, Quaternion.identity);
    //}
    //public void SpawnButton2()
    //{
    //    Instantiate(BabyHound, transform.position, Quaternion.identity);
    //}
    //public void SpawnButton3()
    //{
    //    Instantiate(BigPitbull, transform.position, Quaternion.identity);
    //}
    //public void SpawnButton4()
    //{
    //    Instantiate(BigHound, transform.position, Quaternion.identity);
    //}
    //public void SpawnButton5()
    //{
    //    Instantiate(BigRottweiler, transform.position, Quaternion.identity);
    //}
}

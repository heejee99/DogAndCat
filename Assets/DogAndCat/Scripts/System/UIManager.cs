using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonManager<UIManager> 
{
    public Image SpawnButtonShadow1;
    public Image SpawnButtonShadow2;
    public Image SpawnButtonShadow3;
    public Image SpawnButtonShadow4;
    public Image SpawnButtonShadow5;

    public float SpawnDelayScale1 = 1f;

    private void Start()
    {
        SpawnButtonShadow1.gameObject.SetActive(false);
        SpawnButtonShadow2.gameObject.SetActive(false);
        SpawnButtonShadow3.gameObject.SetActive(false);
        SpawnButtonShadow4.gameObject.SetActive(false);
        SpawnButtonShadow5.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private float currentSpeicalMoveAttackTime;
    private float currentSpeicalMoveDuration = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            StartCoroutine(SpecialMoveAttack());
        }
    }

    IEnumerator SpecialMoveAttack()
    {
        float endTime = Time.time + currentSpeicalMoveDuration;
        Vector2 startPos = new Vector2(transform.position.x, 0);
        Vector2 endPos = Vector2.zero;
        int count = 0;
        while (Time.time < endTime)
        {
            count++;
            print($"¹Ýº¹È½¼ö : {count}");
            float t = (endTime - Time.time) / currentSpeicalMoveDuration;
            Vector2 diraction = Vector2.Lerp(startPos, endPos, 1 - t);
            RaycastHit2D hit = Physics2D.Raycast(startPos, diraction.normalized);
            Debug.DrawRay((Vector2)transform.position + Vector2.up, diraction.normalized * 10, Color.red);

            if (hit.collider != null)
            {
                Debug.Log("Hit : " + hit.collider.name);
            }
            yield return null;
        }
    }
}

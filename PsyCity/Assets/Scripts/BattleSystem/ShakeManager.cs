using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    private RectTransform target;
    private float duration;
    private float strength;
    private float timeElaped = 0;
    private Vector2 originalPos;

    public void Shake(RectTransform target, float time = 1, float strength = 3)
    {
        this.target = target;
        originalPos = target.anchoredPosition;
        duration = time;
        this.strength = strength;
        timeElaped = 0;
    }

    public void Revert()
    {
        target.anchoredPosition = originalPos;
        target = null;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (target == null)
            return;

        if(timeElaped < duration)
        {
            var offsetX = Random.Range(-strength, strength) + originalPos.x;
            var offsetY = Random.Range(-strength, strength) + originalPos.y;

            target.anchoredPosition = new Vector2(offsetX, offsetY);

            timeElaped += Time.deltaTime;
        }
        else
        {
            Revert();
        }
	}
}

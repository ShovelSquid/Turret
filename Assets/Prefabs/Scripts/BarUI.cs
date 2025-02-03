using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class BarUI : MonoBehaviour
{
    public bool hasCanvas;
    public GameObject valueBar;
    public SpriteRenderer sprite;
    private float valueBarWidth = 0;
    public GameObject pivot;
    public GameObject changeFlash;
    public SpriteRenderer flashSprite;
    private float FlashWidth = 0;
    public GameObject flashPivot;
    public GameObject background;
    [Tooltip("value/maxValue")]
    [Range(0f, 1f)]
    public float value = 1f;
    public float currentValue;
    public float maxValue = 1f;
    public int total = 0;
    [Tooltip("change/maxValue")]
    [Range(0f, 1f)]
    public float change = 0f;
    public Camera Camera;
    public bool hasCamera;
    public float rotationSpeed = 1.0f;
    [Range(0f, 10f)]
    public float scalar;
    [Header("Color")]
    public Color healthyColor;
    public Color changedColor;
    public Color negativeColor;
    public Color healingColor;
    [Header("Lerp")]

    [Tooltip("Rate of decay for the flash that appears when hit; lower is slower and higher is flyer baby")]
    public float DecayRate = 0.01f;
    [Tooltip("The time the valueBar waits before starting to fade away hit change")]
    public float DecayPause = 0.2f;
    public bool lerping;
    public Coroutine flashlerp;
    // Start is called before the first frame update
    [Header("Text")]
    public bool hasText;
    public TextMeshPro text;
    public bool hasMax;
    public TextMeshPro maxText;
    public bool hasTotal;
    public TextMeshPro totalText;
    void Start()
    {
        // change = 0f;
        Camera = Camera.main;
        value = getPercentage(currentValue, maxValue);
        sprite.color = healthyColor;
        flashSprite.color = changedColor;
        changeFlash.SetActive(false);
        valueBarWidth = sprite.size.x * valueBar.transform.localScale.x;
        FlashWidth = flashSprite.size.x * changeFlash.transform.localScale.x;
        updateValue(currentValue);
        // SetValue(currentValue, maxValue);
        if (hasText) {
            text.text = "" + currentValue;
            maxText.text = "/" + maxValue;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (hasCamera) {
            Vector3 cameraDirection = Camera.transform.position - transform.position;
            float singleStep = rotationSpeed * Time.deltaTime;
            Vector3 newRotation = Vector3.RotateTowards(transform.forward, cameraDirection, 5, 0);
            transform.rotation = Quaternion.LookRotation(newRotation);
        }
        // lerp update change scale constantly; have a boolean check to see if still lerping
//
//
    }

    public void updateValue(float newvalue)
    {
        float hurt = currentValue - newvalue;
        currentValue = newvalue;
        if (hasText) {
           text.text = "" + currentValue;
        }
        value = getPercentage(currentValue, maxValue);
        if (hurt != 0)
        {
            change += getPercentage(hurt, maxValue);
            Debug.Log(change);
            if (change > 0) {
                flashSprite.color = changedColor;
            }
            else if (change < 0) {
                flashSprite.color = healingColor;
            }
            if (value < 0) {
                sprite.color = negativeColor;
            }
            else {
                sprite.color = healthyColor;
            }
        }
        else {
            return;
        }
        changeFlash.SetActive(true);
        if (flashlerp != null) {
            StopCoroutine(flashlerp);
        }
        flashlerp = StartCoroutine(FlashLerp(DecayRate, DecayPause));
        // set flashPivot to pivot + bounding box x width
        flashPivot.transform.localPosition = new Vector3(pivot.transform.localPosition.x - valueBarWidth * value,
        pivot.transform.localPosition.y, pivot.transform.localPosition.z);
//
        setBarScale("value");
        setBarScale("flash");
    }

    void setBarScale(string bar)
    {
        if (bar == "value") {
            var sc = pivot.transform.localScale;
            pivot.transform.localScale = new Vector3(value, sc.y, sc.z);
        }
        else if (bar == "flash") {
            var sf = flashPivot.transform.localScale;
            flashPivot.transform.localScale = new Vector3(change, sf.y, sf.z);
        }
    }

    static float getPercentage(float at, float max)
    {
        var pct = at/max;
        return pct;
    }

    private IEnumerator FlashLerp(float time, float pauseTime)
    {
        yield return new WaitForSeconds(pauseTime);
        lerping = true;
        float t = 0f;
        float a = change;
        while (t < time) {
            change = a - (a * (float)Math.Pow((t/time), 3));
            setBarScale("flash");
            yield return new WaitForSeconds(0.01f);
            t += 0.01f;
        }

        // if (change < 0) {
        //     while (change < 0) {
        //         change += DecayRate;
        //         setBarScale("flash");
        //         yield return new WaitForSeconds(0.01f);
        //     }
        // }
        // else if (change > 0) {
        //     while (change > 0) {
        //         change -= DecayRate;
        //         setBarScale("flash");
        //         yield return new WaitForSeconds(0.01f);
        //     }
        // }
        change = 0;
        changeFlash.SetActive(false);
        lerping = false;
        flashlerp = null;
    }

    public void SetValue(float maxvalue, float currentvalue)
    {
        maxValue = maxvalue;
        currentValue = currentvalue;
        value = getPercentage(currentValue, maxValue);
    }

    public void UpdateTotal(int amt) {
        total = amt;
        totalText.text = "" + total;
    }
}

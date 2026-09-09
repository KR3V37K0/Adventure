using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimeVisualizer : MonoBehaviour
{
    [SerializeField] TMP_Text txt_Time,txt_Sec;
    Coroutine watchCoroutine;

    private void OnEnable()
    {
        if (watchCoroutine != null)
            StopCoroutine(watchCoroutine);
        watchCoroutine = StartCoroutine(Watch());
    }

    private void OnDisable()
    {
        if (watchCoroutine != null)
        {
            StopCoroutine(watchCoroutine);
            watchCoroutine = null;
        }
    }

    private IEnumerator Watch()
    {
        while (true)
        {
            txt_Time.text = DateTime.Now.ToString("HH:mm");
            txt_Sec.text = DateTime.Now.ToString(":ss");
            yield return new WaitForSeconds(1f);
        }
    }
}
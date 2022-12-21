using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    private float length = 5f; // toujours entier mais float pour faire une division



    public void SetLength(float _length)
    {
        length = _length;
    }
    public float GetFillRatio()
    {
        float fillRatio = GetComponent<Scrollbar>().size;
        return fillRatio;
    }

    public float GetNewFillRatio( float _additionalFillRatio)
    {
        return Mathf.Clamp(GetComponent<Scrollbar>().size + _additionalFillRatio / length, 0.0f, 1.0f); // division par la longueur pour ramener le remplissage dans [0,1]
    }
    public void SetFillRatio(float _fillRatio)
    {
        _fillRatio = Mathf.Clamp(_fillRatio, 0.0f, 1.0f);
        GetComponent<Scrollbar>().size = _fillRatio;
    }

    public void FillGauge(float _additionalFillRatio)
    {
        SetFillRatio(GetComponent<Scrollbar>().size + _additionalFillRatio / length); // division par la longueur pour ramener le remplissage dans [0,1]
    }

    public IEnumerator FillGaugeCoroutine(float _targetRatio)
    {
        //float targetRatio = Mathf.Clamp(_gauge.GetComponent<Scrollbar>().size + _additionalFillRatio, 0.0f, 1.0f);
        Debug.Log("gaugecoroutine");
        while (Mathf.Abs(GetComponent<Scrollbar>().size - _targetRatio) > 0.01)
        {

            GetComponent<Scrollbar>().size = Mathf.Lerp(GetComponent<Scrollbar>().size, _targetRatio, 5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        GetComponent<Scrollbar>().size = _targetRatio;
    }
}

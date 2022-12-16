using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Place 
{
    public int counter = 0;
    public string[] texts;
    public Sprite advisorSprite;
    public Transform building3D;
    public float height;

    public float politics = 1/5f;
    public float culture = 0;
    public float water = 0 ;
    public float prestige = 0;

    public float[] gaugeRatios = new float[] { 1/5, 0, 0, 0 };
    public float[] targetEmpireRatios = new float[] { 0, 0, 0, 0 };

    public Place()
    {

    }

    public Place(string[] _texts)
    {
        texts = _texts;
    }

    public void SetBuilding3D(Transform _building3D, float _height = 5)
    {
        building3D = _building3D;
        height = _height;
    }

    public string GetCurrentText()
    {
        return texts[counter%texts.Length];
    }
    public void IncreaseCount() { counter++; }

    public void SetAdvisor(Sprite _sprite)
    {
        advisorSprite = _sprite;
    }

    // TODO renommer les variables ici
    public void SetPolitics(float _politics)
    {
        politics = _politics;
    }

    public void SetCulture(float _politics)
    {
        culture = _politics;
    }

    public void SetWater(float _politics)
    {
        water = _politics;
    }

    public void SetPrestigePolitics(float _politics)
    {
        prestige = _politics;
    }

    public IEnumerator BuildBuilding()
    {
        GM.Instance.canAct = false;

        
        /*for (int i = 0; i < gaugeRatios.Length; i++)
        {
            targetEmpireRatios[i] = UI_Manager.Instance.GetNewFillRatio(UI_Manager.Instance.imperialGauges[i], gaugeRatios[i]);
        }
        Debug.Log(targetEmpireRatios);
        float targetPeopleRatio = UI_Manager.Instance.GetNewFillRatio(UI_Manager.Instance.peopleGauge, -1/5f);

        Debug.Log(UI_Manager.Instance.GetFillRatio(UI_Manager.Instance.imperialGauges[0]));
        Debug.Log(targetEmpireRatios);

        for (int i = 0; i < gaugeRatios.Length; i++)
        {
            UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FillGaugeCoroutine(UI_Manager.Instance.imperialGauges[1], targetEmpireRatios[i]));
        }
        UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FillGaugeCoroutine(UI_Manager.Instance.peopleGauge, targetPeopleRatio));

        while (UI_Manager.Instance.GetFillRatio(UI_Manager.Instance.imperialGauges[0]) != targetEmpireRatios[0] || UI_Manager.Instance.GetFillRatio(UI_Manager.Instance.peopleGauge) != targetPeopleRatio)
        {
            Debug.Log("Hold on");
            yield return new WaitForEndOfFrame();
        }*/

        //UI_Manager.Instance.UI_Choice.SetActive(false);
        UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FadeUI(UI_Manager.Instance.UI_Choice.GetComponent<CanvasGroup>(), 0));

        building3D.gameObject.SetActive(true);
        Vector3 initialBuildingPos = building3D.position;
        building3D.position -= height * Vector3.up;
        Vector3 targetPos = Camera_Manager.Instance.GetTargetPosition(building3D);
        

        while ((Camera_Manager.Instance.cam.transform.position - targetPos).magnitude > 0.1)
        {
            Debug.Log(Camera_Manager.Instance.cam.transform.position - targetPos);
            Camera_Manager.Instance.cam.transform.position = Vector3.Lerp(Camera_Manager.Instance.cam.transform.position, targetPos, 1 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        while ((building3D.transform.position - initialBuildingPos).magnitude > 0.1)
        {
            building3D.position = Vector3.Lerp (building3D.position, initialBuildingPos, 0.7f * Time.deltaTime);
            Camera_Manager.Instance.cam.transform.position = targetPos + 0.1f * Random.insideUnitSphere;
            yield return new WaitForEndOfFrame();
        }
        Camera_Manager.Instance.cam.transform.position = targetPos;

        GM.Instance.canAct = true;
        //UI_Manager.Instance.UI_Choice.SetActive(true);
        UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FadeUI(UI_Manager.Instance.UI_Choice.GetComponent<CanvasGroup>(), 1));
        GM.Instance.MoveToNextChoices();
    }
}

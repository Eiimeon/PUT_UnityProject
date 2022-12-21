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

    // TODO delete if useless
    /*public float politics = 1/5f;
    public float culture = 0;
    public float water = 0 ;
    public float prestige = 0;*/
    public float people = 0;

    public float[] gaugeRatios = new float[] { 0f, 0, 0, 0 };

    // Ici pour la coroutine, ça marche pas en local
    public float[] targetEmpireRatios = new float[] { 0.5f, 0.5f, 0, 0 };

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

    public void SetPolitics(float _politics)
    {
        gaugeRatios[0] = _politics;
    }

    public void SetCulture(float _culture)
    {
        gaugeRatios[1] = _culture;
    }

    public void SetWater(float _water)
    {
        gaugeRatios[2] = _water;
    }

    public void SetPrestige(float _prestige)
    {
        gaugeRatios[3] = _prestige;
    }

    public void SetPeople(float _people)
    {
        people = _people;
    }

    public void SetRatios(float[] _ratios)
    {
        gaugeRatios = _ratios;
    }

    public IEnumerator BuildBuilding()
    {
        GM.Instance.canAct = false;

        
        /*for (int i = 0; i < gaugeRatios.Length; i++)
        {
            targetEmpireRatios[i] = UI_Manager.Instance.GetNewFillRatio(UI_Manager.Instance.imperialGauges[i], gaugeRatios[i]);
        }*/

        for (int i = 0; i < gaugeRatios.Length; i++)
        {
            Debug.Log(UI_Manager.Instance.imperialGauges[i]);
            Debug.Log(gaugeRatios[i]);
            //Debug.Log(UI_Manager.Instance.GetNewFillRatio(UI_Manager.Instance.imperialGauges[i], 1/5f));
            targetEmpireRatios[i] = UI_Manager.Instance.imperialGauges[i].GetComponent<Gauge>().GetNewFillRatio(gaugeRatios[i]);
        }
        //Debug.Log(targetEmpireRatios);
        float targetPeopleRatio = UI_Manager.Instance.peopleGauge.GetComponent<Gauge>().GetNewFillRatio(people - 1);

        //Debug.Log(UI_Manager.Instance.GetFillRatio(UI_Manager.Instance.imperialGauges[0]));
        //Debug.Log(targetEmpireRatios);

        for (int i = 0; i < gaugeRatios.Length; i++)
        {
            //UI_Manager.Instance.imperialGauges[i].GetComponent<Gauge>().StartCoroutine(FillGaugeCoroutine(targetEmpireRatios[i]));
            UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.imperialGauges[i].GetComponent<Gauge>().FillGaugeCoroutine(targetEmpireRatios[i]));
        }
        //UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FillGaugeCoroutine(UI_Manager.Instance.imperialGauges[0], targetEmpireRatios[0]));
        //UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FillGaugeCoroutine(UI_Manager.Instance.peopleGauge, targetPeopleRatio));

        UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.peopleGauge.GetComponent<Gauge>().FillGaugeCoroutine(targetPeopleRatio));

        while (UI_Manager.Instance.imperialGauges[0].GetComponent<Gauge>().GetFillRatio() != targetEmpireRatios[0] || UI_Manager.Instance.peopleGauge.GetComponent<Gauge>().GetFillRatio() != targetPeopleRatio)
        {
            //Debug.Log("Hold on");
            yield return new WaitForEndOfFrame();
        }

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

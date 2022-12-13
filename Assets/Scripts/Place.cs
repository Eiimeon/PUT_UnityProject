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

    public IEnumerator BuildBuilding()
    {
        GM.Instance.canAct = false;
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

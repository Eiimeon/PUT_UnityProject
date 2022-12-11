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

    public Place()
    {

    }

    public Place(string[] _texts)
    {
        texts = _texts;
    }

    public void SetBuilding3D(Transform _building3D)
    {
        building3D = _building3D;
    }

    public string GetCurrentText()
    {
        return texts[counter%texts.Length];
    }
    public void IncreaseCount() { counter++; }

    /*public void BuildBuilding()
    {
        building3D.gameObject.SetActive(true);
    }*/

    public IEnumerator BuildBuilding()
    {
        GM.Instance.canAct = false;
        //UI_Manager.Instance.UI_Choice.SetActive(false);
        UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FadeUI(UI_Manager.Instance.UI_Choice.GetComponent<CanvasGroup>(), 0));

        building3D.gameObject.SetActive(true);
        Vector3 initialBuildingPos = building3D.position;
        building3D.position -= 25 * Vector3.up;
        Vector3 targetPos = Camera_Manager.Instance.GetTargetPosition(building3D);
        

        while ((Camera_Manager.Instance.cam.transform.position - targetPos).magnitude > 10)
        {
            Debug.Log(Camera_Manager.Instance.cam.transform.position - targetPos);
            Camera_Manager.Instance.cam.transform.position = Vector3.Lerp(Camera_Manager.Instance.cam.transform.position, targetPos, 1 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        while ((building3D.transform.position - initialBuildingPos).magnitude > 1)
        {
            building3D.position = Vector3.Lerp (building3D.position, initialBuildingPos, 0.5f * Time.deltaTime);
            Camera_Manager.Instance.cam.transform.position = targetPos + 1f * Random.insideUnitSphere;
            yield return new WaitForEndOfFrame();
        }
        Camera_Manager.Instance.cam.transform.position = targetPos;

        GM.Instance.canAct = true;
        //UI_Manager.Instance.UI_Choice.SetActive(true);
        UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.FadeUI(UI_Manager.Instance.UI_Choice.GetComponent<CanvasGroup>(), 1));
        GM.Instance.MoveToNextChoices();
    }
}

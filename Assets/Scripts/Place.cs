using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void setBuilding3D(Transform _building3D)
    {
        building3D = _building3D;
    }

    public void IncreaseCount() { counter++; }

    public void BuildBuilding()
    {
        building3D.gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;


[Serializable]
public class Emissary
{
    public int index;
    public bool firstAppearance = true;

    public int speechCounter = 0;

    public TextMeshProUGUI displayedText;

    public string[] introTexts;

    public string[] endTexts;
    public string[] failureTexts;
    public string[] successTexts;
    public string[] specialSuccessTexts;

    //public Image[] emissaryImage;

    public Emissary()
    {

    }

    public Emissary(string[] _it, string[] _st, string[] _ft, string[] _sst)
    {
        introTexts = _it;
        failureTexts = _ft;
        successTexts = _st;
        specialSuccessTexts = _sst;
    }

    public Emissary(string[] _it, string[] _st, string[] _ft, string[] _sst, int _index)
    {
        introTexts = _it;
        failureTexts = _ft;
        successTexts = _st;
        specialSuccessTexts = _sst;

        index = _index;
    }

    public void SetTexts(string[] _it, string[] _st, string[] _ft, string[] _sst)
    {
        introTexts = _it;
        failureTexts = _ft;
        successTexts = _st;
        specialSuccessTexts = _sst;
    }
    public void SetTMP()
    {
        displayedText = UI_Manager.Instance.emissaryText;
    }

    /*public Emissary(string[] _texts, Image[] _image)
    {
        //this.texts = _texts;
        emissaryImage = _image;
    }*/

}

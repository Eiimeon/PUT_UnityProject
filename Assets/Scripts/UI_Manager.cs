using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public class Place : MonoBehaviour
    {

    }

    

    public Dictionary<string, Place> places = new Dictionary<string, Place>();

    public Place leftPlace;
    public Place rightPlace;

    public Image leftAdvisor;
    public Image rightAdvisor;

    public TextMeshProUGUI displayedText;

    public string leftText;
    public string rightText;
    public string midText = "Appuyez sur un conseiller pour �couter ce qu'il a � vous dire. Restez appuy�(e) pour construire le b�timent qu'il vous sugg�re";

    // Start is called before the first frame update
    void Start()
    {
        displayedText.text = midText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

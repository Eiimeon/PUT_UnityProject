using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public class Place : MonoBehaviour
    {
        public int counter = 0;
        public string[] texts;
        public Image advisorImage;

        public Place()
        {

        }

        public Place(string[] _texts)
        {
            texts = _texts;
        }
    }

    public Dictionary<string, Place> places = new Dictionary<string, Place>();

    public string[,] level = { { "Forum","N�cropole"} };
    public int choiceCounter = -1; // Car le premier MoveToNextChoices fera passer � 0

    public Image leftAdvisor;
    public Image rightAdvisor;

    public TextMeshProUGUI displayedText;

    
    public string midText = "Appuyez sur un conseiller pour �couter ce qu'il a � vous dire. Restez appuy�(e) pour construire le b�timent qu'il vous sugg�re";

    // "curr variables"

    string currLeftKey;
    string currRightKey;

    public Place currLeftPlace;
    public Place currRightPlace;

    Sprite currLeftAdvisor;
    Sprite currRightAdvisor;

    public string currLeftText;
    public string currRightText;

    // Timers
    float QTimer = 0;
    float DTimer = 0;

    // Start is called before the first frame update

    private void BuildDictionnary()
    {
        string[] currTexts = { "forum" };
        Place currPlace = new Place(currTexts);
        places["Forum"] = currPlace;

        currTexts = new string[] { "n�cro1","n�cro2","n�cro3" };
        currPlace = new Place(currTexts);
        places["N�cropole"] = currPlace;

        currTexts = new string[] { "temple" };
        currPlace = new Place(currTexts);
        places["Temple"] = currPlace;

        currTexts = new string[] { "domus1","domus2" };
        currPlace = new Place(currTexts);
        places["Domus"] = currPlace;

        currTexts = new string[] { "egouts1","egouts2","egouts3" };
        currPlace = new Place(currTexts);
        places["�gouts"] = currPlace;

        currTexts = new string[] { "thermes" };
        currPlace = new Place(currTexts);
        places["Thermes"] = currPlace;

        currTexts = new string[] { "fontaine" };
        currPlace = new Place(currTexts);
        places["Fontaine"] = currPlace;

        currTexts = new string[] { "theatre" };
        currPlace = new Place(currTexts);
        places["Th��tre"] = currPlace;

        currTexts = new string[] { "remparts" };
        currPlace = new Place(currTexts);
        places["Remparts"] = currPlace;

        currTexts = new string[] { "teinturerie" };
        currPlace = new Place(currTexts);
        places["Teinturerie"] = currPlace;
    }

    private string GetKeyFromLevel(bool left,int counter)
    {
        
        if (left)
        {
            return level[counter, 0];
        }
        else
        {
            return level[counter, 1];
        }
    }

    private void MoveToNextChoices()
    {
        Debug.Log(choiceCounter);
        //choiceCounter++;
        Debug.Log(choiceCounter);

        currLeftKey = GetKeyFromLevel(true, choiceCounter);
        currRightKey = GetKeyFromLevel(false, choiceCounter);

        currLeftPlace = places[currLeftKey];
        currRightPlace = places[currRightKey];

        // Ins�rer r�cup�ratrion des images

        currLeftText = currLeftPlace.texts[currLeftPlace.counter%currLeftPlace.texts.Length];
        currRightText = currRightPlace.texts[currRightPlace.counter % currRightPlace.texts.Length];
        Debug.Log(currLeftText + "    " + currRightText);
    }

    void Start()
    {
        BuildDictionnary();
        displayedText.text = midText;
        MoveToNextChoices();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            displayedText.text = currLeftText;
            QTimer += Time.deltaTime;
        }
        else
        {
            QTimer = 0; 
        }
        if (Input.GetKey(KeyCode.C))
        {
            displayedText.text = currRightText;
            DTimer += Time.deltaTime;
        }
        else
        {
            DTimer = 0;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            displayedText.text = midText;
        }

        if (QTimer > 1) { Debug.Log("Choic gauche"); }
        if (DTimer > 1) { Debug.Log("Choic droite"); }
    }
}

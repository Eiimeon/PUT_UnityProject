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

    public string[,] level = { { "Forum","N�cropole" } , { "Temple", "Domus" } , { "Domus", "Buffer" } , { "�gouts", "Thermes" } , { "Buffer", "Fontaine" } , { "Thermes", "Buffer" } , { "Th��tre", "Fontaine" } , { "Buffer", "Buffer" } , { "Remparts", "Teinturerie" } , { "Buffer", "Buffer" } };
    public List<string> buffer = new List<string>();
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

    // Mechanics

    bool canChoose = true;

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
        string levelKey;
        
        if (left)
        {
            levelKey = level[counter, 0];
        }
        else
        {
            levelKey =  level[counter, 1];
        }

        if (levelKey == "Buffer")
        {
            if (buffer.Contains("N�cropole"))
            {
                levelKey = "N�cropole";
            }
            else if (buffer.Contains("�gouts"))
            {
                levelKey = "�gouts";
            }
            else
            {
                levelKey = buffer[0];
            }
        }
        return levelKey;
    }

    private void MoveToNextChoices()
    {
        Debug.Log(choiceCounter);
        choiceCounter++;
        Debug.Log(choiceCounter);

        currLeftKey = GetKeyFromLevel(true, choiceCounter);
        currRightKey = GetKeyFromLevel(false, choiceCounter);

        currLeftPlace = places[currLeftKey];
        currRightPlace = places[currRightKey];

        // Ins�rer r�cup�ratrion des images

        currLeftText = currLeftPlace.texts[currLeftPlace.counter%currLeftPlace.texts.Length];
        currRightText = currRightPlace.texts[currRightPlace.counter % currRightPlace.texts.Length];
        Debug.Log(currLeftText + "    " + currRightText);

        displayedText.text = midText;
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
            QTimer += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.C))
        {
            DTimer += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            displayedText.text = currLeftText;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            displayedText.text = currRightText;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            displayedText.text = midText;
        }

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.C))
        {
            canChoose = true;
            QTimer = 0;
            DTimer = 0;
        }

        if (QTimer > 1 & canChoose) 
        { 
            canChoose = false;
            Debug.Log("Choic gauche");
            buffer.Add(currRightKey);
            MoveToNextChoices();
        }
        if (DTimer > 1 & canChoose) 
        { 
            canChoose=false;
            Debug.Log("Choic droite");
            buffer.Add(currLeftKey);
            MoveToNextChoices();
        }
    }
}

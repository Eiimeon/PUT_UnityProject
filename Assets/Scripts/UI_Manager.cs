using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;

public class UI_Manager : MonoBehaviour
{
    public class Place : MonoBehaviour
    {
        public int counter = 0;
        public string[] texts;
        public Image advisorImage;
        public GameObject building3D;

        public Place()
        {

        }

        public Place(string[] _texts)
        {
            texts = _texts;
        }

        public void setBuilding3D(GameObject _building3D)
        {
            building3D = _building3D;
        }

        public void IncreaseCount() { counter++; }

        public void BuildBuilding()
        {
            building3D.SetActive(true);
        }

        
    }

    public class Emissary
    {
        public string[] introTexts;
        public string[] failureTexts;
        public string[] successTexts;
        public string[] specialSuccessTexts;

        public Image emissaryImage;

        public Emissary(string[] _it, string[] _ft, string[] st, string[] sst)
        {
            this.introTexts = _it;
            this.failureTexts = _ft;
            this.successTexts = st;
            this.specialSuccessTexts = sst;
        }
        public Emissary(string[] _texts, Image _image)
        {
            //this.texts = _texts;
            this.emissaryImage = _image;
        }
    }

    //public GameObject buildings3D;
    //public Transform[] allBuildings3D; 

    public GameObject UI_Choice;
    public GameObject UI_Emissary;
    public GameObject UI_City;

    public GameObject[] allBuildings3D;

    public List<Emissary> emissaryList;
    public int emissaryIndex = 0;

    public Dictionary<string, Place> places = new Dictionary<string, Place>();

    //public string[,] level = { { "Forum","N�cropole" } }; // short level for engame test
    public string[,] level = { { "Forum", "N�cropole" }, { "Temple", "Domus" }, { "Domus", "Buffer" }, { "�gouts", "Thermes1" }, { "Buffer", "Fontaine1" }, { "Thermes2", "Buffer" }, { "Th��tre", "Fontaine2" }, { "Buffer", "Buffer" }, { "Remparts", "Teinturerie" }, { "Buffer", "Buffer" } };
    public List<string> buffer = new List<string>();
    public List<string> deadKeys = new List<string>();
    
    public int choiceCounter = -1; // Car le premier MoveToNextChoices fera passer � 0

    public Image leftAdvisor;
    public Image rightAdvisor;
    public Image emissary;

    public TextMeshProUGUI displayedText;
    public TextMeshProUGUI emissaryText;
    
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

    public string currBuffer;

    // Mechanics

    bool canChoose = true;

    float QTimer = 0;
    float DTimer = 0;

    // Flags

    public bool choiceMode = true;
    public bool cityMode = false;
    public bool emissaryMode = false;

    // Start is called before the first frame update

    private void BuildBuildings3DArray()
    {
        foreach (GameObject child in allBuildings3D)
        {
            child.SetActive(false);
        }
    }
    private void BuildDictionnary()
    {
        string[] currTexts = { "Je vous sugg�re de construire un FORUM au centre de la ville. C'est un lieu d'�change o� les citoyens pourraient se retrouver pour �changer sur les probl�matiques de la cit�." };
        Place currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[0]);
        places["Forum"] = currPlace;

        currTexts = new string[] { "La cit� est naissante, mais les gens ne savent pas o� enterrer leurs morts, s'il vous plait, construisez une N�CROPOLE juste au del� des limites de la cit�.",
                                    "La situation devient urgente, �a fait des ann�es que les gens enterrent leurs morts � l'arrache, construisez une N�CROPOLE bon sang !",
                                    "Le peuple en a marre ! Construisez une N�CROPOLE ! Ca suffit de devoir enterrer nos morts comme des clochards !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[1]);
        places["N�cropole"] = currPlace;

        currTexts = new string[] { "Nous avons obtenu les droits pour cr�er � Toulouse un TEMPLE d�di� � la triade capitoline ! C'est ext�mement prestigieux ! Il y a Minerve, d�esse de la sagesse, Junon d�esse du foyer, et surtout Jupiter, dieu des dieux !\r\n" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[2].gameObject);
        places["Temple"] = currPlace;

        currTexts = new string[] { "Je pense que vous devriez cr�er un quartier r�sidentiel autour d'une DOMUS romaine. Ce sont des maisons � la pointe du bon go�t !",
                                    "Ce premier quartier avec DOMUS romaine est fabuleux ! Ne nous arr�tons pas en si bon chemin ! Je vous sous entends �videmment d'en cr�er un deuxi�me !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[3].gameObject);
        places["Domus"] = currPlace;

        currTexts = new string[] { "La construction de l'aqueduc nous a apport� plein d'eau, on va pouvoir mettre en place un r�seau d'�GOUTS avec les techniques romaines pour assainir la ville.",
                                    "La ville a plein d'eau et pourtant l'hygi�ne est toujours pourrie, �a ne va pas du tout, faut vraiment construire un r�seau d'�GOUTS !",
                                    "Construisez un r�seau d'�GOUTS ! C'est inadmissible ! Enfin ! On peut pas avoir autant d'eau et avoir des rues qui puent la mort !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[4].gameObject);
        places["�gouts"] = currPlace;

        currTexts = new string[] { "Nous pourrions agr�menter le forum de THERMES. Ces bains publics sont d'une part un lieu de relaxation, mais aussi un excellent lieu dans lequel aborder les discutions politiques." };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[5].gameObject);
        places["Thermes1"] = currPlace;

        currTexts = new string[] { "Nous pourrions agr�menter le forum de THERMES. Ces bains publics sont d'une part un lieu de relaxation, mais aussi un excellent lieu dans lequel aborder les discutions politiques." };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[6].gameObject);
        places["Thermes2"] = currPlace;

        currTexts = new string[] { "Avec toute cette eau, nous allons pouvoir faire de magnifiques FONTAINES ! Avec de fort belles sculptures racontant d'h�ro�ques mythes romains !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[7].gameObject);
        places["Fontaine1"] = currPlace;

        currTexts = new string[] { "Avec toute cette eau, nous allons pouvoir faire de magnifiques FONTAINES ! Avec de fort belles sculptures racontant d'h�ro�ques mythes romains !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[8].gameObject);
        places["Fontaine2"] = currPlace;

        currTexts = new string[] { "Notre cit� a une population importante d�sormais, je vous sugg�re de construire un gigantesque TH�ATRE, qui pourrait accueillir la moiti� de la population Toulousaine, afin de montrer des pi�ces romaines." };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[9].gameObject);
        places["Th��tre"] = currPlace;

        currTexts = new string[] { "Les REMPARTS de Tib�re commencent � dater un peu, nous pourrions leur redonner une petite jeunesse en y ajouter des ornements et des dorures ! Ca ne prot�ge de rien, mais �a en jette !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[10].gameObject);
        places["Remparts"] = currPlace;

        currTexts = new string[] { "Nous avons les ressources aux alentours pour nous lancer dans le commerce de pigments et cr�er une TEINTURERIE. Ce nouveau commerce permettrait � Toulouse de gagner en renomm�e aux alentours." };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[11].gameObject);
        places["Teinturerie"] = currPlace;
    }

    private void BuildEmissaries()
    {
        emissaryList.Add(new Emissary(
            new string[] {"Haha ! C'est du bel ouvrage ! Tu vois petit gars, �a c'est les bases d'une grande ville, de grandes routes perpendiculaires, et surtout de grandes portes pour montrer qu'ici, c'est chez nous !",
                            "Tu as de la chance que l'empereur ait d�cid� de financer la reconstruction de Tolosa et accept� ma requ�te de te placer ici. Mais ne te m�prends pas, superviser l'urbanisme d'une cit� est une grande responsabilit�.",
                            "[Fondu au noir. La construction des portes est achev�e]",
                            "Je laisse la ville entre tes mains, je reviendrai dans 5 ans. J'esp�re que cette ville sera devenue un vrai cit� � mon retour. Fais centraliser l'activit� politique de Tolosa, et alors l'empereur sera content."},
            new string[] { "On a re�u des �chos jusqu'� Rome ! Tolosa est une vrai petite cit� maintenant ! Je suis fier de toi, maintenant j'en ai le c�ur net, je peux valider sans crainte la d�cision de l'empereur de faire don de remparts � ta ville !" },
            new string[] { },
            new string[] { }));
        emissaryList.Add(new Emissary(
            new string[] { },
            new string[] { },
            new string[] { },
            new string[] { }));
        emissaryList.Add(new Emissary(
            new string[] { },
            new string[] { },
            new string[] { },
            new string[] { }));
        emissaryList.Add(new Emissary(
            new string[] { },
            new string[] { },
            new string[] { },
            new string[] { }));
    }

    private string GetKeyFromLevel(bool left,int counter)
    {     
        if (left)
        {
            currBuffer = level[counter, 0];
        }
        else
        {
            currBuffer =  level[counter, 1];
        }

        if (currBuffer == "Buffer")
        {
            if (buffer.Contains("N�cropole"))
            {
                currBuffer = "N�cropole";
            }
            else if (buffer.Contains("�gouts"))
            {
                currBuffer = "�gouts";
            }
            else
            {
                currBuffer = buffer[0];
                deadKeys.Add(currBuffer);
            }
            buffer.Remove(currBuffer);
        }
        return currBuffer;
    }

    private void Shadow(Image advisor, bool isShadowed)
    {
        if (isShadowed)
        {
            advisor.GetComponent<Image>().color = Color.grey;
            advisor.GetComponent<Image>().rectTransform.localScale = 1f * Vector3.one;
        }
        else
        {
            advisor.GetComponent<Image>().color = Color.white;
            advisor.GetComponent<Image>().rectTransform.localScale = 1.5f * Vector3.one;
        }
    }

    private void MakeAChoice()
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
            Shadow(leftAdvisor, false);
            Shadow(rightAdvisor, true);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            displayedText.text = currRightText;
            Shadow(rightAdvisor, false);
            Shadow(leftAdvisor, true);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            displayedText.text = midText;
            Shadow(leftAdvisor, true);
            Shadow(rightAdvisor, true);
        }

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.C))
        {
            canChoose = true;
            QTimer = 0;
            DTimer = 0;
        }

        if ((QTimer > 1 || DTimer > 1) && canChoose)
        {
            Choose();
        }
    }
    private void Choose()
    {
        canChoose = false;
        if (QTimer > 1)
        {
            currLeftPlace.BuildBuilding();
            if (!deadKeys.Contains(currRightKey))
            {
                buffer.Add(currRightKey);
            }
        }
        if (DTimer > 1)
        {
            currRightPlace.BuildBuilding();
            if (!deadKeys.Contains(currLeftKey))
            {
                buffer.Add(currLeftKey);
            }
        }
        currLeftPlace.IncreaseCount();
        currRightPlace.IncreaseCount();
        MoveToNextChoices();
    }
    private void MoveToNextChoices()
    {
        choiceCounter++;

        if (choiceCounter < level.Length/2)
        {
            currLeftKey = GetKeyFromLevel(true, choiceCounter);
            currRightKey = GetKeyFromLevel(false, choiceCounter);

            currLeftPlace = places[currLeftKey];
            currRightPlace = places[currRightKey];

            // Ins�rer r�cup�ratrion des images

            currLeftText = currLeftPlace.texts[currLeftPlace.counter % currLeftPlace.texts.Length];
            currRightText = currRightPlace.texts[currRightPlace.counter % currRightPlace.texts.Length];
            Debug.Log(currLeftText + "    " + currRightText);

            displayedText.text = midText;

            Shadow(leftAdvisor, true);
            Shadow(rightAdvisor, true);
        }
        else
        {
            displayedText.text = "c'est fini";
        }
        
    }

    private void beginEmissarySection(int emissaryIndex)
    {
        SwitchMode(true);
    }

    private void SwitchMode(bool emissary = false)
    {
        if (!emissaryMode && !emissary)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (choiceMode)
                {
                    choiceMode = false;
                    cityMode = true;
                }
                else if (cityMode)
                {
                    cityMode = false;
                    choiceMode = true;
                }
            }
        }
        else if (!emissaryMode && emissary)
        {
            cityMode = false;
            choiceMode = false;
            emissaryMode = true;
        }
        else if (emissaryMode && !emissary)
        {
            emissaryMode = false;
            cityMode = false;
            choiceMode = true;
        }
        else if ( emissaryMode && emissary ){
            Debug.Log("Normalement, �a n'arrive pas");
        }
        UI_Choice.SetActive(choiceMode);
        UI_Emissary.SetActive(emissaryMode);
        UI_City.SetActive(cityMode);

    }

    void Start()
    {
        BuildBuildings3DArray();
        BuildDictionnary();
        displayedText.text = midText;
        MoveToNextChoices();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchMode(!emissaryMode);
        }
        if (Input.GetKeyDown(KeyCode.M) && !emissaryMode)
        {
            SwitchMode();
        }
        if (choiceMode)
        {
            MakeAChoice();
        }
    }
}

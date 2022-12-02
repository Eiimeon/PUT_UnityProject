using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public bool firstAppearance = true;

        public int speechCounter = 0;

        public string[] introTexts;

        public string[] endTexts;
        public string[] failureTexts;
        public string[] successTexts;
        public string[] specialSuccessTexts;

        public Image emissaryImage;

        public Emissary(string[] _it, string[] _st, string[] _ft, string[] _sst)
        {
            this.introTexts = _it;
            this.failureTexts = _ft;
            this.successTexts = _st;
            this.specialSuccessTexts = _sst;
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

    public List<Emissary> emissaryList = new List<Emissary>();
    public int emissaryIndex = 0;
    public string successState = "success";

    public Dictionary<string, Place> places = new Dictionary<string, Place>();

    //public string[,] level = { { "Forum","Nécropole" } }; // short level for engame test
    public string[,] level = { { "Forum", "Nécropole" }, { "Temple", "Domus" }, { "Domus", "Buffer" }, { "Égouts", "Thermes1" }, { "Buffer", "Fontaine1" }, { "Thermes2", "Buffer" }, { "Théâtre", "Fontaine2" }, { "Buffer", "Buffer" }, { "Remparts", "Teinturerie" }, { "Buffer", "Buffer" } };
    public List<string> built = new List<string>();
    public List<string> builtThisLevel = new List<string>();
    public List<string> buffer = new List<string>();
    public List<string> deadKeys = new List<string>();
    
    public int choiceCounter = -1; // Car le premier MoveToNextChoices fera passer à 0
    public int[] levelStarts = { 1, 3, 6, 10 };

    public Image blackPanel;
    public Image leftAdvisor;
    public Image rightAdvisor;
    public Image emissary;

    public TextMeshProUGUI displayedText;
    public TextMeshProUGUI emissaryText;
    
    public string midText = "Appuyez sur un conseiller pour écouter ce qu'il a à vous dire. Restez appuyé(e) pour construire le bâtiment qu'il vous suggère";

    // "curr variables"

    public Emissary currEmissary;

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

    public bool monitorFirstAppearance;

    public bool choiceMode = true;
    public bool cityMode = false;
    public bool emissaryMode = false;

    public bool fading = false;

    private void BuildBuildings3DArray()
    {
        foreach (GameObject child in allBuildings3D)
        {
            child.SetActive(false);
        }
    }
    private void BuildDictionnary()
    {
        string[] currTexts = { "Je vous suggère de construire un FORUM au centre de la ville. C'est un lieu d'échange où les citoyens pourraient se retrouver pour échanger sur les problématiques de la cité." };
        Place currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[0]);
        places["Forum"] = currPlace;

        currTexts = new string[] { "La cité est naissante, mais les gens ne savent pas où enterrer leurs morts, s'il vous plait, construisez une NÉCROPOLE juste au delà des limites de la cité.",
                                    "La situation devient urgente, ça fait des années que les gens enterrent leurs morts à l'arrache, construisez une NÉCROPOLE bon sang !",
                                    "Le peuple en a marre ! Construisez une NÉCROPOLE ! Ca suffit de devoir enterrer nos morts comme des clochards !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[1]);
        places["Nécropole"] = currPlace;

        currTexts = new string[] { "Nous avons obtenu les droits pour créer à Toulouse un TEMPLE dédié à la triade capitoline ! C'est extêmement prestigieux ! Il y a Minerve, déesse de la sagesse, Junon déesse du foyer, et surtout Jupiter, dieu des dieux !\r\n" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[2].gameObject);
        places["Temple"] = currPlace;

        currTexts = new string[] { "Je pense que vous devriez créer un quartier résidentiel autour d'une DOMUS romaine. Ce sont des maisons à la pointe du bon goût !",
                                    "Ce premier quartier avec DOMUS romaine est fabuleux ! Ne nous arrêtons pas en si bon chemin ! Je vous sous entends évidemment d'en créer un deuxième !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[3].gameObject);
        places["Domus"] = currPlace;

        currTexts = new string[] { "La construction de l'aqueduc nous a apporté plein d'eau, on va pouvoir mettre en place un réseau d'ÉGOUTS avec les techniques romaines pour assainir la ville.",
                                    "La ville a plein d'eau et pourtant l'hygiène est toujours pourrie, ça ne va pas du tout, faut vraiment construire un réseau d'ÉGOUTS !",
                                    "Construisez un réseau d'ÉGOUTS ! C'est inadmissible ! Enfin ! On peut pas avoir autant d'eau et avoir des rues qui puent la mort !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[4].gameObject);
        places["Égouts"] = currPlace;

        currTexts = new string[] { "Nous pourrions agrémenter le forum de THERMES. Ces bains publics sont d'une part un lieu de relaxation, mais aussi un excellent lieu dans lequel aborder les discutions politiques." };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[5].gameObject);
        places["Thermes1"] = currPlace;

        currTexts = new string[] { "Nous pourrions agrémenter le forum de THERMES. Ces bains publics sont d'une part un lieu de relaxation, mais aussi un excellent lieu dans lequel aborder les discutions politiques." };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[6].gameObject);
        places["Thermes2"] = currPlace;

        currTexts = new string[] { "Avec toute cette eau, nous allons pouvoir faire de magnifiques FONTAINES ! Avec de fort belles sculptures racontant d'héroïques mythes romains !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[7].gameObject);
        places["Fontaine1"] = currPlace;

        currTexts = new string[] { "Avec toute cette eau, nous allons pouvoir faire de magnifiques FONTAINES ! Avec de fort belles sculptures racontant d'héroïques mythes romains !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[8].gameObject);
        places["Fontaine2"] = currPlace;

        currTexts = new string[] { "Notre cité a une population importante désormais, je vous suggère de construire un gigantesque THÉATRE, qui pourrait accueillir la moitié de la population Toulousaine, afin de montrer des pièces romaines." };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[9].gameObject);
        places["Théâtre"] = currPlace;

        currTexts = new string[] { "Les REMPARTS de Tibère commencent à dater un peu, nous pourrions leur redonner une petite jeunesse en y ajouter des ornements et des dorures ! Ca ne protège de rien, mais ça en jette !" };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[10].gameObject);
        places["Remparts"] = currPlace;

        currTexts = new string[] { "Nous avons les ressources aux alentours pour nous lancer dans le commerce de pigments et créer une TEINTURERIE. Ce nouveau commerce permettrait à Toulouse de gagner en renommée aux alentours." };
        currPlace = new Place(currTexts);
        currPlace.setBuilding3D(allBuildings3D[11].gameObject);
        places["Teinturerie"] = currPlace;
    }

    private void BuildEmissaries()
    {
        Emissary temp = new Emissary(
            new string[] {"Haha ! C'est du bel ouvrage ! Tu vois petit gars, ça c'est les bases d'une grande ville, de grandes routes perpendiculaires, et surtout de grandes portes pour montrer qu'ici, c'est chez nous !",
                            "Tu as de la chance que l'empereur ait décidé de financer la reconstruction de Tolosa et accepté ma requête de te placer ici. Mais ne te méprends pas, superviser l'urbanisme d'une cité est une grande responsabilité.",
                            "[Fondu au noir. La construction des portes est achevée]",
                            "Je laisse la ville entre tes mains, je reviendrai dans 5 ans. J'espère que cette ville sera devenue un vrai cité à mon retour. Fais centraliser l'activité politique de Tolosa, et alors l'empereur sera content."},
            new string[] { "On a reçu des échos jusqu'à Rome ! Tolosa est une vrai petite cité maintenant ! Je suis fier de toi, maintenant j'en ai le cœur net, je peux valider sans crainte la décision de l'empereur de faire don de remparts à ta ville !" },
            new string[] { "Sérieusement ?! Je te laisse 5 ans, et tout ce que fais c'est une pauvre nécropole ?! Tu comprends bien que je ne peux pas mentir dans mon rapport.. L'empereur voulait t'offrir des remparts pour ta ville, mais après avoir vu ça, je pense qu'il va surtout t'offrir un aller simple pour la légion étrangère." },
            new string[] { "" });
        emissaryList.Add(temp);

        temp = new Emissary(
            new string[] { "Hm...","...","Oui...","...","Euh...","...",
                            "Les portes sont conformes, je reconnais là le style de feu Auguste.",
                            "...","Toutefois votre ville n'est guère plus romaine que ça. Il va falloir faire mieux. Je vous laisse un peu moins d'une décennie."},
            new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire romain ici. Je suis en route pour l'Italie, mais je peux vous dire que l'Empereur vous sera favorable." },
            new string[] { "Eh bien, une domus ? C'est tout ce vous à proposer ? Je suis en route pour l'Italie, je ne manquerai pas de dire à l'empereur que je n'ai rien à lui dire" },
            new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire rom...",
                            "!!!",
                            "Oh mais vous avez un temple dédié à la triade capitoline ?! Il est si beau, il me rappelle celui de Rome ! Ce que j'ai hâte d'y retourner.. Croyez moi, je ne manquerai pas de louanger votre cité à l'empereur !"});
        emissaryList.Add(temp);

        temp = new Emissary(
        new string[] { "Alors comme ça le grand Canigula a aidé votre ville à financer son aqueduc ?",
                        "Quel grand magnanime !",
                        "Comme c'est excitant, vous allez pouvoir faire des tonnes de jolies choses avec toute cette eau !",
                        "N'est-ce pas ?" },
        new string[] { "Oh comme c'est beau ! Tous ces bâtiments sont si grâcieux …","... Du moment qu'on ne regarde pas avec le nez." },
        new string[] { "Ca manque un peu d'eau par ici.","Ne vous inquiétez pas, en prison vous aurez toute l'eau de vos larmes" },
        new string[] { "Oh comme c'est beau ! Tous ces bâtiments sont si grâcieux …","... et vous avez même enlevé la vieille odeur de rat crevé." });
        emissaryList.Add(temp);

        temp = new Emissary(
        new string[] { "Là c'est la tirade de l'empereur" },
        new string[] { "ok tier" },
        new string[] { "nul" },
        new string[] { "wah tema la cité bimillénaire" });
        emissaryList.Add(temp);
    }

    // ###################################################################

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
            if (buffer.Contains("Nécropole"))
            {
                currBuffer = "Nécropole";
            }
            else if (buffer.Contains("Égouts"))
            {
                currBuffer = "Égouts";
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
            built.Add(currLeftKey);
            builtThisLevel.Add(currLeftKey);
            if (!deadKeys.Contains(currRightKey))
            {
                buffer.Add(currRightKey);
                
            }
        }
        if (DTimer > 1)
        {
            currRightPlace.BuildBuilding();
            built.Add(currRightKey);
            builtThisLevel.Add(currRightKey);
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

            // Insérer récupératrion des images

            currLeftText = currLeftPlace.texts[currLeftPlace.counter % currLeftPlace.texts.Length];
            currRightText = currRightPlace.texts[currRightPlace.counter % currRightPlace.texts.Length];
            Debug.Log(currLeftText + "    " + currRightText);

            displayedText.text = midText;

            Shadow(leftAdvisor, true);
            Shadow(rightAdvisor, true);
        }
        else
        {
            checkLevelSwitch();
            displayedText.text = "c'est fini";
        }
        checkLevelSwitch();
    }

    private void SetSuccessState()
    {

    }

    private void BeginEmissarySection(int emissaryIndex)
    {
        SwitchMode(true);
        if (emissaryIndex < emissaryList.Count)
        {
            currEmissary = emissaryList[emissaryIndex];
            if (currEmissary.firstAppearance)
            {
                emissaryText.text = currEmissary.introTexts[0];
            }
            else
            {
                SetSuccessState();
                switch (successState)
                {
                    case "success":
                        currEmissary.endTexts = currEmissary.successTexts;
                        break;
                    case "specialSuccess":
                        currEmissary.endTexts = currEmissary.specialSuccessTexts;
                        break;
                    case "failure":
                        currEmissary.endTexts = currEmissary.failureTexts;
                        break;
                }
                emissaryText.text = currEmissary.endTexts[0];
            }
        }
        else
        {
            emissaryText.text = "c'est fini";
        }
    }

    private void EndEmissarySection()
    {
        currEmissary.speechCounter = 0;
        if (currEmissary.firstAppearance)
        {
            currEmissary.firstAppearance = false;
            SwitchMode(false);
        }
        else
        {
            if (successState == "failure")
            {
                currEmissary.firstAppearance = true;
            }
            else
            {
                emissaryIndex++;
            }
            SwitchMode(false);
            BeginEmissarySection(emissaryIndex);
        }
    }

    private void EmissaryMode()
    {
        if (currEmissary.firstAppearance)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currEmissary.speechCounter++;
                if (currEmissary.speechCounter < currEmissary.introTexts.Length)
                {
                    emissaryText.text = currEmissary.introTexts[currEmissary.speechCounter];
                }
                else
                {
                    EndEmissarySection();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currEmissary.speechCounter++;
                if (currEmissary.speechCounter < currEmissary.endTexts.Length)
                {
                    emissaryText.text = currEmissary.endTexts[currEmissary.speechCounter];
                }
                else
                {
                    EndEmissarySection();
                }
            }
        }
    }

    private void checkLevelSwitch()
    {
        if (levelStarts.Contains(choiceCounter))
        {
            SetSuccessState();
            builtThisLevel.Clear();
            BeginEmissarySection(emissaryIndex);
        }
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
            Debug.Log("Normalement, ça n'arrive pas");
        }
        UI_Choice.SetActive(choiceMode);
        UI_Emissary.SetActive(emissaryMode);
        UI_City.SetActive(cityMode);

    }

    // Coroutines ################################################## 

    public IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
    }
    public IEnumerator FadeTo(float targetAlpha, float duration, Action after = null, bool unfade = false)
    {
        float timer = 0f;
        Color initialColor = blackPanel.GetComponent<Image>().color;
        while (timer<duration) 
        {
            Debug.Log(timer);
            timer += Time.deltaTime;
            //blackPanel.GetComponent<Image>().color = Color.Lerp(initialColor, new Color(0, 0, 0, targetAlpha), timer / duration);
            blackPanel.GetComponent<Image>().color = Color.Lerp(blackPanel.GetComponent<Image>().color, new Color(0, 0, 0, targetAlpha), timer / duration);
            yield return null;
        }
        /*while (Mathf.Abs(blackPanel.GetComponent<Image>().color.a - targetAlpha)>0.005)
        {
            Debug.Log(timer);
            timer += Time.deltaTime;
            blackPanel.GetComponent<Image>().color = Color.Lerp(blackPanel.GetComponent<Image>().color, new Color(0, 0, 0, targetAlpha), 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }*/
        if (after!=null)
        {
            after();
        }
        if (unfade)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FadeTo(1 - targetAlpha, duration));
        }
    }
    /*public void FadeToBlack()
    {
        //Debug.Log("Enter FTB");
        //bool fading = true;
        Color color = blackPanel.GetComponent<Image>().color;
        Debug.Log(color.a);
        //blackPanel.GetComponent<Image>().color = new Color(color.r, color.g, color.b,1);
        float lerpSpeed = 100;

        if (fading)
        {
            Debug.Log("Fading");
            Color.Lerp(blackPanel.GetComponent<Image>().color, new Color(color.r, color.g, color.b, 1), lerpSpeed * Time.deltaTime);
        }
        else
        {
            Color.Lerp(blackPanel.GetComponent<Image>().color, new Color(color.r, color.g, color.b, 0), lerpSpeed * Time.deltaTime);
        }

        /*while (blackPanel.GetComponent<Image>().color.a < 0.95)
        {
            Debug.Log("shadowing");
            Color.Lerp(blackPanel.GetComponent<Image>().color, new Color(color.r, color.g, color.b, 1), lerpSpeed * Time.deltaTime); 
        }*/
        /*while (alpha > 0.05)
        {
            Mathf.Lerp(alpha, 0, lerpSpeed * Time.deltaTime);
        }
        //yield return null;
    }*/

    void Start()
    {
        BuildBuildings3DArray();
        BuildDictionnary();
        displayedText.text = midText;
        MoveToNextChoices();
        BuildEmissaries();
        currEmissary = emissaryList[0];
        monitorFirstAppearance = currEmissary.firstAppearance;
        Debug.Log(currEmissary);
        BeginEmissarySection(emissaryIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FadeTo(1f,1,null,true));
            fading = !fading;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //SwitchMode(!emissaryMode);
            BeginEmissarySection(emissaryIndex);
        }
        if (Input.GetKeyDown(KeyCode.M) && !emissaryMode)
        {
            SwitchMode();
        }
        if (choiceMode)
        {
            MakeAChoice();
        }
        if (emissaryMode)
        {
            EmissaryMode();
        }
        // "coroutines" du bled
        // FadeToBlack();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GM : MonoBehaviour
{

    // ----------------------------------------------------------
    //                         SINGLETON
    // ----------------------------------------------------------

    private static GM _instance;

    public static GM Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    // ----------------------------------------------------------
    // ----------------------------------------------------------

    /***
         *                                              
         *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ 
         *               __          __        ___  __  
         *    \  /  /\  |__) |  /\  |__) |    |__  /__` 
         *     \/  /~~\ |  \ | /~~\ |__) |___ |___ .__/ 
         *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ 
         *                                              
         *                                              
         */
    #region VARIABLES

    public List<Emissary> emissaries;
    public int emissaryIndex = 0;

    public Dictionary<string, Place> places = new Dictionary<string, Place>();
    [SerializeField] public List<Level> levels = new List<Level>();

    public Place currLeftPlace; // places[currLevel.placeKeys[currLevel.index,0]]
    public Place currRightPlace; // places[currLevel.placeKeys[currLevel.index,1]]
    public string currLeftKey; // currLevel.placeKeys[currLevel.index,0]
    public string currRightKey; // currLevel.placeKeys[currLevel.index,1]
    public Level currLevel;

    public List<string> built = new List<string>();
    public List<string> buffer = new List<string>();
    public List<string> deadKeys = new List<string>();

    public string successState = "success";
    public readonly string midText = "Appuyez sur un conseiller pour �couter ce qu'il a � vous dire. Restez appuy�(e) pour construire le b�timent qu'il vous sugg�re";

    float QTimer = 0;
    float DTimer = 0;

    bool canChoose = true;
    public bool canAct = true;

    public Transform[] buildingsTransforms;

    #endregion


    /***
     *                                                                
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __                   
     *     __               __            ___ ___       __   __   __  
     *    |__) |  | | |    |  \     |\/| |__   |  |__| /  \ |  \ /__` 
     *    |__) \__/ | |___ |__/     |  | |___  |  |  | \__/ |__/ .__/ 
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __                   
     *                                                                
     *                                                                
     */

    #region BUILD METHODS
    private void BuildDictionnary()
    {
        string[] currTexts = { "Je vous sugg�re de construire un FORUM au centre de la ville. C'est un lieu d'�change o� les citoyens pourraient se retrouver pour �changer sur les probl�matiques de la cit�." };
        Place currPlace = new Place(currTexts);
        currPlace.SetBuilding3D(buildingsTransforms[0]);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
        /*currPlace.SetPolitics(4);
        currPlace.SetCulture(2);
        currPlace.SetPrestige(2);*/
        float[] tempRatios = { 4, 2, 0, 2 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(0);
        places["Forum"] = currPlace;

        currTexts = new string[] { "La cit� est naissante, mais les gens ne savent pas o� enterrer leurs morts, s'il vous plait, construisez une N�CROPOLE juste au del� des limites de la cit�.",
                                    "La situation devient urgente, �a fait des ann�es que les gens enterrent leurs morts � l'arrache, construisez une N�CROPOLE bon sang !",
                                    "Le peuple en a marre ! Construisez une N�CROPOLE ! Ca suffit de devoir enterrer nos morts comme des clochards !" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[2]);
        tempRatios = new float[] { 0, 0, 0, 0 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(3);
        places["N�cropole"] = currPlace;

        currTexts = new string[] { "Port" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
        tempRatios = new float[] { 2, 2, 0, 0};
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(1);
        places["Port"] = currPlace;

        currTexts = new string[] { "March�" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[2]);
        tempRatios = new float[] { 0, 1, 0, 0 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(2);
        places["March�"] = currPlace;

        currTexts = new string[] { "Nous avons obtenu les droits pour cr�er � Toulouse un TEMPLE d�di� � la triade capitoline ! C'est ext�mement prestigieux ! Il y a Minerve, d�esse de la sagesse, Junon d�esse du foyer, et surtout Jupiter, dieu des dieux !\r\n" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
        tempRatios = new float[] { 0, 4, 0, 2 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(0);
        places["Temple"] = currPlace;

        currTexts = new string[] { "Je pense que vous devriez cr�er un quartier r�sidentiel autour d'une DOMUS romaine. Ce sont des maisons � la pointe du bon go�t !",
                                    "Ce premier quartier avec DOMUS romaine est fabuleux ! Ne nous arr�tons pas en si bon chemin ! Je vous sous entends �videmment d'en cr�er un deuxi�me !" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
        tempRatios = new float[] { 0, 2, 0, 0 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(1);
        places["Domus"] = currPlace;

        currTexts = new string[] { "La construction de l'aqueduc nous a apport� plein d'eau, on va pouvoir mettre en place un r�seau d'�GOUTS avec les techniques romaines pour assainir la ville.",
                                    "La ville a plein d'eau et pourtant l'hygi�ne est toujours pourrie, �a ne va pas du tout, faut vraiment construire un r�seau d'�GOUTS !",
                                    "Construisez un r�seau d'�GOUTS ! C'est inadmissible ! Enfin ! On peut pas avoir autant d'eau et avoir des rues qui puent la mort !" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[3]);
        places["�gouts"] = currPlace;

        currTexts = new string[] { "Nous pourrions agr�menter le forum de THERMES. Ces bains publics sont d'une part un lieu de relaxation, mais aussi un excellent lieu dans lequel aborder les discutions politiques." };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
        tempRatios = new float[] { 0, 3, 4, 0 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(1);
        places["Thermes Nord"] = currPlace;

        currTexts = new string[] { "Nous pourrions agr�menter le forum de THERMES. Ces bains publics sont d'une part un lieu de relaxation, mais aussi un excellent lieu dans lequel aborder les discutions politiques." };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
        tempRatios = new float[] { 0, 0, 4, 0 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(1);
        places["Thermes Sud"] = currPlace;

        currTexts = new string[] { "Avec toute cette eau, nous allons pouvoir faire de magnifiques FONTAINES ! Avec de fort belles sculptures racontant d'h�ro�ques mythes romains !" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
        tempRatios = new float[] { 0, 1, 2, 1 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(0);
        places["Fontaine Forum"] = currPlace;

        currTexts = new string[] { "Avec toute cette eau, nous allons pouvoir faire de magnifiques FONTAINES ! Avec de fort belles sculptures racontant d'h�ro�ques mythes romains !" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
        tempRatios = new float[] { 0, 0, 4, 1 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(0);
        places["Fontaine Monumentale"] = currPlace;

        currTexts = new string[] { "Chateau d'Eau" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[3]);
        tempRatios = new float[] { 0, 0, 1, 0 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(2);
        places["Chateau d'Eau"] = currPlace;

        currTexts = new string[] { "Notre cit� a une population importante d�sormais, je vous sugg�re de construire un gigantesque TH�ATRE, qui pourrait accueillir la moiti� de la population Toulousaine, afin de montrer des pi�ces romaines." };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
        tempRatios = new float[] { 0, 0, 0, 2 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(1);
        places["Th��tre"] = currPlace;

        currTexts = new string[] { "Jardins" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
        tempRatios = new float[] { 0, 0, 2, 2 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(0);
        places["Jardins"] = currPlace;

        currTexts = new string[] { "Les REMPARTS de Tib�re commencent � dater un peu, nous pourrions leur redonner une petite jeunesse en y ajouter des ornements et des dorures ! Ca ne prot�ge de rien, mais �a en jette !" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
        tempRatios = new float[] { 0, 0, 0, 3 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(0);
        places["Remparts+"] = currPlace;

        currTexts = new string[] { "Nous avons les ressources aux alentours pour nous lancer dans le commerce de pigments et cr�er une TEINTURERIE. Ce nouveau commerce permettrait � Toulouse de gagner en renomm�e aux alentours." };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
        places["Teinturerie"] = currPlace;

        currTexts = new string[] { "Domus+" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
        tempRatios = new float[] { 0, 0, 0, 0 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(2);
        places["Domus+"] = currPlace;

        currTexts = new string[] { "Port+" };
        currPlace = new Place(currTexts);
        currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
        tempRatios = new float[] { 0, 0, 0, 2 };
        currPlace.SetRatios(tempRatios);
        currPlace.SetPeople(0);
        places["Port+"] = currPlace;

        // Places offertes pas l'empereur entre les niveaux
        currTexts = new string[] { "Portes" };
        currPlace = new Place(currTexts);
        places["Portes"] = currPlace;

        currTexts = new string[] { "Remparts" };
        currPlace = new Place(currTexts);
        places["Remparts"] = currPlace;

        currTexts = new string[] { "Aqueduc" };
        currPlace = new Place(currTexts);
        places["Aqueduc"] = currPlace;

        // On assigne � chaque place le building donc le nom du GO correspond
        foreach (string key in places.Keys)
        {
            foreach(Transform t in buildingsTransforms)
            {
                if (key == t.name)
                {
                    places[key].SetBuilding3D(t);
                }
            }
        }
    }

    private void BuildEmissaries()
    {
        Emissary temp = new Emissary(
            new string[] {"Haha ! C'est du bel ouvrage ! Tu vois petit gars, �a c'est les bases d'une grande ville, de grandes routes perpendiculaires, et surtout de grandes portes pour montrer qu'ici, c'est chez nous !",
                            "Tu as de la chance que l'empereur ait d�cid� de financer la reconstruction de Tolosa et accept� ma requ�te de te placer ici. Mais ne te m�prends pas, superviser l'urbanisme d'une cit� est une grande responsabilit�.",
                            "[Fondu au noir. La construction des portes est achev�e]",
                            "Je laisse la ville entre tes mains, je reviendrai dans 5 ans. J'esp�re que cette ville sera devenue un vrai cit� � mon retour. Fais centraliser l'activit� politique de Tolosa, et alors l'empereur sera content."},
            new string[] { "On a re�u des �chos jusqu'� Rome ! Tolosa est une vrai petite cit� maintenant ! Je suis fier de toi, maintenant j'en ai le c�ur net, je peux valider sans crainte la d�cision de l'empereur de faire don de remparts � ta ville !" },
            new string[] { "S�rieusement ?! Je te laisse 5 ans, et tout ce que fais c'est une pauvre n�cropole ?! Tu comprends bien que je ne peux pas mentir dans mon rapport.. L'empereur voulait t'offrir des remparts pour ta ville, mais apr�s avoir vu �a, je pense qu'il va surtout t'offrir un aller simple pour la l�gion �trang�re." },
            new string[] { "" },
            0);
        emissaries.Add(temp);
        
        temp = new Emissary(
            new string[] { "Hm...","...","Oui...","...","Euh...","...",
                            "Les portes sont conformes, je reconnais l� le style de feu Auguste.",
                            "...","Toutefois votre ville n'est gu�re plus romaine que �a. Il va falloir faire mieux. Je vous laisse un peu moins d'une d�cennie."},
            new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire romain ici. Je suis en route pour l'Italie, mais je peux vous dire que l'Empereur vous sera favorable." },
            new string[] { "Eh bien, une domus ? C'est tout ce vous � proposer ? Je suis en route pour l'Italie, je ne manquerai pas de dire � l'empereur que je n'ai rien � lui dire" },
            new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire rom...",
                            "!!!",
                            "Oh mais vous avez un temple d�di� � la triade capitoline ?! Il est si beau, il me rappelle celui de Rome ! Ce que j'ai h�te d'y retourner.. Croyez moi, je ne manquerai pas de louanger votre cit� � l'empereur !" },
            1);
        emissaries.Add(temp);

        temp = new Emissary(
            new string[] { "Alors comme �a le grand Canigula a aid� votre ville � financer son aqueduc ?",
                        "Quel grand magnanime !",
                        "Comme c'est excitant, vous allez pouvoir faire des tonnes de jolies choses avec toute cette eau !",
                        "N'est-ce pas ?" },
        new string[] { "Oh comme c'est beau ! Tous ces b�timents sont si gr�cieux �", "... Du moment qu'on ne regarde pas avec le nez." },
        new string[] { "Ca manque un peu d'eau par ici.", "Ne vous inqui�tez pas, en prison vous aurez toute l'eau de vos larmes" },
        new string[] { "Oh comme c'est beau ! Tous ces b�timents sont si gr�cieux �", "... et vous avez m�me enlev� la vieille odeur de rat crev�." },
        2);
        emissaries.Add(temp);

        temp = new Emissary(
            new string[] { "L� c'est la tirade de l'empereur" },
        new string[] { "ok tier" },
        new string[] { "nul" },
        new string[] { "wah tema la cit� bimill�naire" },
        3);
        emissaries.Add(temp);

        
    }
    
    private void BuildLevels()
    {
        Level temp = new Level(0, new string[,] { { "Forum", "N�cropole" } , { "Port", "March�" } , { "Buffer", "Buffer" } }, emissaries[0]);
        levels.Add(temp);
        levels.Add(new Level(1, new string[,] { { "Temple", "Thermes Nord" }, { "Domus", "Fontaine Forum" }, { "Buffer", "Buffer" } }, emissaries[1]));
        levels.Add(new Level(2, new string[,] { { "Chateau d'Eau", "Fontaine Monumentale" }, { "Thermes Sud", "Jardins" }, { "Buffer", "Buffer" } }, emissaries[2]));
        levels.Add(new Level(3, new string[,] { { "Remparts+", "Domus+" }, { "Th��tre", "Port+" }, { "Buffer", "Buffer" } }, emissaries[3]));

        levels[0].SetPlace(places["Portes"]);
        levels[1].SetPlace(places["Remparts"]);
        levels[2].SetPlace(places["Aqueduc"]);
        levels[3].SetPlace(places["Portes"]);

        currLevel = levels[0];
    }

    #endregion


    /***
     *                                              
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ 
     *           ___ ___       __   __   ___  __    
     *     |\/| |__   |  |__| /  \ |  \ |__  /__`   
     *     |  | |___  |  |  | \__/ |__/ |___ .__/   
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ 
     *                                              
     *                                              
     */

    #region METHODS
    public void SetSuccessState()
    {

    }



    private void EmissaryMode()
    {
        if (currLevel.emissary.firstAppearance)
        {
            // KEYBOARD CONTROLS
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("Pressed Space");
                currLevel.emissary.speechCounter++;
                if (currLevel.emissary.speechCounter < currLevel.emissary.introTexts.Length)
                {
                    UI_Manager.Instance.emissaryText.text = currLevel.emissary.introTexts[currLevel.emissary.speechCounter];
                }
                else
                {
                    currLevel.EndEmissarySection();
                    //MoveToNextChoices();
                }
            }
            // TOUCH CONTROLS
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Pressed Space");
                currLevel.emissary.speechCounter++;
                if (currLevel.emissary.speechCounter < currLevel.emissary.introTexts.Length)
                {
                    UI_Manager.Instance.emissaryText.text = currLevel.emissary.introTexts[currLevel.emissary.speechCounter];
                }
                else
                {
                    currLevel.EndEmissarySection();
                    //MoveToNextChoices();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currLevel.emissary.speechCounter++;
                if (currLevel.emissary.speechCounter < currLevel.emissary.endTexts.Length)
                {
                    UI_Manager.Instance.emissaryText.text = currLevel.emissary.endTexts[currLevel.emissary.speechCounter];
                }
                else
                {
                    currLevel.EndEmissarySection();
                    //MoveToNextChoices();
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                currLevel.emissary.speechCounter++;
                if (currLevel.emissary.speechCounter < currLevel.emissary.endTexts.Length)
                {
                    UI_Manager.Instance.emissaryText.text = currLevel.emissary.endTexts[currLevel.emissary.speechCounter];
                }
                else
                {
                    currLevel.EndEmissarySection();
                    //MoveToNextChoices();
                }
            }
        }
    }

    private void MakeAChoice()
    {
        // KEYBOARD CONTROLS
        if (Input.GetKey(KeyCode.X))
        {
            QTimer += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.C))
        {
            DTimer += Time.deltaTime;
        }

        if (//true
            !UI_Manager.Instance.isShadowing
            /*!UI_Manager.Instance.IsShadowing(UI_Manager.Instance.leftAdvisor)&&
            !UI_Manager.Instance.IsShadowing(UI_Manager.Instance.rightAdvisor)*/)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                UI_Manager.Instance.displayedText.text = currLeftPlace.GetCurrentText();
                //UI_Manager.Instance.Shadow(UI_Manager.Instance.leftAdvisor, false);
                //UI_Manager.Instance.Shadow(UI_Manager.Instance.rightAdvisor, true);
                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.leftAdvisor, false));
                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.rightAdvisor, true));

            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                UI_Manager.Instance.displayedText.text = currRightPlace.GetCurrentText();
                //UI_Manager.Instance.Shadow(UI_Manager.Instance.rightAdvisor, false);
                //UI_Manager.Instance.Shadow(UI_Manager.Instance.leftAdvisor, true);
                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.leftAdvisor, true));
                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.rightAdvisor, false));
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                UI_Manager.Instance.displayedText.text = midText;
                //UI_Manager.Instance.Shadow(UI_Manager.Instance.leftAdvisor, true);
                //UI_Manager.Instance.Shadow(UI_Manager.Instance.rightAdvisor, true);
                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.leftAdvisor, true));
                UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.rightAdvisor, true));
            }
        }

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.C))
        {
            canChoose = true;
            QTimer = 0;
            DTimer = 0;
        }

        // TOUCH CONTROLS

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            int width = Screen.width;
            if (touch.position.x < width/2)
            {
                QTimer += Time.deltaTime;
            }
            else
            {
                DTimer += Time.deltaTime;
            }  
        }

        if (!UI_Manager.Instance.isShadowing)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];

                if (touch.position.y > Screen.height * 2 / 3)
                {
                    UI_Manager.Instance.displayedText.text = midText;
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.leftAdvisor, true));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.rightAdvisor, true));
                }
                else
                {
                    if ( touch.position.x < Screen.height /2 )
                    {
                        UI_Manager.Instance.displayedText.text = currLeftPlace.GetCurrentText();
                        UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.leftAdvisor, false));
                        UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.rightAdvisor, true));
                    }
                    else
                    {
                        UI_Manager.Instance.displayedText.text = currRightPlace.GetCurrentText();
                        UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.leftAdvisor, true));
                        UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.rightAdvisor, false));
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.C) /*|| Input.touchCount == 0*/)
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
            StartCoroutine(currLeftPlace.BuildBuilding());
            built.Add(currLeftKey);
            currLevel.built.Add(currLeftKey);
            if (!deadKeys.Contains(currRightKey))
            {
                buffer.Add(currRightKey);

            }
        }
        if (DTimer > 1)
        {
            StartCoroutine(currRightPlace.BuildBuilding());
            built.Add(currRightKey);
            currLevel.built.Add(currRightKey);
            if (!deadKeys.Contains(currLeftKey))
            {
                buffer.Add(currLeftKey);
            }
        }
        currLeftPlace.IncreaseCount();
        currRightPlace.IncreaseCount();
        //MoveToNextChoices();
    }

    public void MoveToNextChoices()
    {
        currLevel.choiceCounter++;
        if (currLevel.choiceCounter < currLevel.keys.Length / 2)
        {
            currLeftKey = GetKeyFromLevel(true, currLevel.choiceCounter);
            currRightKey = GetKeyFromLevel(false, currLevel.choiceCounter);

            currLeftPlace = places[currLeftKey];
            currRightPlace = places[currRightKey];

            UI_Manager.Instance.leftAdvisor.sprite = currLeftPlace.advisorSprite;
            UI_Manager.Instance.rightAdvisor.sprite = currRightPlace.advisorSprite;

            //currLeftText = currLeftPlace.texts[currLeftPlace.counter % currLeftPlace.texts.Length];
            //currRightText = currRightPlace.texts[currRightPlace.counter % currRightPlace.texts.Length];
            //Debug.Log(currLeftText + "    " + currRightText);

            UI_Manager.Instance.displayedText.text = midText;

            UI_Manager.Instance.Shadow(UI_Manager.Instance.leftAdvisor, true);
            UI_Manager.Instance.Shadow(UI_Manager.Instance.rightAdvisor, true);
        }
        else
        {
            CheckLevelSwitch();
            UI_Manager.Instance.displayedText.text = "c'est fini";
        }
        CheckLevelSwitch();
    }

    private void CheckLevelSwitch()
    {
        if (currLevel.choiceCounter >= currLevel.keys.Length/2)
        {
            SetSuccessState();
            currLevel.BeginEmissarySection(emissaryIndex);
        }
    }

    private string GetKeyFromLevel(bool left, int counter)
    {
        string currBuffer;
        if (left)
        {
            currBuffer = currLevel.keys[counter, 0];
        }
        else
        {
            currBuffer = currLevel.keys[counter, 1];
        }

        if (currBuffer == "Buffer")
        {
            /*if (buffer.Contains("N�cropole"))
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
            }*/
            currBuffer = buffer[0];
            deadKeys.Add(currBuffer);
            buffer.Remove(currBuffer);
        }
        return currBuffer;
    }
    #endregion

    /***
     *                                                                            
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __                                
     *     __  ___       __  ___          __   __       ___  ___     ___ ___  __  
     *    /__`  |   /\  |__)  |     |  | |__) |  \  /\   |  |__     |__   |  /  ` 
     *    .__/  |  /~~\ |  \  |     \__/ |    |__/ /~~\  |  |___    |___  |  \__, 
     *    __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __ __                               
     *                                                                            
     *                                                                            
     */

    void Start()
    {
        buildingsTransforms = Buildings_Manager.Instance.buildingsTransform;
        BuildDictionnary();
        BuildEmissaries();
        BuildLevels();
        currLevel.giftedPlace.building3D.gameObject.SetActive(true);
        currLevel.BeginEmissarySection(currLevel.emissary.index);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Debug.Log(Input.GetTouch(0).position);
        }
        if (canAct)
        {
            // KEYBOARD CONTROLS
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(UI_Manager.Instance.FadeTo(1f, 1, null, true));
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                //SwitchMode(!emissaryMode);
                //BeginEmissarySection(emissaryIndex);
            }
            if (Input.GetKeyDown(KeyCode.M) && !UI_Manager.Instance.emissaryMode)
            {
                UI_Manager.Instance.SwitchMode();
            }
            if (UI_Manager.Instance.choiceMode)
            {
                MakeAChoice();
            }
            if (UI_Manager.Instance.emissaryMode)
            {
                //Debug.Log("emissary mode");
                EmissaryMode();
            }

            //TOUCH CONTROLS
        }
    }
}

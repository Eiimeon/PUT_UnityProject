using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public readonly string midText = "Appuyez sur un conseiller pour écouter ce qu'il a à vous dire. Restez appuyé(e) pour construire le bâtiment qu'il vous suggère";

    float QTimer = 0;
    float DTimer = 0;

    bool canChoose = true;
    public bool canAct = true;

    public Transform[] buildingsTransforms;
    public Transform[] districtTransforms;

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
        if (MusicAndData_Manager.Instance.isFrench)
        {
            string[] currTexts = { "Je vous suggère de construire un FORUM au centre de la ville. C'est un lieu d'échange où les citoyens pourraient se retrouver pour échanger sur les problématiques de la cité." };
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

            currTexts = new string[] { "La cité est naissante, mais les gens ne savent pas où enterrer leurs morts, s'il vous plait, construisez une NÉCROPOLE juste au delà des limites de la cité."/*,
                                    "La situation devient urgente, ça fait des années que les gens enterrent leurs morts à l'arrache, construisez une NÉCROPOLE bon sang !",
                                    "Le peuple en a marre ! Construisez une NÉCROPOLE ! Ca suffit de devoir enterrer nos morts comme des clochards !"*/ };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[3]);
            tempRatios = new float[] { 0, 0, 0, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(3);
            places["Nécropole"] = currPlace;

            currTexts = new string[] { "Si nous avons installé la nouvelle Tolosa au bord de la Garonne, c'est pour la munir d'un PORT, afin d'avoir un commerce florissant et bonnes relations internationnales." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            tempRatios = new float[] { 2, 2, 0, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Port"] = currPlace;

            currTexts = new string[] { "Nous pourrions construire un MARCHÉ afin que les habitants puissent commercer et se fournir en ressources de tous les jours." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[2]);
            tempRatios = new float[] { 0, 0, 0, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(3);
            places["Marché"] = currPlace;

            currTexts = new string[] { "Nous avons obtenu les droits pour créer à Toulouse un TEMPLE dédié à la triade capitoline ! C'est extêmement prestigieux ! Ce sont les dieux vénérés à Rome !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
            tempRatios = new float[] { 0, 4, 0, 2 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(0);
            places["Temple"] = currPlace;

            currTexts = new string[] {  "Rome propose de nous envoyer des habitants pour notre cité à condition que nous leur construisions des DOMUS, de belles villas romaines, où les installer."
                                    /*"Je pense que vous devriez créer un quartier résidentiel autour d'une DOMUS romaine. Ce sont des maisons à la pointe du bon goût !",
                                    "Ce premier quartier avec DOMUS romaine est fabuleux ! Ne nous arrêtons pas en si bon chemin ! Je vous sous entends évidemment d'en créer un deuxième !"*/ };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            tempRatios = new float[] { 0, 2, 0, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Domus"] = currPlace;

            currTexts = new string[] { "La construction de l'aqueduc nous a apporté plein d'eau, on va pouvoir mettre en place un réseau d'ÉGOUTS avec les techniques romaines pour assainir la ville.",
                                    "La ville a plein d'eau et pourtant l'hygiène est toujours pourrie, ça ne va pas du tout, faut vraiment construire un réseau d'ÉGOUTS !",
                                    "Construisez un réseau d'ÉGOUTS ! C'est inadmissible ! Enfin ! On peut pas avoir autant d'eau et avoir des rues qui puent la mort !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[3]);
            places["Égouts"] = currPlace;

            currTexts = new string[] { "Nous pourrions agrémenter le forum de THERMES. Ces bains publics sont d'une part un lieu de relaxation, mais aussi un excellent lieu dans lequel aborder les discutions politiques." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            tempRatios = new float[] { 0, 3, 4, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Thermes Nord"] = currPlace;

            currTexts = new string[] { "Nous pourrions construire des THERMES au sud de la ville. Ces bains publics sont d'une part un lieu de relaxation, mais aussi un excellent lieu dans lequel aborder les discutions politiques." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
            tempRatios = new float[] { 0, 0, 4, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Thermes Sud"] = currPlace;

            currTexts = new string[] { "Notre forum est très beau ! Mais il le serait encore plus si nous l'agrémentions d'une magnique FONTAINE !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
            tempRatios = new float[] { 0, 2, 4, 1 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(0);
            places["Fontaine Forum"] = currPlace;

            currTexts = new string[] { "Avec toute cette eau, nous allons pouvoir faire un magnifique FONTAINE MONUMENTALE ! Avec de fort belles sculptures racontant d'héroïques mythes romains !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            tempRatios = new float[] { 0, 0, 4, 1 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(0);
            places["Fontaine Monumentale"] = currPlace;

            currTexts = new string[] { "Nous pourrions munir l'aqueduc d'un CHATEAU D'EAU afin de stocker l'eau et permettre aux habitants de s'en fournir de façon plus stable." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[3]);
            tempRatios = new float[] { 0, 0, 2, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(2);
            places["Chateau D'Eau"] = currPlace;

            currTexts = new string[] { "Notre cité a une population importante désormais, je vous suggère de construire un gigantesque THÉATRE, qui pourrait accueillir la moitié de la population Toulousaine !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            tempRatios = new float[] { 0, 0, 0, 2 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Théâtre"] = currPlace;

            currTexts = new string[] { "Avec toute cette eau, nous allons pouvoir cultiver de beaux JARDINS ! Ce sera magnifique et les gens pourront s'y prélasser." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[3]);
            tempRatios = new float[] { 0, 0, 2, 2 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Jardins"] = currPlace;

            currTexts = new string[] { "Les REMPARTS de Tibère commencent à dater un peu, nous pourrions leur redonner une petite jeunesse en y ajoutant des petites alcoves décoratives ! Ca ne protège de rien, mais ça en jette !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
            tempRatios = new float[] { 0, 0, 0, 3 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(0);
            places["Remparts+"] = currPlace;

            currTexts = new string[] { "Nous avons les ressources aux alentours pour nous lancer dans le commerce de pigments et créer une TEINTURERIE. Ce nouveau commerce permettrait à Toulouse de gagner en renommée aux alentours." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            places["Teinturerie"] = currPlace;

            currTexts = new string[] { "Nous pourrions créer un quartier de belles villas romaines, des DOMUS, pour nos habitants les plus aisés. Ils seront content et la ville présentera encore mieux !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
            tempRatios = new float[] { 0, 0, 0, 2 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Domus+"] = currPlace;

            currTexts = new string[] { "Et si nous augmentions les capacités de stockage du port ? Nous pourrions développer le commerce et en faire bénéficier le peuple !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[2]);
            tempRatios = new float[] { 0, 0, 0, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(2);
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

            currTexts = new string[] { "Vide" };
            currPlace = new Place(currTexts);
            places["Vide"] = currPlace;
        }
        //-------------------------------------------------------------------------------------------------------------------
        // English
        //-------------------------------------------------------------------------------------------------------------------
        else
        {
            string[] currTexts = { "I suggest you build a FORUM at the of the town. It's a place of exchage where citizen could meet and debate about the issues of the city." };
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

            currTexts = new string[] { "The city is dawning, but people doesn't know where they should bury their dead. Please build a NECROPOLIS right outside the city's boundaries."/*,
                                    "La situation devient urgente, ça fait des années que les gens enterrent leurs morts à l'arrache, construisez une NÉCROPOLE bon sang !",
                                    "Le peuple en a marre ! Construisez une NÉCROPOLE ! Ca suffit de devoir enterrer nos morts comme des clochards !"*/ };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[3]);
            tempRatios = new float[] { 0, 0, 0, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(3);
            places["Nécropole"] = currPlace;

            currTexts = new string[] { "If we've set new Tolosa by the Garonne, it is so we provide it a harbour in order to have flourishing trades and good international relations." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            tempRatios = new float[] { 2, 2, 0, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Port"] = currPlace;

            currTexts = new string[] { "We could build a MARKETPLACE so that people could trade and get the everyday supplies they need." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[2]);
            tempRatios = new float[] { 0, 0, 0, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(3);
            places["Marché"] = currPlace;

            currTexts = new string[] { "We've been allowed to build a TEMPLE dedicated to the capitoline triad ! This is extremely prestigious ! These are the gods worshiped in Rome !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
            tempRatios = new float[] { 0, 4, 0, 2 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(0);
            places["Temple"] = currPlace;

            currTexts = new string[] {  "Rome offered to send us inhabitants for our city provided that we build them DOMUSES, beautiful roman villas, where they'd live."
                                    /*"Je pense que vous devriez créer un quartier résidentiel autour d'une DOMUS romaine. Ce sont des maisons à la pointe du bon goût !",
                                    "Ce premier quartier avec DOMUS romaine est fabuleux ! Ne nous arrêtons pas en si bon chemin ! Je vous sous entends évidemment d'en créer un deuxième !"*/ };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            tempRatios = new float[] { 0, 2, 0, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Domus"] = currPlace;

            currTexts = new string[] { "La construction de l'aqueduc nous a apporté plein d'eau, on va pouvoir mettre en place un réseau d'ÉGOUTS avec les techniques romaines pour assainir la ville.",
                                    "La ville a plein d'eau et pourtant l'hygiène est toujours pourrie, ça ne va pas du tout, faut vraiment construire un réseau d'ÉGOUTS !",
                                    "Construisez un réseau d'ÉGOUTS ! C'est inadmissible ! Enfin ! On peut pas avoir autant d'eau et avoir des rues qui puent la mort !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[3]);
            places["Égouts"] = currPlace;

            currTexts = new string[] { "We could enhance the forum with THERMAE. These public baths are on the one hand a relaxation place, and on the other hand, a place to discuss about politics." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            tempRatios = new float[] { 0, 3, 4, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Thermes Nord"] = currPlace;

            currTexts = new string[] { "We could build THERMAE south of the city. These public baths are on the one hand a relaxation place, and on the other hand, a place to discuss about politics." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
            tempRatios = new float[] { 0, 0, 4, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Thermes Sud"] = currPlace;

            currTexts = new string[] { "Our forum is pretty ! But it'd be even prettier if we enhanced it with a beautiful FOUNTAIN !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
            tempRatios = new float[] { 0, 2, 4, 1 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(0);
            places["Fontaine Forum"] = currPlace;

            currTexts = new string[] { "With all this water, we are able tu build a beautiful MONUMENTAL FOUNTAIN ! With very pretty sculptures telling heroic roman myths." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            tempRatios = new float[] { 0, 0, 4, 1 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(0);
            places["Fontaine Monumentale"] = currPlace;

            currTexts = new string[] { "We could enhance the aqueduct with a WATER TOWER to store water and enable people to get some more steadily." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[3]);
            tempRatios = new float[] { 0, 0, 2, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(2);
            places["Chateau D'Eau"] = currPlace;

            currTexts = new string[] { "Our city is densely populated now. I suggest you build a gigantic THEATER that could host half the toulousian population !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            tempRatios = new float[] { 0, 0, 0, 2 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Théâtre"] = currPlace;

            currTexts = new string[] { "With a ll this water, we'll be able to cultivate pretty GARDENS ! It'll be beautiful and people could lounge there." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[3]);
            tempRatios = new float[] { 0, 0, 2, 2 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Jardins"] = currPlace;

            currTexts = new string[] { "Tiberius' FORTIFICATIONS are getting old, we could give it a new lease of life by adding small decorative alcoves ! Doesn't protect anything but nifty !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
            tempRatios = new float[] { 0, 0, 0, 3 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(0);
            places["Remparts+"] = currPlace;

            currTexts = new string[] { "Nous avons les ressources aux alentours pour nous lancer dans le commerce de pigments et créer une TEINTURERIE. Ce nouveau commerce permettrait à Toulouse de gagner en renommée aux alentours." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[0]);
            places["Teinturerie"] = currPlace;

            currTexts = new string[] { "We could build a district of beatiful roman villas, DOMUSES, for our richest people. They'll be happy and the town will look even niftier !" };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[1]);
            tempRatios = new float[] { 0, 0, 0, 2 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(1);
            places["Domus+"] = currPlace;

            currTexts = new string[] { "What if we increase the storage of the harbour ? We could increase our trading capacity and make people benefit from it." };
            currPlace = new Place(currTexts);
            currPlace.SetAdvisor(UI_Manager.Instance.advisors[2]);
            tempRatios = new float[] { 0, 0, 0, 0 };
            currPlace.SetRatios(tempRatios);
            currPlace.SetPeople(2);
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

            currTexts = new string[] { "Vide" };
            currPlace = new Place(currTexts);
            places["Vide"] = currPlace;
        }

        // On assigne à chaque place le building donc le nom du GO correspond
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

        foreach (string key in places.Keys)
        {
            foreach (Transform t in Buildings_Manager.Instance.districtTransforms)
            {
                Debug.Log("D_"+key);
                if ("D_"+key == t.name)
                {
                    places[key].SetDistrict(t);
                }
            }
        }

        // On assigne à chaque place le district donc le nom du GO correspond

        // On assigne à chaque bâtiment sa hauteur, qui correspond à la pronfondeur à laquelle on doit l'enterrer avant de le faire surgir de terre
        places["Forum"].height = 5f;
        places["Domus"].height = 3f;
        places["Domus+"].height = 3f;
        places["Aqueduc"].height = 15f;
        places["Nécropole"].height = 4f;
        places["Fontaine Monumentale"].height = 5f;
        places["Fontaine Forum"].height = 5f;
        places["Portes"].height = 15f;
        places["Remparts"].height = 15f;
        places["Remparts+"].height = 15f;
        places["Temple"].height = 6f;
        places["Thermes Nord"].height = 5f;
        places["Thermes Sud"].height = 5f;
        places["Théâtre"].height = 8f;
        places["Port"].height = 7f;
        places["Port+"].height = 7f;
        places["Jardins"].height = 3f;
        places["Marché"].height = 5f;
        places["Chateau D'Eau"].height = 7f;
    }

    private void BuildEmissaries()
    {
        if (MusicAndData_Manager.Instance.isFrench)
        {
            Emissary temp = new Emissary(
            new string[] {"Haha ! C'est du bel ouvrage ! Tu vois petit gars, ça c'est les bases d'une grande ville, de grandes routes perpendiculaires, et surtout de grandes portes pour montrer qu'ici, c'est chez nous !",
                            "Tu as de la chance que l'empereur Auguste ait décidé de financer la reconstruction de Tolosa et accepté ma requête de te placer ici.",
                            "Mais ne te méprends pas, superviser l'urbanisme d'une cité est une grande responsabilité.",
                            "Mais ne t'en fais pas, tu ne seras pas seul. Des conseillers viendront t'aider en te proposant des projets de construction.",
                            "Je laisse la ville entre tes mains, je te laisse le temps de construire trois bâtiments.",
                            "J'espère que cette ville sera devenue un vrai cité à mon retour. Développe l'activité politique de Tolosa, et alors l'empereur sera content.",
                            "Mais fais aussi attention à le pas négliger ton peuple !"},
            new string[] { "On a reçu des échos jusqu'à Rome ! Tolosa est une vrai petite cité maintenant !",
                            "Je suis fier de toi, maintenant j'en ai le cœur net, je peux valider sans crainte la décision de l'empereur de faire don de remparts à ta ville !" },
            new string[] { "Sérieusement ?! Ta ville ne sera jamais une vraie cité sans forum pour la politique intérieure et sans port pour la politique extérieure !",
                            "Tu comprends bien que je ne peux pas mentir dans mon rapport.. L'empereur voulait t'offrir des remparts pour ta ville, mais après avoir vu ça, je pense qu'il va surtout t'offrir un aller simple pour la légion étrangère." },
            new string[] { "" },
            0);
            emissaries.Add(temp);

            temp = new Emissary(
                new string[] { "Hm...","...","Oui...","...","Euh...","...",
                            "Les remparts sont conformes, je reconnais là le style de l'empereur Tibère.",
                            "...","Toutefois votre ville n'est guère plus romaine que ça. Il va falloir faire mieux.",
                            "Je vous laisse trois bâtiments."},
                new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire rom...",
                            "!!!",
                            "Oh mais vous avez un temple dédié à la triade capitoline ?! Il est si beau, il me rappelle celui de Rome ! Ce que j'ai hâte d'y retourner...",
                            "Croyez moi, je ne manquerai pas de louanger votre cité à l'empereur !" },
                new string[] { "Eh bien, je vois que vous avez essayé...","...",
                            "Mais je crois que vous êtes passé à côté de l'essentiel : où sont vos dieux ?",
                            "Je suis en route pour l'Italie, je ne manquerai pas de dire à l'empereur que je n'ai rien à lui dire." },
                new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire rom...",
                            "!!!",
                            "Oh mais vous avez un temple dédié à la triade capitoline ?! Il est si beau, il me rappelle celui de Rome ! Ce que j'ai hâte d'y retourner.. Croyez moi, je ne manquerai pas de louanger votre cité à l'empereur !" },
                1);
            emissaries.Add(temp);

            temp = new Emissary(
                new string[] { "Alors comme ça le grand Caligula a aidé votre ville à financer son aqueduc ?",
                        "Quel grand magnanime !",
                        "En même temps, neuf kilomètres de conduites d'eau et un pont, ce n'est pas à la portée de toutes les bourses.",
                        "Comme c'est excitant, vous allez pouvoir faire des tonnes de jolies choses avec toute cette eau !",
                        "N'est-ce pas ?","Allez, on se revoit dans trois bâtiments !" },
                new string[] { "Oh comme c'est beau ! Tous ces bâtiments sont si grâcieux …", "... et vous avez même enlevé la vieille odeur de rat crevé." },
                new string[] { "Ca manque un peu d'eau par ici.", "Ne vous inquiétez pas, en prison vous aurez toute l'eau de vos larmes" },
                new string[] { "Oh comme c'est beau ! Tous ces bâtiments sont si grâcieux …", "... et vous avez même enlevé la vieille odeur de rat crevé." },
                2);
            emissaries.Add(temp);

            temp = new Emissary(
                new string[] { "C'est donc à ça que ressemble Tolosa ?",
                        "La longue entreprise de feu Auguste se porte bien à ce que je vois.",
                        "Toulousains, je vous met au défi !",
                        "Je vous laisse trois bâtiments pour me montrer tout le prestige de votre cité !",
                        "Ne me décevez pas."},
                new string[] { "Je suis comblé !",
                        "Je n'en attendais pas moins de vous !",
                        "Les toulousains ont su se montrer dignes de la volonté d'Auguste.",
                        "Votre cité a un grand avenir, et elle sera sûrement encore debout dans deux mille ans !"},
                new string[] { "Vous êtes décevants, Toulousains.",
                        "Vous n'êtes pas dignes de porter la volonté d'auguste."},
                new string[] { "wah tema la cité bimillénaire" },
                3);
            emissaries.Add(temp);
        }
        //-------------------------------------------------------------------------------------------------------------------
        // English
        //-------------------------------------------------------------------------------------------------------------------
        else
        {
            Emissary temp = new Emissary(
            new string[] {"Haha ! It is beautiful work ! You see that, boy ? This is the basis of a great city-state, two great perpendicular roads, and most importanly great doors to show that here is our home !",
                            "You're lucky the emperor Augustus decided to fund the rebuilding of Tolosa and accepted my request tu put you here.",
                            "But don't be mistaken, oversee a city's urbanism is a great responsability.",
                            "But don't worry, you won't be alone. Advisors will help you by suggesting buildings you should add to your town.",
                            "The town is in your hands now, I leave you enough time to build three buildings.",
                            "I hope this town will have become a real city state when I'll be back. Develop Tolosa's political activity, and the emperor will be pleased.",
                            "But also be careful not to neglect your people !"},
            new string[] { "Rumors have made their way up to rome ! Tolosa's a real city state now !",
                            "I'm proud of you, now I'm sure of it, I can without a doubt approve the emperor's decision to give your town fortifications !" },
            new string[] { "Seriously ?! Your town will nerver be a real city-state without a forum for domestic policy and a harbour for foreign policy !",
                            "You undeerstand I cannot lie in my report... The emperor wanted to give your town fortifications,but now that I've seen that, I think he's more likely to send you straight to the foreign legion !" },
            new string[] { "" },
            0);
            emissaries.Add(temp);

            temp = new Emissary(
                new string[] { "Hm...","...","Yes...","...","Euh...","...",
                            "The fortifications are in accordance with the emperor Tiberius' style.",
                            "...","However, your city is hardly roman. You will have to do better than that.",
                            "I leave you enough time to build three buildings"},
                new string[] { "Hm, I see you at least tried. This place feels a little more rom...",
                            "!!!",
                            "Oh you have a temple dedicated to the capitoline triad ?! It is so beatiful, it reminds me of Rome's ! Oh I'm looking forward to go back...",
                            "Believe me, I'll be sure to praise your city before the emperor !" },
                new string[] { "Well, I see you at least tried...","...",
                            "But I think you missed the essential : where are your gods ?",
                            "I'm on my way to Italy, I'll be sure to do so to tell the emperor I have nothing to tell him." },
                new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire rom...",
                            "!!!",
                            "Oh mais vous avez un temple dédié à la triade capitoline ?! Il est si beau, il me rappelle celui de Rome ! Ce que j'ai hâte d'y retourner.. Croyez moi, je ne manquerai pas de louanger votre cité à l'empereur !" },
                1);
            emissaries.Add(temp);

            temp = new Emissary(
                new string[] { "So the great Caligula helped your city to fund its aqueduct ?",
                        "How magnanimous !",
                        "Mind you, a nine kilometers long water pipe and a bridge are clearly not affordable for all.",
                        "How exiting, you are going to build a ton of pretty things thanks to all that water !",
                        "Aren't you ?","Go, see you in three buildings !" },
                new string[] { "Oh how pretty ! All those buildings are so graceful ...", "... and you even got rid of that rotten rat smell." },
                new string[] { "This place lacks some water.", "Don't worry, in prison you'll have all the water of your tears." },
                new string[] { "Oh comme c'est beau ! Tous ces bâtiments sont si grâcieux …", "... et vous avez même enlevé la vieille odeur de rat crevé." },
                2);
            emissaries.Add(temp);

            temp = new Emissary(
                new string[] { "So this is what Tolosa looks like ?",
                        "Augustus' long undertaking is well underway, as I can see.",
                        "Toulousians, I have a challenge for you !",
                        "I allow you two build three more buildings to show me how prestigious you city can be !",
                        "Do not deceive me."},
                new string[] { "I'm am fulfilled !",
                        "I was not expecting less from you !",
                        "Toulousians have proved themselves worthy of Augustus will.",
                        "Your city has a bright future, and will probably still be there in two thousand years !"},
                new string[] { "You are deceiving, Toulousians",
                        "You are not worthy of Augustus' will."},
                new string[] { "wah tema la cité bimillénaire" },
                3);
            emissaries.Add(temp);
        }

        
    }
    
    private void BuildLevels()
    {
        Level temp = new Level(0, new string[,] { { "Forum", "Nécropole" } , { "Port", "Marché" } , { "Buffer", "Buffer" } }, emissaries[0]);
        levels.Add(temp);
        levels.Add(new Level(1, new string[,] { { "Temple", "Thermes Nord" }, { "Domus", "Fontaine Forum" }, { "Buffer", "Buffer" } }, emissaries[1]));
        levels.Add(new Level(2, new string[,] { { "Chateau D'Eau", "Fontaine Monumentale" }, { "Thermes Sud", "Jardins" }, { "Buffer", "Buffer" } }, emissaries[2]));
        levels.Add(new Level(3, new string[,] { { "Remparts+", "Domus+" }, { "Théâtre", "Port+" }, { "Buffer", "Buffer" } }, emissaries[3]));

        levels[0].SetPlace(places["Portes"]);
        levels[1].SetPlace(places["Remparts"]);
        levels[2].SetPlace(places["Aqueduc"]);
        //levels[3].SetPlace(places["Vide"]);

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
        if (UI_Manager.Instance.imperialGauges[currLevel.levelIndex].GetComponent<Scrollbar>().size >= 1 )
        {
            successState = "success";
        }
        else
        {
            successState = "failure";
        }
    }

    public IEnumerator CantActForSeconds(float delay)
    {
        canAct = false;
        yield return new WaitForSeconds(delay);
        canAct = true;
    }

    public void CheckPeopleEnding()
    {
        if (UI_Manager.Instance.peopleGauge.GetComponent<Scrollbar>().size <= 0)
        {
            SceneManager.LoadScene("S_Lost_People");
        }
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
            if (touch.position.x < Screen.width * 1 / 2)
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

                MusicAndData_Manager.Instance.advisors.GetComponent<AudioSource>().Play();

                if (touch.position.y < Screen.height * 1 / 3)
                {
                    UI_Manager.Instance.displayedText.text = midText;
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.leftAdvisor, true));
                    UI_Manager.Instance.StartCoroutine(UI_Manager.Instance.ShadowCoroutine(UI_Manager.Instance.rightAdvisor, true));
                }
                else
                {
                    if ( touch.position.x < Screen.width * 1 / 2 )
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

        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.C) || Input.touchCount == 0) // crtl+f touch comment
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
            /*if (buffer.Contains("Nécropole"))
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

    private IEnumerator DelayStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        currLevel.BeginEmissarySection(currLevel.emissary.index);
    }
    void Start()
    {
        SceneManager.UnloadSceneAsync("S_Menu");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("S_City_New"));

        buildingsTransforms = Buildings_Manager.Instance.buildingsTransform;
        BuildDictionnary();
        BuildEmissaries();
        BuildLevels();
        //currLevel.giftedPlace.building3D.gameObject.SetActive(true);
        //currLevel.BeginEmissarySection(currLevel.emissary.index);

        StartCoroutine(DelayStart(0.5f));
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

            //-----------------------------------------------------------------------------
            if (UI_Manager.Instance.choiceMode)
            {
                MakeAChoice();
            }
            if (UI_Manager.Instance.emissaryMode)
            {
                //Debug.Log("emissary mode");
                EmissaryMode();
            }
        }
    }
}

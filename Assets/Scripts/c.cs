using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public class Place : MonoBehaviour
    {
        public int counter = 0;
        public string[] texts;
        public Sprite advisorSprite;
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

        //public Image[] emissaryImage;

        public Emissary(string[] _it, string[] _st, string[] _ft, string[] _sst)
        {
            introTexts = _it;
            failureTexts = _ft;
            successTexts = _st;
            specialSuccessTexts = _sst;
        }
        /*public Emissary(string[] _texts, Image[] _image)
        {
            //this.texts = _texts;
            emissaryImage = _image;
        }*/
    }

    public class Level
    {
        public int levelIndex = 0;
        public string[,] placeKeys;
        public Emissary emissaire;
        public List<string> built = new List<string>();

        public Level(string[,] placeKeys, Emissary emissaire)
        {
            this.placeKeys = placeKeys;
            this.emissaire = emissaire;
        }

        private void BeginEmissarySection(int emissaryIndex)
        {
            /*SwitchMode(true);
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
            }*/
        }

        private void EndEmissarySection()
        {
            /*currEmissary.speechCounter = 0;
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
            }*/
        }

    }

    public GameObject UI;

    public Dictionary<string, Place> places = new Dictionary<string, Place>();
    public GameObject[] allBuildings3D;
    public List<Emissary> emissaryList = new List<Emissary>();
    public List<Level> levels = new List<Level>();
    public List<string> built = new List<string>();
    public List<string> builtThisLevel = new List<string>();
    public List<string> buffer = new List<string>();
    public List<string> deadKeys = new List<string>();


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
        new string[] { "Oh comme c'est beau ! Tous ces bâtiments sont si grâcieux …", "... Du moment qu'on ne regarde pas avec le nez." },
        new string[] { "Ca manque un peu d'eau par ici.", "Ne vous inquiétez pas, en prison vous aurez toute l'eau de vos larmes" },
        new string[] { "Oh comme c'est beau ! Tous ces bâtiments sont si grâcieux …", "... et vous avez même enlevé la vieille odeur de rat crevé." });
        emissaryList.Add(temp);

        temp = new Emissary(
        new string[] { "Là c'est la tirade de l'empereur" },
        new string[] { "ok tier" },
        new string[] { "nul" },
        new string[] { "wah tema la cité bimillénaire" });
        emissaryList.Add(temp);
    }

    private void BuildLevels()
    {
        levels.Add(new Level(new string[,] { { "Forum", "Nécropole" } }, emissaryList[0]));
        levels.Add(new Level(new string[,] { { "Temple", "Domus" }, { "Domus", "Buffer" } }, emissaryList[1]));
        levels.Add(new Level(new string[,] { { "Égouts", "Thermes1" }, { "Buffer", "Fontaine1" }, { "Thermes2", "Buffer" } }, emissaryList[2]));
        levels.Add(new Level(new string[,] { { "Théâtre", "Fontaine2" }, { "Buffer", "Buffer" }, { "Remparts", "Teinturerie" }, { "Buffer", "Buffer" } }, emissaryList[3]));
    }

    // Start is called before the first frame update
    void Start()
    {
        BuildBuildings3DArray();
        BuildDictionnary();
        BuildEmissaries();
        BuildLevels();
    }

    // Update is called once per frame
    void Update()
    {

    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;
using Object = UnityEngine.Object;

public class GM : MonoBehaviour
{
    public Object UI;

    public List<Emissary> emissaries;
    public List<Level> levels;
  

    // Start is called before the first frame update

    private void BuildEmissaries()
    {
        Emissary temp = gameObject.AddComponent<Emissary>();
        temp.SetTexts(new string[] {"Haha ! C'est du bel ouvrage ! Tu vois petit gars, �a c'est les bases d'une grande ville, de grandes routes perpendiculaires, et surtout de grandes portes pour montrer qu'ici, c'est chez nous !",
                            "Tu as de la chance que l'empereur ait d�cid� de financer la reconstruction de Tolosa et accept� ma requ�te de te placer ici. Mais ne te m�prends pas, superviser l'urbanisme d'une cit� est une grande responsabilit�.",
                            "[Fondu au noir. La construction des portes est achev�e]",
                            "Je laisse la ville entre tes mains, je reviendrai dans 5 ans. J'esp�re que cette ville sera devenue un vrai cit� � mon retour. Fais centraliser l'activit� politique de Tolosa, et alors l'empereur sera content."},
            new string[] { "On a re�u des �chos jusqu'� Rome ! Tolosa est une vrai petite cit� maintenant ! Je suis fier de toi, maintenant j'en ai le c�ur net, je peux valider sans crainte la d�cision de l'empereur de faire don de remparts � ta ville !" },
            new string[] { "S�rieusement ?! Je te laisse 5 ans, et tout ce que fais c'est une pauvre n�cropole ?! Tu comprends bien que je ne peux pas mentir dans mon rapport.. L'empereur voulait t'offrir des remparts pour ta ville, mais apr�s avoir vu �a, je pense qu'il va surtout t'offrir un aller simple pour la l�gion �trang�re." },
            new string[] { "" });
        temp.index = 0;
        emissaries.Add(temp);

        temp = gameObject.AddComponent<Emissary>();
        temp.SetTexts(new string[] { "Hm...","...","Oui...","...","Euh...","...",
                            "Les portes sont conformes, je reconnais l� le style de feu Auguste.",
                            "...","Toutefois votre ville n'est gu�re plus romaine que �a. Il va falloir faire mieux. Je vous laisse un peu moins d'une d�cennie."},
            new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire romain ici. Je suis en route pour l'Italie, mais je peux vous dire que l'Empereur vous sera favorable." },
            new string[] { "Eh bien, une domus ? C'est tout ce vous � proposer ? Je suis en route pour l'Italie, je ne manquerai pas de dire � l'empereur que je n'ai rien � lui dire" },
            new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire rom...",
                            "!!!",
                            "Oh mais vous avez un temple d�di� � la triade capitoline ?! Il est si beau, il me rappelle celui de Rome ! Ce que j'ai h�te d'y retourner.. Croyez moi, je ne manquerai pas de louanger votre cit� � l'empereur !" });
        temp.index = 1;
        emissaries.Add(temp);

        temp = gameObject.AddComponent<Emissary>();
        temp.SetTexts(new string[] { "Alors comme �a le grand Canigula a aid� votre ville � financer son aqueduc ?",
                        "Quel grand magnanime !",
                        "Comme c'est excitant, vous allez pouvoir faire des tonnes de jolies choses avec toute cette eau !",
                        "N'est-ce pas ?" },
        new string[] { "Oh comme c'est beau ! Tous ces b�timents sont si gr�cieux �", "... Du moment qu'on ne regarde pas avec le nez." },
        new string[] { "Ca manque un peu d'eau par ici.", "Ne vous inqui�tez pas, en prison vous aurez toute l'eau de vos larmes" },
        new string[] { "Oh comme c'est beau ! Tous ces b�timents sont si gr�cieux �", "... et vous avez m�me enlev� la vieille odeur de rat crev�." });
        temp.index = 2;
        emissaries.Add(temp);

        temp = gameObject.AddComponent<Emissary>() ;
        temp.SetTexts(new string[] { "L� c'est la tirade de l'empereur" },
        new string[] { "ok tier" },
        new string[] { "nul" },
        new string[] { "wah tema la cit� bimill�naire" });
        temp.index = 3;
        emissaries.Add(temp);
    }
    
    private void BuildLevels()
    {
        Level temp;
        temp = gameObject.AddComponent<Level>();
        temp.SetUp(new string[,] { { "Forum", "N�cropole" } }, emissaries[0]);
        levels.Add(temp);
        temp = gameObject.AddComponent<Level>();
        temp.SetUp(new string[,] { { "Temple", "Domus" }, { "Domus", "Buffer" } }, emissaries[1]);
        levels.Add(temp);
        temp = gameObject.AddComponent<Level>();
        temp.SetUp(new string[,] { { "�gouts", "Thermes1" }, { "Buffer", "Fontaine1" }, { "Thermes2", "Buffer" } }, emissaries[2]);
        levels.Add(temp);
        temp = gameObject.AddComponent<Level>();
        temp.SetUp(new string[,] { { "Th��tre", "Fontaine2" }, { "Buffer", "Buffer" }, { "Remparts", "Teinturerie" }, { "Buffer", "Buffer" } }, emissaries[3]);
        levels.Add(temp);
    }
    void Start()
    {
        BuildEmissaries();
        BuildLevels();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

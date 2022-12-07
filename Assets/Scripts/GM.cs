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
        temp.SetTexts(new string[] {"Haha ! C'est du bel ouvrage ! Tu vois petit gars, ça c'est les bases d'une grande ville, de grandes routes perpendiculaires, et surtout de grandes portes pour montrer qu'ici, c'est chez nous !",
                            "Tu as de la chance que l'empereur ait décidé de financer la reconstruction de Tolosa et accepté ma requête de te placer ici. Mais ne te méprends pas, superviser l'urbanisme d'une cité est une grande responsabilité.",
                            "[Fondu au noir. La construction des portes est achevée]",
                            "Je laisse la ville entre tes mains, je reviendrai dans 5 ans. J'espère que cette ville sera devenue un vrai cité à mon retour. Fais centraliser l'activité politique de Tolosa, et alors l'empereur sera content."},
            new string[] { "On a reçu des échos jusqu'à Rome ! Tolosa est une vrai petite cité maintenant ! Je suis fier de toi, maintenant j'en ai le cœur net, je peux valider sans crainte la décision de l'empereur de faire don de remparts à ta ville !" },
            new string[] { "Sérieusement ?! Je te laisse 5 ans, et tout ce que fais c'est une pauvre nécropole ?! Tu comprends bien que je ne peux pas mentir dans mon rapport.. L'empereur voulait t'offrir des remparts pour ta ville, mais après avoir vu ça, je pense qu'il va surtout t'offrir un aller simple pour la légion étrangère." },
            new string[] { "" });
        temp.index = 0;
        emissaries.Add(temp);

        temp = gameObject.AddComponent<Emissary>();
        temp.SetTexts(new string[] { "Hm...","...","Oui...","...","Euh...","...",
                            "Les portes sont conformes, je reconnais là le style de feu Auguste.",
                            "...","Toutefois votre ville n'est guère plus romaine que ça. Il va falloir faire mieux. Je vous laisse un peu moins d'une décennie."},
            new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire romain ici. Je suis en route pour l'Italie, mais je peux vous dire que l'Empereur vous sera favorable." },
            new string[] { "Eh bien, une domus ? C'est tout ce vous à proposer ? Je suis en route pour l'Italie, je ne manquerai pas de dire à l'empereur que je n'ai rien à lui dire" },
            new string[] { "Hm, je vois que vous avez fait des efforts. On se sent un peu plus en territoire rom...",
                            "!!!",
                            "Oh mais vous avez un temple dédié à la triade capitoline ?! Il est si beau, il me rappelle celui de Rome ! Ce que j'ai hâte d'y retourner.. Croyez moi, je ne manquerai pas de louanger votre cité à l'empereur !" });
        temp.index = 1;
        emissaries.Add(temp);

        temp = gameObject.AddComponent<Emissary>();
        temp.SetTexts(new string[] { "Alors comme ça le grand Canigula a aidé votre ville à financer son aqueduc ?",
                        "Quel grand magnanime !",
                        "Comme c'est excitant, vous allez pouvoir faire des tonnes de jolies choses avec toute cette eau !",
                        "N'est-ce pas ?" },
        new string[] { "Oh comme c'est beau ! Tous ces bâtiments sont si grâcieux …", "... Du moment qu'on ne regarde pas avec le nez." },
        new string[] { "Ca manque un peu d'eau par ici.", "Ne vous inquiétez pas, en prison vous aurez toute l'eau de vos larmes" },
        new string[] { "Oh comme c'est beau ! Tous ces bâtiments sont si grâcieux …", "... et vous avez même enlevé la vieille odeur de rat crevé." });
        temp.index = 2;
        emissaries.Add(temp);

        temp = gameObject.AddComponent<Emissary>() ;
        temp.SetTexts(new string[] { "Là c'est la tirade de l'empereur" },
        new string[] { "ok tier" },
        new string[] { "nul" },
        new string[] { "wah tema la cité bimillénaire" });
        temp.index = 3;
        emissaries.Add(temp);
    }
    
    private void BuildLevels()
    {
        Level temp;
        temp = gameObject.AddComponent<Level>();
        temp.SetUp(new string[,] { { "Forum", "Nécropole" } }, emissaries[0]);
        levels.Add(temp);
        temp = gameObject.AddComponent<Level>();
        temp.SetUp(new string[,] { { "Temple", "Domus" }, { "Domus", "Buffer" } }, emissaries[1]);
        levels.Add(temp);
        temp = gameObject.AddComponent<Level>();
        temp.SetUp(new string[,] { { "Égouts", "Thermes1" }, { "Buffer", "Fontaine1" }, { "Thermes2", "Buffer" } }, emissaries[2]);
        levels.Add(temp);
        temp = gameObject.AddComponent<Level>();
        temp.SetUp(new string[,] { { "Théâtre", "Fontaine2" }, { "Buffer", "Buffer" }, { "Remparts", "Teinturerie" }, { "Buffer", "Buffer" } }, emissaries[3]);
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

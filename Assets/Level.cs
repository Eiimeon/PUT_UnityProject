using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update

    public int levelIndex = 0;
    public string[,] placeKeys;
    public Emissary emissaire;
    public List<string> built = new List<string>();

    public void SetUp(string[,] placeKeys, Emissary emissaire)
    {
        this.placeKeys = placeKeys;
        this.emissaire = emissaire;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

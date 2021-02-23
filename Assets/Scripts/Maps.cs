using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour
{
    public List<Map> listMaps;
    public int levelUnlocked;
    public int totalStars;
    public int currentStage;

    public static Maps Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdateData()
    {
        DataSystem.SavePlayer(Instance);
    }
}

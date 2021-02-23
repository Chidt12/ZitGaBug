using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject stageRowPrefab;

    [SerializeField]
    GameObject stagesContent;

    [SerializeField]
    TextMeshProUGUI textStars;

    [SerializeField]
    GameObject loader;

    [SerializeField]
    AudioSource musicSource;
    [SerializeField]
    AudioSource effectSource;

    [SerializeField]
    AudioClip clickClip;

    [SerializeField]
    Scrollbar scrollbar;
    public float lastValue = 0;

    List<GameObject> rows;
    int startPoolIndx;
    int endPoolIndx;

    public static GameManager Instance { get; set; }

    private void OnEnable()
    {
        //Subscribe to the Scrollbar event
        scrollbar.onValueChanged.AddListener(scrollbarCallBack);
        lastValue = scrollbar.value;
    }

    //Will be called when Scrollbar changes
    void scrollbarCallBack(float value)
    {
        if (lastValue > value)
        {
            // GO DOWN
            if(value < 0.1)
            {
                int count = startPoolIndx > 1 ? 2 : startPoolIndx;
                
                if(count > 0)
                {
                    for (int i = 1; i <= count; i++)
                    {
                        rows[startPoolIndx - i].SetActive(true);
                        rows[endPoolIndx+1 - i].SetActive(false);
                    }
                    startPoolIndx = startPoolIndx - count;
                    endPoolIndx = endPoolIndx - count;
                }
            }
        }
        else
        {   // GO UP
            if(value > 0.9)
            {
                int count = endPoolIndx < 248  ? 2 : 249 - endPoolIndx;

                if (count > 0)
                {
                    for (int i = 1; i <= count; i++)
                    {
                        rows[startPoolIndx - 1 + i].SetActive(false);
                        rows[endPoolIndx  + i].SetActive(true);
                    }
                    startPoolIndx = startPoolIndx + count;
                    endPoolIndx = endPoolIndx + count;
                }
            }
        }
        lastValue = value;

        Debug.Log("START " + startPoolIndx);
        Debug.Log("END" + endPoolIndx);
    }

    void OnDisable()
    {
        //Un-Subscribe To Scrollbar Event
        scrollbar.onValueChanged.RemoveListener(scrollbarCallBack);
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        LoadData();


        rows = new List<GameObject>();

        for (int i = 0; i <= 999; i += 4)
        {
            var row = Instantiate(stageRowPrefab, stagesContent.transform);
            row.SetActive(false);
            rows.Add(row);
        }

        int indxRowsTarget = Maps.Instance.levelUnlocked / 4;

        if(indxRowsTarget < 8)
        {
            for (int i = 0; i < 17; i++)
            {
                rows[i].SetActive(true);
            }
            startPoolIndx = 0;
            endPoolIndx = 16;
        }
        else if(indxRowsTarget > 250 - 8)
        {
            for (int i = 250 - 17; i < 250; i++)
            {
                rows[i].SetActive(true);
            }

            startPoolIndx = 233;
            endPoolIndx = 249;
        }
        else
        {
            int start = indxRowsTarget - 8;
            int end = indxRowsTarget + 8;
            for (int i = start; i <= end; i++)
            {
                rows[i].SetActive(true);
            }

            startPoolIndx = start;
            endPoolIndx = end;
        }

        UIController.Instance.SnapTo(rows[indxRowsTarget].GetComponent<RectTransform>());
    }



    void LoadData()
    {
        MapsData mapData = DataSystem.LoadPlayer();
        if(mapData == null)
        {

            Maps.Instance.levelUnlocked = Random.Range(1, 1000);
            int countStarsTotal = 0;
            for (int i = 0; i < 1000; i++)
            {
                if(i < Maps.Instance.levelUnlocked)
                {
                    var newMap = Map.Create(10, 13, i, true);
                    Maps.Instance.listMaps.Add(newMap);
                    countStarsTotal += newMap.Stars;
                }
                else if(i == Maps.Instance.levelUnlocked)
                {
                    var newMap = Map.Create(10, 13, i, true);
                    newMap.Stars = 0;
                    Maps.Instance.listMaps.Add(newMap);
                }
                else
                {
                    var newMap = Map.Create(10, 13, i, false);
                    Maps.Instance.listMaps.Add(newMap);
                    countStarsTotal += newMap.Stars;
                }
            }

            Maps.Instance.totalStars = countStarsTotal;
            Maps.UpdateData();
        }
        else
        {
            Maps.Instance.levelUnlocked = mapData.LevelUnlocked;
            Maps.Instance.totalStars = mapData.TotalStars;
            Maps.Instance.listMaps = mapData.Maps;
        }

        textStars.text = Maps.Instance.totalStars.ToString();
    
    }

    public void ClickReset()
    {
        effectSource.PlayOneShot(clickClip);
        var listMaps = Maps.Instance.listMaps.Where(x => x.UnLocked);
        foreach (var item in listMaps)
        {
            item.Stars = 0;
            item.UnLocked = false;
        }

        Maps.Instance.listMaps[0].UnLocked = true;
        Maps.Instance.totalStars = 0;
        Maps.Instance.levelUnlocked = 1;
        Maps.UpdateData();

        textStars.text = Maps.Instance.totalStars.ToString();
        var stagesSlot = FindObjectsOfType<StageSlot>();
        foreach (var item in stagesSlot)
        {
            item.ReRender();
        }
    }


    public void playClick()
    {
        effectSource.PlayOneShot(clickClip);
    }

    public void loadEnd()
    {
        loader.GetComponent<Animator>().SetTrigger("End");
    }
}

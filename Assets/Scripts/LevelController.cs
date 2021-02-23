using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    GameObject darkPanel;
    [SerializeField]
    RectTransform endPanel;
    [SerializeField]
    GameObject Maze;

    [SerializeField]
    TextMeshProUGUI levelNum;

    [SerializeField]
    GameObject status0Star;
    [SerializeField]
    GameObject status1Star;
    [SerializeField]
    GameObject status2Star;
    [SerializeField]
    GameObject status3Star;

    [SerializeField]
    GameObject Player;
    [SerializeField]
    GameObject Gate;

    [SerializeField]
    AudioSource musicSource;
    [SerializeField]
    AudioSource effectSource;
    [SerializeField]
    AudioClip winSound;
    [SerializeField]
    AudioClip clickSound;

    [SerializeField]
    GameObject loader;

    

    [SerializeField]
    GameObject lineRendererPrefab;
    public List<GameObject> lines;

    public Map mapHold;

    public List<CellSlot> Cells;

    public static LevelController Instance { get; set; }

    public bool DrawMethod;
    public bool AuToRun;
    public bool startedGame;
    public bool playing;

    private void Awake()
    {
        playing = false;
        startedGame = false;
        mapHold = Maps.Instance.listMaps[Maps.Instance.currentStage - 1];

        levelNum.text = "No. " + Maps.Instance.currentStage.ToString();
        if(mapHold.Stars == 1)
        {
            status1Star.SetActive(true);
        }
        else if (mapHold.Stars == 2)
        {
            status2Star.SetActive(true);
        }
        else if (mapHold.Stars == 3)
        {
            status3Star.SetActive(true);
        }

        Instance = this;
        Cells = new List<CellSlot>();
        var rows = new List<GameObject>();

        rows = GameObject.FindGameObjectsWithTag("Row").OrderBy(x => x.transform.GetSiblingIndex()).ToList();
        foreach (var row in rows)
        {
            Cells.AddRange
                (row.GetComponentsInChildren<CellSlot>()
                .OrderBy(x => x.transform.GetSiblingIndex())
                .ToList());
        }

        for (int i = 0; i < mapHold.Method.Count - 1; i++)
        {
            var line = Instantiate(lineRendererPrefab, transform);
            var lineRenderer = line.GetComponent<LineRenderer>();
            lines.Add(line);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, Cells[mapHold.Method[i]].transform.position);
            lineRenderer.SetPosition(1, Cells[mapHold.Method[i + 1]].transform.position);
            line.SetActive(false);
        }

        DrawMap(mapHold);
    }

    void DrawMap(Map map)
    {
        for (int i = 0; i < map.Cells.Count; i++)
        {
            Cells[i].DrawCell(map.Cells[i][0], map.Cells[i][1], map.Cells[i][2], map.Cells[i][3]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startedGame = true;
        Instantiate(Player, Cells[0].transform.position, Quaternion.identity);
        Instantiate(Gate, Cells[mapHold.Method.LastOrDefault()].transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void autoButton()
    {
        effectSource.PlayOneShot(clickSound);
        if (startedGame && !playing)
        {
            playing = true;
        }
    }

    public void guideButton()
    {
        effectSource.PlayOneShot(clickSound);
        if (startedGame)
        {
            foreach (var item in mapHold.Method)
            {
                Cells[item].toggleGuide(true);
            }

            foreach (var item in lines)
            {
                item.SetActive(true);
            }
        }
    }

    public void nextButton()
    {
        effectSource.PlayOneShot(clickSound);
        loader.GetComponent<Animator>().SetTrigger("End");

        StartCoroutine(NextStageCoroutine());
    }

    IEnumerator NextStageCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        if (!Maps.Instance.listMaps[Maps.Instance.currentStage].HadMap)
        {
            Maps.Instance.listMaps[Maps.Instance.currentStage].CreateMap();
        }

        Maps.Instance.currentStage += 1;
        Maps.UpdateData();
        SceneManager.LoadScene("Level1");
    }

    public void retryButton()
    {
        effectSource.PlayOneShot(clickSound);
        ClearObjects();

        StartGame();

        darkPanel.SetActive(false);
        LeanTween.move(endPanel, new Vector2(0, 2000), 0);
        LeanTween.scale(endPanel, new Vector2(0.4f, 0.4f), 0.3f);
    }

    void StartGame()
    {
        startedGame = true;
        Instantiate(Player, Cells[0].transform.position, Quaternion.identity);
    }

    void ClearObjects()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        foreach (var item in lines)
        {
            item.SetActive(false);
        }

        foreach (var item in mapHold.Method)
        {
            Cells[item].toggleGuide(false);
        }
    }

    public void backHome()
    {
        effectSource.PlayOneShot(clickSound);
        loader.GetComponent<Animator>().SetTrigger("End");

        StartCoroutine(BackHomeCoroutine());

    }

    IEnumerator BackHomeCoroutine()
    {
        yield return new WaitForSeconds(0.6f); 
        SceneManager.LoadScene("MainMenu");
    }

    public void endGame()
    {
        effectSource.PlayOneShot(winSound);
        startedGame = false;
        playing = false;
        darkPanel.SetActive(true);
        LeanTween.move(endPanel, new Vector2(0, 0), 0);
        LeanTween.scale(endPanel, new Vector2(1, 1), 0.3f);

        bool hasChange = false;
        if (!Maps.Instance.listMaps[Maps.Instance.currentStage].UnLocked)
        {
            hasChange = true;
            Maps.Instance.listMaps[Maps.Instance.currentStage].UnLocked = true;
        }


        if (Maps.Instance.listMaps[Maps.Instance.currentStage - 1].Stars == 0)
        {
            hasChange = true;
            int stars = Random.Range(1, 4);
            Maps.Instance.listMaps[Maps.Instance.currentStage - 1].Stars = stars;
            Maps.Instance.totalStars += stars;
        }

        if (hasChange)
        {
            Maps.UpdateData();
        }
    }

}

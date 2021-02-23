using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSlot : MonoBehaviour
{
    public int Index;

    [SerializeField]
    GameObject connectTop;
    [SerializeField]
    GameObject Net;
    [SerializeField]
    GameObject[] stars;

    [SerializeField]
    GameObject tutorialText;

    [SerializeField]
    TextMeshProUGUI textLevel;

    public Transform parent;

    private void Awake()
    {
        parent = transform.parent;
    }

    // Start is called before the first frame update
    void Start()
    {

        if ((parent.GetSiblingIndex() + 1) % 2 == 1)
        {
            Index = parent.GetSiblingIndex() * 4 + (transform.GetSiblingIndex());
        }
        else
        {
            Index = parent.GetSiblingIndex() * 4 + 5 - transform.GetSiblingIndex();
        }



        if (Index % 4 == 0 && Index != 1000)
        {
            connectTop.SetActive(true);
        }
        else
        {
            connectTop.SetActive(false);
        }

        if (Maps.Instance.listMaps[Index - 1].Stars == 3)
        {

        }
        else if (Maps.Instance.listMaps[Index - 1].Stars == 2)
        {
            stars[2].SetActive(false);
        }
        else if (Maps.Instance.listMaps[Index - 1].Stars == 1)
        {
            stars[2].SetActive(false);
            stars[1].SetActive(false);
        }
        else if (Maps.Instance.listMaps[Index - 1].Stars == 0)
        {
            stars[2].SetActive(false);
            stars[1].SetActive(false);
            stars[0].SetActive(false);
        }

        if (Index == 1)
        {
            tutorialText.SetActive(true);
            textLevel.text = "";
        }
        else
        {
            textLevel.text = Index.ToString();
        }

        if (!Maps.Instance.listMaps[Index - 1].UnLocked)
        {
            Net.SetActive(true);
        }
    }

    public void ReRender()
    {
        if (Maps.Instance.listMaps[Index - 1].Stars == 3)
        {

        }
        else if (Maps.Instance.listMaps[Index - 1].Stars == 2)
        {
            stars[2].SetActive(false);
        }
        else if (Maps.Instance.listMaps[Index - 1].Stars == 1)
        {
            stars[2].SetActive(false);
            stars[1].SetActive(false);
        }
        else if (Maps.Instance.listMaps[Index - 1].Stars == 0)
        {
            stars[2].SetActive(false);
            stars[1].SetActive(false);
            stars[0].SetActive(false);
        }

        if (Index == 1)
        {
            tutorialText.SetActive(true);
            textLevel.text = "";
        }
        else
        {
            textLevel.text = Index.ToString();
        }

        if (!Maps.Instance.listMaps[Index - 1].UnLocked)
        {
            Net.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        GameManager.Instance.playClick();
        if (Maps.Instance.listMaps[Index - 1].UnLocked)
        {
            GameManager.Instance.loadEnd();
            StartCoroutine(GoPlayCoroutine());
        }
    }


    IEnumerator GoPlayCoroutine()
    {
        yield return new WaitForSeconds(0.6f);
       
        if (!Maps.Instance.listMaps[Index - 1].HadMap)
        {
            Maps.Instance.listMaps[Index - 1].CreateMap();

        }
        Maps.Instance.currentStage = Index;
        Maps.UpdateData();
        SceneManager.LoadScene("Level1");

    }
    
}

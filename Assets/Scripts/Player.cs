using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public List<GameObject> roadMap;
    public int target;

    [SerializeField]
    float speed;

    // 0 down 1 right 2 up 3 left
    int direction;

    public static Player Instance { get; set; }

    private void Awake()
    {
        Instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        target = 0;
        direction = 0;
        roadMap = new List<GameObject>();
        foreach (var item in LevelController.Instance.mapHold.Method)
        {
            roadMap.Add(LevelController.Instance.Cells[item].gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelController.Instance.startedGame && LevelController.Instance.playing
                && target < roadMap.Count
            )
        {
            var targetPos = roadMap[target].transform.position;
            transform.position = Vector2.MoveTowards(transform.position
                , targetPos, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) <= Mathf.Epsilon)
            {
                target = target + 1;
                if(target == roadMap.Count)
                {
                    LevelController.Instance.endGame();
                }
            }

            

            if(Mathf.Abs(transform.position.x - targetPos.x) > Mathf.Abs(transform.position.y - targetPos.y))
            {
                if(transform.position.x > targetPos.x)
                {
                    if(direction != 3)
                    {
                        goDirect(3);
                    }
                }
                else
                {
                    if(direction != 1)
                    {
                        goDirect(1);
                    }
                }
            }
            else
            {
                if (transform.position.y > targetPos.y)
                {
                    if (direction != 0)
                    {
                        goDirect(0);
                    }
                }
                else
                {
                    if (direction != 2)
                    {
                        goDirect(2);
                    }
                }
            }
        }
    }

    private void goDirect(int direction)
    {
        if(direction == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if(direction == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if(direction == 2)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(direction == 3)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        this.direction = direction;
    } 
}

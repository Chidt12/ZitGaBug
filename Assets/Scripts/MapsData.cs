using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapsData 
{
    public List<Map> Maps { get; set; }

    public int TotalStars { get; set; }
    public int LevelUnlocked { get; set; }

    public MapsData(Maps data)
    {
        Maps = data.listMaps;
        TotalStars = data.totalStars;
        LevelUnlocked = data.levelUnlocked;
    }
}

[System.Serializable]
public class Map
{
    public int Id { get; set; }
    public bool HadMap { get; set; }
    public int Stars { get; set; }
    public bool UnLocked { get; set; }

    public List<bool[]> Cells { get; set; }
    public List<int> Method { get; set; }



    public static Map Create(int cols, int rows, int id, bool unlocked)
    {
        var map = new Map();
        map.UnLocked = unlocked;
        map.Id = id;
        map.Stars = unlocked ? Random.Range(1, 4) : 0;
        map.HadMap = false; 
        map.Method = new List<int>();

        int cellNum = cols * rows;


        if (cellNum > 0)
        {
            map.Cells = new List<bool[]>();
            for (int i = 0; i < cellNum; i++)
            {
                map.Cells.Add(new bool[] { false, false, false, false });
            }
        }

        return map;
    }


    // FOR CREATING MAP
    public void CreateMap()
    {
        HadMap = true;
        Stack<int> stack = new Stack<int>();
        List<bool> visited = new List<bool>(new bool[130]);

        int currentIdx = 0;
        visited[currentIdx] = true;
        
        Method.Add(currentIdx);
        stack.Push(currentIdx);
        bool hasMethod = false;

        while(currentIdx != -1)
        {
            int nextIdx = index(currentIdx, visited);

            if(nextIdx == -1)
            {
                hasMethod = true;
                if(stack.Count > 0)
                {
                    int top = stack.Pop();
                    currentIdx = top;
                }
                else
                {
                    currentIdx = -1;
                }

            }
            else
            {
                // Remove Wall

                // RIGHT
                if(nextIdx == currentIdx + 1)
                {
                    Cells[currentIdx][3] = true;
                    Cells[nextIdx][1] = true;
                }
                // LEFT
                else if(nextIdx == currentIdx - 1)
                {
                    Cells[currentIdx][1] = true;
                    Cells[nextIdx][3] = true;
                }
                // TOP
                else if(nextIdx == currentIdx - 10)
                {
                    Cells[currentIdx][0] = true;
                    Cells[nextIdx][2] = true;
                }
                // BOTTOM
                else if(nextIdx == currentIdx + 10)
                {
                    Cells[currentIdx][2] = true;
                    Cells[nextIdx][0] = true;
                }
                // End Remove Wall

                visited[nextIdx] = true;
                currentIdx = nextIdx;
                stack.Push(currentIdx);
                if (!hasMethod)
                {
                   Method.Add(currentIdx);
                }
            }

            
        }
    }

    int index(int currentIdx, List<bool> visited)
    {
        List<int> neighbors = new List<int>();
        if(currentIdx > 9 && !visited[currentIdx - 10])
        {
            // Top
            neighbors.Add(currentIdx - 10);
        }

        if((currentIdx + 1) % 10 != 0 && !visited[currentIdx + 1])
        {
            // Right
            neighbors.Add(currentIdx + 1);
        }

        if (currentIdx < 120 && !visited[currentIdx + 10])
        {
            // Bottom
            neighbors.Add(currentIdx + 10);
        }

        if (currentIdx % 10 != 0 && !visited[currentIdx - 1])
        {
            // Left
            neighbors.Add(currentIdx - 1);
        }

        if(neighbors.Count > 0)
        {
            int IdxRand = Random.Range(0, neighbors.Count);
            return neighbors[IdxRand];
        }
        else
        {
            return -1;
        }

    }
}


using UnityEngine;
using System.Collections;

public partial class CGameManager
{
    public GameObject   box;
    public GameObject   cask;
    public GameObject   screw;
    public Texture2D[]  m_level = new Texture2D[2];
    public Texture2D    m_flag;

    public static int map_width = 18;
    public static int map_height = 10;

    private float start_x = -8.5f;
    private float start_y = 4.5f;

    /*private static int[ , ] pirate_map = 
                        {
                            {0, 0, (1 | 0x00020000), 0, 1, 0, 1, 1, 1, 1, 0, 65538, 0, (1 | 0x00030000), 0, 0},
                            {0, (3 | 0x00010000), 1, (1 | 0x00020000), 1, 1, 0, 0, 0, 0, (1 | 0x00050000), 1, 1, 1, (3 | 0x00040000), 0},
                            {1, 1, (1 | 0x00020000), 1, 1, (1 | 0x00020000), 2, 2, 2, 2, (1 | 0x00010000), 1, 1, (1 | 0x00010000), (1 | 0x00020000), 1},
                            {1, 1, 1, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 1, 1, 1},
                            {0, 0, (1 | 0x00010000), 1, 0, 2, 0, 0, 0, 0, (2 | 0x00010000), 0, 1, (1 | 0x00020000), 0, 0},
                            {0, 0, 1, (1 | 0x00020000), 0, 2, 0, 9, 9, 0, 2, 0, 1, 1, 0, 0},
                            {1, (1 | 0x00020000), 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 0, 1, 65537},
                            {1, (1 | 0x00010000), 1, 1, (1 | 0x00020000), 1, 0, (2 | 0x00010000), 2, 0, 1, 1, (1 | 0x00020000), 1, 1, 1},
                            {0, (3 | 0x00010000), (1 | 0x00020000), 1, 1, 1, 1, (1 | 0x00010000), 1, 1, (1 | 0x00020000), 1, 1, 1, 3, 0},
                            {0, 0, 1, 0, (1 | 0x00020000), 0, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0},
                        };
    */
    private static int[,] pirate_map =
                       {
                            {0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 0},
                            {0, 3, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 0},
                            {1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 1, 1, 1, 1},
                            {0, 0, 1, 1, 1, 0, 2, 0, 0, 0, 0, 2, 0, 1, 1, 1, 0, 0},
                            {0, 0, 1, 1, 1, 0, 2, 0, 9, 9, 0, 2, 0, 1, 1, 1, 0, 0},
                            {1, 1, 1, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 0, 1, 0, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1},
                            {0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 0},
                            {0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 0},
                        };
    
    bool CreateMap(int level_num)
    {
        if (level_num < MAX_LEVEL_NUM)
        {
            /* create map */
            GameObject bkgObj = new GameObject();
            bkgObj.name = "Background";
            bkgObj.AddComponent<SpriteRenderer>();
            Sprite s = Sprite.Create(m_level[level_num], new Rect(0, 0, m_level[level_num].width,
                m_level[level_num].height), new Vector3(0.5f, 0.5f, 0));
            bkgObj.GetComponent<SpriteRenderer>().sprite = s;
            if (level_num == 0)
            {
                /* create special things */
                GameObject flagObj = new GameObject();
                flagObj.name = "Flag";
                flagObj.AddComponent<SpriteRenderer>();
                Sprite ss = Sprite.Create(m_flag, new Rect(0, 0, m_flag.width,
                    m_flag.height), new Vector3(0.5f, 0.5f, 0));
                flagObj.transform.position = new Vector3(0.0f, 0.8f, -1.0f);
                flagObj.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
                flagObj.GetComponent<SpriteRenderer>().sortingOrder = 14;
                flagObj.GetComponent<SpriteRenderer>().sprite = ss;
                flagObj.AddComponent<BoxCollider2D>();
                BoxCollider2D col = flagObj.GetComponent<BoxCollider2D>();
                col.offset = new Vector2(0.007018559f, -1.696823f);
                col.size = new Vector2(2.2064f, 1.126348f);
                GenerateObstacle(pirate_map);
            }
            return true;
        }
        return false;
    }

    void GenerateObstacle(int[ , ] map)
    {
        /* map array */
        var sr_box = box.GetComponent<SpriteRenderer>();
        var sr_cask = cask.GetComponent<SpriteRenderer>();
        var sr_screw = screw.GetComponent<SpriteRenderer>();
        for (int i = 0; i < map_height; i++)
        {
            for (int j = 0; j < map_width; j++)
            {

                if (map[i, j] != 0)
                {
                    sr_box.sortingOrder = i*2 + 8;
                    sr_cask.sortingOrder = i*2 + 8;
                    sr_screw.sortingOrder = i*2 + 8;

                    int num = map[i, j] & 0x0000ffff;
                    GameObject obj;
                    switch (num)
                    {
                        case 1:
                            obj = Instantiate(box, new Vector3(start_x + 1.0f * j, 4.5f - 1.0f * i, 0), gameObject.transform.rotation) as GameObject;
                            obj.GetComponent<CObstacle>().SetWoodenType(map[i, j]);
                            break;
                        case 2:
                            obj = Instantiate(cask, new Vector3(start_x + 1.0f * j, 4.5f - 1.0f * i, 0), gameObject.transform.rotation) as GameObject;
                            obj.GetComponent<CObstacle>().SetWoodenType(map[i, j]);
                            break;
                        case 3:
                            obj = Instantiate(screw, new Vector3(start_x + 1.0f * j, 4.5f - 1.0f * i, 0), gameObject.transform.rotation) as GameObject;
                            obj.GetComponent<CObstacle>().SetWoodenType(map[i, j]);
                            break;
                    }
                    
                }
            }
        }
    }
}

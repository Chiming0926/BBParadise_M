using UnityEngine;
using System.Collections;

public partial class CGameManager
{
    public GameObject   box;
    public GameObject   cask;
    public GameObject   screw;
    public GameObject   pirate_box;
    public GameObject   block;
    public GameObject   snowbox1;
    public GameObject   crystal;
    public GameObject   snow_stone;
    public Texture2D[]  m_level = new Texture2D[2];
    public Texture2D    m_flag;

    public static int map_width = 18;
    public static int map_height = 10;

    private float start_x = 0;
    private float start_y = 10f;


	enum GAME_RESOURCE
    {
        WBALL   = 1,
        SYRUP   = 2,
        NULL    = 4
    };

    enum GAME_LEVEL
    {
        GAME_PIRATE = 0,
        GAME_OO     = 1,
        GAME_SNOW1  = 2,
    };

    /*
    private static int[ , ] pirate_map = 
                        {
                            {0, 0, 1, 0, (1 | 0x00020000), 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1| 0x00030000, 0, 0},
                            {0, (3 | 0x00010000), 1, 1, (1 | 0x00020000), 1, 1, 0, 0, 0, 0, (1 | 0x00050000), 1, 1, 1, 1, (3 | 0x00040000), 0},
                            {1, 1, 1, (1 | 0x00020000), 1, 1, (1 | 0x00020000), 2, 2, 2, 2, (1 | 0x00010000), 1, 1, 1, (1 | 0x00010000), (1 | 0x00020000), 1},
                            {1, 1, 1, 1, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 1, 1, 1, 1},
                            {0, 0, 1, (1 | 0x00010000), 1, 0, 2, 0, 0, 0, 0, (2 | 0x00010000), 0, 1, 1, (1 | 0x00020000), 0, 0},
                            {0, 0, 1, 1, (1 | 0x00020000), 0, 2, 0, 9, 9, 0, 2, 0, 1, 1, 1, 0, 0},
                            {1, (1 | 0x00020000), 1, 0, 0, 1, 0, 2, 0, 0, 2, 0, 1, 0, 1, 0, 1, 65537},
                            {1, (1 | 0x00010000), 1, 1, 1, (1 | 0x00020000), 1, 0, (2 | 0x00010000), 2, 0, 1, 1, 1, (1 | 0x00020000), 1, 1, 1},
                            {0, (3 | 0x00010000), 1, (1 | 0x00020000), 1, 1, 1, 1, (1 | 0x00010000), 1, 1, (1 | 0x00020000), 1, 1, 1,  1, 3, 0},
                            {0, 0, 1, 1, 0, (1 | 0x00020000), 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 0},
                        }; */

    private static int[,] pirate_map =
                        {
                            {0, 0, 1, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 0},
                            {0, 3, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 0},
                            {1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 1, 1, 1, 1},
                            {0, 0, 1, 1, 1, 0, 2, 0, 0, 0, 0, 2, 0, 1, 1, 1, 0, 0},
                            {0, 0, 1, 1, 1, 0, 2, 0, 9, 9, 0, 2, 0, 1, 1, 1, 0, 0},
                            {1, 1, 1, 0, 0, 1, 0, 2, 0, 0, 2, 0, 1, 0, 1, 0, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 0, 1, 1, 1, 1, 1, 1, 1},
                            {0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 0},
                            {0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 0, 0},
                        };

    private static int[,] resource_map =
                        {
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                        };

    private static int[,] snow_map1 =
                        {
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0},
                            {0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 2, 0, 0, 0, 0, 1, 0},
                            {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0},
                            {0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 3, 0, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 2, 2, 2, 2, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0},
                            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 1, 0},
                            {0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0},
                            {0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
                        };
    /*
       private static int[,] pirate_map =
                           {
                               {0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               {0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                               {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                           };
                           /*
    private static int[,] pirate_map =
                       {
                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                        };*/
    /*
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
    */
    void CreateLevel2()
    {
        start_y = 9.8f;
        GameObject bkgObj = new GameObject();
        bkgObj.name = "Background";
        bkgObj.AddComponent<SpriteRenderer>();
        Texture2D textBkg = Resources.Load("Textures/Game/Background/snow") as Texture2D;

        Sprite s = Sprite.Create(textBkg, new Rect(0, 0, textBkg.width, textBkg.height), new Vector3(0.5f, 0.5f, 0));
        bkgObj.GetComponent<SpriteRenderer>().sprite = s;
        bkgObj.transform.position = new Vector3(8.5f, 5.5f, 1);
        bkgObj.transform.localScale = new Vector3(1.1f, 1.1f, 1.0f);

        /* create tree */
        GameObject treeObj = new GameObject(); 
        Texture2D  snow_tree = Resources.Load("Textures/Game/Obstacle/snow_tree") as Texture2D;

        treeObj.name = "Tree";
        treeObj.AddComponent<SpriteRenderer>();
        Sprite ss = Sprite.Create(snow_tree, new Rect(0, 0, snow_tree.width,
            snow_tree.height), new Vector3(0.5f, 0.5f, 0));
        treeObj.transform.position = new Vector3(5.0f, 6.0f, -1.0f);
        treeObj.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
        treeObj.GetComponent<SpriteRenderer>().sortingOrder = 14;
        treeObj.GetComponent<SpriteRenderer>().sprite = ss;
        treeObj.AddComponent<BoxCollider2D>();
        BoxCollider2D col = treeObj.GetComponent<BoxCollider2D>();
        col.offset = new Vector2(0.007018447f, -1.857827f);
        col.size = new Vector2(2.2064f, 0.9921765f);
        treeObj.name = "obstacle" + 5 + "/" + 6;

        GenerateObstacle(GAME_LEVEL.GAME_SNOW1, snow_map1);
    }

    void CreateLevel0()
    {
        /* create map */
        GameObject bkgObj = new GameObject();
        bkgObj.name = "Background";
        bkgObj.AddComponent<SpriteRenderer>();
        Sprite s = Sprite.Create(m_level[0], new Rect(0, 0, m_level[0].width,
            m_level[0].height), new Vector3(0.5f, 0.5f, 0));
        bkgObj.GetComponent<SpriteRenderer>().sprite = s;
        bkgObj.transform.position = new Vector3(8.5f, 5.5f, 1);

        /* create special things */
        GameObject flagObj = new GameObject();
        flagObj.name = "Flag";
        flagObj.AddComponent<SpriteRenderer>();
        Sprite ss = Sprite.Create(m_flag, new Rect(0, 0, m_flag.width,
            m_flag.height), new Vector3(0.5f, 0.5f, 0));
        flagObj.transform.position = new Vector3(8.5f, 6.2f, -1.0f);
        flagObj.transform.localScale = new Vector3(0.8f, 0.8f, 1.0f);
        flagObj.GetComponent<SpriteRenderer>().sortingOrder = 14;
        flagObj.GetComponent<SpriteRenderer>().sprite = ss;
        flagObj.AddComponent<BoxCollider2D>();
        BoxCollider2D col = flagObj.GetComponent<BoxCollider2D>();
        col.offset = new Vector2(0.007018559f, -1.696823f);
        col.size = new Vector2(2.2064f, 1.126348f);
        GenerateObstacle(0, pirate_map);

        /* create the wall */
        GameObject wallObj = new GameObject();
        for (int i = -1; i < map_width + 1; i++)
        {
            var sr_block = screw.GetComponent<SpriteRenderer>();
            sr_block.sortingOrder = 0;
            GameObject obj;
            obj = Instantiate(screw, new Vector3(i, 10.83f, 0), gameObject.transform.rotation) as GameObject;
            sr_block.sortingOrder = 21;
            Destroy(obj.GetComponent<BoxCollider2D>());
            obj = Instantiate(screw, new Vector3(i, -0.32f, 0), gameObject.transform.rotation) as GameObject;
            Destroy(obj.GetComponent<BoxCollider2D>());
        }
        for (int i = 1; i <= map_height; i++)
        {
            GameObject obj;
            var sr_block = screw.GetComponent<SpriteRenderer>();
            sr_block.sortingOrder = i + 1 + 10;
            obj = Instantiate(screw, new Vector3(-1, 10.75f - i, 0), gameObject.transform.rotation) as GameObject;
            Destroy(obj.GetComponent<BoxCollider2D>());
            obj = Instantiate(screw, new Vector3(18, 10.75f - i, 0), gameObject.transform.rotation) as GameObject;
            Destroy(obj.GetComponent<BoxCollider2D>());
        }
    }

    bool CreateMap(int level_num)
    {
        if (level_num < MAX_LEVEL_NUM)
        {
            if (level_num == 0)
            {
                CreateLevel0();
            }
            else if (level_num == 1)
            {

            }
            else if (level_num == 2)
            {
                CreateLevel2();
            }

            return true;
        }
        return false;
    }

    internal void SetResource(string type, GameObject obj)
    {
		int typeNum = 0;
		switch (type)
        {
            case "wball":
                typeNum = (int)GAME_RESOURCE.WBALL;
                break;
            case "syrup":
                typeNum = (int)GAME_RESOURCE.SYRUP;
                break;
        }
		obj.GetComponent<CObstacle>().SetObstacleResource(typeNum);
    }

    void GenerateObstacle(GAME_LEVEL level_num, int[ , ] map)
    {
        /* map array */
        var sr_box = pirate_box.GetComponent<SpriteRenderer>();
        var sr_cask = cask.GetComponent<SpriteRenderer>();
        var sr_screw = screw.GetComponent<SpriteRenderer>();
        for (int i = 0; i < map_height; i++)
        {
            for (int j = 0; j < map_width; j++)
            {

                if (map[i, j] != 0)
                {
                    sr_box.sortingOrder = i + 8;
                    sr_cask.sortingOrder = i + 8;
                    sr_screw.sortingOrder = i + 8;

                    int num = map[i, j] & 0x0000ffff;
                    GameObject obj;
                    float posx = start_x + 1 * j;
                    float posy = start_y - 1 * i;
                    int obstacle_x = (int)start_x + 1 * j;
                    int obstacle_y = (int)start_y - 1 * i;
                    switch (num)
                    {
                        case 1:
                            //obj = Instantiate(box, new Vector3(start_x + 1.0f * j, 4.5f - 1.0f * i, 0), gameObject.transform.rotation) as GameObject;
                            if (level_num == GAME_LEVEL.GAME_PIRATE)
                            {
                                obj = Instantiate(pirate_box, new Vector3(posx, posy, 0), gameObject.transform.rotation) as GameObject;
                                obj.name = "obstacle" + obstacle_x + "/" + obstacle_y;
                            }
                            else if (level_num == GAME_LEVEL.GAME_SNOW1)
                            {
                                obj = Instantiate(snowbox1, new Vector3(posx, posy, 0), gameObject.transform.rotation) as GameObject;
                                obj.name = "obstacle" + obstacle_x + "/" + obstacle_y;
                            }

                            break;
                        case 2:
                            if (level_num == GAME_LEVEL.GAME_PIRATE)
                            {
                                obj = Instantiate(cask, new Vector3(posx, posy, 0), gameObject.transform.rotation) as GameObject;
                                obj.name = "obstacle" + obstacle_x + "/" + obstacle_y;
                            }
                            else if (level_num == GAME_LEVEL.GAME_SNOW1)
                            {
                                obj = Instantiate(snow_stone, new Vector3(posx, posy, 0), gameObject.transform.rotation) as GameObject;
                                obj.name = "obstacle" + obstacle_x + "/" + obstacle_y;
                            }
                            break;
                        case 3:
                            if (level_num == GAME_LEVEL.GAME_PIRATE)
                            {
                                obj = Instantiate(screw, new Vector3(posx, posy, 0), gameObject.transform.rotation) as GameObject;
                                obj.name = "obstacle" + obstacle_x + "/" + obstacle_y;
                            }
                            else if (level_num == GAME_LEVEL.GAME_SNOW1)
                            {
                                obj = Instantiate(crystal, new Vector3(posx, posy, 0), gameObject.transform.rotation) as GameObject;
                                obj.name = "obstacle" + obstacle_x + "/" + obstacle_y;
                            }
                            break;
                    }
                    
                }
            }
        }
    }

    internal void HandleBBResource(string msg)
    {
        string[] m = msg.Split('/');
		int num = int.Parse(m[1]);
		for (int i = 1; i <= num; i++)
        {
            string objName = "obstacle" + int.Parse(m[i * 2 + 1]) + "/" + int.Parse(m[i * 2 ]);
			GameObject obj = GameObject.Find(objName);
			if (obj)
				SetResource(m[0], obj);
        }
    }

    internal void CreateMonster()
    {
        GameObject obj;
        obj = Instantiate(cask, new Vector3(3, 3, 0), gameObject.transform.rotation) as GameObject;
    }
}

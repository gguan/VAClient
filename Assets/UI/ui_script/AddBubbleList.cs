using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class AddBubbleList : MonoBehaviour {

	// Use this for initialization
    public string[] texts = new string[] { "最近有什么好看的电影?", "今天的星座运势", "最近流行什么美食?", "给我推荐一款游戏?" };
	private Boolean isCreatenNew = false;
    GameObject bubbleGameObject;
    GameObject[] oldGameObjects;
    void Start()
    {
		bubbleGameObject = (GameObject)Resources.Load("Prefabs/Bubble");
        print(bubbleGameObject);
        Invoke("createBubble", 2.0f);
    }

      
    private void createBubble()
    {
        oldGameObjects = new GameObject[texts.Length];
        int x = 850;
        int y = -10;
        int z = 0;
        for (int len = texts.Length, i = len - 1; i >= 0; i--)
        {
            y = y + 170;
            GameObject g = Instantiate(bubbleGameObject);
            oldGameObjects[i] = g;
            g.GetComponent<Transform>().SetParent(GameObject.Find("ui").GetComponent<Transform>(), true);
            g.GetComponent<Transform>().SetPositionAndRotation(new Vector3(x, y, z), new Quaternion());

            Text text = g.GetComponent<Transform>().Find("BubbleText").gameObject.GetComponent<Text>();

            if (text != null)
            {
                text.text = texts[i];
            }
           
        }
    }


	public void createNewBubble(string[] newstr){
		isCreatenNew = true;
		texts = newstr;
	}

    // Update is called once per frame
    void Update()
    {
        if (isCreatenNew)
        {
            DestoryOldView();
            createBubble();
			isCreatenNew = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            texts = new string[] { "电影?", "星座", "餐饮?", "娱乐?", "新闻?" };
            DestoryOldView();
            createBubble();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            texts = new string[] { "最近有什么好看的电影?", "今天的星座运势", "最近流行什么美食?", "给我推荐一款游戏?" };
            DestoryOldView();
            createBubble();
        }
    }

    private void DestoryOldView()
    {
        for (int i = 0, len = oldGameObjects.Length; i < len; i++)
        {
            Destroy(oldGameObjects[i]);
        }
    }
}

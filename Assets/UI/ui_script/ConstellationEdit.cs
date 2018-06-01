using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstellationEdit : MonoBehaviour {



	public string title ="12星座运势";
	public int loveLuckStar =1;
	public int moneyLuckStar =2;
	public int careerLuckStar=3;
	public int totalLuckStar=4;
	public int luckNumber=5;
	public string luckColor="绿色";
	public string starConstellation ="双鱼座";

	public string descriptionText= "今天不要出门!!!";
	public int type =2;// 星座图标
	public string constellationDate;
    public string constellationName;

	GameObject titleGameObject;
	GameObject loveLuckGameObject;
	GameObject moneyLuckGameObject;
	GameObject careerLuckGameObject;
	GameObject totalLuckGameObject;
	GameObject luckNumberGameObject;
	GameObject luckColorGameObject;
	GameObject bewareConstellationGameObject;
	GameObject starConstellationGameObject;
	GameObject iconOfConstellationGameObject;
	GameObject dateOfConstellationGameObject;
	GameObject descriptionTextOfConstellationGameObject;
	// Use this for initialization
	void Start () {
		titleGameObject =  transform.Find("constellation_luck_title").gameObject;
		loveLuckGameObject = transform.Find("love_luck/star").gameObject;
		moneyLuckGameObject = transform.Find("money_luck/star").gameObject;
		careerLuckGameObject = transform.Find("career_luck/star").gameObject;
		totalLuckGameObject = transform.Find("total_luck/star").gameObject;
		luckNumberGameObject = transform.Find("luck_number").gameObject;
		luckColorGameObject = transform.Find("luck_color").gameObject;
         
		starConstellationGameObject = transform.Find("agree_constellation").gameObject;
		iconOfConstellationGameObject = transform.Find("constellation_icon").gameObject;
		dateOfConstellationGameObject = transform.Find("constellation_icon/constellation_date").gameObject;
		descriptionTextOfConstellationGameObject = transform.Find("detailed_description/constellation_descirption_text").gameObject;
        
	}



	private string getStarPath(int count)
	{
		switch(count){
			case 0:
				return "star/0星";
			case 1:
				return "star/1星";
			case 2:
				return "star/2星";
			case 3:
				return "star/3星";
			case 4:
				return "star/4星";
			case 5:
				return "star/5星";
		}
		return "0星";
	}

	private string getIconPath(int index){
		switch(index){
			case 0:
				return "constellation/0_白羊座";
            case 1:
				return "constellation/1_金牛座";
            case 2:
				return "constellation/2_双子座";
            case 3:
				return "constellation/3_巨蟹座";
            case 4:
				return "constellation/4_狮子座";
            case 5:
				return "constellation/5_处女座";
            case 6:
				return "constellation/6_天秤座";
            case 7:
				return "constellation/7_天蝎座";
            case 8:
				return "constellation/8_射手座";
            case 9:
				return "constellation/9_摩蝎座";
            case 10:
				return "constellation/10_水瓶座";
			case 11:
				return "constellation/11_双鱼座";
		}
		return "";
	}

	// Update is called once per frame
	void Update () {
		

	}

	private void OnGUI()
	{
		setConstellationTitle(title);
        setStar(loveLuckGameObject, loveLuckStar);
        setStar(moneyLuckGameObject, moneyLuckStar);
        setStar(careerLuckGameObject, careerLuckStar);
        setStar(totalLuckGameObject, totalLuckStar);

        setConstellationInfo(luckNumberGameObject, "幸运数字", luckNumber + "");
        setConstellationInfo(luckColorGameObject, "幸运颜色", luckColor);
		setConstellationInfo(starConstellationGameObject, "幸运星座", starConstellation);


		setConstellationDate(constellationName +"\n" +constellationDate);
        setConstellationDescriptionText(descriptionText);
        setIcon(iconOfConstellationGameObject, type);
	}

	//设置总运势星星
	public void setTotalConstellationStar(int count){
		totalLuckStar = count;
		setStar(totalLuckGameObject,totalLuckStar);
	}

    //设置事业运星星
	public void setCareerConstellationStar(int count){
		careerLuckStar = count;
		setStar(careerLuckGameObject,count);
	}

    //设置金钱运 星星
	public void setMoneyConstellationStar(int count){
		moneyLuckStar = count;
		setStar(moneyLuckGameObject,count);
	}

	public void setLoveLuckConstellationStar(int count){
		loveLuckStar = count;
		setStar(loveLuckGameObject,count);
	}

    //设置星座运势面板标题
	public void setConstellationTitle(string t){
		title = t;
		titleGameObject.GetComponent<Text>().text = title;
	}

    // 设置运势描述文字
    public void setConstellationDescriptionText(string text)
	{
		descriptionText = text;
		descriptionTextOfConstellationGameObject.GetComponent<Text>().text = text;
	}

    // 设置星座日期
	public void setConstellationDate( string text)
	{
		dateOfConstellationGameObject.GetComponent<Text>().text = text;
	}

	private void setConstellationInfo(GameObject game,string descrition,string  color)
	{
		game.GetComponent<Text>().text = descrition+ " : " +  color;
	}

	private void setStar(GameObject g,int count)
	{
		RawImage rawImage = g.GetComponent<RawImage>();
		rawImage.texture = Resources.Load(getStarPath(count)) as Texture;
	}

	private void setIcon(GameObject g, int index)
    {
        RawImage rawImage = g.GetComponent<RawImage>();
		rawImage.texture = Resources.Load(getIconPath(index)) as Texture;
    }

	void prinComponents(GameObject game){
		Component[] components = game.GetComponents<Component>();
        for (int i = 0, len = components.Length; i < len; i++)
        {
            Component c = components[i];
            if (c != null)
            {
                Debug.Log(c.GetType() + "---" + c.name);
            }

        }
	}
}

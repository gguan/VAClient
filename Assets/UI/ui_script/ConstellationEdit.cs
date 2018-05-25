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
	public string bewareConstellation= "白羊座";
	public string agreeConstellation ="双鱼座";
	public string dateConstellation = "天平座\n9.23-10.23";
	public string descriptionText= "今天不要出门!!!";

	GameObject titleGameObject;
	GameObject loveLuckGameObject;
	GameObject moneyLuckGameObject;
	GameObject careerLuckGameObject;
	GameObject totalLuckGameObject;
	GameObject luckNumberGameObject;
	GameObject luckColorGameObject;
	GameObject bewareConstellationGameObject;
	GameObject agreeConstellationGameObject;
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
		bewareConstellationGameObject = transform.Find("agree_constellation").gameObject;
		agreeConstellationGameObject = transform.Find("beware_constellation").gameObject;
		iconOfConstellationGameObject = transform.Find("constellation_icon").gameObject;
		dateOfConstellationGameObject = transform.Find("constellation_icon/constellation_date").gameObject;
		descriptionTextOfConstellationGameObject = transform.Find("constellation_descirption_text").gameObject;
	}



	private string getStarPath(int count)
	{
		switch(count){
			case 0:
				return "0星";
			case 1:
				return "1星";
			case 2:
                return "2星";
			case 3:
                return "3星";
			case 4:
                return "4星";
			case 5:
                return "5星";
		}
		return "0星";
	}

	// Update is called once per frame
	void Update () {

		setConstellationTitle(title);
		setStar(loveLuckGameObject,loveLuckStar);
		setStar (moneyLuckGameObject, moneyLuckStar);
		setStar( careerLuckGameObject,careerLuckStar);
		setStar(totalLuckGameObject, totalLuckStar);

		setConstellationInfo(luckNumberGameObject, "幸运数字", luckNumber+"");
		setConstellationInfo(luckColorGameObject,"幸运颜色",luckColor);
		setConstellationInfo(bewareConstellationGameObject, "提防星座", bewareConstellation);
		setConstellationInfo(agreeConstellationGameObject,"契合星座",agreeConstellation);

		setConstellationDate(dateConstellation);
		setConstellationDescriptionText(descriptionText);
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
		dateConstellation = text;
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

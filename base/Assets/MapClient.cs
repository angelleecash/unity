using UnityEngine;
using System.Collections;
using System;

public class MapClient : MonoBehaviour {
	
	public static int MAP_WIDTH = 5000, MAP_HEIGHT = 5000;
	public static int MAP_WIDTH_HALF, MAP_HEIGHT_HALF;
	
	public static int VIEW_WIDTH = 100, VIEW_HEIGHT = 100;
	public static int VIEW_WIDTH_HALF, VIEW_HEIGHT_HALF;
	
	public int viewWidth, viewHeight;
	
	public int playerMoveStep;
	public int playerX, playerY;
	public Rect window;
	public Texture2D texture;
	public Map map;
	public DateTime lastCheckTime;
	
	// Use this for initialization
	void Start () {
		map = new Map(MAP_WIDTH, MAP_HEIGHT);
		
		VIEW_WIDTH_HALF = VIEW_WIDTH >> 1;
		VIEW_HEIGHT_HALF = VIEW_HEIGHT >> 1;
		
		playerMoveStep = 5;
		window = new Rect(0, 0, VIEW_WIDTH, VIEW_HEIGHT);
		
		Color color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
		texture = new Texture2D(VIEW_WIDTH, VIEW_HEIGHT);
		for(int i=0;i < VIEW_WIDTH;i++)
		{
			for(int j=0; j < VIEW_HEIGHT;j ++)
			{
				texture.SetPixel(i, j, color);
			}
		}
		
		texture.Apply();
		
		
		GUIStyle generic_style = new GUIStyle();
	    GUI.skin.box = generic_style;
		
		lastCheckTime = DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () 
	{
		map.Update(30);
	}
	
	void playerMoveTo(int x, int y){
		Rectangle currentView = new Rectangle(playerX - VIEW_WIDTH_HALF, playerY - VIEW_HEIGHT_HALF, VIEW_WIDTH, VIEW_HEIGHT);
		Rectangle targetView = new Rectangle(x - VIEW_WIDTH_HALF, y - VIEW_HEIGHT_HALF, VIEW_WIDTH, VIEW_HEIGHT);
		
		Rectangle[] newAreas = targetView.minus(currentView);
		for(int i=0 ;i < newAreas.Length; i++)
		{
			
		}
	}
	
	void OnGUI()
	{
		GUI.Box (window, texture);
	}
}

using UnityEngine;
using System.Collections;
using System;

public class MapClient : MonoBehaviour {
	
	private static int MAP_WIDTH = 10000, MAP_HEIGHT = 10000;
	private static int MAP_WIDTH_HALF, MAP_HEIGHT_HALF;
	
	private static int VIEW_WIDTH = 10000, VIEW_HEIGHT = 10000;
	private static int VIEW_WIDTH_HALF, VIEW_HEIGHT_HALF;
	
	private int viewWidth, viewHeight;
	
	private int playerMoveStep;
	private int playerX, playerY;
	
	// Use this for initialization
	void Start () {
		VIEW_WIDTH_HALF = VIEW_WIDTH >> 1;
		VIEW_HEIGHT_HALF = VIEW_HEIGHT >> 1;
		
		playerMoveStep = 5;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void playerMoveTo(int x, int y){
		Rectangle currentView = new Rectangle(playerX - VIEW_WIDTH_HALF, playerY - VIEW_HEIGHT_HALF, VIEW_WIDTH, VIEW_HEIGHT);
		Rectangle targetView = new Rectangle(x - VIEW_WIDTH_HALF, y - VIEW_HEIGHT_HALF, VIEW_WIDTH, VIEW_HEIGHT);
		
		Rectangle difference = targetView.intersect(currentView);
		if(!difference.isEmpty())
		{
			
		}
	}
}

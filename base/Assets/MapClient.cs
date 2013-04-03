using UnityEngine;
using System.Collections;
using System;

public class MapClient : MonoBehaviour
{
	
	public int mapWidth, mapHeight;
	public int viewWidth, viewHeight;
	public int viewWidthHalf, viewHeightHalf;
	public int playerMoveStep = 5;
	public int playerX, playerY;
	public Rect window;
	public Texture2D texture;
	public Map map;
	public DateTime lastCheckTime;
	public Rectangle viewArea, mapArea;
	public Color backgroundColor, requestingColor, readyColor;
	public RequestManager requestManager;
	
	// Use this for initialization
	void Start ()
	{
		mapWidth = Screen.width;
		mapHeight = Screen.height;
		
		viewWidth = 80;
		viewHeight = 80;
		
		viewWidthHalf = viewWidth >> 1;
		viewHeightHalf = viewHeight >> 1;
		
		playerX = 200;
		playerY = 200;
		
		requestManager = new RequestManager ();
		
		map = new Map (mapWidth, mapHeight);
		
		playerMoveStep = 5;
		window = new Rect (0, 0, mapWidth, mapHeight);
		
		backgroundColor = new Color (0.0f, 0.0f, 0.0f, 1.0f);
		requestingColor = new Color (0.0f, 1.0f, 0.0f, 1.0f);
		readyColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		
		texture = new Texture2D (mapWidth, mapHeight);
		for (int i=0; i < mapWidth; i++) {
			for (int j=0; j < mapHeight; j ++) {
				texture.SetPixel (i, j, backgroundColor);
			}
		}
		
		texture.Apply ();
		
		mapArea = new Rectangle (0, 0, mapWidth, mapHeight);
		
		viewArea = new Rectangle (playerX - viewWidthHalf, playerY - viewHeightHalf, viewWidth, viewHeight);
		viewArea = viewArea.intersect (mapArea);
		
		print ("x="+viewArea.left+" y="+viewArea.top+"width="+viewArea.width+"height="+viewArea.height);
		
		//GUIStyle generic_style = new GUIStyle();
		//GUI.skin.box = generic_style;
		
		lastCheckTime = DateTime.Now;
		
		print ("Init Done");
	}
	
	// Update is called once per frame
	void Update ()
	{
		//render player view
		viewArea.Set (playerX - viewWidthHalf, playerY - viewHeightHalf, viewWidth, viewHeight);
		
		viewArea = viewArea.intersect (mapArea);
		
		//print ("x="+viewArea.left+" y="+viewArea.top+"width="+viewArea.width+"height="+viewArea.height);
		
		for (int row=viewArea.top; row < viewArea.getBottom(); row++) {
			for (int col=viewArea.left; col < viewArea.getRight(); col++) {
				MapNode mapNode = map.getMapNode (col, row);
				//print ("row="+row+" col="+col);
				int x = col;
				int y = mapHeight - row - 1;
				Color renderColor = backgroundColor;
				if (mapNode.state == MapNode.STATE_UNKNOWN) {
					renderColor = backgroundColor;
				} else if (mapNode.state == MapNode.STATE_READY) {
					renderColor = readyColor;
				} else if (mapNode.state == MapNode.STATE_REQUEST) {
					renderColor = requestingColor;
				}
				
				texture.SetPixel (x, y, renderColor);
			}
		}
		
		texture.Apply ();
		//print ("map rendered.");
		
		//update map
		map.Update (30);
		//print ("map updated.");
		
		requestManager.Tick ();
		//print ("request processed.");
		
		CheckArea (viewArea);
	}
	
	void PlayerMoveTo (int x, int y)
	{
		Rectangle currentView = new Rectangle (playerX - viewWidthHalf, playerY - viewHeightHalf, viewWidth, viewHeight);
		Rectangle targetView = new Rectangle (x - viewWidthHalf, y - viewHeightHalf, viewWidth, viewHeight);
		
		Rectangle[] newAreas = targetView.minus (currentView);
		for (int i=0; i < newAreas.Length; i++) {
			Rectangle newArea = newAreas [i];
			CheckArea(newArea);
		}
	}
	
	void CheckArea (Rectangle newArea)
	{
		for (int row=newArea.top; row < newArea.getBottom(); row++) {
			for (int col=newArea.left; col < newArea.getRight(); col++) {
				MapNode mapNode = map.getMapNode (col, row);
				if (mapNode.state == MapNode.STATE_UNKNOWN) {
					requestManager.requestMapData (mapNode);				
				}
			}
		}
	}
	
	void OnGUI ()
	{
		GUI.Box (window, texture);
		//print ("map rendered.");
	}
}

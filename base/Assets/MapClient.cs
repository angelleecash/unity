using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapClient : MonoBehaviour
{
	
	public int mapWidth, mapHeight;
	
	//should be initialized from server
	public int mapCellWidth, mapCellHeight;
	
	public int viewWidth, viewHeight;
	public int viewWidthHalf, viewHeightHalf;
	public int playerX, playerY;
	public Texture2D texture;
	public Map map;
	public DateTime lastCheckTime;
	public Rectangle viewArea, mapArea;
	public Color backgroundColor, requestingColor, readyColor;
	public RequestManager requestManager;
	public Boolean dragging;
	public Vector3 mousePositionWhenStartToDrag;
	public int playerXWhenStartingToDrag, playerYWhenStartingToDrag;
	public Rect window;
	
	// Use this for initialization
	void Start ()
	{
		mapWidth = 1000;
		mapHeight = 1000;
		
		mapCellWidth = 100;
		mapCellHeight = 100;
		
		map = new Map (mapWidth, mapHeight, mapCellWidth, mapCellHeight);
		mapArea = new Rectangle (0, 0, mapWidth, mapHeight);
		
		requestManager = new RequestManager ();
		
		viewWidth = Screen.width;
		viewHeight = Screen.height;
		
		viewWidthHalf = viewWidth >> 1;
		viewHeightHalf = viewHeight >> 1;
		
		texture = new Texture2D (viewWidth, viewHeight);
		window = new Rect (0, 0, viewWidth, viewHeight);
		
		playerX = 70;
		playerY = 70;
		
		viewArea = new Rectangle (playerX - viewWidthHalf, playerY - viewHeightHalf, viewWidth, viewHeight);
		
		ValidatePlayerPosition();
		RequestMapCells();
		
		backgroundColor = new Color (0.0f, 0.0f, 0.0f, 1.0f);
		requestingColor = new Color (0.0f, 1.0f, 0.0f, 1.0f);
		readyColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		
		lastCheckTime = DateTime.Now;
		
		print ("Init Done");
	}
	
	void CheckArea (Rectangle newArea)
	{
		
	}
	
	private void RenderMap ()
	{
		for (int row=viewArea.top; row < viewArea.getBottom(); row++) {
			for (int col=viewArea.left; col < viewArea.getRight(); col++) {
				
				int data = map.getData(col, row);
				MapCell mapCell = map.getMapCell(col, row);
				
				int x = col - viewArea.left;
				int y = viewArea.height - (row - viewArea.top);
				
				Color renderColor = backgroundColor;
				if (mapCell.state == MapCell.STATE_UNKNOWN) {
					
				} else if (mapCell.state == MapCell.STATE_READY) {
					float factor = mapCell.lifeTime * 1.0f / MapNode.DATA_LIFE_TIME;
					renderColor = readyColor * factor;
					renderColor.a = 1.0f;
				} else if (mapCell.state == MapCell.STATE_REQUEST) {
					renderColor = requestingColor;
				}
				
				texture.SetPixel (x, y, renderColor);
				
			}
		}
		
		texture.Apply ();
	}
	
	private void RequestMapCells()
	{
		List<MapCell> mapCells = map.getMapCells(viewArea.left, viewArea.top, viewArea.getRight(), viewArea.getBottom());
		
		for(int i=mapCells.Count-1; i >= 0; i--)
		{
			MapCell mapCell = mapCells[i];
			if(mapCell.state == MapCell.STATE_REQUEST)
			{
				mapCells.RemoveAt(i);
			}
		}
		
		if(mapCells.Count > 0)
		{
			requestManager.requestMapCells(mapCells);			
		}
	}
	
	private void ProcessUserInput ()
	{
		if (Input.GetMouseButtonDown (0)) {
			if (dragging) {
				mousePositionWhenStartToDrag = Input.mousePosition;
				playerXWhenStartingToDrag = playerX;
				playerYWhenStartingToDrag = playerY;
					
				dragging = true;
			} else {
				Vector3 delta = Input.mousePosition - mousePositionWhenStartToDrag;
			
				int deltaX = (int)delta.x;
				int deltaY = (int)delta.y;
				
				playerX = playerXWhenStartingToDrag - deltaX;
				playerY = playerYWhenStartingToDrag + deltaY;
				
				RequestMapCells();
			}	
		}
		
		if (Input.GetMouseButtonUp (0)) 
		{
			dragging = false;
		}
	}
	
	
	
	private void ValidatePlayerPosition ()
	{
		playerX = Util.Clamp (playerX, viewWidthHalf, mapWidth - viewWidthHalf);
		playerY = Util.Clamp (playerY, viewHeightHalf, mapHeight - viewHeightHalf);
		
		viewArea.Set (playerX - viewWidthHalf, playerY - viewHeightHalf, viewWidth, viewHeight);
	}
	
	private void CalculateViewArea ()
	{
		ValidatePlayerPosition ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		ProcessUserInput ();
		CalculateViewArea ();
		map.Update (30);
		RenderMap ();
	}
	
	void PlayerMoveTo (int x, int y)
	{
		Rectangle currentView = new Rectangle (playerX - viewWidthHalf, playerY - viewHeightHalf, viewWidth, viewHeight);
		Rectangle targetView = new Rectangle (x - viewWidthHalf, y - viewHeightHalf, viewWidth, viewHeight);
		
		Rectangle[] newAreas = targetView.minus (currentView);
		for (int i=0; i < newAreas.Length; i++) {
			Rectangle newArea = newAreas [i];
			CheckArea (newArea);
		}
	}
	
	void OnGUI ()
	{
		GUI.Box (window, texture);
	}
}

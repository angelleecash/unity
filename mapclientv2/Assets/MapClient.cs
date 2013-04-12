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
	
	public Map map;
	
	public Rectangle viewArea, mapArea;
	public Color backgroundColor, requestingColor, readyColor;
	public RequestManager requestManager;
	public Boolean dragging;
	public Vector3 mousePositionWhenStartToDrag;
	public int playerXWhenStartingToDrag, playerYWhenStartingToDrag;
	public Rect window;
	public Boolean requestMapWhenMouseUp;
	public Material myMaterial;
	public MapStorage mapStorage;
	
	// Use this for initialization
	void Start ()
	{
		mapWidth = 10000;
		mapHeight = 10000;
		
		mapCellWidth = 50;
		mapCellHeight = 50;
		
		map = new Map (mapWidth, mapHeight, mapCellWidth, mapCellHeight);
		mapArea = new Rectangle (0, 0, mapWidth, mapHeight);
		
		requestManager = new RequestManager ();
		
		viewWidth = Screen.width;
		viewHeight = Screen.height;
		
		viewWidthHalf = viewWidth >> 1;
		viewHeightHalf = viewHeight >> 1;
		
		window = new Rect (0, 0, viewWidth, viewHeight);
		
		playerX = 70;
		playerY = 70;
		
		viewArea = new Rectangle (playerX - viewWidthHalf, playerY - viewHeightHalf, viewWidth, viewHeight);
		
		UpdateViewArea ();
		RequestMapCells ();
		
		backgroundColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		requestingColor = new Color (1.0f, 1.0f, 0.0f, 1.0f);
		readyColor = new Color (1.0f, 0.0f, 0.0f, 1.0f);
		
		mapStorage = new MapStorage();
		//mapStorage.load(map);
		//GL.Viewport(window);
		//print ("Init Done");
	}
	
	void CheckArea (Rectangle newArea)
	{
		
	}
	
	private void fillTriangle(int left, int top, int right, int bottom)
	{
	}
	
	private void fillRectangle (Rectangle bound, Color renderColor)
	{
		myMaterial.SetPass(0);
		GL.Color (renderColor);
		GL.Begin (GL.QUADS);
		GL.Vertex3 (bound.left, bound.top, 0);
		GL.Vertex3 (bound.getRight(), bound.top, 0);
		GL.Vertex3 (bound.getRight (), bound.getBottom(), 0);
		GL.Vertex3 (bound.left, bound.getBottom(), 0);
		GL.End ();
	}
	
	private void RequestMapCells ()
	{
		List<MapCell> mapCells = map.getMapCells (viewArea.left, viewArea.top, viewArea.getRight (), viewArea.getBottom ());
		
		for (int i=mapCells.Count-1; i >= 0; i--) {
			MapCell mapCell = mapCells [i];
			if (mapCell.state != MapCell.STATE_UNKNOWN) {
				mapCells.RemoveAt (i);
			}
		}
		
		if (mapCells.Count > 0) {
			/*
			print ("requesting " + mapCells.Count);
			for (int i=0; i < mapCells.Count; i++) {
				print("requesting " + mapCells[i].cellX + " " + mapCells[i].cellY);
			}
			*/
			requestManager.requestMapCells (mapCells);			
		}
	}
	
	private void UpdateUserPosition ()
	{
		Vector3 delta = Input.mousePosition - mousePositionWhenStartToDrag;
		//print ("Mouse moving " + Input.mousePosition.x + " " + Input.mousePosition.y);
		int deltaX = (int)delta.x;
		int deltaY = (int)delta.y;
		int ox = playerX;
		int oy = playerY;
		
		playerX = playerXWhenStartingToDrag - deltaX;
		playerY = playerYWhenStartingToDrag + deltaY;
		
		UpdateViewArea ();
	}
	
	private void ProcessDragMap ()
	{
		if (dragging) {
			UpdateUserPosition ();
			
			if (requestMapWhenMouseUp) {
				RequestMapCells ();	
			}
		}
	}
	
	private void DetectUserDragMap ()
	{
		if (Input.GetMouseButtonDown (0)) {
			if (!dragging) {
				mousePositionWhenStartToDrag = Input.mousePosition;
				playerXWhenStartingToDrag = playerX;
				playerYWhenStartingToDrag = playerY;
				
				dragging = true;
				
				//print ("start dragging " + mousePositionWhenStartToDrag.x + " " + mousePositionWhenStartToDrag.y);
			}	
		}
		
		if (Input.GetMouseButtonUp (0)) {
			if (dragging) {
				UpdateUserPosition ();
				RequestMapCells ();
			}
			
			dragging = false;
		}
	}
	
	private void UpdateViewArea ()
	{
		//print ("Clamping playerX="+playerX+" playerY="+playerY);
		playerX = Util.Clamp (playerX, viewWidthHalf, mapWidth - viewWidthHalf);
		playerY = Util.Clamp (playerY, viewHeightHalf, mapHeight - viewHeightHalf);
		//print ("After Clamping playerX="+playerX+" playerY="+playerY);
		
		viewArea.Set (playerX - viewWidthHalf, playerY - viewHeightHalf, viewWidth, viewHeight);
	}
	
	private void RenderMap (List<MapCell> mapCells)
	{
		for (int i=0; i < mapCells.Count; i++) {
			MapCell mapCell = mapCells [i];
			Color renderColor = backgroundColor;
			if (mapCell.state == MapCell.STATE_UNKNOWN) {
				
			} else if (mapCell.state == MapCell.STATE_READY) {
				float factor = mapCell.lifeTime * 1.0f / MapCell.DATA_LIFE_TIME;
				//print (factor);
				renderColor = readyColor * factor;
				renderColor.a = 1.0f;
				
				//renderColor = readyColor;
				//print ("render " + renderColor);
			} else if (mapCell.state == MapCell.STATE_REQUEST) {
				renderColor = requestingColor;
			}
			
			
			Rectangle bound = mapCell.bound.intersect (viewArea);
			if (!bound.isEmpty ()) {
				bound.left -= viewArea.left;
				bound.top -= viewArea.top;
				fillRectangle (bound, renderColor);
			}
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		map.Update (30);
		RequestMapCells();
		ProcessInput();
	}
	
	void ProcessInput()
	{
		if(Input.GetKeyUp(KeyCode.S))
		{
			mapStorage.save(map);
			//print("Map saved.");
		}
		else if(Input.GetKeyUp(KeyCode.L))
		{
			mapStorage.load(map);
			//print("Map loaded.");
		}
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
		//print ("OnGUI");
		//RequestMapCells();
		Event currentEvent = Event.current;
		if (currentEvent != null) {
			EventType eventType = Event.current.type;			
			if (eventType == EventType.MouseDrag) {
				//print ("dragging");
				ProcessDragMap ();
			} else if (eventType == EventType.MouseDown) {
				if (!dragging) {
					mousePositionWhenStartToDrag = Input.mousePosition;
					playerXWhenStartingToDrag = playerX;
					playerYWhenStartingToDrag = playerY;
				
					dragging = true;
				
					//print ("start dragging " + mousePositionWhenStartToDrag.x + " " + mousePositionWhenStartToDrag.y);
				}	
			} else if (eventType == EventType.MouseUp) {
				if (dragging) {
					
					//print ("mouse up handler");
					UpdateUserPosition ();
					RequestMapCells ();
					
					//print ("stop dragging");
				}
			
				dragging = false;
			} else if (eventType == EventType.Repaint) {
				List<MapCell> mapCells = map.getMapCells (viewArea.left, viewArea.top, viewArea.getRight (), viewArea.getBottom ());
				RenderMap(mapCells);						
			}
			
			
		}
		
	}
	
}

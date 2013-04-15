using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapClient : MonoBehaviour, LoadMapRequestListener
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
	
	public Material myMaterial;
	public MapStorage mapStorage;
	private int lastPlayerX, lastPlayerY;
	
	private Dictionary<String, Color> colors;
	private System.Random random;
	// Use this for initialization
	void Start ()
	{
		mapWidth = 10000;
		mapHeight = 10000;
		
		mapCellWidth = 50;
		mapCellHeight = 50;
		
		colors = new Dictionary<string, Color>();
		random = new System.Random();
		
		map = new Map (mapWidth, mapHeight, mapCellWidth, mapCellHeight);
		//myMaterial = Resources.Load("CubeMaterial") as Material;
		mapStorage = new MapStorage(map);
		mapStorage.Init();
		
		mapArea = new Rectangle (0, 0, mapWidth, mapHeight);
		
		requestManager = new RequestManager ();
		
		viewWidth = Screen.width;
		viewHeight = Screen.height;
		
		viewWidthHalf = viewWidth >> 1;
		viewHeightHalf = viewHeight >> 1;
		
		window = new Rect (0, 0, viewWidth, viewHeight);
		
		playerX = 70;
		playerY = 70;
		
		lastPlayerX = playerX;
		lastPlayerY = playerY;
		
		viewArea = new Rectangle (playerX - viewWidthHalf, playerY - viewHeightHalf, viewWidth, viewHeight);
		
		UpdateViewArea ();
		RequestMapCells ();
		
		backgroundColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		requestingColor = new Color (1.0f, 1.0f, 0.0f, 1.0f);
		readyColor = new Color (1.0f, 0.0f, 0.0f, 1.0f);
	}
	
	private void FillRectangle (Rectangle bound, Color renderColor)
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
		FilterMapCells(mapCells);
		
		if (mapCells.Count > 0) {
			requestManager.RequestMapCells (mapCells, this);			
			
			List<MapCell> additionalMapCells = LoadAdditionalArea();
			FilterMapCells(additionalMapCells);
			
			if(additionalMapCells.Count > 0)
			{
				requestManager.RequestMapCells (additionalMapCells, this);			
			}
		}
	}
	
	private void FilterMapCells(List<MapCell> mapCells)
	{
		for (int i=mapCells.Count-1; i >= 0; i--) {
			MapCell mapCell = mapCells [i];
			if (mapCell.state != MapCell.STATE_UNKNOWN) {
				mapCells.RemoveAt (i);
			}
		}
	}
	
	private Color GetMapCellColor(MapCell mapCell)
	{
		String key = mapCell.cellX+"_"+mapCell.cellY;
		
		if(!colors.ContainsKey(key))
		{
			colors.Add(key, new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1.0f));	
		}
		
		
		
		return colors[key];
	}
	
	private List<MapCell> LoadAdditionalArea()
	{
		//require additional map info based on user input
		
		List<MapCell> mapCells = new List<MapCell>();
		
		int deltaX = playerX - lastPlayerX;
		int deltaY = playerY - lastPlayerY;
		
		int xHintCellCount = 0;
		int yHintCellCount = 0;
		
		if(deltaX >= 0)
		{
			xHintCellCount = (deltaX + mapCellWidth - 1) / mapCellWidth;
		}
		else{
			xHintCellCount = (deltaX - mapCellWidth + 1) / mapCellWidth;
		}
		
		if(deltaY >= 0)
		{
			yHintCellCount = (deltaY + mapCellHeight - 1) / mapCellHeight;
		}
		else{
			yHintCellCount = (deltaY - mapCellHeight + 1) / mapCellHeight;
		}
		
		Rectangle hintArea = new Rectangle(viewArea.left + xHintCellCount*mapCellWidth, viewArea.top + yHintCellCount*mapCellHeight, viewArea.width, viewArea.height);
		hintArea = hintArea.intersect(mapArea);		
		if(!hintArea.isEmpty())
		{
			mapCells = map.getMapCells (hintArea.left, hintArea.top, hintArea.getRight (), hintArea.getBottom ());
		}
		
		return mapCells;
	}
	
	private void UpdateUserPosition ()
	{
		Vector3 delta = Input.mousePosition - mousePositionWhenStartToDrag;

		int deltaX = (int)delta.x;
		int deltaY = (int)delta.y;
		
		lastPlayerX = playerX;
		lastPlayerY = playerY;
		
		playerX = playerXWhenStartingToDrag - deltaX;
		playerY = playerYWhenStartingToDrag + deltaY;
		
		UpdateViewArea ();
	}
	
	private void ProcessDragMap ()
	{
		if (dragging) {
			UpdateUserPosition ();
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
		playerX = Util.Clamp (playerX, viewWidthHalf, mapWidth - viewWidthHalf -1);
		playerY = Util.Clamp (playerY, viewHeightHalf, mapHeight - viewHeightHalf -1);
		
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
				renderColor = GetMapCellColor(mapCell);
				renderColor *= factor;
				renderColor.a = 1.0f;
			} else if (mapCell.state == MapCell.STATE_REQUEST) {
				renderColor = requestingColor;
			}
			
			Rectangle bound = mapCell.bound.intersect (viewArea);
			if (!bound.isEmpty ()) {
				bound.left -= viewArea.left;
				bound.top -= viewArea.top;
				FillRectangle (bound, renderColor);
			}
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		map.Update (30);
		RequestMapCells();
	}
	
	void OnGUI ()
	{
		Event currentEvent = Event.current;
		if (currentEvent != null) {
			EventType eventType = Event.current.type;			
			if (eventType == EventType.MouseDrag) {
				ProcessDragMap ();
			} else if (eventType == EventType.MouseDown) {
				if (!dragging) {
					mousePositionWhenStartToDrag = Input.mousePosition;
					playerXWhenStartingToDrag = playerX;
					playerYWhenStartingToDrag = playerY;
				
					dragging = true;
				}	
			} else if (eventType == EventType.MouseUp) {
				if (dragging) {
					UpdateUserPosition ();
				}
			
				dragging = false;
			} else if (eventType == EventType.Repaint) {
				List<MapCell> mapCells = map.getMapCells (viewArea.left, viewArea.top, viewArea.getRight (), viewArea.getBottom ());
				RenderMap(mapCells);						
			}
		}
		
	}
	
	public void MapCellsLoaded(MapCell[] mapCells)
	{
		 mapStorage.WriteMapCellsToFiles(mapCells);
	}
	
	public void MapCellsLoadFail()
	{
		// :(
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapClient : MonoBehaviour
{
	
	private int mapWidth, mapHeight;
	
	//should be initialized from server
	private int mapCellWidth, mapCellHeight;
	private int viewWidth, viewHeight;
	private int viewWidthHalf, viewHeightHalf;
	private int playerX, playerY;
	
	private Map map;
	
	private Rectangle viewArea;
	private Color backgroundColor, requestingColor;
	private RequestManager requestManager;
	private Boolean dragging;
	private Vector3 mousePositionWhenStartToDrag;
	private int playerXWhenStartingToDrag, playerYWhenStartingToDrag;
	
	private Dictionary<String, Color> colors;
	private System.Random random;
	
	public Material myMaterial;
	
	// Use this for initialization
	void Start ()
	{
		mapWidth = 10000;
		mapHeight = 10000;
		
		mapCellWidth = 50;
		mapCellHeight = 50;
		
		map = new Map (mapWidth, mapHeight, mapCellWidth, mapCellHeight);
		
		colors = new Dictionary<string, Color>();
		random = new System.Random();
		
		requestManager = new RequestManager ();
		
		viewWidth = Screen.width;
		viewHeight = Screen.height;
		
		viewWidthHalf = viewWidth >> 1;
		viewHeightHalf = viewHeight >> 1;
		
		playerX = 70;
		playerY = 70;
		
		viewArea = new Rectangle (playerX - viewWidthHalf, playerY - viewHeightHalf, viewWidth, viewHeight);
		
		backgroundColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		requestingColor = new Color (1.0f, 1.0f, 0.0f, 1.0f);
		
		
		//myMaterial = Instantiate(Resources.Load("CubeMaterial")) as Material;
		
		UpdateViewArea ();
		RequestMapCells ();
	}
	
	void CheckArea (Rectangle newArea)
	{
		
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
		
		for (int i=mapCells.Count-1; i >= 0; i--) {
			MapCell mapCell = mapCells [i];
			if (mapCell.state != MapCell.STATE_UNKNOWN) {
				mapCells.RemoveAt (i);
			}
		}
		
		if (mapCells.Count > 0) {
			requestManager.requestMapCells (mapCells);			
		}
	}
	
	private void UpdateUserPosition ()
	{
		Vector3 delta = Input.mousePosition - mousePositionWhenStartToDrag;
		
		int deltaX = (int)delta.x;
		int deltaY = (int)delta.y;
		
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
	
	private Color GetMapCellColor(MapCell mapCell)
	{
		String key = mapCell.cellX+"_"+mapCell.cellY;
		
		if(!colors.ContainsKey(key))
		{
			colors.Add(key, new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1.0f));	
		}
		
		
		
		return colors[key];
	}
	
	// Update is called once per frame
	void Update ()
	{
		map.Update (30);
		RequestMapCells();
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
		if (Event.current != null) {
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
					RequestMapCells ();
				}
			
				dragging = false;
			} else if (eventType == EventType.Repaint) {
				List<MapCell> mapCells = map.getMapCells (viewArea.left, viewArea.top, viewArea.getRight (), viewArea.getBottom ());
				RenderMap(mapCells);						
			}
			
		}
		
	}
	
}

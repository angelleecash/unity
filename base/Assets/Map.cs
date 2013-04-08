using System;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
	public int width, height;
	public int mapCellWidth, mapCellHeight;
	
	public List<MapCell> mapCells;
	public int[] data;
	
	private int mapCellsPerRow;
	
	public Map (int width, int height, int mapCellWidth, int mapCellHeight)
	{
		this.width = width;
		this.height = height;
		this.mapCellWidth = mapCellWidth;
		this.mapCellHeight = mapCellHeight;
		
		data = new int[width*height];
		
		mapCellsPerRow = (width + mapCellWidth - 1) / mapCellWidth;
		
		mapCells = new List<MapCell>();
		
		int cellY = 0;
		for(int y=0; y < height; y+= mapCellHeight)
		{
			int yDiff = height-y;
			
			int cellHeight = yDiff >= mapCellHeight ? mapCellHeight : yDiff;
			int cellX = 0;
			for(int x=0; x < width; x += mapCellWidth)
			{
				int xDiff = width - x;
				int cellWidth = xDiff >= mapCellWidth ? mapCellWidth : xDiff;
				
				MapCell mapCell = new MapCell(cellX, cellY, x, y, cellWidth, cellHeight);
				mapCells.Add(mapCell);
				
				cellX ++;
			}
			
			cellY ++;
		}
		
		
	}
	
	public void Update(int timeElapsed)
	{
		
	}
	
	public int getData(int x, int y)
	{
		int index = y*width + x;
		return data[index];
	}
	
	public MapCell getMapCell(int x, int y)
	{
		for(int i=0;i < mapCells.Count ;i ++)
		{
			MapCell mapCell = mapCells[i];
			if(mapCell.Contains(x, y))
			{
				return mapCell;
			}
		}
		
		return null;
	}
	
	public List<MapCell> getMapCells(int left, int top, int right, int bottom)
	{
		List<MapCell> mapCells = new List<MapCell>();
		
		MapCell leftTopMapCell = getMapCell(left, top);
		MapCell rightBottomMapCell = getMapCell(right, bottom);
		
		for(int cellY= leftTopMapCell.cellY; cellY <= rightBottomMapCell.cellY; cellY ++)
		{
			for(int cellX= leftTopMapCell.cellX; cellX <= rightBottomMapCell.cellX; cellX++)
			{
				int index = cellY * mapCellsPerRow + cellX;
				if(index < 0 || index >= this.mapCells.Count)
				{
					MonoBehaviour.print("FAIL left="+left+" top="+top+" right="+right+" bottom="+bottom);
				}
				MapCell mapCell = this.mapCells[index];
				
				mapCells.Add(mapCell);
			}
		}
		
		return mapCells;
	}
}


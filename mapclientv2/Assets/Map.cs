using System;
using System.Collections.Generic;

public class Map
{
	internal int width, height;
	internal int mapCellWidth, mapCellHeight;
	
	internal List<MapCell> mapCells;
	internal int[] data;
	
	internal int mapCellsPerRow;
	internal Dictionary<String, MapCell> mapCellsMap;
	
	public Map (int width, int height, int mapCellWidth, int mapCellHeight)
	{
		this.width = width;
		this.height = height;
		this.mapCellWidth = mapCellWidth;
		this.mapCellHeight = mapCellHeight;
		mapCellsMap = new Dictionary<string, MapCell>();
		
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
				
				MapCell mapCell = new MapCell(this, cellX, cellY, x, y, cellWidth, cellHeight);
				
				mapCells.Add(mapCell);
				
				mapCellsMap.Add(generateId(cellX, cellY), mapCell);
				
				cellX ++;
			}
			
			cellY ++;
		}
	}
	
	private String generateId(int cellX, int cellY)
	{
		return cellX + ":"+ cellY;
	}
	
	public void Update(int timeElapsed)
	{
		for(int i=0; i < mapCells.Count; i++)
		{
			MapCell mapCell = mapCells[i];
			mapCell.Update(timeElapsed);
		}
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
	
	public MapCell getMapCellByCellPosition(int cellX, int cellY)
	{
		return mapCellsMap[generateId(cellX, cellY)];
	}
	
	public List<MapCell> getMapCells(int left, int top, int right, int bottom)
	{
		List<MapCell> mapCells = new List<MapCell>();
		
		MapCell leftTopMapCell = getMapCell(left, top);
		MapCell rightBottomMapCell = getMapCell(right-1, bottom-1);
		
		for(int cellY= leftTopMapCell.cellY; cellY <= rightBottomMapCell.cellY; cellY ++)
		{
			for(int cellX= leftTopMapCell.cellX; cellX <= rightBottomMapCell.cellX; cellX++)
			{
				
				MapCell mapCell = getMapCellByCellPosition(cellX, cellY);
				
				mapCells.Add(mapCell);
			}
		}
		
		return mapCells;
	}
}


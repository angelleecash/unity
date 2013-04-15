using System;
using System.Collections.Generic;
using System.Net;

public class LoadMapRequest
{	
	private List<MapCell> mapCells;
	private static Random random;
	private LoadMapRequestListener listener;
	
	public LoadMapRequest(List<MapCell> mapCells, LoadMapRequestListener listener)
	{
		this.mapCells = mapCells;
		this.listener = listener;
		
		if(random == null)
		{
			random = new Random();
		}
	}
	
	public void execute()
	{
		for(int i=0;i < mapCells.Count;i ++)
		{
			//pass parameters to server
			MapCell mapCell = mapCells[i];	
			//only parameters needed are map cell position
			//mapCell.cellX & mapCell.cellY
			
			mapCell.state = MapCell.STATE_REQUEST;
		}
		
		//read server response
		for(int i=0;i < mapCells.Count;i ++)
		{
			
			//based on the cellX & cellY 
			//We know exactly how many data needs to be read
			//from the server response
			MapCell mapCell = mapCells[i];	
			int[] data = new int[mapCell.bound.width*mapCell.bound.height];
			Map map = mapCell.map;
			for(int y=mapCell.bound.top; y < mapCell.bound.getBottom(); y++)
			{
				Array.Copy(data, (y-mapCell.bound.top)*mapCell.bound.width, map.data, y*map.width+mapCell.bound.left, mapCell.bound.width);
			}
			mapCell.DataReady();
		}
		
		if(listener != null)
		{
			listener.MapCellsLoaded(mapCells.ToArray());
		}
	}
	
	
}
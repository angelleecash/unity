using System;
using System.Collections.Generic;
using System.Net;

public class LoadMapRequest
{	
	public List<MapCell> mapCells;
	public static Random random;
	
	public LoadMapRequest(List<MapCell> mapCells)
	{
		this.mapCells = mapCells;
		if(random == null)
		{
			random = new Random();
		}
	}
	
	public void execute()
	{
		for(int i=0;i < mapCells.Count;i++)
		{
			mapCells[i].state = MapCell.STATE_REQUEST;
		}
		
		int networkSimulation = random.Next(500, 1000);
		System.Threading.Thread.Sleep(networkSimulation);
		
		for(int i=0;i < mapCells.Count;i ++)
		{
			//pass parameters to server
			//MapCell mapCell = mapCells[i];	
			//only parameters needed are map cell position
			//mapCell.cellX & mapCell.cellY
		}
		
		for(int i=0;i < mapCells.Count;i ++)
		{
			//read the data;
			//based on the cellX & cellY 
			//We know exactly how many data needs to be read
			//from the server response
			MapCell mapCell = mapCells[i];	
			mapCell.SetData(new int[mapCell.bound.width*mapCell.bound.height]);
		}
	}
	
	
}
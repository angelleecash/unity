using System;
using System.Collections.Generic;
using System.Net;

public class LoadMapRequest
{	
	public Boolean finished;
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
		
		int networkSimulation = random.Next(1000, 1500);
		System.Threading.Thread.Sleep(networkSimulation);
		
		/*
		String uri = "http://chenliang.info";
		WebClient webClient = new WebClient();
		byte[] response = webClient.DownloadData(uri);
		*/
		for(int i=0;i < mapCells.Count;i ++)
		{
			//pass parameters to server
			MapCell mapCell = mapCells[i];	
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
			int[] data = new int[mapCell.bound.width*mapCell.bound.height];
			Map map = mapCell.map;
			for(int y=mapCell.bound.top; y < mapCell.bound.getBottom(); y++)
			{
				Array.Copy(data, (y-mapCell.bound.top)*mapCell.bound.width, map.data, y*map.width+mapCell.bound.left, mapCell.bound.width);
			}
			mapCell.dataReady();
		}
	}
	
	
}
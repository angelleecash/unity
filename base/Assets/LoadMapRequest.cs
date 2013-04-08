using System;
using System.Collections.Generic;
using System.Net;

public class LoadMapRequest
{	
	public MapNode mapNode;
	public Boolean finished;
	public List<MapCell> mapCells;
	
	public LoadMapRequest(List<MapCell> mapCells)
	{
		this.mapCells = mapCells;
	}
	
	public void execute()
	{
		String uri = "http://chenliang.info";
		WebClient wc = new WebClient();
		
		for(int i=0;i < mapCells.Count;i ++)
		{
			//pass parameters to server
			MapCell mapCell = mapCells[i];	
			//only parameters needed are map cell position
			//mapCell.cellX & mapCell.cellY
		}
		
		byte[] bResponse = wc.DownloadData(uri);
		
		for(int i=0;i < mapCells.Count;i ++)
		{
			//read the data;
			//based on the cellX & cellY 
			//We know exactly how many data needs to be read
			//from the server response
			MapCell mapCell = mapCells[i];	
			mapCell.setData(new int[mapCell.bound.width*mapCell.bound.height]);
		}
	}
	
	
}
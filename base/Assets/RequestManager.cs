using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

public class RequestManager
{
	public RequestManager ()
	{
	}
	
	public void requestMapCells(List<MapCell> mapCells)
	{
		LoadMapRequest loadMapRequest = new LoadMapRequest(mapCells);
		
		Thread thread = new Thread(new ThreadStart(loadMapRequest.execute));
		thread.Start();
	}
}
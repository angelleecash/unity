using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

public class RequestManager
{
	public RequestManager ()
	{
	}
	
	public void RequestMapCells(List<MapCell> mapCells, LoadMapRequestListener listener)
	{
		LoadMapRequest loadMapRequest = new LoadMapRequest(mapCells, listener);
		
		Thread thread = new Thread(new ThreadStart(loadMapRequest.execute));
		thread.Start();
	}
}
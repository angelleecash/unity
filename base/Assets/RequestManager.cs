using UnityEngine;
using System;
using System.Collections.Generic;

public class RequestManager:LoadMapListener
{
	public List<LoadMapRequest> requests;
	
	public RequestManager ()
	{
		requests = new List<LoadMapRequest>();
	}
	
	public void requestMapData(MapNode mapNode)
	{
		LoadMapRequest loadMapRequest = new LoadMapRequest(this, mapNode);
		requests.Add(loadMapRequest);
	}
	
	public void Tick()
	{
		int count = 0;
		while(requests.Count > 0 && count ++ < 100)
		{
			LoadMapRequest loadMapRequest = requests[0];
			loadMapRequest.execute();
			
			requests.RemoveAt(0);
		}
		
	}
	
	public void MapAreaLoaded(MapNode mapNode, int data)
	{
		mapNode.SetData(data);
		//MonoBehaviour.print("loaded x="+mapNode.x+" y= "+mapNode.y);
	}
	
	public void MapAreaLoadFail(MapNode mapNode)
	{
		
	}
}
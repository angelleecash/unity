using System;

public class LoadMapRequest
{	
	public LoadMapListener loadMapListener;
	public MapNode mapNode;
	
	public LoadMapRequest(LoadMapListener loadMapListener, MapNode mapNode)
	{
		this.loadMapListener = loadMapListener;
		this.mapNode = mapNode;
	}
	
	public void execute()
	{
		loadMapListener.MapAreaLoaded(mapNode, 0);
	}
}
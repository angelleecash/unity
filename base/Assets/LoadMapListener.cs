using System;

public interface LoadMapListener
{
	void MapAreaLoaded(MapNode mapNode, int data);
	void MapAreaLoadFail(MapNode mapNode);
}

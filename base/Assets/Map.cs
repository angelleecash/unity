using System;
using System.Collections.Generic;

public class Map
{
	private int width, height;
	private List<MapNode> mapNodes;
	
	public Map (int width, int height)
	{
		this.width = width;
		this.height = height;
		
		mapNodes = new List<MapNode>(width*height);
	}
	
	public void Update(int timeElapsed)
	{
		for(int row=0; row < height; row++)
		{
			for(int col=0; col < width; col++)
			{
				int index = row*width + col;
				MapNode mapNode = mapNodes[index];
				mapNode.Update(timeElapsed);
			}
		}
	}
	
	public void request(int x, int y, int width, int height)
	{
		
	}
}


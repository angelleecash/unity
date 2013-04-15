using System;
using UnityEngine;

public class MapCell
{
	public Rectangle bound;
	
	public const int STATE_UNKNOWN = 0;
	public const int STATE_READY = 1;
	public const int STATE_REQUEST = 2;
	
	public int state;
	
	public static int DATA_LIFE_TIME = 200000;
	
	public long lifeTime;
	
	public int cellX, cellY;
	public Map map;
	
	public MapCell (Map map, int cellX, int cellY, int x, int y, int width, int height)
	{
		this.map = map;
		this.cellX = cellX;
		this.cellY = cellY;
		bound = new Rectangle(x, y, width, height);
	}
	
	public Boolean Contains(int x, int y)
	{
		return bound.Contains(x, y);
	}
	
	public void SetData(int[] data)
	{
		//only map holds the real data
		//we only need to copy the data for 
		//this cell to the correct position
		
		for(int y=bound.top; y < bound.getBottom(); y++)
		{
			Array.Copy(data, (y-bound.top)*bound.width, map.data, y*map.width+bound.left, bound.width);
		}
		
		
		lifeTime = DATA_LIFE_TIME;
		state = STATE_READY;
	}
	
	public void Update(int timeElapsed)
	{
		if(state == STATE_READY)
		{
			lifeTime -= timeElapsed;
			//MonoBehaviour.print("cellX="+cellX+" cellY="+cellY+" life="+lifeTime);			
			lifeTime = lifeTime <= 0 ? 0 : lifeTime;
			if(lifeTime <= 0)
			{
				state = STATE_UNKNOWN;
			}
		}
	}
}

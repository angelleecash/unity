using System;

public class MapCell
{
	public int index;
	
	public Rectangle bound;
	
	public const int STATE_UNKNOWN = 0;
	public const int STATE_READY = 1;
	public const int STATE_REQUEST = 2;
	
	public int state;
	
	public static int DATA_LIFE_TIME = 10000;
	
	public long lifeTime;
	
	public int cellX, cellY;
	
	public MapCell (int cellX, int cellY, int x, int y, int width, int height)
	{
		this.cellX = cellX;
		this.cellY = cellY;
		bound = new Rectangle(x, y, width, height);
	}
	
	public Boolean Contains(int x, int y)
	{
		return bound.Contains(x, y);
	}
	
	public void setData(int[] data)
	{
		//only map holds the real data
		//we only need to copy the data for 
		//this cell to the correct position
		
		lifeTime = DATA_LIFE_TIME;
		state = STATE_READY;
	}
}

using System;

public class MapCell
{
	public Rectangle bound;
	
	public const int STATE_UNKNOWN = 0;
	public const int STATE_READY = 1;
	public const int STATE_REQUEST = 2;
	
	public int state;
	
	public static int DATA_LIFE_TIME = 200000;
	
	public int lifeTime;
	
	public int cellX, cellY;
	public Map map;
	public int hitCount;
	
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
	
	public void Update(int timeElapsed)
	{
		if(state == STATE_READY)
		{
			lifeTime -= timeElapsed;
			lifeTime = lifeTime <= 0 ? 0 : lifeTime;
			if(lifeTime <= 0)
			{
				state = STATE_UNKNOWN;
			}
		}
	}
	
	public void DataReady()
	{
		hitCount++;
		
		lifeTime = DATA_LIFE_TIME;
		state = STATE_READY;
	}	
}

using System;

public class MapNode
{
	public const int STATE_UNKNOWN = 0;
	public const int STATE_READY = 1;
	public const int STATE_REQUEST = 2;
	public int state;
	public static int DATA_LIFE_TIME = 10000;
	public long lifeTime;
	public int data;
	public int x, y;
	
	public MapNode (int x, int y)
	{
		this.x = x;
		this.y = y;
		state = STATE_UNKNOWN;
	}
	
	public void SetData (int data)
	{
		this.data = data;
		lifeTime = DATA_LIFE_TIME;
		state = STATE_READY;
	}
	
	public void Update (int timeElapsed)
	{
		lifeTime -= timeElapsed;
		lifeTime = lifeTime <= 0 ? 0 : lifeTime;
		if (lifeTime <= 0) {
			state = STATE_UNKNOWN;
		}
	}
}


using System;

public class MapNode
{
	public const int STATE_UNKNOWN = 0;
	public const int STATE_READY = 1;
	public const int STATE_REQUEST = 2;
	
	private int state;
	
	private static int DATA_LIFE_TIME = 200000;
	
	private long lifeTime;
	
	private int data;
	
	public MapNode ()
	{
		state = STATE_UNKNOWN;
	}
	
	public void SetData(int data)
	{
		this.data = data;
		lifeTime = DATA_LIFE_TIME;
	}
	
	public void Update(int timeElapsed)
	{
		lifeTime -= timeElapsed;
		lifeTime = lifeTime <= 0 ? 0: lifeTime;
	}
}


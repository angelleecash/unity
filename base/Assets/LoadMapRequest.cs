using System;

public class LoadMapRequest
{
	public Rectangle area;
	public LoadMapListener loadMapListener;
	
	public LoadMapRequest(LoadMapListener loadMapListener, Rectangle area)
	{
		this.area = area;
	}
}
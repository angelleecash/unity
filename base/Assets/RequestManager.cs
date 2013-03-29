using System;
using System.Collections.Generic;

public class RequestManager
{
	public List<LoadMapRequest> requests;
	
	public RequestManager ()
	{
		requests = new List<LoadMapRequest>();
	}
	
	public void loadArea(Rectangle area)
	{
	}
	
	public void loadArea(List<Rectangle> areas)
	{
	}
}
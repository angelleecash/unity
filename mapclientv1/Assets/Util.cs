using System;

public class Util
{
	public Util ()
	{
	}
	
	public static int Clamp (int v, int min, int max)
	{
		v = v < min ? min : v;
		v = v > max ? max : v;
		return v;
	}
}

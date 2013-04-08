using System;

public class Util
{
	public Util ()
	{
	}
	
	public static int Clamp (int value, int min, int max)
	{
		value = value < min ? min : value;
		value = value > max ? max : value;
		return value;
	}
}

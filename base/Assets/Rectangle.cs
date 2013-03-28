using System;

public class Rectangle
{
	private int left, top, width, height;
	
	public Rectangle (int left, int top, int width, int height)
	{
		this.left = left;
		this.top = top;
		this.width = width;
		this.height = height;
	}
	
	public Rectangle(Rectangle rectangle)
	{
		this(rectangle.left, rectangle.top, rectangle.width, rectangle.height);
	}
	
	public Rectangle intersect(Rectangle rectangle)
	{
		if(left >= rectangle.getRight() ||
		   getRight() <= rectangle.left ||
		   top >= rectangle.getBottom() ||
		   getBottom() <= rectangle.top)
		{
			return new Rectangle(0, 0, 0, 0);
		}
		
		int maxLeft = Math.Max(left, rectangle.left);
		int minRight = Math.Min(getRight(), rectangle.getRight());
		
		int maxTop = Math.Max(top, rectangle.top);
		int minBottom = Math.Min(getBottom(), rectangle.getBottom());
		
		return new Rectangle(maxLeft, maxTop, minRight - maxLeft);
	}
	
	public Rectangle[] minus(Rectangle rectangle)
	{
		Rectangle intersection = intersect(rectangle);
		if(intersection.isEmpty())
		{
			return new Rectangle(this);
		}	
		
		if(intersection.left > left)
		{
			if(intersection.top > top)
			{
				return new Rectangle[]{new Rectangle(left, top, width, height - intersection.height), 
									   new Rectangle(left, intersection.top, width - intersection.width, intersection.height)};
			}
			else
			{
			}
		}
		else
		{
			
		}
	}
	
	public int getRight()
	{
		return left + width;
	}
	
	public int getBottom()
	{
		return top + height;
	}
	
	public Boolean isEmpty()
	{
		return width == 0 || height == 0;
	}
}
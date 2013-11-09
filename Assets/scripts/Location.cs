using System;

public class Location
{
	public int X { get; set; }
	public int Y { get; set; }
	
	public Location ()
	{
		this.X = 0;
		this.Y = 0;
	}
	
	public Location(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}
	
	public static double GetDistance(Location loc1, Location loc2)
	{
		return Math.Sqrt(Math.Pow(loc2.X - loc1.X, 2) + Math.Pow(loc2.Y - loc1.Y, 2));
	}
	
	public override bool Equals(Object obj)
	{
		if (obj is Location)
		{
			Location location = (Location) obj;
			
			return (this.X == location.X) && (this.Y == location.Y);
		}
		
		return false;
	}
}
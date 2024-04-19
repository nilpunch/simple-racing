using System.Runtime.CompilerServices;

public static class AngleUtils
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Angle Normalize360(this Angle angle)
	{
		return Angle.Normalize360(angle);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Angle Normalize180(this Angle angle)
	{
		return Angle.Normalize180(angle);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Angle Abs(this Angle angle)
	{
		return Angle.Abs(angle);
	}
}
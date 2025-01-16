using System.Runtime.InteropServices;

namespace Autoclicker.Data;

[StructLayout(LayoutKind.Sequential)]
public struct MousePoint(int x, int y)
{
	public int X = x;
	public int Y = y;
}
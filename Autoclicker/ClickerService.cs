using System.Runtime.InteropServices;
using Autoclicker.Data;

namespace Autoclicker;

public static partial class ClickerService
{
	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static partial bool GetCursorPos(out MousePoint lpMousePoint);

	[LibraryImport("user32.dll")]
	private static partial void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
	
	private static MousePoint GetCursorPosition()
	{
		var gotPoint = GetCursorPos(out var currentMousePoint);
		if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
		return currentMousePoint;
	}

	public static void MouseEvent(MouseEventFlags value)
	{
		var position = GetCursorPosition();

		mouse_event
			((int)value,
				position.X,
				position.Y,
				0,
				0);
	}
}
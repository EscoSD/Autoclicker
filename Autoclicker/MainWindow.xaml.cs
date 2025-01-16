using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Autoclicker.Data;
using Gma.System.MouseKeyHook;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace Autoclicker;

public partial class MainWindow
{
	[GeneratedRegex(@"^\d+$")]
	private static partial Regex MyRegex();
	
	private bool _active;
	private int _clickInterval;
	
	private readonly IKeyboardMouseEvents _globalHook;

	public MainWindow()
	{
		InitializeComponent();
		
		var clickerThread = new Thread(ClickerThread)
		{
			IsBackground = true
		};
		clickerThread.Start();
		
		_globalHook = Hook.GlobalEvents();
		_globalHook.KeyDown += GlobalHook_KeyDown;
	}

	private void GlobalHook_KeyDown(object? sender, KeyEventArgs keyEventArgs)
	{
		if (keyEventArgs.KeyCode != Keys.LControlKey) return;
		ToggleClicker();
	}
	
	private void ToggleButton_ButtonClick(object sender, RoutedEventArgs e)
	{
		ToggleClicker();
	}

	private void InputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		e.Handled = !MyRegex().IsMatch(e.Text);
	}
	
	private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		InputTextBox.Text = InputTextBox.Text.Replace(" ", "");
		
		HintTextBlock.Visibility = string.IsNullOrEmpty(InputTextBox.Text)
			? Visibility.Visible 
			: Visibility.Collapsed;
		
		_clickInterval =  string.IsNullOrEmpty(InputTextBox.Text) ? 0 : Convert.ToInt32(InputTextBox.Text);
	}

	private void ToggleClicker()
	{
		if (string.IsNullOrEmpty(InputTextBox.Text) || _clickInterval < 1) return;
		
		_active ^= true;
		ToggleButton.Content = _active ? "Stop" : "Start";
	}
	
	private void ClickerThread()
	{
		while (true)
		{
			if (!_active)
			{
				Thread.Sleep(200);
				continue;
			}

			ClickerService.MouseEvent(MouseEventFlags.LeftDown);
			Thread.Sleep(10);
			ClickerService.MouseEvent(MouseEventFlags.LeftUp);
			Thread.Sleep(_clickInterval);
		}
	}
}

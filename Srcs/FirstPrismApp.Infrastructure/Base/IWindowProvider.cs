using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace FirstPrismApp.Infrastructure.Base
{
	public interface IWindowProvider
	{
		IWindow Generate(string scope);
	}

	public interface IWindow
	{
		Window Owner { get; set; }
		event CancelEventHandler Closing;
		event RoutedEventHandler Loaded;
		string Title { get; set; }
		WindowStartupLocation WindowStartupLocation { get; set; }
		WindowStyle WindowStyle { get; set; }
		void Show();
		void Hide();
	}

	public interface IWindowMapper
	{
		void Map(string scope, Type windType);
		void UnMap(string scope);
		void Clear();
		Type Get(string scope);
	}
}

using System.Windows.Input;

namespace Core.Infrastructure.Base
{
	public interface ICommandable
	{
		ICommand Command { get; }

		object CommandParameter { get; set; }

		string InputGestureText { get; }
	}
}
using System.Windows.Input;

namespace Core.Infrastructure.Base
{
	public class AbstractCommandable : AbstractPrioritizedTree<AbstractCommandable>, ICommandable
	{
		#region CTOR

		public AbstractCommandable()
			: base()
		{
		}

		#endregion CTOR

		#region ICommandable

		public virtual ICommand Command { get; internal set; }

		public virtual object CommandParameter { get; set; }

		public string InputGestureText { get; internal set; }

		#endregion ICommandable
	}
}
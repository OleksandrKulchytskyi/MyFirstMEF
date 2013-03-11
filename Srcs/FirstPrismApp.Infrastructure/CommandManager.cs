using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace FirstPrismApp.Infrastructure
{
	public interface ICommandManager
	{
		bool RegisterCommand(string name, ICommand command);
		ICommand GetCommand(string name);
		void Refresh();
	}

	public sealed class CommandManager : ICommandManager
	{
		private Dictionary<string, ICommand> _commands;

		public CommandManager()
		{
			_commands = new Dictionary<string, ICommand>();
		}

		public bool RegisterCommand(string name, ICommand command)
		{
			if (_commands.ContainsKey(name))
				throw new Exception("Commmand " + name + " already exists !");

			_commands.Add(name, command);
			return true;
		}

		public ICommand GetCommand(string name)
		{
			if (_commands.ContainsKey(name))
				return _commands[name];
			return null;
		}


		public void Refresh()
		{
			foreach (KeyValuePair<string, ICommand> keyValuePair in _commands)
			{
				if (keyValuePair.Value is DelegateCommand)
				{
					DelegateCommand c = keyValuePair.Value as DelegateCommand;
					c.RaiseCanExecuteChanged();
				}
			}
		}
	}
}

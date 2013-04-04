using Core.Infrastructure.Base;
using Microsoft.Practices.Prism.Events;

namespace Core.Infrastructure.Events
{
	public class ThemeChangeEvent : CompositePresentationEvent<ITheme>
	{
	}
}
using FirstPrismApp.Infrastructure.Base;
using Microsoft.Practices.Prism.Events;

namespace FirstPrismApp.Infrastructure.Events
{
	public class ThemeChangeEvent : CompositePresentationEvent<ITheme>
	{
	}
}
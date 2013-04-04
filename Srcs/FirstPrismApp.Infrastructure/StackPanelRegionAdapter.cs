using Microsoft.Practices.Prism.Regions;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Core.Infrastructure
{
	public class StackPanelRegionAdapter : RegionAdapterBase<StackPanel>
	{
		public StackPanelRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{
		}

		protected override void Adapt(IRegion region, StackPanel regionTarget)
		{
			region.Views.CollectionChanged += (s, args) =>
			{
				if (args.Action == NotifyCollectionChangedAction.Add)
				{
					foreach (FrameworkElement element in args.NewItems)
					{
						regionTarget.Children.Add(element);
					}
				}

				//handle remove
				if (args.Action == NotifyCollectionChangedAction.Remove)
				{
					foreach (FrameworkElement element in args.OldItems)
					{
						regionTarget.Children.Remove(element);
					}
				}
			};
		}

		protected override IRegion CreateRegion()
		{
			return new AllActiveRegion();
		}
	}
}
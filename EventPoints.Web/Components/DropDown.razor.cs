using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace EventPoints.Web.Components
{
	public partial class DropDown<TItem>
	{
		[Parameter] public ObservableCollection<TItem> Items { get; set; } = new();
		[Parameter] public TItem? SelectedItem { get; set; }
		[Parameter] public EventCallback<TItem> SelectedItemChanged { get; set; }
		[Parameter] public Func<TItem, string> DisplaySelector { get; set; } = _ => string.Empty;

		private bool IsOpen;

		private void ToggleDropdown()
		{
			IsOpen = !IsOpen;
		}

		private async Task SelectItem(TItem item)
		{
			IsOpen = false;
			await SelectedItemChanged.InvokeAsync(item);
		}

		private string GetDisplayText(TItem? item)
		{
			if ( item == null ) return "Select an item";
			return DisplaySelector(item);
		}
	}
}
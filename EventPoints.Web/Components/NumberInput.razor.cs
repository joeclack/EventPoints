using Microsoft.AspNetCore.Components;

namespace EventPoints.Web.Components
{
	public partial class NumberInput
	{
		[Parameter] public int Value { get; set; }
		[Parameter] public EventCallback<int> ValueChanged { get; set; }
		[Parameter] public string? Placeholder { get; set; }
		private async Task OnValueChanged(ChangeEventArgs e)
		{
			if (int.TryParse(e.Value?.ToString(), out var newValue))
			{
				Value = newValue;
				await ValueChanged.InvokeAsync(Value);
			}
		}
	}
}

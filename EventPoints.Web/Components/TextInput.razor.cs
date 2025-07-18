using Microsoft.AspNetCore.Components;

namespace EventPoints.Web.Components
{
	public partial class TextInput
	{
		[Parameter] public string Value { get; set; }
		[Parameter] public EventCallback<string> ValueChanged { get; set; }
		[Parameter] public string? Placeholder { get; set; }
		private async Task OnValueChanged(ChangeEventArgs e)
		{
			Value = e.Value?.ToString();
			await ValueChanged.InvokeAsync(Value);
		}
	}
}

using Microsoft.AspNetCore.Components;

namespace EventPoints.Web.Components
{
	public partial class Button
	{
		[Parameter] public string Label { get; set; } = string.Empty;
		[Parameter] public EventCallback OnClick { get; set; }
	}
}

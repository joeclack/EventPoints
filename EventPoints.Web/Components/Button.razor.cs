using EventPoints.Web.Enums;
using Microsoft.AspNetCore.Components;

namespace EventPoints.Web.Components
{
	public partial class Button
	{
		[Parameter] public string Label { get; set; } = string.Empty;
		[Parameter] public EventCallback OnClick { get; set; }
		[Parameter] public ButtonType ButtonType { get; set; }
		public string ButtonClass => ButtonType switch
		{
			ButtonType.Primary => "button button-primary",
			ButtonType.Secondary => "button button-secondary",
			ButtonType.Delete => "button button-delete",
			_ => "button button-default"
		};
	}
}

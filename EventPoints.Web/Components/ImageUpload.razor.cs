using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventPoints.Web.Components
{
	public partial class ImageUpload
	{
		[Inject] private HttpClient Http { get; set; } = default!;
		[Parameter] public Guid TeamId { get; set; }
		[Parameter] public EventCallback OnUploadSuccess { get; set; }

		IBrowserFile uploadedFile;
		string previewUrl;
		string statusMessage;
		bool isError;

		async Task HandleDrop(InputFileChangeEventArgs e)
		{
			uploadedFile = e.File;
			var buffer = new byte[uploadedFile.Size];
			await uploadedFile.OpenReadStream().ReadAsync(buffer);

			previewUrl = $"data:{uploadedFile.ContentType};base64,{Convert.ToBase64String(buffer)}";

			var content = new MultipartFormDataContent();
			var fileContent = new StreamContent(uploadedFile.OpenReadStream(maxAllowedSize: 10_000_000));
			fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(uploadedFile.ContentType);

			content.Add(fileContent, "file", uploadedFile.Name);

			var response = await Http.PostAsync($"Events/teams/{TeamId}/upload-image", content);

			if ( response.IsSuccessStatusCode )
			{
				statusMessage = "Upload successful!";
				isError = false;
				await OnUploadSuccess.InvokeAsync();
			}
			else
			{
				statusMessage = "Upload failed.";
				isError = true;
			}
		}
	}
}
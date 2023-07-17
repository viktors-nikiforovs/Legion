using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ProgressHub : Hub
{
	public async Task SendProgress(int progress)
	{
		await Clients.All.SendAsync("ReceiveProgress", progress);
	}
}

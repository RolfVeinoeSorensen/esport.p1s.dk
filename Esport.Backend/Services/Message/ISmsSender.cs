namespace Esport.Backend.Services.Message
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}

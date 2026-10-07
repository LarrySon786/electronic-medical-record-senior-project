using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ElectronicMedicalRecord.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly MessageService _messageService;
    private readonly EmployeeService _employeeService;

    public ChatHub(MessageService messageService, EmployeeService employeeService)
    {
        _messageService = messageService;
        _employeeService = employeeService;
    }

    public async Task JoinChat(int chatId)
    {
        //Verify authenticate user
        var userId = Context.UserIdentifier ?? throw new HubException("User not authenticated.");

        //Fetch current employee
        Employee employee = await _employeeService.GetEmployeeByUserIdAsync(userId)
            ?? throw new HubException("Could not Join the Chat. Employee not found.");

        //Add employee to chat if not
        Chat? chat = await _messageService.UpdateChatAsync(chatId, employee.Id);

        if (chat != null)
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString()); //Connect to receive chat from Group "chatId" 
    }

    //Use to send message by employee
    public async Task SendMessageAsync(int chatId, string content)
    {
        //Verify authenticate user
        var userId = Context.UserIdentifier;

        if (userId == null) throw new HubException("User not authenticated.");

        //Fetch current employee
        Employee employee = await _employeeService.GetEmployeeByUserIdAsync(userId)
            ?? throw new HubException("Could not send message. Employee not found.");

        try
        {
            Message message = await _messageService.AddMessageAsync(
                chatId,
                employee.Id,
                content);

            await Clients
            .Group(chatId.ToString())
            .SendAsync(
                "ReceiveMessage",
                message.Id,
                message.ChatId,
                employee.Id,
                message.Content,
                message.SendAt);
        }
        catch (UnauthorizedAccessException)
        {
            throw new HubException(
                "You don't have access to this chat.");
        }
    }

    //Use to create chat
    public async Task<int> CreateChat(string message)
    {
        //Verify authenticate user
        var userId = Context.UserIdentifier;

        if (userId == null) throw new HubException("User not authenticated.");

        //Fetch current employee
        Employee employee = await _employeeService.GetEmployeeByUserIdAsync(userId)
            ?? throw new HubException("Could not send message. Employee not found.");

        //Create chat
        Chat chat = await _messageService.CreateNewChatAsync(employee.Id, message);

        // Tell every connected employee to refresh their chat list
        await Clients.All.SendAsync("ChatCreated");

        return chat.Id;
    }
}
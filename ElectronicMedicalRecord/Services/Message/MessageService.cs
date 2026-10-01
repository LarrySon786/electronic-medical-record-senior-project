using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services;

public class MessageService
{
    private readonly DbContextFactoryHelper _context;

    public MessageService(DbContextFactoryHelper context)
    {
        _context = context;
    }

    // Get All messages by Chat Id
    public async Task<List<Message>> GetMessageByChatIdAsync(int chatId, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            return await MessageQuery(db)
                 .Where(m => m.ChatId == chatId)
                 .ToListAsync();
        }, context);
    }

    //Add Message to chat
    public async Task<Message> AddMessageAsync(int chatId, int employeeId, string content, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            var chat = await db.ChatDb
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat == null)
                throw new InvalidOperationException("Chat not found.");

            if (!chat.ParticipantIds.Contains(employeeId))
                throw new UnauthorizedAccessException(
                    "Employee is not a participant of this chat.");

            var message = new Message
            {
                ChatId = chatId,
                EmployeeId = employeeId,
                Content = content
            };

            db.MessageDb.Add(message);

            await db.SaveChangesAsync();

            return message;

        }, context);

    }

    //Get Chat by  Id
    public async Task<Chat?> GetChatByIdAsync(int chatId, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            return await ChatQuery(db)
                 .Where(c => c.Id == chatId)
                 .FirstOrDefaultAsync();
        }, context);
    }

    //Get All Chats by User Id
    public async Task<List<Chat>> GetChatByUserIdAsync(int employeeId, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
         {
             return await ChatQuery(db)
                  .Where(c => c.ParticipantIds.Contains(employeeId))
                  .ToListAsync();
         }, context);
    }


    //Create new chat
    public async Task<Chat> CreateNewChatAsync(int employeeId, string content, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            Chat chat = new()
            {
                ParticipantIds = new List<int>
                {
                    employeeId
                }
            };

            Message message = new()
            {
                EmployeeId = employeeId,
                Content = content,
                Chat = chat
            };

            chat.Messages.Add(message);

            // Add new Chat to Datbase, Save Changes, Return new entity
            db.ChatDb.Add(chat);
            await db.SaveChangesAsync();

            return chat;

        }, context);
    }

    //Add new users to an existing chat
    public async Task<Chat?> UpdateChatAsync(int chatId, int employeeId, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            //check existing chat
            Chat? chat = await db.ChatDb
            .FirstOrDefaultAsync(c => c.Id == chatId) ?? throw new InvalidOperationException("Chat not found.");

            if (!chat.ParticipantIds.Contains(employeeId))
            {
                chat.ParticipantIds.Add(employeeId);
                await db.SaveChangesAsync();
            }

            return chat;

        }, context);
    }

    private IQueryable<Message> MessageQuery(ProjectDatabaseConnection context)
    {
        return context.MessageDb.Include(m => m.Employee);
    }

    private IQueryable<Chat> ChatQuery(ProjectDatabaseConnection context)
    {
        return context.ChatDb.Include(c => c.Messages);
    }
}
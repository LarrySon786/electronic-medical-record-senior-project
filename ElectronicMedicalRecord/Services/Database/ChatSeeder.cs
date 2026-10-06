using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services.Database;

public class ChatSeeder
{
    // Responsible for seeding chat and message data.
    // Must run AFTER EmployeeSeeder, because every message needs an existing sender.
    public async Task SeedChatsAsync(ProjectDatabaseConnection context)
    {
        // Avoid duplicates: do nothing if chats already exist in the database
        if (await context.ChatDb.AnyAsync())
            return;

        // Fetch Json file data 
        var file = await File.ReadAllTextAsync("JSON/ChatSeed.json");
        var chatDefinitions = JsonSerializer.Deserialize<List<Chat>>(file, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        });

        if (chatDefinitions is null || chatDefinitions.Count == 0)
            return;


        // Add all chats; EF also inserts their messages through the Messages collection
        context.ChatDb.AddRange(chatDefinitions);
        await context.SaveChangesAsync();
    }
}
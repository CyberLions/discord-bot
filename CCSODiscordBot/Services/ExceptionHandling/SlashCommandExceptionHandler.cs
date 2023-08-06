using System;
using Discord;
using Discord.Interactions;
using Zitadel.Api;

namespace CCSODiscordBot.Services.ExceptionHandling
{
	public class SlashCommandExceptionHandler
	{
        public static Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, Discord.Interactions.IResult arg3)
        {
            // Return if successful:
            if (arg3.IsSuccess)
            {
                return Task.CompletedTask;
            }

            // Defer if not already done:
            try
            {
                arg2.Interaction.DeferAsync(true).GetAwaiter().GetResult();
            }
            catch(System.InvalidOperationException e)
            when(e.Source!=null && e.Source.Equals("Discord.Net.WebSocket"))
            {
                // Ignore. Already defered.
            }

            // Write error to console:
            Console.WriteLine("Error: " + arg3.Error);

            switch (arg3.Error)
            {
                case InteractionCommandError.UnmetPrecondition:
                    // Check for userperm error:
                    if (arg3.ErrorReason.Contains("UserPerm"))
                    {
                        arg2.Interaction.FollowupAsync("You do not have permission to execute this command.", ephemeral: true);
                        break;
                    }
                    arg2.Interaction.FollowupAsync("Command Failed\n" + arg3.ErrorReason, ephemeral: true);
                    break;
                case InteractionCommandError.UnknownCommand:
                    arg2.Interaction.FollowupAsync("Unknown command. It may have been recently removed or changed.", ephemeral: true);
                    break;
                case InteractionCommandError.BadArgs:
                    arg2.Interaction.FollowupAsync("The provided values are invalid. (BadArgs)", ephemeral: true);
                    break;
                case InteractionCommandError.Exception:
                    //notify owner if desired:
                    if (arg3.ErrorReason.Contains("Invalid Form Body"))
                    {
                        arg2.Interaction.FollowupAsync("Invalid form body. Please check to ensure that all of your parameters are correct.", ephemeral: true);
                        break;
                    }
                    arg2.Interaction.FollowupAsync("Sorry, Something went wrong...", ephemeral: true);
                    break;
                default:
                    arg2.Interaction.FollowupAsync("Sorry, Something went wrong...");
                    break;
            }

            return Task.CompletedTask;
        }
    }
}


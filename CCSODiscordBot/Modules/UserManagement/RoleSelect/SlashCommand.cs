using System;
using CCSODiscordBot.Services.Database.Repository;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace CCSODiscordBot.Modules.UserManagement.RoleSelect
{
    public class SlashCommand : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly IGuildRepository _iGuildRepository;
        public SlashCommand(IGuildRepository guildRepository)
        {
            _iGuildRepository = guildRepository;
        }

        [SlashCommand("roleselection", "Add embed for role selection.")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task RoleSelection([Summary(description: "The channel to post the role embed in.")] SocketTextChannel channel)
        {
            await Context.Interaction.DeferAsync(true);

            var dbGuild = await _iGuildRepository.GetByDiscordIdAsync(Context.Guild.Id);
            await channel.SendMessageAsync(embed: RolePrompt.RolePromptEmbeds.Embeds(true, dbGuild.ClassStandings, dbGuild.InterestRoles).Build(), components: RolePrompt.RolePromptComponents.BtnComponent(true, dbGuild.ClassStandings, dbGuild.InterestRoles).Build());

            await Context.Interaction.FollowupAsync("Embed created!", ephemeral: true);
        }
    }
}


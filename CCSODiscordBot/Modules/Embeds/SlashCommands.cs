using System;
using CCSODiscordBot.Modules.Embeds.Modals;
using Discord;
using Discord.Interactions;

namespace CCSODiscordBot.Modules.Embeds
{
    public class SlashCommands : InteractionModuleBase<ShardedInteractionContext>
    {
        [SlashCommand("create_embed", "Creates an embed")]
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task Command()
        {
            await Context.Interaction.RespondWithModalAsync<EmbedCreator>("embed_creator");
        }
    }
}


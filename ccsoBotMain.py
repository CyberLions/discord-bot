import discord
from discord import webhook
import ccsoBotReactions
import ccsoBotCreds
import ccsoBotCool
import requests
import random
from discord.ext import tasks, commands


# Main: Handles main bot code and links everything together. Will use commands to run, start, and stop features.

# ------------------------------------------------------------------------------------------------------------


# Intents library required for the on_raw_reaction_add function
intents = discord.Intents.default()
intents.reactions = True
client = discord.Client(intents=intents)

# ------------------------------------- Bot commands / events -------------------------------------
# ------------------------------------- 1) Reaction Role Message -------------------------------------
# Takes an emoji and returns a role based on the input, default case is None, this return is handled in on_raw__reaction_add()

# moved to ccsoreactions!

@client.event
async def on_ready():
    # What do you want the bot to do at login?
    print('We have logged in as {0.user}'.format(client))

# ------------------------------------- 2) Youtube Parsing/Messaging -------------------------------------

# Checks for new content and posts


# ------------------------------------- 3) Role Reactions -------------------------------------

@client.event
async def on_raw_reaction_add(payload):
    # Defines the message, reaction, user, and role variables

    # Waits for a specific message to be reacted to, then adds a user to a specific role
    await ccsoBotReactions.addRoleReaction(payload)



# ------------------------------------- 4) Bot Commands -------------------------------------

@client.event
async def on_message(message):
    if message.author == client.user:
        return

    # if the message !rules is sent, it will send the rules message into #rules ADD: check for server admin role
    elif message.content.startswith("!rules"):
        print(message.author.roles)
        for role in message.author.roles:
            if role.name == "Server Manager":
                rulesChannel = discord.utils.get(
                    client.get_all_channels(), name="rules-test")
                await rulesChannel.purge()
                rulesVar = discord.Embed(
                    title="CCSO Server Rules", description="")
                rulesVar.add_field(
                    name="Rules", value="Feature still in testing", inline=False)
                await rulesChannel.send(embed=rulesVar)

    # This command purges the roles channel and sends the message to react to
    elif message.content == "!embedRoles":
        for role in message.author.roles:
            if role.name == "Server Manager":
                await ccsoBotReactions.embedRoleMessage()

    # pop smoke command
    elif message.content == "!pop":
        await ccsoBotCool.youCannotSayPopAndForgetTheSmoke(message)


# ------------------------------------------------------------------------------------------------------------

# ------------------------------------------------------------------------------------------------------------

def bootstrapping():

    # Initial log in and check from the bot
    # grab Discord API token from creds.py file
    token = ccsoBotCreds.getDiscordKey()
    gcpKey = ccsoBotCreds.getGCPKey()

    ccsoBotReactions.setClientToken(client)
    '''
    roleChanId = discord.utils.get(client.get_all_channels(), name="role-selection")
    print(roleChanId)
    ccsoBotReactions.setRoleChannelId(roleChanId)
    '''
    # starts the content scheduler
    print("Bootstrapping done")
    client.run(token)


bootstrapping()

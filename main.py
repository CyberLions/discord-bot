import discord
import time

from discord import webhook
import reactionBot
import creds
import ContentScheduler
import requests

# Main: Handles main bot code and links everything together. Will use commands to run, start, and stop features. 

# ------------------------------------------------------------------------------------------------------------

# Emojis for roles
emojis = {
    '👍',
    '👀',
    '🍉',
    '<:coolspot:854115885013663774>'
}

# Intents library required for the on_raw_reaction_add function
intents = discord.Intents.default()
intents.reactions = True

client = discord.Client(intents=intents)

# ------------------------------------- Bot commands / events -------------------------------------
# ------------------------------------- 1) Reaction Role Message -------------------------------------
# Takes an emoji and returns a role based on the input, default case is None, this return is handled in on_raw__reaction_add()

@client.event
async def on_ready():
    print('We have logged in as {0.user}'.format(client))

    #channel and message id of msg to react to for roles 
    channel_id = 852925086238900225
    message_id = 852925086238900225

    message = await client.get_channel(channel_id).fetch_message(message_id)

    # Adds the emoji reactions to the message initially
    for emoji in emojis:
        await message.add_reaction(emoji)

# ------------------------------------- 2) Youtube Parsing/Messaging -------------------------------------
def grabContent():
    gcpKey = creds.getGCPKey()
    print(gcpKey)

    videos = ContentScheduler.do_something(gcpKey)
    print(videos)

    video = videos[0]

    # now, to send the message 
    # REMOVE THIS CODE AND REPLACE W DISCORD API OR HIGHER LEVEL FUNC FOR MESSAGING 

    webhookUrl = creds.getWebhookUrl()

    params = {'username': 'webhook-test',
            'avatar_url': "", 'content': "New video from {}!".format(video['channelTitle']), }
    params["embeds"] = [
        {
            'image': {
                "url": video['thumbnail'],
            }, 
            "description": video['description'],
            "title": video['title'],
            "url": video['url'],
            "color" : 4, #colors: https://gist.github.com/thomasbnt/b6f455e2c7d743b796917fa3c205f812
            "author": {
                "name": video['channelTitle'],
                "url": video['channelId'],
            }
        }   
    ]

    try:
        requests.post(webhookUrl, json=params)

    except:
        print("request error")

    

# ------------------------------------- Talk to josh abt. modularizing these -------------------------------------

def rolePicker(string, message):
    role1 = discord.utils.get(message.guild.roles, name="test role 1")
    role2 = discord.utils.get(message.guild.roles, name="balls")
    role3 = discord.utils.get(message.guild.roles, name="super")
    role4 = discord.utils.get(message.guild.roles, name="server-manager")

    switcher = {
        '👍': role1,
        '👀': role2,
        '🍉': role3,
        '<:coolspot:854115885013663774>': role4
    }
    return switcher.get(string, None)

# Waits for a specific message to be reacted to, then adds a user to a specific role


@client.event
async def on_raw_reaction_add(payload):
    # Defines the message, reaction, user, and role variables
    message = await client.get_channel(payload.channel_id).fetch_message(payload.message_id)
    message_channel = client.get_channel(payload.channel_id)

    emoji = str(payload.emoji)
    user = payload.member
    userId = payload.user_id
    print(emoji, user)

    # Check to make sure the user isnt the bot, so the bot doesn't react to the message then immeadietly remove the react
    if userId != client.user.id:
        # Check to see if the react is in the correct channel so it doesnt trigger in other channels
        if discord.utils.get(client.get_all_channels(), name="reactions-channel") == message_channel:
            try:
                # Gets the role based on the emoji and adds it to the user w/ a print
                role = rolePicker(emoji, message)

                if role in user.roles:
                    await user.remove_roles(role, message)
                    print("removed role " + str(role))
                else:
                    await user.add_roles(role, message)
                    print("added role " + str(role))

            # Error handling
            except AttributeError:
                print("role does not exist")
            except discord.errors.NotFound:
                print("tried to add role user already has")
                
            await message.remove_reaction(emoji, user)


@client.event
async def on_message(message):
    if message.author == client.user:
        return
    elif message.content == "test":
        print("recognize command")
        embedVar = discord.Embed(
            title="balls in my face title", description="awesome description")
        embedVar.add_field(
            name="Field1", value="balls in my face 1", inline=False)
        embedVar.add_field(
            name="Field2", value="balls in my face 2", inline=True)
        await message.channel.send(embed=embedVar)
    
    # if the message !rules is sent, it will send the rules message into #rules ADD: check for server admin role 
    elif message.content.startswith("!rules"):
        print(message.author.roles)
        for role in message.author.roles:
            if role.name == "server-manager":
                rulesChannel = discord.utils.get(client.get_all_channels(), name="rules")
                await rulesChannel.purge()
                rulesVar = discord.Embed(
                    title="CCSO Server Rules", description="")
                rulesVar.add_field(
                    name="Rules", value="I have a ginormous colkc and \nalso my balls are ginorumes", inline=False)
                await rulesChannel.send(embed=rulesVar)

    # best cmmand
    elif message.content == "balls in my face":
        counter = 0
        while (True):
            await message.channel.send(message.content)
            time.sleep(1)
            counter += 1
            if counter > 10:
                return False

# ------------------------------------------------------------------------------------------------------------

def bootstrapping(): 

    # Initial log in and check from the bot
    #grab Discord API token from creds.py file 
    token = creds.getDiscordKey()

    grabContent()

    client.run(token)

bootstrapping()
import discord
import os
import time
import reactionBot

# Intents library required for the on_raw_reaction_add function
intents=discord.Intents.default()
intents.reactions=True

# Emojis for roles
emojis = {
        '👍',
        '👀',
        '🍉'
    }


# Initial log in and check from the bot
client = discord.Client(intents=intents)
@client.event
async def on_ready():
    print('We have logged in as {0.user}'.format(client))

    message = await client.get_channel(852925086238900225).fetch_message(852925110939287624)

    # Adds the emoji reactions to the message initially 
    for emoji in emojis:            
        await message.add_reaction(emoji)

# Takes an emoji and returns a role based on the input, default case is None, this return is handled in on_raw__reaction_add()
def rolePicker(string, message):
    role1 = discord.utils.get(message.guild.roles, name="test role 1")
    role2 = discord.utils.get(message.guild.roles, name="balls")
    role3 = discord.utils.get(message.guild.roles, name="super")

    switcher={
        '👍':role1,
        '👀':role2,
        '🍉':role3
    }
    return switcher.get(string, None)
   
# Waits for a specific message to be reacted to, then adds a user to a specific role 
@client.event
async def on_raw_reaction_add(payload):
    # Defines the message, reaction, user, and role variables
    message = await client.get_channel(payload.channel_id).fetch_message(payload.message_id)

    emoji = str(payload.emoji)
    user = payload.member
    userId = payload.user_id
    print(emoji, user)

    # Check to make sure the user isnt the bot, so the bot doesn't react to the message then immeadietly remove the react    
    if userId != client.user.id:    
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

        await message.remove_reaction(emoji,user)
        


@client.event
async def on_message(message):
    if message.author == client.user:
        return
    if message.content == "test":
        print("recognize command")
        embedVar = discord.Embed(title="balls in my face title", description="awesome description")
        embedVar.add_field(name="Field1", value="balls in my face 1", inline=False)
        embedVar.add_field(name="Field2", value="balls in my face 2", inline=False)
        await message.channel.send(embed=embedVar)


client.run('TOKEN')

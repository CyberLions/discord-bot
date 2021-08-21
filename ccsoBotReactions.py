import discord
import os
import time

# Intents library required for the on_raw_reaction_add function
intents = discord.Intents.default()
intents.reactions = True

client = discord.Client()


def sendClientToken(token):
    print("client set!")
    global client
    client = token
    # sets global var pog


# Emojis for roles
emojis = {
    '👍',
    '👀',
    '🍉',
    '<:coolspot:854115885013663774>',
    '🏳️',
    '⚔️',
    '🛡️',
    '🎮',
    '⚪',
    '🔵',
    '🟤',
    '⚫',

}

staticRoles = [
    '1️⃣',
    '2️⃣',
    '3️⃣',
    '4️⃣',
    '⭐'
]

dynamicRoles = [
    '⚔️',
    '🛡️',
    '🏳️',
    '🎮',
]


def getRoleEmojis():
    return staticRoles + dynamicRoles


def rolePicker(string, message):
    role1 = discord.utils.get(message.guild.roles, name="test role 1")
    role2 = discord.utils.get(message.guild.roles, name="balls")
    role3 = discord.utils.get(message.guild.roles, name="super")
    role4 = discord.utils.get(message.guild.roles, name="server-manager"),

    ctfRole = discord.utils.get(message.guild.roles, name="CTF")
    cptcRole = discord.utils.get(message.guild.roles, name="Offense (CPTC)")
    ccdcRole = discord.utils.get(message.guild.roles, name="Defense (CCDC)")
    gamingRole = discord.utils.get(message.guild.roles, name="Gaming")

    firstYear = discord.utils.get(message.guild.roles, name="Freshman")
    secondYear = discord.utils.get(message.guild.roles, name="Sophomore")
    thirdYear = discord.utils.get(message.guild.roles, name="Junior")
    fourthYear = discord.utils.get(message.guild.roles, name="Senior")
    alumni = discord.utils.get(message.guild.roles, name="Alumni")


    switcher = {
        '👍': role1,
        '👀': role2,
        '🍉': role3,
        '<:coolspot:854115885013663774>': role4,
        '🏳️': ctfRole,
        '⚔️': cptcRole,
        '🛡️': ccdcRole,
        '🎮': gamingRole,

        # '⚪': firstYear,
        # '🔵': secondYear,
        # '🟤': thirdYear,
        # '⚫': fourthYear,
        '1️⃣': firstYear,
        '2️⃣': secondYear,
        '3️⃣': thirdYear,
        '4️⃣': fourthYear,
        '⭐': alumni
    }
    return switcher.get(string, None)


async def addReactionsToMessage(staticMessage, dynamicMessage):

    # Adds the emoji reactions to the message initially
    for emoji in emojis:
        # await message.add_reaction(emoji)
        print("")

    # to be in order
    # this doesn't work since add_reaction is a coroutine
    # staticRoles = ['1️⃣',
    #                '2️⃣',
    #                '3️⃣',
    #                '4️⃣',
    #                '⭐']

    # staticMessage.add_reaction(staticRoles[0])
    # staticMessage.add_reaction(staticRoles[1])
    # staticMessage.add_reaction(staticRoles[2])
    # staticMessage.add_reaction(staticRoles[3])
    # staticMessage.add_reaction(staticRoles[4])

    for role in staticRoles:
        await staticMessage.add_reaction(role)

    for role in dynamicRoles:
        await dynamicMessage.add_reaction(role)


async def addRoleReaction(payload, a):

    # client = a

    print("sssss")
    print(payload.channel_id)
    print(client)
    print(client.get_channel(payload.channel_id))

    message = await client.get_channel(payload.channel_id).fetch_message(payload.message_id)
    message_channel = client.get_channel(payload.channel_id)

    emoji = str(payload.emoji)
    user = payload.member
    userId = payload.user_id
    print(emoji, user)

    # Check to make sure the user isnt the bot, so the bot doesn't react to the message then immeadietly remove the react
    if userId != client.user.id:
        # Check to see if the react is in the correct channel so it doesnt trigger in other channels
        # REPLACE THIS CHANNEL NAME AS NEEDED
        if discord.utils.get(client.get_all_channels(), name="bot-react-post-test") == message_channel:
            try:
                # Gets the role based on the emoji and adds it to the user w/ a print
                role = rolePicker(emoji, message)

                if role in user.roles:
                    await user.remove_roles(role, message)
                    print("removed role " + str(role))
                else:
                    # check if its static or dynamic
                    # if dynamic, remove other dynamic roles before adding

                    for staticRole in staticRoles:

                        if emoji == staticRole:

                            firstYear = discord.utils.get(
                                message.guild.roles, name="Freshman")
                            secondYear = discord.utils.get(
                                message.guild.roles, name="Sophomore")
                            thirdYear = discord.utils.get(
                                message.guild.roles, name="Junior")
                            fourthYear = discord.utils.get(
                                message.guild.roles, name="Senior")
                            alumni = discord.utils.get(
                                message.guild.roles, name="Alumni")
                            rolesToRemove = [
                                firstYear, secondYear, thirdYear, fourthYear, alumni]
                            roleToKeep = rolePicker(emoji, message)

                            rolesToRemove.remove(roleToKeep)
                            for toRemove in rolesToRemove:
                                await user.remove_roles(toRemove)

                    await user.add_roles(role, message)
                    print("added role " + str(role))

            # Error handling
            except AttributeError as err:
                print(err)
                print("role does not exist")
            except discord.errors.NotFound:
                print("tried to add role user already has")

            await message.remove_reaction(emoji, user)


class React:
    def __init__(self, balls):
        self.balls = balls

        # Takes an emoji and returns a role based on the input, default case is None, this return is handled in on_raw__reaction_add()

    def rolePicker(self, string, message):
        role1 = discord.utils.get(message.guild.roles, name="test role 1")
        role2 = discord.utils.get(message.guild.roles, name="balls")
        role3 = discord.utils.get(message.guild.roles, name="super")

        switcher = {
            '👍': role1,
            '👀': role2,
            '🍉': role3
        }
        return switcher.get(string, None)

    # Waits for a specific message to be reacted to, then adds a user to a specific role
    @client.event
    async def on_raw_reaction_add(self, payload):
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
                role = self.rolePicker(emoji, message)

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

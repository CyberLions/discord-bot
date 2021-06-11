import discord
import os
import time

client = discord.Client()

@client.event
async def on_ready():
    print('We have logged in as {0.user}'.format(client))

@client.event
async def on_message(message):
    if message.author == client.user:
        return

    embedVar = discord.Embed(title="balls in my face title", description="awesome description")
    embedVar.add_field(name="Field1", value="balls in my face 1", inline=False)
    embedVar.add_field(name="Field2", value="balls in my face 2", inline=False)
    await message.channel.send(embed=embedVar)


client.run('TOKEN')

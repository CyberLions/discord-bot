'''
Content and time: Links the content features and has the main timing logic for posting articles. 
Timing logic will have a boolean feature within so that it can be stopped if it isn’t working from a command. 

Goal is to have the bot check for articles and videos regularly (2 hour spans between 8 AM and 10 PM, stop at night)
'''

"""
    This class needs: 

    -a timer that checks for new content every two hours 
    -interoperability with articleParser and videoParser
"""

from datetime import datetime
import botCreds
import discord
from discord.ext import tasks, commands
checking_for_content = False

"""
    Method that handles the time-scheduling for checking new content.
    Checks for new content and posts
    https://discordpy.readthedocs.io/en/latest/ext/tasks/
"""
# change the interval using this
@tasks.loop(hours=2)
async def scheduler():
    # toggle
    global checking_for_content
    if(checking_for_content == False):
        print('Starting content check!')
        checking_for_content = True
    else:
        print('Stopping content checks...')
        checking_for_content = False

    # Are we looking?
    if(checking_for_content == False):
        # do nothing
        print("Not currently looking for content - perhaps it is off hours, or the scheulder was toggled off?")
    else:
        # Check if we are within the bounds of time (8am-10pm)
        current_hour = datetime.now().hour
        if(current_hour >= 8 and current_hour < 22):
            # If we are in the current bounds of time...
            print("we live!")
            # CALL THE METHODS THAT CHECK FOR CONTENT 
            # article
            # video

        else:
            # It's too early or late. 
            print("outside of bot content checking hours")

        


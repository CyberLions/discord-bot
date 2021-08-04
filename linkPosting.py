# goal: to be able to send new youtube videos from certain channels
# from an RSS feed, to the server via webhooks

# get channels
# for each channel, see if there are new videos
# if there are new videos, post them.

import os
import time
import sched
import urllib
import json
from datetime import datetime
import requests
# for http reqs for webhooks

# npr music
# https://www.youtube.com/channel/UC4eYXhJI4-7wSWc8UNRwD4A

# gets the current time to check for videos uploaded after the current time
# checks if new vids have been uploaded in the past hour

api_key = os.environ.get('GCP_KEY')

def getTime():
    # datetime object containing current date and time
    now = datetime.now()

    # replaces with an hour before
    currHour = now.hour
    now = now.replace(hour=currHour-2)

    dt_string = now.strftime("%Y/%m/%d %H:%M:%S")
    print("date and time =", dt_string)

    # good code
    dtSplit = dt_string.split(".")[0]
    dtRFC = dtSplit.replace(" ", "T")
    dtRFC = dtRFC.replace("/", "-")
    dtRFC = dtRFC + "Z"
    print(dtRFC)

    return dtRFC

def get_all_video_in_channel(channel_id):

    base_video_url = 'https://www.youtube.com/watch?v='
    base_search_url = 'https://www.googleapis.com/youtube/v3/search?'
    channel_base_url = 'https://www.youtube.com/channel/'

    # RFC formatted time
    # 1970-01-01T00:00:00Z
    # time = '2021-06-16T00:00:00Z'
    time = '2021-08-01T00:00:00Z'
    # time = getTime()

    first_url = base_search_url + \
        'key={}&channelId={}&publishedAfter={}&order=date&maxResults=10&part=snippet'.format(
            api_key, channel_id, time)

    # adds videos (if returned) to a list
    videos = []
    url = first_url
    while True:
        inp = urllib.request.urlopen(url, timeout=1)
        resp = json.load(inp)

        for i in resp['items']:
            if i['id']['kind'] == "youtube#video":

                # print(i['snippet'])

                video = {
                    'title': i['snippet']['title'],
                    'url': base_video_url + i['id']['videoId'],
                    'description': i['snippet']['description'],
                    'thumbnail': i['snippet']['thumbnails']['high']['url'],
                    'channelId': channel_base_url + i['snippet']['channelId'],
                    'channelTitle': i['snippet']['channelTitle'],
                }

                videos.append(video)
                # print(i['snippet']['thumbnails']['high']['url'])

        try:
            next_page_token = resp['nextPageToken']
            url = first_url + '&pageToken={}'.format(next_page_token)
        except:
            break

    return videos

def sendMessage(video):
    # https://gist.github.com/Bilka2/5dd2ca2b6e9f3573e0c2defe5d3031b2#file-webhook-py-L13
    # https://birdie0.github.io/discord-webhooks-guide/discord_webhook.html

    webhookUrl = "https://discord.com/api/webhooks/853734898526322738/M9XXwr_UxCf-otst4ZyeNiXkeehTVr8woQDnUBV3ThYZ3XRYUjq9VjSp2gtLG6Y8SRp8"
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
        # requests.post(webhookUrl, params)
        requests.post(webhookUrl, json=params)

    except:
        print("request error")

# Single Running ------------------------------------

channel_to_get = 'UC4eYXhJI4-7wSWc8UNRwD4A' 
# NPR music 

links = get_all_video_in_channel(channel_to_get)
vidTest = links[0]
sendMessage(vidTest)

print(os.environ.get('PYTHON_TEST'))

# Repeated running ------------------------------------

# s = sched.scheduler(time.time, time.sleep)

# def do_something(sc):
#     print("Sending videos!...")

#     links = get_all_video_in_channel('UC4eYXhJI4-7wSWc8UNRwD4A')

#     for link in links:
#         sendMessage(link)

#     s.enter(15, 1, do_something, (sc,))

# s.enter(15, 1, do_something, (s,))

# s.run()

# TODOS:
# when do we get timers? start 8am, check every two hours until 10pm

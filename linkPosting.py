# webhook url
# https://discord.com/api/webhooks/853734898526322738/M9XXwr_UxCf-otst4ZyeNiXkeehTVr8woQDnUBV3ThYZ3XRYUjq9VjSp2gtLG6Y8SRp8

# goal: to be able to send new youtube videos from certain channels
# from an RSS feed, to the server via webhooks

# get channels
# for each channel, see if there are new videos
# if there are new videos, post them.

import urllib
import json
import discord
import requests
# for http reqs for webhooks


# npr music
# https://www.youtube.com/channel/UC4eYXhJI4-7wSWc8UNRwD4A

from datetime import datetime

# gets the current time to check for videos uploaded after the current time
# checks if new vids have been uploaded in the past hour

# GCP key
api_key = 'AIzaSyAhwdqhmemItC802nSKAgBVHezeldhlOYs'
client = discord.Client()
disc_key = "ODUyNzI0MjcxOTk3Nzc5OTg4.YMK_XQ.2KDpTkD5VeZ7xXIUiy7OffciHlI"


def getTime():
    # datetime object containing current date and time
    now = datetime.now()

    # replaces with an hour before
    currHour = now.hour
    now = now.replace(hour=currHour-1)

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

    # RFC formatted time
    # 1970-01-01T00:00:00Z
    time = '2021-06-16T00:00:00Z'
    # time = getTime()

    first_url = base_search_url + \
        'key={}&channelId={}&publishedAfter={}&order=date&maxResults=25'.format(
            api_key, channel_id, time)

    # adds videos (if returned) to a list
    video_links = []
    url = first_url
    while True:
        inp = urllib.request.urlopen(url,timeout=1)
        resp = json.load(inp)

        for i in resp['items']:
            if i['id']['kind'] == "youtube#video":
                video_links.append(base_video_url + i['id']['videoId'])

        try:
            next_page_token = resp['nextPageToken']
            url = first_url + '&pageToken={}'.format(next_page_token)
        except:
            break
     
    return video_links


def sendMessage(firstLink):
# https://gist.github.com/Bilka2/5dd2ca2b6e9f3573e0c2defe5d3031b2#file-webhook-py-L13

    # what is sent in the message
    # content = "eugene goated"
    content = "new video just dropped! \n{}".format(firstLink)

    #https://birdie0.github.io/discord-webhooks-guide/discord_webhook.html

    webhookUrl = "https://discord.com/api/webhooks/853734898526322738/M9XXwr_UxCf-otst4ZyeNiXkeehTVr8woQDnUBV3ThYZ3XRYUjq9VjSp2gtLG6Y8SRp8"
    params = {'username': 'webhook-test',
              'avatar_url': "", 'content': "", }
    params["embeds"] = [
        {
            "description": firstLink,
            "title": "Title",
            "url": firstLink, 
        }
    ]

    try:
        # requests.post(webhookUrl, params)
        requests.post(webhookUrl, json = params)

    except:
        print("request error")


# sample http request
# response = requests.get(
#     'https://api.github.com/search/repositories',
#      params={'q': 'requests+language:python'},
#      headers={'Accept': 'application/vnd.github.v3.text-match+json'},
# )
links = get_all_video_in_channel('UC4eYXhJI4-7wSWc8UNRwD4A')
vidTest = links[0]
# links = "blergh"

sendMessage(vidTest)

# client.run(disc_key)

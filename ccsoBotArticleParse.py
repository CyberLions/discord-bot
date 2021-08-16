import feedparser
from dhooks import Embed, Webhook

hook = Webhook("https://discord.com/api/webhooks/854120969223012352/tH_ksryZu32e8ZorRj4kCkAUW3pULvGkWmOHHjOoM5VmR6DfSsYIisE2tU-X8IcDEtmt")

link = "https://www.darkreading.com/rss_simple.asp?f_n=644&f_ln=Attacks/Breaches"

dReadArticle = feedparser.parse(link)

#Test out print stuff

print(len(dReadArticle.entries))

enNum = 0

dReadEmbedArticle = Embed(

    title= dReadArticle.entries[enNum].title,
    image_url= 'https://img.deusm.com/darkreading/dr_staff_125x125.jpg', #Can't find image within feed or entries for Dark reading
    url= dReadArticle.entries[enNum].link,
    description= dReadArticle.entries[enNum].summary,
    color= 1 #colors: https://gist.github.com/thomasbnt/b6f455e2c7d743b796917fa3c205f812
    
)


#Have to add footer and author afterwards (read API doc)

dReadEmbedArticle.set_footer(text= dReadArticle.entries[enNum].published)
dReadEmbedArticle.set_author(name= dReadArticle.feed.title)
dReadEmbedArticle.add_field(name= "Author", value= dReadArticle.entries[enNum].author, inline= True)

#https://feedparser.readthedocs.io/en/latest/common-rss-elements.html (API Doc) 

def postArticle(Article):

    hook.send(embed=Article)

postArticle(dReadEmbedArticle)

# VPN Request Module
This module makes an API request to an API that issues user credentials to log into a VPN for a lab.
## Slash Commands
- /setvpnrequest
    - By default, only admins can use this command.
    - This command should be provided a URL endpoint to post data to as well as an API token to authenticate to the server with.
    - Set by guild
- /requestvpn
    - This command can be used by anyone with send message permission (by default).
    - Will send the user's information to the VPN API to create an account.
    - Will only work if the user is verified and the server has the VPN request API set up.
    - Use `/setvpnrequest` to set the VPN API up

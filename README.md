# Phantom Stealth
Niall Nulty 18/04/2024

## Brief Project Overview
The purpose of this project is to integrate the backend platform brainCloud into a simple Unity client. The project itself will be a single player stealth game where the objective is to make it to the end goal of the level. The level will have a time limit and a path to follow for the player to make it to their goal. The player will have to avoid detection from an enemy while making their way to the goal. 

The player can play the game without registering i.e. anonymously. Some generic player data, such as last login or login count etc. will still be sent to brainCloud without full registration, by using a unique identifier, but features will be limited. With brainCloud, it is possible for a user to be set up anonymously and then for the user to later register and link their anonymous brainCloud data via a unique identifier. The anonymous player will be given the option to register upon level completion.

To register, players need to supply a username and password. This information will be securely stored in the brainCloud backend. The game will use cloud only saves, meaning no save data is stored locally. The game is unplayable without an internet connection. When players complete a level, their score will be added to the backend database and will be viewable in a leaderboard within the game if their score is in the top 5 scores in the leaderboard. If a player has already completed a level, their score will only be updated in brainCloud if it is greater than their current highest score. 

The game will also implement a custom feature called “Ghost Path”. This will allow the players to see a “ghost” of a previous run of the level, similar to time trials in many racing games. When a player completes the level, they will be prompted if they want to store the path. It is stored in a User / Player Entity in brainCloud. If a user chooses to “Store path” it will overwrite the existing path they have stored in their User / Player Entity, if one exists. The data is then pulled down if the player chooses ”Ghost Path” and a transparent player character will be seen following the player's path pulled down from brainCloud. If there is no data for the player path in brainCloud, the playerGhost object will be set to inactive


## Features 
- Anonymous Authentication
- Universal Authentication
- Updating a Username
- Posting Score To a Leaderboard
- Getting Scores From a Leaderboard
- Creating an Entity
- Updating an Entity

## How To Demo
### Client
- Download the latest release found in the [releases](https://github.com/NiallNulty/PhantomStealth/releases) section.
- Extract the folder and run PhantomStealth.exe
- There is a controls menu that can be selected in the main menu of the game.
- To Demo Anonymous Authentication, press the Play (Anonymous) button. If the authentication fails, you will remain in the main menu. Once you complete the level as an anonymous user, you will be prompted to register (Universal Authentication). If you chose to register at this stage, and the request is successful, your score will automatically be posted to the Leaderboard. You will also be given the option to "Store Path" (Update an Entity).
- To Demo Universal Authentication, press the Play (Login/Register) button. If you enter the username and password of an existing user you will be logged in. If you enter a new username and password, a new user will be registered. If the authentication fails, you will remain in the main menu. When the level starts, the username should be visible at the bottom of the screen. Upon level completion, the user score will be automatically posted to the Leaderboard. You will also be given the option to "Store Path" (Update an Entity).
- To Demo Updating a Username, press the "Update Username Button". You need to enter your existing username, your existing password & the new username you wish to choose. If the existing username and password don't match an existing user, no new account will be registered at this stage. If you are successful with updating the username, you should be able to login with the credentials in the Login/Register section mentioned previously.
- To Demo the Leaderboards, press the "Leaderboard" button in the main menu. By default, this uses anonymous authentication, as the user is always logged out when navigating the main menu. If authentication is successful, the top 5 scores in the leaderboard should be shown.

### Backend
#### Log Files
Each request is outputted to a log (txt) file while the game is running. This will be in a folder called "Logs" in the same directory as the game exe. This will allow you to monitor
- If the user is authenticated
- If any requests are successful
- If any requests fail
- Output JSON data returned from certain requests
- Output JSON data generated for certain requests

#### brainCloud Dashboard
- You can view the users in the User Browser
- Selecting a user and pressing "Go To User" will allow you to view their profile 
- When a user is selected, you can also select Users > Data > User Entities to view their Ghost Path if one exists
- You can view the Leaderboards by selecting Global > Leaderboards > Leaderboards and selecting "Main" from the dropdown

## What I found easy
Anonymous and Universal Authentication were relatively easy to implement. The brainCloud Dashboard is also very easy to use so you know the implemented changes are working.

## What I found difficult
Implementing User Entities was probably the most difficult, as that requires requesting an EnitityID before Updating an Entity.

## Skills Learned
- Post and Pull data from brainCloud
- How to navigate brainCloud Dashboards
- Project Planning and Time Management (doing project in free time / estimating delivery date and meeting the deadline)

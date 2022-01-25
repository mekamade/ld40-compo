# U Snooze, U Booze : Ludem Dare 40 Compo Submission

## What is Ludem Dare?
Ludum Dare is an online event where games are made from scratch in a weekend. There are two categories, Jam and Compo. You can read more about it [here](https://ldjam.com/events/ludum-dare/rules). 

## About the game
This game was made for Ludem Dare 40 - Compo, which was held between December 2nd - 3rd, 2017. The topic for the game-jam was:

 > "The more you have, the worse it is."
 
## Game Design:
In order to comply with the Ludem Dare Compo requirements, here is a list of everything that went into building this game:
- All the functionality and scripts are written in C# on the Unity Engine. 
- All the artwork, sprites and graphics are made from ground up on Asperite.
- All the sound effects in the game are vocally recorded by myself.
- All the background music sequences are generated using Wolfram Tones.
- The two fonts used are 'La Tequila' and 'Acknowledge', which are free to use for non-commercial purposes.
- The entire game's files were made within a discontinuous span of 60-70 hours including development. (Greater than 48 hours, as it was a failed submission).

## Screenshots
<p float="left">
  <img src="https://img.itch.zone/aW1hZ2UvMjE1NDU0LzEwMTk2NzgucG5n/250x600/8IGyIQ.png" width="240" />
  <img src="https://img.itch.zone/aW1hZ2UvMjE1NDU0LzEwMTk2NzkucG5n/250x600/4Hyzzi.png" width="240" /> 
  <img src="https://img.itch.zone/aW1hZ2UvMjE1NDU0LzEwMTk2ODAucG5n/250x600/Fu2ROT.png" width="240" />
  <img src="https://img.itch.zone/aW1hZ2UvMjE1NDU0LzEwMTk2ODEucG5n/250x600/9hkFUS.png" width="240" />
</p>

## How-to-Play:

> I suggest the players to play the game multiple number of times to understand how the game works. Although not perfect, I have attempted to make it understandable, as you keep playing it.

- The game contains a conveyer belt that brings drinks to the user. 
- There are two types of beverages:
  - **Alcoholic:** Beer Pint, Beer Bottle, Vodka Shot, Whiskey, Martini, Margarita and Wine
  - **Non-Alcoholic:** Water, Cola and an Energy Drink
- There is a status panel on the left side, which contains three main components:
  - **Directional Sequence:** A sequence of 4 directions that get updated for every new drink
  - **Timer Bar:** A blue countdown timer bar above the directional sequence that counts down to end of every drink round
  - **Logic Light:** The light to the left of the directional sequence & the timer bar. The logic light could be either 'Green' or 'Red'.  It is suppose to be reflective of how an individual fights to convince/manipulate themselves when inebriated and are crippled with poor judgement.
- The objective of the game is to survive as long as possible. 
- Consuming alcoholic drinks increases the 'Booze-o-Meter' value. Consuming non-alcoholic drinks can help reduce it. 
- The following actions need to be performed, based on the condition (Drink Type, Logic Light):

| Drink Type    | Logic Light | Desired Action                                           | Desired Outcome    | Undesired Action                                   | Undesired Outcome  |
|---------------|-------------|----------------------------------------------------------|--------------------|----------------------------------------------------|--------------------|
| Alcoholic     | Green       | Input the Directional Sequence before the Timer runs out | Skips the drink    | Failing to properly input the directional sequence | Consumes the drink |
| Non-Alcoholic | Green       | Don't input any key                                      | Consumes the drink | Making even a single input                         | Skips the drink    |
| Alcoholic     | Red         | Don't input any key                                      | Skips the drink    | Making even a single input                         | Consumes the drink |
| Non-Alcoholic | Red         | Input the Directional Sequence before the Timer runs out | Consumes the drink | Failing to properly input the directional sequence | Skips the drink    |

## Controls:
- Directional Inputs (Up, Left, Down, Right): W, A, S, D (or) Arrow Keys.
- Pause Game: Escape

## Failed LudemDare Submission:
This was my first attempt in building a game for LudemDare, and also my first fully playable standalone game, ever. Unfortunately, I didn't manage to finish the project within the deadline of the game-jam due to various reasons including, Unity Engine crashing and losing 4 hours of work. I've recently finished my planned game for the LudemDare submission. Let's hope I get better at planning and estimation in game building for the next LudemDare and also, always use (local, atleast) source control. :P


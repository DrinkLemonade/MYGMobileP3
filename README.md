# Connecte-Moi Ça (MYG Mobile Projet 3)
"Connecte-Moi Ça" ("CMC" for short) is a logic game designed for mobile platforms. The game's goal is to find the logic connecting the variety of words shown on the screen. Each puzzle consists of 4 categories, each containing 4 words. The player must think carefully due to their limited number of attempts, and use their general knowledge and logical association skills in order to solve each puzzle.

## Trello
![Trello screenshot](https://github.com/user-attachments/assets/b0f265c2-7817-4803-b8ca-faa917814dbd)

## Original block-out mockups on Figma
![Connecte Moi Ca Original Mockup](https://github.com/DrinkLemonade/MYGMobileP3/assets/117670511/5330badd-a29d-4c67-8cd7-a75262fb5354)

## UML Diagrams
# Old Diagram
UML diagram used to plan the project in advance.
![Connections FR UML](https://github.com/DrinkLemonade/MYGMobileP3/assets/117670511/a183d760-e8a1-4142-86c4-075df2678d7d)
# New Diagram
UML diagram reflecting the current state of the project.
![Connecte Moi Ca UML v2](https://github.com/user-attachments/assets/d8738b78-ddc7-4bb8-af61-540dccca06c8)

# Documentation
DocFX-generated documentation is available at https://drinklemonade.github.io/
See below for more information on classes central to the game's proper operation.

## GameManager
The GameManager singleton covers transition between game states, handling events such as changes in music when the game switches to the Start state, or creating and holding a GameSession object.

## GameSession
A GameSession is an ongoing game of CMC. The class's constructor takes in information regarding which words and categories are in play. This class is responsible for handling game logic: evaluating the player's word guesses, rewarding or punishing the player when a guess is correct or incorrect, keeping track of previously-attempted guesses, calling the hint UI when the player is only missing one word or repeating a previous guess, and finally calling the victory/defeat UI when conditions are met.

## PlayFabLogin
When the game enters its main menu, this classes attempts to establish a connection to the PlayFab server which holds the game's level data. If successful, this level data is downloaded in JSON format, which is then sent to the MainMenu class in order to be parsed and processed.

## MainMenu
MainMenu is rezsponsible for transitions between sub-menus (the start screen and the puzzle list, for instance). It parses retrieved JSON level data and transforms it into a list of GameLevel structs, which inform the button grid that makes up the puzzle list sub-menu.

## UIManager
UIManager handles the main gameplay scene's UI logic. It obtains the current puzzle information from GameManager's ongoing GameSession and arranges the puzzle's various words into a grid. Various methods are responsible for calling the coroutines used to animate the game's UI, such as GridAnimation. Logic also exists to check the player's interaction with the UI, for instance allowing the submission of a guess attempt only if the attempts contains no more and no less than 4 words. Methods to enable the game's victory and defeat UI panel also exist for GameSession to call when the conditions are met.

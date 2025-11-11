# NYTGames
A place with all of your favorite puzzle games are held. In seige week 10 I recreated NYTGames's Wordle in Unity.

Process of making:
Coming up with the idea
   -
    - This was a hard process that took me a while to think about/talk to chatgpt
    - Then it hit me when doing NYT's daily puzzles
    - They are all grid based
    - So i came up with the idea was to recreate as many of their puzzles that I could.
Then I setup my Unity project
   -
     - Made a new Github repo and cloned it on my device
     - I created a new Unity 2D project, saved within the repo
     - Then setup my file system with all of the needed folders to keep myself organized
         - Scenes
         - Scripts
         - Prefabs
         - Sprites
         - UI
Creating my window system
   -
     - For this project I wanted to design around growth
     - So I made a window system that makes it really easy to add new puzzles
     - To start I made the Window interface that all new windows would be based on
     - Then made the Winodw script which extends IWindow (The name of the Window Interface file)
     - Then made the Window Manager scritp which makes it easy to switch between windows.
         - With a method to easily use to call with a button call
Setting up my scene
   -
     - I made a new canvas to hold the different game/puzzle windows
     - Then made a new game object to hold the Window manager script
     - Made my the Main menu window with button that call the scene manager to switch to a specified window(ex. Wordle)
     - I put this button in a parent game object with a grid layout to make all of the buttons alligned (this ended up not being needed because I only had time for making wordle)
Setting up Wordle
   -
     - I made a new Window called Wordle Window
     - Created a new Grid Layout Group to hold all of the boxes that you can type into(answer boxes)
     - Then I setup my Tile script and put this into a Tile prefab
     - This script controls what color the tile should be.
     - After that i made a the Wordle Window script
       - This controls typing into the game
       - Checking if the word added is right
       - Checking the state of the tiles and changing them
       - Showing the play again button
     - Maing the WordBank script to control the choosing of a randow word with the word bank I gave it

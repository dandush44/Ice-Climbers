# Ice Climbers

A 2D endless typing-based climbing game. The player climbs an icy mountain by typing the word shown on the next platform and submitting it. A correct answer moves the climber to the next platform, while an incorrect answer causes the player to fall and ends the run.

The game becomes faster as the player progresses, rewarding quick and accurate typing. The goal is to climb as many platforms as possible and achieve a new high score.


---

## Requirements

| | |
|---|---|
| Unity | **6000.3.20f1** (Unity 6.3 LTS) |
| Game type | 2D |
| Render pipeline | Built-in |
| Main systems | Physics2D, Unity UI, TextMesh Pro |
| Targets | Windows standalone, Android |

## How to run

1. Open the project root folder with Unity Hub using editor version `6000.3.20f1`.
2. Open `Assets/Scenes/Menu.unity`.
3. Press Play.
4. Press the Play button in the Main Menu to start the game.

The build settings contain two scenes:

- `Assets/Scenes/Menu.unity`
- `Assets/Scenes/Game.unity`

## Controls

| Action | Windows | Android |
|---|---|---|
| Type the target word | Physical keyboard | On-screen keyboard |
| Submit word / climb | Enter | Mobile keyboard submit / Enter |
| Pause / Resume | On-screen Pause button | Touch Pause button |
| Retry | Retry button | Touch Retry button |
| Return to menu | Menu button | Touch Menu button |

## Gameplay

- The player begins on an ice platform.
- The next platform displays a target word.
- The player types the word into the input field and submits it.
- A correct answer increases the score and platform count, then moves the player toward the next platform.
- New platforms are generated as the player continues climbing.
- The scrolling speed increases as the run progresses.
- An incorrect submitted word causes the player to fall and ends the run.
- The best score is stored locally using `PlayerPrefs`.

### Android gameplay note

The Android version is intentionally more forgiving and gives the player a few extra seconds before failure, because typing with a mobile on-screen keyboard is slower and more difficult than typing with a physical PC keyboard. The Android build also uses mobile-specific camera framing so the game remains comfortable and readable on a phone screen.

---

## Project structure

```text
Assets/
  Animations/     Player, platform and UI animation clips/controllers
  Music/          Background music
  Prefabs/        Platform prefab
  Resources/      data.csv — word list used by the game
  Scenes/
    Menu.unity    Main menu
    Game.unity    Main gameplay scene
  Scripts/
    CameraScroll.cs
    FollowPlayer.cs
    GameManager.cs
    MenuManager.cs
    ParallaxVertical.cs
    ParallexEffext.cs
    PlayerController.cs
    SpriteText.cs
  Sounds/         Correct, wrong and failure sound effects
  Sprites/        Player, platforms, backgrounds and UI artwork
  TextMesh Pro/   TextMesh Pro resources

Packages/          Unity package configuration
ProjectSettings/   Unity project and editor settings
GDD.md             Game Design Document
README.md          Project overview and run instructions
```

## Technical design

### `GameManager`

`GameManager` controls the main gameplay loop, including:

- loading and shuffling words from `Assets/Resources/data.csv`
- validating submitted words
- scoring and platform progress
- creating new platforms
- difficulty progression
- pause and Game Over behaviour
- saving the local high score

### Coroutine

`GameManager.WaitForJump()` is a coroutine used during a successful jump. It temporarily disables the word input field for `0.75` seconds while the player transitions to the next platform, then restores input for the next word.

### Camera and progression

`CameraScroll` controls the continuous scrolling speed and increases the pace as the player progresses.

`FollowPlayer` follows the player and contains Android-specific camera settings using `UNITY_ANDROID`, allowing separate camera size and offset values for the mobile build.

### Player

`PlayerController` handles failure-related player behaviour and detects collision with the failure boundary.

### Local high score

The best score is stored with Unity `PlayerPrefs`, allowing it to remain available between game sessions on the same device.

### Words

The game loads its word list from:

`Assets/Resources/data.csv`

The words are shuffled during gameplay and reused after the full list has been completed.

---

## Art & Audio

The project contains:

- player idle, jump and fall animations
- platform animations
- multiple frozen-background sprites
- background music
- sound effects for correct answers, incorrect answers and failure
- UI sprites and menu elements

Visual and audio assets are located under `Assets/Sprites`, `Assets/Animations`, `Assets/Music`, and `Assets/Sounds`.

---

## Documentation

- [Game Design Document](GDD.md)

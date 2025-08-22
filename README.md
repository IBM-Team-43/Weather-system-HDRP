IBM Team 43

# **Dynamic Weather System**

## ([GitHub](https://github.com/IBM-Team-43/Weather-system-HDRP))

<img width="512" height="288" alt="image" src="https://github.com/user-attachments/assets/00892951-df73-4ac6-9001-946370f1cc7c" />
<img width="512" height="286" alt="image" src="https://github.com/user-attachments/assets/1f74ab1e-fe3a-486c-853e-3c24f07069d5" />


# **Introduction**

This project presents the design and implementation of a **Dynamic Weather System**, developed to simulate a range of weather conditions such as **sunny**, **cloudy**, **rainy**, **foggy**, **snowy**, **dust storms**, and **thunderstorms**. The system transitions between these states based on predefined seasonal probabilities and in-game time, offering an ever-changing environment that responds both visually and functionally.

**The system supports:**

* **Season-based weather probabilities**, where different seasons favor different weather types.  
* **Real-time weather transitions**, with smooth interpolation of visual and audio effects.  
* **Modular design**, allowing easy extension or integration into larger simulation/game projects (Game Clock, Weather API).  
* **Environmental influence**, affecting lighting, ambient sounds, fog, and visual effects.

The goal of this system is not only to enhance realism and aesthetics but also to serve as a **base architecture** for further gameplay interactivity—such as influencing crop growth in a farming game, NPC behavior in survival games, or visual storytelling in narrative-driven experiences.

# **Objectives**

The primary objective of this project is to design and implement a **Dynamic Weather System** that enhances realism, immersion, and interactivity in a virtual environment by simulating varied atmospheric conditions over time. The system is intended to be modular, scalable, and seasonally adaptive, serving as a foundational component for larger game or simulation projects.

### **Specific Objectives:**

1. **Simulate Multiple Weather Conditions:**  
   Developed realistic and visually distinct weather effects including sunny, cloudy, rainy, foggy, snowy, dust storm, and thunderstorm conditions.  
2. **Implement Seasonal Weather Probabilities:**  
   Define and integrate a season-based probability that affects which weather conditions are more likely to occur in different seasons (e.g., snow in winter, rain in monsoon).  
3. **Enable Real-Time Transitions Between Weathers:**  
   Implemented smooth transitions between weather states using blending techniques for visual and audio elements.  
4. **Create a Modular Architecture:**  
   Structured the system in a way that allows easy integration into any Unity project and supports future extensibility (e.g., new weather types, regional climates).  
1) **Sync Weather with In-Game Time System:**  
   Integrate the weather logic with a time progression system to simulate daily and seasonal cycles dynamically.  
2) **Enable Real-World Weather Integration via API Calls:**  
   Include support for fetching live weather data using an external weather API, allowing the in-game weather to reflect real-time atmospheric conditions.  
5. **Support Game Integration Use Cases:**  
   1. Crop System: Growth of crops affected by weather conditions.  
   2. Other items affected by weather: Containers, Windmill.

# **Tech Stack (Dependencies):**

* **Engine Version**: Unity 6000.0.36f1+(High Definition Render Pipeline)  
* **Programming Language**: C\# , [Rider](https://www.jetbrains.com/rider/)(IDE by JetBrains)  
* **Third-Party APIs**: [OpenWeatherMap API](https://openweathermap.org) (for real-time weather integration), [IP Geolocation API](https://ip-api.com/) (longitude,latitude)  
* **Other Tools and libraries**: [Astronomy Engine (C\#)](https://github.com/cosinekitty/astronomy) , [PrimeTween](https://github.com/KyryloKuzyk/PrimeTween), Unity Shader Graph, VFX Graph, [Game CI](https://game.ci/docs/github/builder/#complete-example) (Build automation in GitHub).

# **System Architecture(Weather System):**

<img width="512" height="370" alt="image" src="https://github.com/user-attachments/assets/3880d352-0b4a-473a-98c9-d12ca1fb1441" />


| Dashed Line(----): Inheritance | Solid Line(—): Usage |
| :---- | :---- |

# **Implementation:**

## **WeatherBase (Abstract Base Class):**

* **Purpose and Responsibilities:**

  This is an abstract class that serves as a base class for all the different weather classes that must be played from the weather manager. Includes all the basic setup that might be required for every weather.

  This allows different developers to work on separate weather types without knowing about each other or the weather manager.

* **Core Structure:**

  Implemented by each weather child class

* protected abstract void StartWeather();  
* protected abstract void StopWeather();

  Called by weather manager prevents playing of duplicate weather and calls above two functions

* public void EnableWeather()  
* Public void DisableWeather()

  Representation of WeatherType using enum

public enum WeatherType{

**Sunny, Rainy, Snow, Thunder, Fog, DustStorm, Cloudy** }

## **WeatherManager:**

* **Purpose and Responsibilities:**  
   This class is responsible for managing and switching between different weather types during runtime. It coordinates with `WeatherBase` instances to enable or disable specific weather effects and applies corresponding environmental settings such as cloud presets, wind direction, and sun position.  
   It also integrates with the `SunCalculator` to simulate accurate time-of-day lighting changes and handles volumetric cloud transitions using `HDRP` and `PrimeTween` for smooth blending.  
* **Core Structure:**  
  	**Weather Switching:**  
  * `SwitchWeather(WeatherType weather)` checks if the desired weather is different from the current one, disables the current weather, and enables the selected weather by calling its `EnableWeather()` method.  
  * Applies predefined HDRP volumetric cloud presets (`Sparse`, `Overcast`, `Stormy`, etc.) using the `ApplyCloudPresets` method.

  **Time Control:**

  * `SetTime(DateTime time)` updates the `SunCalculator` to simulate different times of day.  
  * `SetTime(DateTime time, Vector2 loc)` additionally updates sun positioning based on geographic coordinates.

  **Cloud Preset Application:**

  * Uses a nested `CloudSettings` struct to store volumetric cloud parameters such as density, erosion, altitude, and curves.  
  * Supports initializing from existing cloud settings or from predefined `CloudPresets`.  
  * `LerpCloudSettings` uses `PrimeTween` to smoothly interpolate cloud values over a given duration, allowing natural weather transitions.

  **Integration Points:**

  * Works in conjunction with `WeatherBase` subclasses to apply specific effects like rain, snow, fog, etc.  
  * Central control point for both visual and environmental weather parameters, ensuring consistent blending between weather states.  
  * Invokes an event on weather change which can be linked with any other system of the application

## **Weathers Implementation**

* ### **DustStorm**

  **FogSettings Struct**: Encapsulates HDRP fog parameters and supports smooth tweened interpolation (`LerpFogSettings`) of both numeric and color properties using PrimeTween, enabling seamless transitions in fog appearance during weather changes.

  **Global VolumeProfile Handling**: Retrieves or dynamically adds the `Fog` component to the shared volume profile at runtime, ensuring the class can modify fog settings even if not preset in the volume.

  **Cached Fog Settings**: Stores the original fog settings before applying dust storm effects, allowing restoration on weather stop or disable, preventing permanent environmental changes.

  **Serialized FogProfile**: Allows defining custom fog settings externally via `fogVolume`, which are then loaded and tweened into the active scene fog settings for flexible configuration without code changes.

  **Fade Duration Control**: Uses a configurable `fadeDuration` to control the length of fog parameter transitions, improving visual smoothness when enabling or disabling the dust storm.  
  **VFX Graph**: Though not manipulated directly in the shown code, the serialized dust particle system is prepared for integration with the fog effects, hinting at coordinated visual effects during the dust storm.

* ### **Rainy**

  **Particle System Control**: Manages a `ParticleSystem` for rain with dynamic control over emission rate and max particles, enabling smooth start/stop transitions by tweening these parameters over a configurable `transitionDuration`.

  **Direction and Velocity Updates**: Supports changing rain direction (rotation) and velocity range at runtime with value clamping, ensuring physical rain behavior can be adjusted dynamically.

  **Shader Opacity Integration**: Interfaces with a ground object's material shader by modifying an `_opacity` property to visually blend rain effects on surfaces, synchronized with rain intensity for cohesive visuals.

  **Audio Management**: Controls rain sound playback with volume fading linked optionally to rain intensity. Includes setup for audio source if not assigned and supports runtime swapping of rain sound clips.

  **Smooth Transitions**: Uses coroutines (`GraduallyStartRain` and `GraduallyStopRain`) combined with an ease-in-out function for natural interpolation of rain intensity, shader opacity, and audio volume during weather changes.

  **State Tracking**: Maintains boolean flags (`isRaining`, `isTransitioning`) to prevent overlapping transitions and ensure correct state management.

  **Public Properties:** Exposes current rain state, intensity, shader opacity, and rain direction for external querying or UI binding.

  **Safety Measures**: Ensures cursor remains visible/unlocked during gameplay, preventing UI interaction issues caused by the rain controller's updates.

  **Extensibility**: Provides methods for external modification of rain sound and manual shader opacity control, allowing integration with UI or other systems beyond the weather framework.

* ### **Snowy**

   **Purpose**: Controls snow accumulation effect via a global shader property (`_SnowLevel`) and snow particle system.

   **Snow Level**: Gradually increases/decreases `snowLevel` value to simulate snow covering/retreating, updated every frame and synced with shader.

   **Particle System**: Controls emission rate dynamically and fades particles in with a coroutine for smooth start.

   **Day/Night Cycle**: Toggles skybox tint color between day and night using coroutine-based color interpolation.

  **Overrides**: `StartWeather()` initiates snow with gradual buildup and particle play; `StopWeather()` stops particles and snow accumulation.

* ### **ThunderStorm** 

* ### **Foggy** 

* ## **Sunny**

* ## **Cloudy**

  ### (Above weathers uses extended fields of weather base and particle system)

## **WeatherGenerator (Abstract Base Class)**

* Defines a UnityEvent `onWeatherChanged` which broadcasts the current weather type to listeners mostly only the weather manager.  
* Acts as a base for any weather generation system, enforcing a standard event-driven interface for weather updates.  
* The project includes two implementations of this class one uses seasons and other uses weather api.

## **Api Weather Generator**

* **Purpose:** Fetches real-world weather data asynchronously using the OpenWeatherMap API based on either a specified city or the user's geolocation.  
* **API Key Handling:** Uses a serialized `ApiKey` ScriptableObject to securely store and access the API key(hidden from github).  
* **Location Fetching:**  
  * Supports toggling between a fixed city or automatic location detection via IP geolocation (`ipinfo.io`).  
  * Uses `UnityWebRequest` with async-await for non-blocking HTTP calls.  
  * Parses IP geolocation response JSON to extract latitude and longitude.  
* **Weather Fetching:**  
  * Queries OpenWeatherMap API for current weather JSON.  
  * Parses JSON into strongly typed data classes (`WeatherData`, `WeatherInfo`, `Wind`).  
  * Maps API weather description strings to the internal `WeatherType` enum via `MapToWeatherType()`.  
  * Invokes `onWeatherChanged` event with the determined weather.

* **Time Updates:**  
  * Invokes `onTimeChanged` event with current system time and resolved location coordinates for other systems (e.g., sun positioning).

## **Daily Weather Generator**

* **Purpose:** Generates weather internally based on simulated seasonal cycles rather than real-world data.  
* **Seasonal Probability Setup:**  
  * Stores an array mapping each season to its corresponding `SeasonWeatherData`, which contains weighted probabilities for weather types.  
* **Integration with SeasonalClock:**  
  * Listens for day changes via `Update()` by tracking `clock.currentDayOfYear`.  
  * When a new day starts, triggers `GenerateDailyWeather()`.  
* **Weather Generation Logic:**  
  * Uses weighted random sampling on current season's weather probabilities to select today's weather.  
  * Invokes `onWeatherChanged` event when the weather changes.  
* **Utility:**  
  * `ResetWeather()` method for testing or debugging to force a specific weather.  
* **Robustness:**  
  * Provides fallback default weather if no weighted probabilities match.

## **SeasonalClock Class Code Structure:**

* **Simulated Time Progression:** Advances an internal `DateTime` by scaling Unity's `Time.deltaTime` to simulate a full 24-hour day within a customizable `dayLengthInSeconds`. This allows speeding up or slowing down time in the game world.

* **Time Component Sync:** Extracts and updates individual date and time fields (`year`, `month`, `day`, `hour`, `minute`, `second`, `dayOfYear`) from the internal `DateTime` each frame for easy access and serialization.  
* **Season Calculation:** Determines the current season based on `currentDayOfYear` and user-configurable `customSeasons` ranges, supporting seasons that wrap across year boundaries (e.g., winter spanning end and start of year).  
* **Sun Integration:** Optionally references a `Light` with a `SunCalculator` component to synchronize sun position and lighting with simulated time (setup in `Awake`).  
* **Event Notification:** Raises a UnityEvent `onTimeChanged` every frame with the current `DateTime`, allowing other systems (e.g., weather, sun position) to react to time progression.  
* **Robust Design:** Uses careful logic to handle season ranges that span calendar year boundaries, and initializes with user-specified start date/time components for flexible starting points.  
* **Lightweight Data Structs:** Defines serializable structs for season ranges and an enum for seasons, making it easy to configure and extend the seasonal system via the Unity Inspector.

## **SeasonWeatherData ScriptableObject:**

* **Purpose:**  
   Defines weighted probabilities for different weather types in a given season, used by weather generation systems to randomly select daily weather based on configured likelihoods.  
* **Data Storage:**  
   Contains a `List<WeatherWeight>`, each pairing a `WeatherType` enum value with a float weight between 0 and 1 representing that weather's chance for the season.  
* **Flexibility & Extensibility:**  
   By relying on enums and serialized lists, it’s easy to add new weather types or modify probabilities without code changes, making the system modular and designer-friendly.  
* **Integration:**  
   Typically assigned to a season inside `DailyWeatherGenerator` to drive the probabilistic selection of weather each day based on current season.

# GitHub Copilot Instructions for Unity (Latest Version)

## Purpose
These instructions guide Copilot to generate code compatible with the latest stable release of Unity.

## General Guidelines
- Use C# as the programming language.
- Target Unity 2022 LTS or newer.
- Use Unity's built-in APIs and recommended practices.
- Prefer `MonoBehaviour` for scripts attached to GameObjects.
- Use `SerializeField` for private fields that need to be visible in the Inspector.
- Avoid deprecated Unity APIs.
- Use Unity's new Input System for input handling.
- Prefer `UnityEngine.SceneManagement` for scene loading.
- Use `async`/`await` for asynchronous operations where appropriate.
- Include comments for clarity when generating complex code.

## Example Prompts
- "Create a Unity script for player movement using the new Input System."
- "Generate a MonoBehaviour that spawns objects at random positions."
- "Write a Unity C# script to save and load player data using JSON."

## Restrictions
- Do not generate code using obsolete Unity APIs.
- Do not use third-party packages unless explicitly requested.
- Do not generate editor scripts unless specified.

## References
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/)
- [Unity Manual](https://docs.unity3d.com/Manual/index.html)
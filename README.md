# Fury

Fury is a 2D platform game developed using Unity. This repository contains the Unity project along with CI/CD pipelines that automatically build and deploy versions for Android and the web.

## Versions

- **Android Version**: Built automatically via CI/CD for mobile deployment.
- **Web Version**: Built and deployed automatically. Playable online at [https://erickclarivet.github.io/Fury/](https://erickclarivet.github.io/Fury/). The game is playable on both computer browsers and mobile devices, with controls choosable in the game menu.

## Getting Started

To work on the project:

1. Ensure you have Unity installed (compatible version as specified in ProjectVersion.txt).
2. Clone this repository.
3. Open the project in Unity by selecting the root folder.

## CI/CD

The project includes continuous integration and deployment pipelines that:
- Build the Android APK.
- Build and deploy the web version to GitHub Pages.

For more details on the CI/CD setup, check the workflow files in `.github/workflows/`.

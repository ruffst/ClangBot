# ClangBot README

## Overview

ClangBot is a desktop application designed to communicate with the OpenAI API to answer user queries inputted into a file named `chat.txt`. The responses are saved in a `Response.txt` file. The bot has been configured with a specific character in the OpenAI API - it behaves as a sarcastic deity with technical knowledge about Space Engineers.

## Prerequisites

1. .NET Framework or .NET Core (depending on the version used).
2. A valid OpenAI API key saved in the `api.key` file.

## Application Features

- **Start/Stop Button**: This button allows you to start and stop the bot.
- **OpenAI Connection**: The bot connects to the OpenAI API and awaits questions from the `chat.txt` file.
- **File Monitoring**: The bot continuously monitors the `chat.txt` file for new entries. If a new entry is found containing "Klang" or "Clang", a request is sent to the OpenAI API.
- **Responses**: Responses from OpenAI are displayed in both the GUI window and the `Response.txt` file.

## How to Use

1. Launch the application.
2. Load the OpenAI API key by directly entering it into the designated text box or by saving it in a file named `api.key` in the application directory.
3. Click the start button to initiate the bot. The bot will now start monitoring the `chat.txt` file.
4. To stop the bot, click the stop button.

## Notes

- The application expects the `chat.txt` file to be located in the same directory as the executable.
- The application overwrites the content of `chat.txt` after a question has been processed.
- The application writes responses into the `Response.txt` file, also located in the same directory as the executable.

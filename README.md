# SpotifySongDownloader

Spotify Downloader is a lightweight Desktop program that downloads Spotify playlist songs without the need to provide any registration or login information.
It was built using Wpf, .Net 6.0 and Visual Studio 2022.

## How it Works:

We all know that its not possible to download songs from the spotify API without having a premium account, so the idea was to use the spotify api to fetch the song metadata only, and then use youtube to download them.

1. Gets a spotify public token from the spotify api.

2. Fetches playlist and songs metadata using the playlist link provided.

3. Searches for the songs on youtube using the Name of the song and its length.

4. Downloads the song using the best stream possible (Highest bitrate) in .webm format.

5. Converts the song to the .mp3 format and embeds the song cover with file.

## Screenshots:

![image](https://user-images.githubusercontent.com/63502859/181786212-7db8eeac-dfea-4905-b31b-5b41104a1c71.png)

![image](https://user-images.githubusercontent.com/63502859/181786928-a0574b54-557a-4a65-95ba-5f9aa01a296b.png)

![image](https://user-images.githubusercontent.com/63502859/181787357-ab1a1ffe-ac49-4db5-9223-ca5584120260.png)

## Requirements
To build and run this application, you will need:

- Visual Studio 2019 or later
- .NET 6.0 or later

## Future Work
- Add support for downloading albums.
- Bug fixes.

## License
This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details

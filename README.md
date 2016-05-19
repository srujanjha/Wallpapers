# Wallpapers
<br>A simple utility desktop application to download daily Wallpapers from various sources: Bing (IN, US, CN, JP, AU, UK, DE, NZ, CA), NASA and NGC.
<br>The application is developed using Visual Studio and runs on .NET framework. The primary language used for the project is C#.

<b>Requirements for Setup:</b>
- OS: Windows<br>
- The package is bundled with .NET framework, and if your computer doesn't have .NET framework pre-installed, the package shall make an attempt to do so.<br>
  
<b>Instructions to Run:</b><br>
- Run the Setup.exe in the Publish Folder<br>
- Follow the instructions and the application will be installed.<br>
  
<b>Requirements for running the code:</b><br>
- OS: Windows 10 or Windows 8.1<br>
- Visual Studio 2012+<br>
  
<b>Instrcutions to Run:</b><br>
- Open the Wallpapers.sln.<br>
- Build the code in Visual Studio and Run.<br>

#<b>Bing Image</b><br>
URL: http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-US<br>
- As the format parameter indicates, this will return an XML formatted stream that contains information such as the date the images is for, the relative URL, description, and copyright information.<br>
- The idx parameter tells where you want to start from. 0 would start at the current day, 1 the previous day, 2 the day after that, etc. For instance, if the date were 1/30/2011, using idx = 0, the file would start with 20110130; using idx = 1, it would start with 20110129; and so forth.<br>
- The n parameter tells how many images to return. n = 1 would return only one, n = 2 would return two, and so on.<br>
- The mkt parameter tells which of the eight markets Bing is available for you would like images from. The valid values are: en-US, zh-CN, ja-JP, en-AU, en-UK, de-DE, en-NZ, en-CA.<br>

#<b>National Geographic Channel Photo of the Day</b><br>
URL: http://photography.nationalgeographic.com/photography/photo-of-the-day/<br>
The image is located in the source as <code>img src=//images.nationalgeographic.com/wpf/media-live/photos/<Name_of_the_image>.jpg</code><br>

#N<b>ASA</b><br>
URL: http://apod.nasa.gov/apod/astropix.html<br>
The image is located in the source as <code>img src=<Name_of_the_image>.jpg</code><br>

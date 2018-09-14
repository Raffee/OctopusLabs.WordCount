# Octopus Labs Word Counter
Create great looking word-clouds of the words found on your desired web pages by using the Octopus Labs Word Counter.

## Usage
Navigate to https:// wordCounter-OctopusLabs.azurewebsites.com. The page is very easy to use and self-explanatory (just like the Google search page). Enter the URL of the desired page into the search text-box and click the “Get Words” button. Within a few seconds the page will display a great looking and colorful word-cloud of all the words found on the page, weighted by the frequency of their occurrence on the page.

## Installation
To install/run the Octopus Labs Word Counter on your local machine simply get the source code from GitHub (clone or download as a ZIP file). Open the solution (.sln file) using Visual Studio 2017 and make sure the OctopusLabs.WordCounter.Web is selected as the startup project. Finally, simply run by clicking Ctrl+F5 or debug by clicking F5 to view the word counter in a browser window. You need an internet connection since the word counter will attempt to write the search results into an online database.

## Tech/Frameworks Used
-	.NET Core 2.0
-	Google Cloud SQL (MySql)
-	wordcloud2.js

## Features
-	Taking advantage of WordCloud2.js gives great looking colorful word clouds.
-	An admin page to see the top 100 most frequent words accumulated over all the searches.

## Versions
1.0.0

## Author
Raffee Parseghian

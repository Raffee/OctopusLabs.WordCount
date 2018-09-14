# Octopus Labs Word Counter
Create great looking word-clouds of the words found on your desired web pages by using the Octopus Labs Word Counter.

## Usage
Navigate to http://wordcounter-octopuslabs.azurewebsites.net/. The page is very easy to use and self-explanatory (just like the Google search page). Enter the URL of the desired page into the search text-box and click the “Get Words” button. Within a few seconds the page will display a great looking and colorful word-cloud of all the words found on the page, weighted by the frequency of their occurrence on the page.

## Installation
To install/run the Octopus Labs Word Counter on your local machine simply get the source code from GitHub (clone or download as a ZIP file). Open the solution (.sln file) using Visual Studio 2017 and make sure the OctopusLabs.WordCounter.Web is selected as the startup project. Finally, simply run by clicking Ctrl+F5 or debug by clicking F5 to view the word counter in a browser window. You need an internet connection since the word counter will attempt to write the search results into an online database.

## Tech/Frameworks Used
-	.NET Core 2.0
-	Google Cloud SQL (MySql)
-	wordcloud2.js
-	Asymmetric Encryption of data (RSACryptoServiceProvider)

## Features
-	Taking advantage of WordCloud2.js gives great looking colorful word clouds.
-	An admin page to see the top 100 most frequent words accumulated over all the searches.

### Asymmetric Encryption Key Management
Asymmetric Encryption requires a pair of keys to correctly encrypt and decrypt data, a public key and a private key. The public key is used to encrypt the secret data, and could be distributed to anyone (thus the name "public") but the
private key must be known only to the party who is allowed to decrypt and view the encryped secret.
This pair of keys must be related to each other and each public key must have its corresponding private key. Therefore, to correctly generate key pairs we use Cryptographic service providers in .NET,
such as "RSACryptoServiceProvider," which have built in methods to generate correct and secure keys.
But, in a software application, we face a problem when we store encryped data and need to decrypt it later on: RSACryptoServiceProvider generates a new key with each instance, which means we need to secure the keys (specifically the private key) securely.
Luckily, the .NET framework provides the "CspParameters" class which allows us to store an instance of the Crypto Service Provider (CSP) to ue when generating and retrieving public/private keys.
The usage of the CspParameters class in very straight-forward as described in https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.cspparameters?view=netframework-4.7.2. Basically, we need to create a named storage for the CSP and retrieve it when needed.
It works somehow similarly to the singleton design pattern in the sense that if there isn't a CSP stored and new one is generated and returned, but if one exists, then the existing instance is returned.

The CSP is stored perpetually, therefore we don't need to worry about having different keys across different sessions or operations.
One important thing to remember is to set the "Flags" property of the CspParameters to "CspProviderFlags.UseMachineKeyStore" in order to use the key store on the host machine. Otherwise you will get a "File Not Found" exception.

### New Technologies/Frameworks/Libraries Learned
-	Google Cloud SQL (MySql)
-	WordCloud2.js
-	Practical Implementation of Asymmetric Encryption 

## Test
Some unit tests are found in a separate Tests project. These are not exhaustive and they are here only to provide a sample of how to write unit tests to check the code.

## Upcoming Features and Enhancements
-	Authentication to access the Admin section.
-	Complete the REST API.
-	Use Auto-Mapping to convert Entities to Dtos.
-	Save to database asynchronously to free the web page and speed up the response time.

## Known Issues
### Duplicate Keys
Since the app is also hosted on Azure and we are using machine specific pricate/public keys for encryption, there will be issues when the application is run locally because a new pair of keys will be generated on the local machine and therefore there will be
different  records in the database encryped using different keys. This will result in an error when trying to access the Admin page to read the list of records from the database as there will most probably be records that are not encryped with the key generated on the current host.

### Aenemic Domain Model
It is obvious that the domain model (namely the WordCount class) does not contain any domain logic at this time. To adhere to DDD guidelines and have a more flexible and testable system we should try to move the existing logic from the services to the domain entities (
as much as possible, wherever the logic is truly related to the "domain").

## Versions
1.0.0

## Author
Raffee Parseghian

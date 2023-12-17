
# Mantel Group Programming Task
LogParser

C# .NET 8 Console Application

Can download latest SDK from here

https://dotnet.microsoft.com/en-us/download/dotnet/8.0


## Assumptions

* A **single space** delimiter between fields 
* A **-** signifies no value for field
* **Exact match** of pattern using **RegEx**, any non matches will add to a LogError property on LogParser that will identify *line number* and *line detail*  
   (this can be toggled via "ShowLogErrorDetail" setting in appsettings.json)
* I've added a **report summary** which details the number of lines, the number of records parsed, the number of empty lines and the number of errors  
   (this can be toggled via "ShowReportSummary" setting in appsettings.json)
* Very **rudimentary logic** added to determine **Top 3**    
   Selects the **first top 3 only**, even if there maybe more records with the same number or occurences for both  
  *The Top 3 most visited URLs*  
  *The Top 3 most active IP addresses*  
  (I've added a setting to alter the Top Number to display via "TopNumberOfUrls" and "TopNumberOfIpAddresses" settings in appsettings.json, defaulted to 3 for both)
* I record the following fields as recognised by their values

   | Name          | Example     | Note    
   | ------------- | ------------ | ----  
   | IPAddress      | 192.168.1.1  | IPv4  |
   | User      | admin   | can be empty |
   | DateTime | 11/Jul/2018:17:31:56 +0200      | assuming UTC  |  
   | Method | GET  | http request method  |  
   | Url | /intranet-analytics/ | not excluding assets eg .js or .css files  |  
   | Protocol | HTTP/1.1 | http request protocol  |  
   | StatusCode | 200 | http request status code, not excluding based on code  |  
   | UserAgent | Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7 | http user agent  |  
   | *Extra* | junk extra | capturing but not recording  |  


 
## Run Locally

Clone the project

```
  git clone https://github.com/michaelpettorosso/mantelgroup-programming-task.git
```

Go to the project directory

```
  cd LogParser
```

Build Project

```
  dotnet build
```

Run The Program

```
  dotnet run
```

Close The Program

```
  CTRL + C
```
## Running Tests

To run tests, run the following commands

Go to the root directory

```
  dotnet test
```


## Application Settings

To run this project, you will can modify the appsettings.json in the LogParser directory

```
"LogReportOptions": {
  "FileName": "example-data.log",
  //"FileName": "example-with-bad-data.log",
  "TopNumberOfUrls": 3,
  "TopNumberOfIpAddresses": 3,
  "ShowReportSummary": true,
  "ShowLogErrorDetail": true
},
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft": "Warning",
    "Microsoft.Hosting.Lifetime": "Warning"
  }
}
```

To see result with errors change FileName to "example-with-bad-data.log"

## Usage/Examples

Output from example-data.log (file in Data folder)
```
Read log file 'example-data.log'
    23 line(s)
    23 record(s)
    0 empty line(s)
    0 line(s) with errors

Number of Unique IP Addresses
    11

Top 3 most visited Urls
    /docs/manage-websites/ (2)
    /intranet-analytics/ (1)
    http://example.net/faq/ (1)

Top 3 most active IP Addresses
    168.41.191.40 (4)
    177.71.128.21 (3)
    50.112.00.11 (3)
```
Output from example-with-bad-data.log (file in Data folder)

```
Read log file 'example-with-bad-data.log'
    24 line(s)
    21 record(s)
    1 empty line(s)
      on line(s) 17
    2 line(s) with errors
        (1) 177.71.128.21 - - "GET /intranet-analytics/ HTTP/1.1" 200 3574 "-" "Mozilla/5.0 (X11; U; Linux x86_64; fr-FR) AppleWebKit/534.7 (KHTML, like Gecko) Epiphany/2.30.6 Safari/534.7"
        (2) 168.41.191.40 - - [09/Jul/2018:10:11:30 +0200] "GET  http://example.net/faq/ HTTP/1.1" 200 3574 "-" "Mozilla/5.0 (Linux; U; Android 2.3.5; en-us; HTC Vision Build/GRI40) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1"

Number of Unique IP Addresses
    11

Top 3 most visited Urls
    /docs/manage-websites/ (2)
    /this/page/does/not/exist/ (1)
    http://example.net/blog/category/meta/ (1)

Top 3 most active IP Addresses
    168.41.191.40 (3)
    50.112.00.11 (3)
    72.44.32.10 (3)
```
* Line (1) is missing DateTime
* Line (2) has 2 spaces after GET
## Author

- [@michaelpettorosso](https://www.github.com/michaelpettorosso)


Thank you for your interest in Beringer Technology Group.
I'd like you to provide a code sample that will show off your ability to develop in Azure.

Here's the requirements for your Function App:
- Written in c#, and using the latest version of .NET
- Timer triggerd, set to run every 30 minutes
- Query the SalesForce API, and return data in a JSON format.
        - Here is the SalesForece API documentation:  https://www.integrate.io/blog/salesforce-rest-api-integration/
- Store the JSON data in a service bus queue called DevTest.
        - Here is the connection string:  Endpoint=sb://dsc00000075-datasynccloud-dev.servicebus.windows.net/;SharedAccessKeyName=DevTest;SharedAccessKey=BTqdrsBxiBawz5AOiJQTE8asCKp5RghRs+ASbC2CjmA=
        - Here is the queue URL:  https://dsc00000075-datasynccloud-dev.servicebus.windows.net/devtest
- Document the function app in a Readme.MD file

Please clone this branch, and upload your completed Visual Studio project to this repository.  I should be able to clone your branch, then compile and run your project.
Reach out to me with any questions.
Good luck :)

Rob Hess
rhess@beringer.net

-------------------------------------------------------------------------------------------------------------------------------
Code By Anas Siddiqui

This contains a timer triggered function that runs every 30 minutes. 
I have used repository design pattern. Since we aren't accessing any data source and just making api call, service layer is sufficient. The timer function first calls the Authentication service which authenticates with SalesForce and returns an object of AuthenticationDTO type. This contains access token which can be used for subsequent GET requests to other SalesForce APIs. This service uses the credentials stored in local.settings.json file to make a POST request to SalesForce OAuth2 endpoint. It deserializes the response to AuthenticationDTO and returns it. Please plug in your values in local settings. The timer triggered function then calls the SalesForceService which is responsible for making the call to the SalesForce Accounts API and retrieves a list of accounts. It deserializes the response to our DTO type and returns. The control then goes back to our timer function which then checks if our SalesForceService actually returned anything or not. If it did not, it simply logs a warning, otherwise it calls the ServiceBusService and passes as parameter the List of DTO objects it got from SalesForceService. The ServiceBusService reads the credentials required for connecting to the service bus from environment variables and forms a connection. Then, it serializes the Accounts object to JSON and puts the message on the service bus, after which it disposes both the sender and the client. Now the question why did I deserialize the response when I have to serialize again in the ServiceBusService? This is because this is a cleaner approach: it gives us strong typing and validation. It ensures that the payload is clean and just needs to be placed on the service bus by our ServiceBusService.
I have used NewtonSoft for serializing and deserializing. I have used dependency injection (would need to download a package for that as well). 

Note: if "AzureWebJobsStorage" is set to "AzureWebJobsStorageConnectionStringValue" in local.settings.json, please change it to
"UseDevelopmentStorage=true". i.e., change the line "AzureWebJobsStorage": "AzureWebJobsStorageConnectionStringValue" to "AzureWebJobsStorage": "UseDevelopmentStorage=true" in local.settings.json. Please plug in your values for ClientId, ClientSecret, Accountname, Password and EndPoint as I have removed mine while committing. I have used the values you provided for QueueUrl and ServiceBusConnectionString.The key names used by me can be different from what you used. I have committed local.settings.json file as well.  

PS. I would love feedback on my code, it will help me improve and learn. 

# Dotnet Application Insights Console App Demo

A basic demo implementing in a console application the Microsoft Application Insights service.

🚨 - Update *appsettings.json* file with the correct Application Insights connection string before to run or publish

## A. Publish console application.

Command to publish as auto-content dotnet application file.

- Note: Run command from base repository path.

```
dotnet publish -p:PublishProfile="ApplicationInsightsConsoleApp\Properties\PublishProfiles\PublishFileAutoContent.pubxml"
```

## B. Util information.

* [Application Insights overview](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview?tabs=net)
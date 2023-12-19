# TaskManagementSystem

I have used InMemory database. The models use an Enum for the Status. In swagger or other way to send requests, use int (0,1,2,3) for the Status of Tasks. The app can be modified through the EnumSchemaFilter to remove the using of integer for the status and use string format of the enum. With integer is harder to make mistakes.
I have a repository that handles all relations to the database and is injected in the controller.
I used AutoMapper to map the entity to the DTOs in requests.

The is a BackgroundService named **DailyStatusCheckService** that runs every 30 seconds and checks/updates the statuses of the tasks. This interval can be changed in appsettings.json, key **DailyStatusCheckPeriod**.

There is a Test project that contains unit tests for the repository and the controller.

Unfortunately, I didn't have the necessary time for the UI.

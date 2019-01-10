# RestCashflow API

This project consists of a web api to register financial entries queuing with RabbitMQ and MariaDB for persistance.

## Run
To run the MariaDB and RabbitMQ  type in the terminal: 
````
docker-compose up
````

**It`s mandatory that you run the docker-compose before running the project, because without MariaDB and RabbitMQ properly running the application will not run.**

**DISCLAIMER:** I was not able to add the application into a container, I can build the image but I get an error from MariaDB while trying to initialize the container, because of that I removed from the docker compose file. It was taking me a lot of time to fix so I skipped that part.

## Project Structrure
Ddveloped with .NET Core 2.2 (C#)

There`re three assemblies in the whole project
- **RestCashflowLibrary:** The core logic, here are the validations, persistance, models and business rules devided in DDD;
- **RestCashflowTests:** Unit tests with xUnit.
- **RestCashflowWebApi:** The Web API interface;

## API Documentation
The documentation is auto generated with XML comments provided by Swashbuckle.AspNetCore  library.  For more details: https://github.com/domaindrivendev/Swashbuckle.AspNetCore

After the project is initialized hit the URL: **http://<host><port>:/swagger** to view the documentation of how to call the web api.

## RabbitMQ User Interface
The user interface of RabbitMQ can be accessed in: **http://<host><port>:15672**

**Username:** user
**Password:** bitnami

## MariaDB Credentials
**User:** root
**Password:** 123456
**Database:** db_cashflow

There`s a SQL dump file in the solution root called: **db_cashflow.sql**
**NOTE:** Every time you initialize the project the table structure will be automatically created if do not exist.

## Configuration
````javascript
{
...
  "ConnectionStrings": {
    "RabbitMqDefault": "amqp://user:bitnami@localhost:5672/",
    "MySqlDefault": "Server=127.0.0.1;Database=db_cashflow;Uid=root;Pwd=123456;Pooling=True;MinimumPoolSize=10;MaximumPoolSize=25;AllowUserVariables=true"
  },
  "Business": {
    "DayAccountLimit":  "20000,00",
    "DayInterest": "0,83"
  }
  ...
}

````
##### ConnectionStrings
Required to connect to MariaDB (MySql Binary Drop) and RabbitMQ.
##### Business
Business parameters.
- **DayAccountLimit:** The day account limit that the balance is allowed to become negative.
- **DayInterest:** The day interest if the day balance is negative.

Change this as you like to adapt to your needs.

## Reference links
RabbitMQ Docker Image
https://github.com/bitnami/bitnami-docker-rabbitmq
<br/>
MariaDB Docker Image
https://hub.docker.com/_/mariadb/

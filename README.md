# CoffeeMassTransit
Démonstration de MassTransit avec une machine à café en SagaStateMachine.

Repository en lien avec [le webinar SOAT du 7 mai 2020](https://youtu.be/dMAtxIPqKfQ)

Article sur le blog SOAT en lien avec ce repository : 
- [Partie 1](https://blog.soat.fr/2020/05/facilitez-les-echanges-entre-vos-microservices-en-net-core-avec-masstransit/)
- [Partie 2](https://blog.soat.fr/2020/06/sagas-et-machine-a-etats-au-sein-de-votre-architecture-micro-services-grace-a-masstransit/)

# Docker
## RabbitMQ
```
docker run -d --hostname rabbitmq --name rabbitmq -p 8080:15672 -p 5672:5672 -p 5671:5671 rabbitmq:3-management
```

## MS-SQL
```
docker run --name mssql -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourStrong!Password' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```
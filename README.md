# Exemplo Azure Function .net 6 integrado ao Queue Service Bus e a Storage Account

Projeto desenvolvido em .net 6 utilizando Azure Function in-process, para receber dados de votação em músicas, os votos são armazenados em uma fila no Service Bus, e então outra Azure Function processa os votos da fila e guarda em um Azure Tables.

## Demo Live
[DEMO](https://musicvoteapp.azurewebsites.net/api/swagger/ui)

## Arquitetura
![Diagrama](/images/diagrama.png "Digrama")

1. Endpoint /Vote recebe o voto e registra em uma fila (Queue) no Service Bus;
2. A Azure Function ProcessVote é invocada para processar as mensagens na fila; 
3. Ocorre a agregação do voto e armazenamento no Azure Table dentro da Storage Account
4. Endpoint /VoteResult consulta a Azure Table e apresenta o resultado da votação

## Documentação da API

[Swagger](http://localhost:7071/api/swagger/ui)

Agradecimento:
Renato Groffe
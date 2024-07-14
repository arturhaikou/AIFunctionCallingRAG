## Description
It's an education project related to AI RAG

## Objectives
* Show the difference between using LLMs with and without RAG.
* Show pure vector search with dense and sparse vectors searching mechanisms.
* Show approaches for improvement the common RAG solution like HyDE(Hypothetical Document),  RAG fusion, andMulti-Query.

## How to run
The solution based on the .NET Aspire.

### Prerequisites
* Docker
* Change the value of the `AI-Key` parameter(It's a OpenAI API key) in the appsettings.json file for a `AIFunctionCallingRAG.AppHost` project.

### Visual studio
Set the `AIFunctionCallingRAG.AppHost` project as a startup project and click run. The containers will be created automatically.

### CLI
```
cd ./AIFunctionCallingRAG.AppHost

dotnet restore

dotnet run
```
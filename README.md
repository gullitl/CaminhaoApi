# CaminhaoApi

## Ponta pé
Para executar a applicação precisa configurar as informações de conexão com o banco de dados. Foi utilizado o SGBD Mysql. A configuração pode ser feita em um UserSecret ou no appSettings como explicado no tópico seguinte. 
## App secrets ou appSettings
Foi colocada a informação do "ConnectionStrings:DefaultConnection" para conexão com Banco de dados em um UserSecret. Para configurar o secret segue a documentação da Microsoft para os ambientes Windows, Linux e macOs: https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=linux

o arquivo do secret é apresentado da forma seguinte:

{
    "ConnectionStrings:DefaultConnection": "Server=127.0.0.1; port=3306; Database=caminhaodb; uid=root; password=#####"
}

Caso preferir utilizar diretamente a informação do "ConnectionStrings:DefaultConnection" no appSettings basta informar o DefaultConnection da ConnectionStrings dessa forma:

"ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1; port=3306; Database=caminhaodb; uid=root; password=#####"
  }
## Banco de Dados
Foi utilizado o SGBD Mysql. Para conectar a applicação com outro SGBD basta instalar os pacotes nugets e alterar a chamada do método "UseMySql()" na linha 32 da classe Startup no projeto CaminhaoApi.Application pela chamada do método do SGBD que for utilizado. E informar o DefaultConnection da ConnectionStrings do SGBD no arquivo do Secret ou no appSettings.

## Testes
Foram criados 48 testes unitários no projeto CaminhaoApi.Test para cobrir ao menos 80% dos fluxos. Há uma mensagem de documentação nso Asserts de cada teste criado. Esta mensagem aparece quando o teste falha.

## Execução da aplicação
A aplicação executa em forma de API. Foi configurado para ser lancado o Swagger afim de possibilitar o teste da aplicação em execução.


Cordialmente.

# Projeto - MLCreditAnalysis

Projeto de análise de credito onde deve analisar os dados informados pelo usuário estão corretos e uma predição da possibilidade desse cliente honrar ou não com o pagamento de credito.

# Arquitetura
O projeto foi criado com diversas linguagens e plataformas.

FIGURA 01

## Modelo de análise de crédito
Foram desenvolvidos por um cientista dados, dois modelos de análise de crédito em Python, o quais não sabemos detalhes de como foram feitos, nosso objetivo nesta aplicação é realizar o trabalho de engenharia de dados e servir os modelos em containers para serem consumidos pelo frontend da aplicação. Para facilitar o acesso a esse modelo em diversas linguagens, criamos uma api web, a qual publicamos em container Docker no cloud da IBM.

## Analise de imagem
Para a análise de imagem, utilizamos duas apis, uma sendo da IBM e outra da Azure.
Ao enviar para a análise de credito, o usuário deve enviar uma self a qual vamos analisar os seguintes dados:
•	Idade, 
•	Sexo, 
•	Quantidade de pessoas na self
•	Conteúdo explicito

### IBM
Utilizamos a api da IBM para identificar se realmente existe uma pessoa na imagem e se o conteúdo informado é explícito.

### Azure
Utilizamos a api da Azure para identificar a quantidade de pessoas na foto, sexo, idade e os pontos fiduciários.

## Frontend
Foi desenvolvida uma aplicação web para o acesso ao usuário, essa aplicação foi criada na linguagem C#.
Criamos uma interface onde o usuário pode selecionar qual dos 2 modelos ele deseja utilizar, informando os dados pessoais do cliente e a self.
A self pode ser enviada de duas formas, sendo elas upload ou pela web cam.

## BackEnd
As regras de negócio foram desenvolvidas em C# e publicada em um site de hospedagem utilizando IIS.
O backend faz o gerenciamento da solicitação do frontend, validação dos dados e coordena as chamadas das apis(IBM, Azure e de análise de crédito) e seus resultados.
Após efetuada as regras de negócio, o sistema informa ao usuário se o credito foi aprovado ou não. Em casos de aprovação, a self com os pontos fiduciários é apresentada.

# Regra de negócio/Fluxo de trabalho

FIGURA 02


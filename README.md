PortalLL

Este é um projeto web desenvolvido com a plataforma .NET, utilizando a linguagem C# e a arquitetura MVC (Model-View-Controller).

🚀 Sobre o Projeto

O PortalLL é uma aplicação web construída com as mais recentes tecnologias da plataforma .NET. O projeto está configurado para ser executado como um site MVC tradicional.

🛠️ Tecnologias Utilizadas

.NET 9.0 

ASP.NET Core

C#

⚙️ Configuração do Projeto

O projeto está estruturado da seguinte forma:


PortalLL.csproj: Arquivo de projeto que define as configurações, dependências e o framework de destino (net9.0).


PortalLL.sln: Arquivo de solução para gerenciar o projeto no Visual Studio.

Program.cs: Ponto de entrada da aplicação. Aqui são configurados os serviços, o pipeline de requisições HTTP e as rotas.

Serviços de Controllers com Views.

Suporte a sessões em memória (AddDistributedMemoryCache e AddSession).

Roteamento padrão configurado para: {controller=Portal}/{action=Index}/{id?}.

appsettings.json: Arquivo de configuração principal da aplicação.

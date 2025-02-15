# Projeto de Conexão Mobile com API  

Este repositório contém dois projetos integrados para oferecer uma solução completa de conexão entre um aplicativo mobile e uma API RESTful:  

- **mobileapp**: Aplicativo mobile desenvolvido para conectar pacientes e médicos, permitindo agendamento de consultas, acompanhamento de exames e localização de médicos próximos.  
- **webapi**: API desenvolvida com ASP.NET para gerenciar o backend do aplicativo, incluindo autenticação, gerenciamento de usuários e CRUDs necessários para o funcionamento da aplicação.  

---

## Descrição do Projeto  

Este projeto foi desenvolvido para facilitar o acesso de pacientes a serviços médicos através de um aplicativo mobile. Ele oferece funcionalidades como:  

- Agendamento de consultas com médicos cadastrados.  
- Acompanhamento de exames médicos.  
- Localização de médicos próximos ao usuário.  

O aplicativo mobile consome dados de uma API RESTful, fornecida pelo projeto webapi, para garantir uma experiência rápida e segura para os usuários.  

---
 
## Estrutura do Repositório  

    .
    ├── mobileapp/     # Aplicativo mobile
    └── webapi/        # API ASP.NET

---

## Como Executar  

### Pré-requisitos  

- Node.js e npm/yarn instalados para o projeto mobile.  
- .NET SDK instalado para o projeto webapi.  
- Android Studio ou emulador para testar o aplicativo mobile.  
- Docker (opcional) para rodar a API em container.  

---

### Executando o Aplicativo Mobile  

1. Navegue até a pasta do aplicativo:  
    ```bash
    cd mobileapp
    ```

2. Instale as dependências:  
    ```bash
    npm install
    ```
    ou  
    ```bash
    yarn install
    ```

3. Execute o aplicativo:  
    ```bash
    npm run serve
    ```
    ou  
    ```bash
    yarn serve
    ```

4. Abra o aplicativo em um emulador ou dispositivo físico.  

---

### Executando a API ASP.NET  

1. Navegue até a pasta do projeto webapi:  
    ```bash
    cd webapi
    ```

2. Restaure as dependências:  
    ```bash
    dotnet restore
    ```

3. Compile o projeto:  
    ```bash
    dotnet build
    ```

4. Execute as migrações para preparar o banco de dados:  
    ```bash
    dotnet ef database update
    ```

5. Inicie a aplicação:  
    ```bash
    dotnet run
    ```

6. A API estará disponível em: [http://localhost:5017](http://localhost:5017)  

---

### Executando a API com Docker (Opcional)  

1. Certifique-se de que o Docker esteja instalado e em execução.  

2. Navegue até a pasta do projeto webapi:  
    ```bash
    cd webapi
    ```

3. Construa a imagem Docker:  
    ```bash
    docker build -t webapi-image .
    ```

4. Inicie o container:  
    ```bash
    docker run -d -p 5017:80 --name webapi-container webapi-image
    ```

5. A API estará disponível em: [http://localhost:5017](http://localhost:5017)  

---

## Tecnologias Utilizadas  

- **mobileapp**: Desenvolvido com Vue.js e Tailwind CSS.  
- **webapi**: Desenvolvido com ASP.NET Core e Entity Framework Core.  

---

## Funcionalidades Principais  

- Agendamento de consultas.  
- Acompanhamento de exames.  
- Localização de médicos próximos.  
- Autenticação de usuários.  

---

## Contribuições  

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues e pull requests para melhorias ou correções.  

---

## Licença  

Este projeto é licenciado sob a [MIT License](LICENSE).  

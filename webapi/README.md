
# Documentação da API

### Autenticação Usuário

**POST /api/v1/auth/user/login**
- **Payload:**
  ```json
  {
    "email": "string",
    "password": "string"
  }
  ```
- **Resposta:**
  ```json
  {
    "token": "string",
    "refreshToken": "string"
  }
  ```

**POST /api/v1/auth/user/refresh-token**
- **Payload:**
  ```json
  {
    "refreshToken": "string"
  }
  ```
- **Resposta:**
  ```json
  {
    "token": "string",
    "refreshToken": "string"
  }
  ```

**POST /api/v1/auth/user/logout**
- **Resposta:**
  ```json
  {
    "message": "Success"
  }
  ```

**GET /api/v1/auth/user/usuario**
- **Resposta:**
  ```json
  {
    "id": 0,
    "name": "string",
    "email": "string",
    "cpf": "string",
    "address": {
      "id": 0,
      "logradouro": "string",
      "cep": "string",
      "bairro": "string",
      "cidade": "string",
      "estado": "string",
      "pais": "string",
      "numero": "string",
      "complemento": "string",
      "createdAt": "string",
      "updatedAt": "string"
    },
    "createdAt": "string",
    "updatedAt": "string"
  }
  ```

**POST /api/v1/auth/user/register**
- **Payload:**
  ```json
  {
    "name": "string",
    "email": "string",
    "password": "string",
    "cpf": "string",
    "addressId": 0
  }
  ```
- **Resposta:**
  ```json
  {
    "id": 0,
    "name": "string",
    "email": "string",
    "cpf": "string",
    "addressId": 0,
    "createdAt": "string",
    "updatedAt": "string"
  }
  ```

### Autenticação Administrador

**POST /api/v1/auth/admin/login**
- **Payload:**
  ```json
  {
    "email": "string",
    "password": "string"
  }
  ```
- **Resposta:**
  ```json
  {
    "token": "string",
    "refreshToken": "string"
  }
  ```

**POST /api/v1/auth/admin/register**
- **Payload:**
  ```json
  {
    "name": "string",
    "email": "string",
    "password": "string"
  }
  ```
- **Resposta:**
  ```json
  {
    "id": 0,
    "name": "string",
    "email": "string",
    "createdAt": "string",
    "updatedAt": "string"
  }
  ```

### Endereços

**POST /api/v1/enderecos**
- **Payload:**
  ```json
  {
    "logradouro": "string",
    "cep": "string",
    "bairro": "string",
    "cidade": "string",
    "estado": "string",
    "pais": "string",
    "numero": "string",
    "complemento": "string"
  }
  ```
- **Resposta:**
  ```json
  {
    "id": 0,
    "logradouro": "string",
    "cep": "string",
    "bairro": "string",
    "cidade": "string",
    "estado": "string",
    "pais": "string",
    "numero": "string",
    "complemento": "string",
    "createdAt": "string",
    "updatedAt": "string"
  }
  ```

### Administradores

**POST /api/v1/administradores**
- **Payload:**
  ```json
  {
    "name": "string",
    "email": "string",
    "password": "string"
  }
  ```
- **Resposta:**
  ```json
  {
    "id": 0,
    "name": "string",
    "email": "string",
    "createdAt": "string",
    "updatedAt": "string"
  }
  ```

### Agendamentos

**POST /api/v1/agendamentos**
- **Payload:**
  ```json
  {
    "date": "string",
    "notes": "string",
    "status": "string",
    "userId": 0,
    "doctorId": 0
  }
  ```
- **Resposta:**
  ```json
  {
    "id": 0,
    "date": "string",
    "notes": "string",
    "status": "string",
    "doctor": {},
    "user": {},
    "appointmentRating": {},
    "createdAt": "string",
    "updatedAt": "string"
  }
  ```

### Médicos

**POST /api/v1/medicos**
- **Payload:**
  ```json
  {
    "name": "string",
    "cpf": "string",
    "email": "string",
    "crm": "string",
    "especializationId": 0
  }
  ```
- **Resposta:**
  ```json
  {
    "id": 0,
    "name": "string",
    "cpf": "string",
    "email": "string",
    "crm": "string",
    "especialization": {},
    "createdAt": "string",
    "updatedAt": "string"
  }
  ```

### Unidades

**POST /api/v1/unidades**
- **Payload:**
  ```json
  {
    "name": "string",
    "phoneNumber": "+6..5. 16-39.560453.5.6804215.3.56 07 70.63-8.71-.71273 12.-.-8894998. 129.8 4-0779-6576  -5",
    "email": "user@example.com",
    "addressId": 2147483647
  }
  ```
- **Resposta:**
  ```json
  {
    "id": 0,
    "name": "string",
    "phoneNumber": "string",
    "email": "string",
    "createdAt": "2025-03-02T20:01:26.705Z",
    "updatedAt": "2025-03-02T20:01:26.705Z",
    "address": {
      "id": 0,
      "logradouro": "string",
      "cep": "string",
      "bairro": "string",
      "cidade": "string",
      "estado": "string",
      "pais": "string",
      "numero": "string",
      "complemento": "string",
      "createdAt": "2025-03-02T20:01:26.705Z",
      "updatedAt": "2025-03-02T20:01:26.705Z"
    }
  }
  ```

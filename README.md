# Lusodata MRP

## Ao fazer o clone do projeto:

1. no "Package Manage Console", digitar:

```Update-Database -Context ApplicationDbContext```

- OBS1: não esquecendo de setar o "default project" como **Core.API**


2. no "Package Manage Console", digitar 

```Update-Database -Context MyDbContext```

- OBS2: não esquecendo de setar o "default project" como **Core.Data**


## Regras para a Padronização de Controllers

### Exemplo em Operações CRUD
Verbo | Controller | Action | Definição 
------------ | ------------- | ------------- | -------------
GET | /patients | index() | Exibe DataTable
GET | /patients/{id} | edit(guid id) | Form para edição
DELETE | /patients/{id} | delete(guid id) | Para excluir um registo
POST | /patients | create(_view_model) | Para insert ou update
POST | /patients/enable | enable(_view_model) | Para operações custom
POST | /patients/disable | disable(_view_model) | Para operações custom
 
Controllers **sempre** no plural.
Actions **sempre** no singular.

**Atenção:** Commits fora deste padrão serão rejeitados.

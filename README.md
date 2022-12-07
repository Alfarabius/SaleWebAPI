# SaleWebAPI

### Test task for IntelWash Company. 

To statup project, edit:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\<YourLocalDb>;Database=SaleAPIDb,Trusted_Connection=True"
  }
  ```

##In appsettings.json
Change <YourLocalDb> on your local database name
For your local bases names use in terminal: 
``sqllocaldb i``

After that use ``Update-Database`` in Package Manager Console, than build and start SaleAPI and BuyerAPI.

## Swagger UI

The Swagger UI is an open source project to visually render documentation for an API defined with the OpenAPI (Swagger) Specification

### SaleAPI - web API with CRUD operations:
- https://localhost:5001/swagger/index.html

### BuyerAPI - API with Sale buisness logic:
- http://localhost:5003/swagger/index.html

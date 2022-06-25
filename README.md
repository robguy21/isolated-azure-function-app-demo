# To Test

GET http://localhost:7071/api/data
```json

{
  "Name": null,
  "Id": null
}
```

POST http://localhost:7071/api/data
```json

{
  "name": "John Doe"
}
```

GET http://localhost:7071/api/data
```json

{
  "Name": "John Doe",
  "Id": 1
}
```
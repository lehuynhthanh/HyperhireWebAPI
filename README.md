- Requirement: https://hyperhire.notion.site/Java-Spring-assginment-Airbnb-241108-1387ac1c0f2f80578e73cd2bc158efec
- Techstack: .Net 8, SQLServer, codefirst(need migration db)
- API:
  Auth:
    - POST /api/Auth/login: get token for authentication
    - POST /api/Auth: add user
  Order:
    - POST /api/Order: create order, need authentication
    - GET /api/Order: get all order on user authenticated, need authentication
    - GET /api/Order/{id}: get order by id on user authenticated, need authentication
  Category
    - POST /api/Category: create category, need authentication
    - GET /api/Category: get all Category, NO need authentication
  ProductDetail
    - POST /api/ProductDetails: create product, need authentication
    - GET /api/ProductDetails?CategoryId={Category Id}: get all product of this category, NO need authentication
    - GET /api/ProductDetails/{id}: get product by id, NO need authentication
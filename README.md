
# GestorUsuariosLog API

Esta API REST extiende la funcionalidad de la versión anterior para gestionar usuarios, productos, proveedores, categorías y logs, además de autenticar clientes mediante JSON Web Tokens (JWT). Se implementa con ASP.NET Core y Entity Framework Core (Code First)

La API incluye:

- **Autenticación y Gestión de Usuarios:**  
  - Registro de usuarios con validación de datos (DataAnnotations) y almacenamiento seguro de contraseñas mediante SHA-256.  
  - Endpoints de login y refresh para JWT.  
  - Protección de endpoints mediante el atributo `[Authorize]`.

- **Gestión de Productos, Proveedores y Categorías:**  
  - CRUD para cada entidad. 
  - **Producto:** Incluye propiedades como Nombre, Precio, Stock, IdProveedor e IdCategoria. 
  - **Proveedor:** Incluye Nombre y Contacto.
  - **Categoría:** Incluye Nombre.

- **Endpoints de Estadistica para Productos:**  
  - **Estadistica:** Devuelve el producto con el precio más alto, el producto con el precio más bajo, la suma total de precios y el precio promedio.  
    - URL: `/api/productos/Estadistica`
  - **Total de Productos:** Devuelve la cantidad total de productos registrados.  
    - URL: `/api/productos/totalProductos`
- **Creación de Log del registro de usuarios**
  - Crea un log serializado en formato Json con los usuarios registrados
  - Endpoint para leer el log.
---

## Requisitos del Sistema

- [.NET 8 o superior](https://dotnet.microsoft.com/download)
- [SQL Server Express / LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Postman](https://www.postman.com/downloads/) 

---

## Instalación y Configuración

### 1. Clonar el Repositorio


    git clone https://github.com/Pedro-JRC/GestorUsuariosLog.git
    cd GestorUsuariosWebTokens


### 2. Configurar la cadena de conexión

Actualiza el archivo `appsettings.json` con la cadena de conexión y la configuración de JWT. Por ejemplo:

    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=GestorUsuariosWebDb;Trusted_Connection=True;MultipleActiveResultSets=true"
      },
    }

### 3. Aplicar las migraciones

Abre la Consola del Administrador de Paquetes y ejecuta:

  

    Add-Migration InitialCreate
    Update-Database


### 4. Ejecutar la aplicación

    
      dotnet run        


# Pruebas de Postman productos

Esta sección describe los casos de prueba que se deben realizar utilizando Postman para verificar la funcionalidad completa de la API GestorUsuariosProductos. Cada caso de prueba incluye la URL, el método HTTP y un ejemplo de cuerpo.

---

1. **Registro de Usuario**

   - **URL:** https://localhost:7263/api/usuarios
   - **Método:** POST
   - **Cuerpo (raw, JSON):**
     ```json
     {
       "nombre": "Pedro Julio Rosario",
       "nombreUsuario": "pjrosario",
       "correo": "correo@outlook.com",
       "fechaDeNacimiento": "1998-06-15",
       "password": "Prueba.1234"
     }
     ```
   - **Nota:** Este endpoint permite registrar un nuevo usuario. La API recibe la contraseña en texto plano, calcula el hash (SHA-256) y almacena el resultado en la base de datos.
   - **Captura de pantalla:**

     ![registrar usuario](https://github.com/user-attachments/assets/9e9eaa85-a170-4e98-8c7d-01d3f9035c11)


2. **Login (Autenticación)**

   - **URL:** https://localhost:7263/api/auth/login
   - **Método:** POST
   - **Cuerpo (raw, JSON):**
     ```json
     {
       "nombreUsuario": "pjrosario",
       "password": "Prueba.1234"
     }
     ```
   - **Nota:** Este endpoint recibe las credenciales del usuario. Si son correctas, se genera y devuelve un token JWT.
   - **Captura de pantalla:
  
     ![login](https://github.com/user-attachments/assets/60e11002-481b-46ba-a2a5-b13e3d31c5e1)


3. **Refrescar el Token**

   - **URL:** https://localhost:7263/api/auth/refresh
   - **Método:** POST
   - **Nota:** Este endpoint permite renovar el token JWT antes de que expire. Es obligatorio incluir el header:
     ```
     Authorization: Bearer <tu token JWT>
     ```
   - **Captura de pantalla:**
  
     ![RefreshToken](https://github.com/user-attachments/assets/57d68d8d-b777-4d56-bb31-7ea679d93fbd)


4. **Obtener todos los Usuarios**

   - **URL:** https://localhost:7263/api/usuarios
   - **Método:** GET
   - **Nota:** Devuelve la lista completa de usuarios registrados. Se requiere autenticación (envía el header `Authorization: Bearer <tu token JWT>`).
   - **Captura de pantalla error 401:**
  
      ![401 usuarios](https://github.com/user-attachments/assets/f8c3eca4-8e63-4666-b07b-e5ab80e5b5a7)


   - **Captura de pantalla completado:** 

      ![Obtener usuarios](https://github.com/user-attachments/assets/1882da7e-4d99-4e8f-982b-433a2ff712b9)


6. **Obtener un Usuario por ID**

   - **URL:** https://localhost:7263/api/usuarios/{id}
   - **Método:** GET
   - **Nota:** Reemplaza `{id}` por el identificador del usuario. Se requiere autenticación.
   - **Captura de pantalla error 401:**
  
      ![401 usuario id](https://github.com/user-attachments/assets/8a58f05e-f494-4ac7-9c88-fb8a69fb29e3)


   - **Captura de pantalla satisfactorio:** 

      ![Obtener por id](https://github.com/user-attachments/assets/88a07721-800c-420f-833c-788a57341e51)


8. **Actualizar Usuario**

   - **URL:** https://localhost:7263/api/usuarios/{id}
   - **Método:** PUT
   - **Cuerpo (raw, JSON):**
     ```json
     {
       "id": 1,
       "nombre": "Pedro Julio Rosario",
       "nombreUsuario": "pjrosario2",
       "correo": "correonuevo@outlook.com",
       "fechaDeNacimiento": "1998-06-15",
       "password": "nuevaClave123"
     }
     ```
   - **Nota:** Actualiza los datos del usuario. Si se envía el campo `"password"`, la API recalcula el hash y actualiza el campo `PasswordHash`. Se requiere autenticación.
   - **Captura de pantalla error 401:**
  
      ![401 actualizar usuario](https://github.com/user-attachments/assets/ddb86920-3ae2-4166-977a-873b87a22a54)


   - **Captura de pantalla satisfactorio:** 

      ![actualizar usuario](https://github.com/user-attachments/assets/54bd8422-09d0-4bae-a468-020eaf87c606)


9. **Eliminar Usuario**

   - **URL:** https://localhost:7263/api/usuarios/{id}
   - **Método:** DELETE
   - **Nota:** Elimina el usuario especificado por el identificador. Se requiere autenticación.
   - **Captura de pantalla error 401:**
  
      ![401 eliminar usuario](https://github.com/user-attachments/assets/06f57cf0-f393-44a2-b1d5-55bef71f5099)


   - **Captura de pantalla satisfactorio:** 

      ![eliminar usuario](https://github.com/user-attachments/assets/3171c07c-4f68-4be5-8fb3-978652979f67)

---

10. **Obtener Todas las Categorías**

     - **URL:** https://localhost:7263/api/categorias
     - **Método:** GET
     - **Nota:** Se requiere autenticación.
     - **Captura de pantalla:**

     
      ![obtener categorias](https://github.com/user-attachments/assets/0257ce1a-ffa9-400c-a94c-1ab40b9057bb)



11. **Obtener una Categoría por ID**

     - **URL:** https://localhost:7263/api/categorias/{id}
     - **Método:** GET
     - **Nota:** Reemplaza `{id}` por el identificador de la categoría. Se requiere autenticación.
     - **Captura de pantalla:**

     
      ![obtener categoria ID](https://github.com/user-attachments/assets/08dc8cb5-0764-4fad-a631-7413d30197a1)



12. **Crear una Categoría**

   - **URL:** https://localhost:7263/api/categorias
   - **Método:** POST
   - **Cuerpo (raw, JSON):**
     ```json
     {
       "nombre": "Laptops"
     }
     ```
   
   - **Captura de pantalla:**

     
      ![crear categoria](https://github.com/user-attachments/assets/611e1f52-cf90-4a6f-a6f3-3b7eef69b6ed)



13. **Actualizar una Categoría**

   - **URL:** https://localhost:7263/api/categorias/{id}
   - **Método:** PUT
   - **Cuerpo (raw, JSON):**
     ```json
     {
       "id": 1,
       "nombre": "Laptops Actualizadas"
     }
     ```
   - **Nota:** Reemplaza `{id}` por el identificador de la categoría a actualizar.
   - **Captura de pantalla:** 


      ![modificar categorias](https://github.com/user-attachments/assets/4277775e-4395-448a-9ddf-057cfb0217de)


14. **Eliminar una Categoría**

   - **URL:** https://localhost:7263/api/categorias/{id}
   - **Método:** DELETE
   - **Nota:** Reemplaza `{id}` por el identificador de la categoría a eliminar.
   - **Captura de pantalla:** 

      ![eliminar categoria](https://github.com/user-attachments/assets/4b798fdc-ef17-4c20-b053-6198c54bed38)



15. **Obtener Todos los Proveedores**

   - **URL:** https://localhost:7263/api/proveedores
   - **Método:** GET
   - **Nota:** Se requiere autenticación.
   - **Captura de pantalla:** 


      ![obtener proveedores](https://github.com/user-attachments/assets/425ed9a2-8be5-4aa3-9c83-6370024f5ed7)


16. **Obtener un Proveedor por ID**

   - **URL:** https://localhost:7263/api/proveedores/{id}
   - **Método:** GET
   - **Nota:** Reemplaza `{id}` por el identificador del proveedor. Se requiere autenticación.
   - **Captura de pantalla:** 


      ![obtener proveedores ID](https://github.com/user-attachments/assets/cb8e3180-789b-4d41-b4c3-ff62db16fb8a)


17. **Crear un Proveedor**

   - **URL:** https://localhost:7263/api/proveedores
   - **Método:** POST
   - **Cuerpo (raw, JSON):**
     ```json
     {
       "nombre": "TechSupply Inc.",
       "contacto": "techsupply@example.com"
     }
     ```
  
   - **Captura de pantalla:** 

      ![crear proveedor](https://github.com/user-attachments/assets/27fc5d8a-d6cd-4a72-8c55-99adfa29382f)



18. **Actualizar un Proveedor**

   - **URL:** https://localhost:7263/api/proveedores/{id}
   - **Método:** PUT
   - **Cuerpo (raw, JSON):**
     ```json
     {
       "id": 1,
       "nombre": "TechSupply Inc.",
       "contacto": "nuevoemail@example.com"
     }
     ```
   - **Nota:** Reemplaza `{id}` por el identificador del proveedor a actualizar.
   - **Captura de pantalla:** 

      ![modificar proveedor](https://github.com/user-attachments/assets/1c0a2eb9-895e-4def-9ae4-fba2ea5abced)



19. **Eliminar un Proveedor**

   - **URL:** https://localhost:7263/api/proveedores/{id}
   - **Método:** DELETE
   - **Nota:** Reemplaza `{id}` por el identificador del proveedor a eliminar.
   - **Captura de pantalla:**

      ![eliminar proveedor](https://github.com/user-attachments/assets/c7ab48ca-81f2-413b-ba91-2105048018d0)



20. **Obtener Todos los Productos**

   - **URL:** https://localhost:7263/api/productos
   - **Método:** GET
   - **Nota:** Devuelve la lista de productos. Se requiere autenticación.
   - **Captura de pantalla:** 


      ![obtener productos](https://github.com/user-attachments/assets/846fd2bf-08d4-4efe-bbdc-41ecfbfff688)


21. **Obtener un Producto por ID**

   - **URL:** https://localhost:7263/api/productos/{id}
   - **Método:** GET
   - **Nota:** Reemplaza `{id}` por el identificador del producto. Se requiere autenticación.
   - **Captura de pantalla:** 



      ![obtener productos ID](https://github.com/user-attachments/assets/81bc393a-126d-411e-9e27-2330a1a472fb)


22. **Crear un Producto**

   - **URL:** https://localhost:7263/api/productos
   - **Método:** POST
   - **Cuerpo (raw, JSON):**
     ```json
     {
       "nombre": "Nuevo Producto",
       "precio": 199.99,
       "stock": 25,
       "idProveedor": 1,
       "idCategoria": 2
     }
     ```
   
   - **Captura de pantalla:** 


      ![crear producto](https://github.com/user-attachments/assets/3762f0b4-c403-497d-8452-ccbf20dced8c)


23. **Actualizar un Producto**

   - **URL:** https://localhost:7263/api/productos/{id}
   - **Método:** PUT
   - **Cuerpo (raw, JSON):**
     ```json
     {
       "id": 1,
       "nombre": "Producto Actualizado",
       "precio": 189.99,
       "stock": 20,
       "idProveedor": 1,
       "idCategoria": 2
     }
     ```
   - **Nota:** Reemplaza `{id}` por el identificador del producto a actualizar.
   - **Captura de pantalla:** 


      ![modificar producto](https://github.com/user-attachments/assets/b82684f5-1c11-4800-a9bb-4859e920c891)


24. **Eliminar un Producto**

   - **URL:** https://localhost:7263/api/productos/{id}
   - **Método:** DELETE
   - **Nota:** Reemplaza `{id}` por el identificador del producto a eliminar.
   - **Captura de pantalla:**

      ![eliminar producto](https://github.com/user-attachments/assets/b9f265c6-0f74-420d-81ef-e097b5990bb0)



25. **Obtener Estadisticas de Productos**

   - **URL:** https://localhost:7263/api/productos/estadistica
   - **Método:** GET
   - **Nota:**  \- `ProductoConPrecioMaximo`: Producto con el precio más alto  \- `ProductoConPrecioMinimo`: Producto con el precio más bajo  \- `SumaTotalPrecios`: Suma total de precios de todos los productos  \- `PrecioPromedio`: Precio promedio de los productos
   - **Captura de pantalla:** 

      ![estadistica productos](https://github.com/user-attachments/assets/4231984d-6c63-4406-84fa-83ab80dfe75b)


26. **Obtener Total de Productos**

   - **URL:** https://localhost:7263/api/productos/total
   - **Método:** GET
   - **Nota:** Devuelve el total de productos registrados en la base de datos en el campo `TotalProductosRegistrados`.
   - **Captura de pantalla:** 

      ![total productos](https://github.com/user-attachments/assets/ca1cbb46-9f06-4f8c-bb24-9709b6a57c0b)


27. **Obtener Productos por Categoría**

   - **URL:** https://localhost:7263/api/productos/categoria/{categoriaId}
   - **Método:** GET
   - **Nota:** Reemplaza `{categoriaId}` por el identificador de la categoría. Se requiere autenticación.
   - **Captura de pantalla:** 

      ![categoria producto](https://github.com/user-attachments/assets/b46020a4-50c5-4950-abf9-126ad3e72dc6)


28. **Obtener Productos por Proveedor**

   - **URL:** https://localhost:7263/api/productos/proveedor/{proveedorId}
   - **Método:** GET
   - **Nota:** Reemplaza `{proveedorId}` por el identificador del proveedor. Se requiere autenticación.
   - **Captura de pantalla:** 

      ![proveedor producto](https://github.com/user-attachments/assets/2914e30c-ad3c-41a2-8d68-3ad8c5599757)

---

29. **Obtener Log con registro de usuarios**
    - **URL:** https://localhost:7263/api/Log
    - **Método:** GET
    - **Captura de pantalla**
   
      
   ![GetLog](https://github.com/user-attachments/assets/35faf7f5-7909-42fc-9fc6-43ab85c974c9)

   ![ArchivoLog](https://github.com/user-attachments/assets/ecb40606-03f9-432e-838a-133f7324d08c)



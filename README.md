# Proyecto .NET - Backend

Este proyecto fue desarrollado para la empresa colombiana **Ff Shipping S.A.S.**, con el objetivo de crear su página web y proporcionar un sistema de control total de inventario. La solución consta de un **frontend en Angular** y un **backend robusto en .NET**, desplegado en **Kubernetes** para garantizar escalabilidad y estabilidad.

## 🚀 Tecnologías utilizadas

### Backend (.NET + Kubernetes)
- **ASP.NET Core** para la lógica del servidor  
- **Automapper** para el mapeo de clases DTO  
- **Entity Framework Core** y **LINQ** para la gestión de bases de datos  
- **Soporte para MySQL y SQL Server**  
- **ASP.NET Identity Core** para autenticación y control de usuarios  
- **JWT (JSON Web Tokens)** para autenticación segura integrada con el cliente  
- **Middleware** personalizado para gestión de excepciones  
- **Patrón Repository** para una arquitectura modular y escalable  
- **Docker** para la creación de imágenes del backend  
- **Kubernetes** para la orquestación de contenedores de **SQL Server** y **.NET**  
- **Despliegue en AWS** para alta disponibilidad y rendimiento  

## 📌 Instalación y configuración

### 🔹 Requisitos previos
Asegúrate de tener instalados:  
- [.NET SDK](https://dotnet.microsoft.com/)  
- [Docker](https://www.docker.com/)  
- [Kubernetes](https://kubernetes.io/)  
- [MySQL](https://www.mysql.com/) o [SQL Server](https://www.microsoft.com/en-us/sql-server)  
- [AWS CLI](https://aws.amazon.com/cli/) (si deseas desplegar en AWS)  

### 🔹 Clonación del repositorio
```bash
git clone [https://github.com/camjor/netKubernetes.git]
cd tu-repositorio
```
## 📌 Configuración de la base de datos
Edita el archivo `appsettings.json` y agrega la conexión a MySQL o SQL Server:  
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=tu-servidor;Database=tu-base-de-datos;User Id=tu-usuario;Password=tu-contraseña;"
}
```
### Ejecutar migraciones y base de datos
Ejecuta los siguientes comandos para aplicar las migraciones y generar la base de datos:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
### Ejecutar el backend en desarrollo
Para ejecutar el backend en modo desarrollo, usa el siguiente comando:
```bash
dotnet run
```
El backend estará disponible en http://localhost:5000.
## Despliegue con Docker y Kubernetes
### Crear imagen Docker
```bash
docker build -t mi-backend .
```
### Ejecutar contenedor
```bash
docker run -p 5000:5000 mi-backend
```
### Orquestar con Kubernetes
```bash
kubectl apply -f deployment.yaml
```
###  Seguridad y Autenticación
Este backend implementa ASP.NET Identity Core y JWT (JSON Web Tokens) para garantizar autenticación y autorización seguras.


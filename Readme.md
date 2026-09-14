**MotoPOS**



Sistema de Punto de Venta (POS) y gestión de inventario desarrollado para un cliente propietario de una tienda de repuestos para motocicletas.



MotoPOS es una API REST desarrollada con .NET, cuyo objetivo es centralizar la gestión de productos, clientes, proveedores, ventas e inventario, proporcionando una base para una futura aplicación web.



**Descripción**



MotoPOS nació como un proyecto de desarrollo para un cliente propietario de una tienda de repuestos para motocicletas que no contaba con un sistema para gestionar de forma centralizada su inventario y las ventas del negocio.



A partir de esta necesidad se planteó el desarrollo de un sistema POS que permita organizar y controlar las principales operaciones del negocio, incluyendo la gestión de productos, clientes, proveedores, ventas e inventario.



El proyecto se encuentra actualmente en desarrollo y está siendo construido aplicando una arquitectura por capas, separación de responsabilidades y buenas prácticas de desarrollo de software.

## Configuración

### Variables de Entorno Requeridas

El proyecto requiere las siguientes variables de entorno para funcionar correctamente:

**Para autenticación JWT:**
- `JwtSettings__SecretKey`: Clave secreta para firmar tokens JWT (mínimo 32 bytes). Generar con comando PowerShell:
  ```powershell
  $bytes = New-Object byte[] 32
  [Security.Cryptography.RNGCryptoServiceProvider]::Create().GetBytes($bytes)
  [System.Convert]::ToBase64String($bytes)
  ```

**Para base de datos (producción):**
- `DB_SERVER`: Servidor MySQL de producción
- `DB_PORT`: Puerto MySQL (default: 3306)
- `DB_NAME`: Nombre de la base de datos
- `DB_USER`: Usuario MySQL con privilegios mínimos
- `DB_PASSWORD`: Contraseña del usuario MySQL

### Configuración por Ambiente

- **Desarrollo**: Usa `appsettings.json` con conexión local a MySQL
- **Producción**: Usa `appsettings.Production.json` con variables de entorno o valores reemplazados

**IMPORTANTE**: Nunca incluir contraseñas reales o secretos en archivos versionados. Usar variables de entorno o herramientas de gestión de secretos (Azure Key Vault, AWS Secrets Manager, etc.).


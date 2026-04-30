# Guía de Instalación y Ejecución Local (Ticketinador2000)

Esta guía detalla los pasos necesarios para levantar todo el proyecto en un entorno local, tanto el backend (.NET) como el frontend (React + Vite).

---

## 🛠️ Requisitos Previos

Antes de comenzar, asegúrate de tener instalado lo siguiente en tu máquina:

1. **[.NET SDK](https://dotnet.microsoft.com/download)**: Requerido para compilar y ejecutar el backend.
2. **[Node.js](https://nodejs.org/)** (versión 18+ recomendada): Requerido para instalar las dependencias del frontend y ejecutar el servidor de Vite.
3. **SQL Server LocalDB**: Generalmente se instala junto con Visual Studio. El backend está configurado para conectarse a `(localdb)\MSSQLLocalDB`.

---

## 🗄️ 1. Configurar la Base de Datos

El proyecto utiliza Entity Framework Core y está programado para **crear la base de datos automáticamente** (junto con las tablas y datos iniciales) la primera vez que se ejecuta el Backend.

### Opciones de Motor:

- **Opción A (Por defecto - LocalDB):** Si tienes Visual Studio instalado, ya tienes LocalDB. **No tienes que hacer nada**. El proyecto se conectará a `(localdb)\MSSQLLocalDB` y creará la BD `Ticketinador2000Db`.
- **Opción B (SQL Server Express / Docker):** Si prefieres usar una instancia de SQL Server propia (por ejemplo, corriendo en Docker o SSMS), debes abrir el archivo `Api/appsettings.json` y modificar la cadena de conexión:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Ticketinador2000Db;User Id=sa;Password=TuPassword123!;TrustServerCertificate=True;"
  }
  ```

---

## ⚙️ 2. Levantar el Backend (API REST)

El backend está construido con la arquitectura Clean Architecture. **No es necesario correr migraciones manualmente**, ya que la aplicación llamará a `EnsureCreated` y `DbInitializer` automáticamente al arrancar.

### Pasos:

1. Abre una terminal.
2. Navega al directorio del proyecto API:
   ```bash
   cd ruta/a/Ticketinador2000/Api
   ```
3. Ejecuta la aplicación:
   ```bash
   dotnet run
   ```
4. **¿Qué sucede al ejecutar?**
   - Se creará automáticamente la base de datos `Ticketinador2000Db` en tu LocalDB.
   - Se insertarán datos iniciales (Usuarios, Eventos, Sectores y Asientos).
   - La consola mostrará la URL donde se está ejecutando la API (por ejemplo, `https://localhost:7123` o similar).
5. **Swagger UI**: Puedes acceder a la documentación de la API agregando `/swagger` a la URL base en tu navegador (ej: `https://localhost:<puerto>/swagger`).

> [!NOTE]
> La conexión a la base de datos está definida en `Api/appsettings.json`. Si no tienes LocalDB y usas SQL Server estándar, deberás cambiar la cadena de conexión allí.

---

## 💻 3. Levantar el Frontend (Cliente Web)

El frontend es una aplicación React construida con Vite, la cual se comunica asíncronamente con nuestra API REST.

### Pasos:

1. Abre **otra** terminal (manteniendo la del backend abierta).
2. Navega al directorio del cliente web:
   ```bash
   cd ruta/a/Ticketinador2000/TicketingClient
   ```
3. Instala todas las dependencias necesarias de Node:
   ```bash
   npm install
   ```
4. Inicia el servidor de desarrollo de Vite:
   ```bash
   npm run dev
   ```
5. La terminal te mostrará una URL local (generalmente `http://localhost:5173/`). Haz ctrl+clic o abre esa URL en tu navegador para usar la aplicación.

> [!TIP]
> Recuerda que para que el frontend funcione correctamente y pueda realizar reservaciones o ver el catálogo, el **Backend debe estar en ejecución simultáneamente**.

---

## 📋 Resumen de Comandos Rápidos

Si ya tienes todo configurado y sólo quieres levantar el proyecto en tu día a día:

**Terminal 1 (Backend):**
```bash
cd Api
dotnet run
```

**Terminal 2 (Frontend):**
```bash
cd TicketingClient
npm run dev
```

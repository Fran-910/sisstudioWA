# sisstudioWA

Tienda online (e-commerce) desarrollada en **.NET 10** para vender **productos individuales y kits** (paquetes compuestos por varios productos). Incluye el dominio completo de una tienda: usuarios, productos, kits, carritos, pedidos, notificaciones de reposición de stock y envíos.

> 📄 Documento de referencia: `Modelado - Franco Lionel Aguirre (1).pdf`
> Autor: **Franco Lionel Aguirre** · Repo: `https://github.com/Fran-910/sisstudioWA.git`

---

## Stack tecnológico

| Componente | Tecnología |
|---|---|
| Frontend | Blazor WebAssembly (componentes Razor) |
| Backend | ASP.NET Core Web API |
| ORM | Entity Framework Core 10 |
| Base de datos | SQL Server LocalDB (`sisstudioBD`) |
| API docs | Swagger / Swashbuckle (solo en Development) |
| Arquitectura | Capas + patrón Repositorio genérico |

### Prerrequisitos

- .NET 10 SDK
- SQL Server LocalDB (`(localdb)\MSSQLLocalDB`)

### Cómo ejecutar

```bash
# Restaurar paquetes
dotnet restore

# Crear/aplicar migraciones y levantar la BD
dotnet ef database update

# Ejecutar (el Server hostea la API + el cliente Blazor)
dotnet run --project sisstudioWA.Server
```

Swagger queda disponible en Development en `/swagger`.

---

## Estructura del proyecto

Solución `sisstudioWA.slnx` con 6 proyectos.

**Flujo de capas:**

```
Cliente (Blazor) → Servicio (HttpService/HttpResp) → [HTTP] → Controller → Repositorio → BD
```

- El **cliente** consume los **servicios**
- El **servicio** envuelve el `HttpClient` y invoca al **controller** correspondiente vía HTTP
- El **controller** utiliza los **repositorios**
- Los **repositorios** se manejan con la **BD**

```
sisstudioWA/
├── sisstudioWA.BD/            Capa de datos (AppDbContext, entidades, Migrations)
├── sisstudioWA.Repositorio/   Repositorio genérico + repositorios específicos
├── sisstudioWA.Servicios/     Capa de servicios: wrapper genérico de HttpClient
│   └── ServicioHTTP/
│       ├── IHttpService.cs
│       ├── HttpService.cs
│       └── HttpResp.cs
│       (patrón tomado de https://github.com/Sergio-Algorry/Proyecto2026WA)
├── sisstudioWA.Shared/        DTOs y Enums compartidos
├── sisstudioWA.Client/        Frontend Blazor WebAssembly
└── sisstudioWA.Server/        Web API host + Controllers
```

**Modelo de datos (8 tablas):** `Usuarios`, `Productos`, `KitsProductos`, `Pedidos`, `DetallesPedidos`, `Carritos`, `DetallesCarritos`, `NotificacionesStocks`.

---

## Funcionalidades implementadas

- ✅ Modelo de datos con DataAnnotations, FKs y navegaciones + migración `Inicio` (`20260923170626`)
- ✅ Repositorio genérico CRUD (`Repositorio<T>`) + `ProductoRepositorio`
- ✅ API REST `api/producto`:
  - `GET /api/producto` — lista pública (DTO sin stock, expone `HayStock`)
  - `GET /api/producto/admin` — lista completa para administración
  - `GET /api/producto/{id}` — producto por id
  - `POST /api/producto` — crear producto (soporta kits con sus componentes)
  - `PUT /api/producto/{id}` — actualizar producto
  - `DELETE /api/producto/{id}` — eliminar producto
  - `GET /api/producto/kit/{id}` — componentes de un kit
- ✅ Serialización de enums como texto en JSON/Swagger
- ✅ Hosting all-in-one: el Server sirve el cliente Blazor WASM

---

## Roadmap / Tareas (Trello)

Cada tarea tiene un **número de ID** (para referenciarla en commits/PRs), un **objetivo**, **criterios de aceptación** (checklist para darla por terminada) y una **prioridad**. Copia el contenido de cada tarjeta tal cual a Trello.

**Estado general:** `Por hacer: 11` · `En curso: 0` · `Hecho: 6` · **Progreso: 6/17 (35%)**

### 📌 Por hacer

#### T01 — Capa de Servicios (ServicioHTTP) `*(Prioridad: Alta)*`
- **Objetivo:** Implementar la capa `sisstudioWA.Servicios` replicando el patrón de `https://github.com/Sergio-Algorry/Proyecto2026WA` (`Proyecto2026WA.Servicio/ServicioHTTP`): un wrapper genérico de `HttpClient` que el cliente Blazor usa para invocar a los controllers vía HTTP.
- **Criterios de aceptación:**
  - [ ] Creada la carpeta `sisstudioWA.Servicios/ServicioHTTP/` con `IHttpService.cs`, `HttpService.cs` y `HttpResp.cs`
  - [ ] `IHttpService` con `Task<HttpResp<T>> GetAsync<T>(string url)`
  - [ ] `HttpService` implementa `GetAsync<T>` y además `PostAsync<T, TResp>` (JSON serialize/deserialize con `System.Net.Http.Json`)
  - [ ] `HttpResp<T>` con propiedades `Error`, `Respuesta`, `Response`, `Mensaje` (mensaje derivado del StatusCode)
  - [ ] `sisstudioWA.Servicios.csproj` referenciando `sisstudioWA.Shared`
  - [ ] Registrado en DI del cliente: `HttpClient` (con `BaseAddress` = URL base de la API) + `IHttpService`/`HttpService`
  - [ ] El cliente Blazor consume los controllers vía `IHttpService` (ej. `GetAsync<ProductoPublicoDTO>("api/producto")`)
  - [ ] La solución compila sin warnings nuevos y el CRUD de productos sigue funcionando

#### T03 — Controller de Usuarios `*(Prioridad: Alta)*`
- **Objetivo:** Exponer la gestión de usuarios (alta, consulta, login) vía API.
- **Criterios de aceptación:**
  - [ ] `UsuarioController` con: `GET`, `GET /{id}`, `POST` (registro), `PUT /{id}`, `DELETE /{id}`
  - [ ] Endpoint `POST /api/usuario/login` que valida usuario/contraseña y devuelve un resultado estandarizado
  - [ ] Contraseñas nunca devueltas en texto plano en las respuestas (DTO sin password)
  - [ ] Validaciones de `[Required]`/`[StringLength]` responden 400 con mensaje claro

#### T04 — Controller de Pedidos `*(Prioridad: Alta)*`
- **Objetivo:** CRUD de pedidos y sus detalles, incluyendo cambio de estado.
- **Criterios de aceptación:**
  - [ ] `PedidoController` con `GET`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}`
  - [ ] `DetallePedidoController` (o sub-recursos `POST /api/pedido/{id}/detalle`) para las líneas del pedido
  - [ ] `POST` de pedido permite recibir los detalles y los persiste junto con la cabecera
  - [ ] Se puede filtrar pedidos por usuario y por `TipoEstadoPedido`

#### T05 — Controller de Carritos `*(Prioridad: Alta)*`
- **Objetivo:** Habilitar el carrito de compras por usuario (agregar, modificar, vaciar).
- **Criterios de aceptación:**
  - [ ] `CarritoController` con `GET /api/carrito/usuario/{idUsuario}`, `POST`, `PUT /{id}`, `DELETE /{id}`
  - [ ] Alta/baja de `DetalleCarrito` (agregar producto, cambiar cantidad, quitar)
  - [ ] `DELETE /api/carrito/vaciar/{idUsuario}` limpia todos los detalles
  - [ ] No permite cantidades ≤ 0 ni agregar producto sin stock

#### T06 — Autenticación y autorización `*(Prioridad: Alta)*`
- **Objetivo:** Proteger los endpoints administrativos y diferenciar rol usuario/admin.
- **Criterios de aceptación:**
  - [ ] Endpoints `admin`, `POST/PUT/DELETE` requieren usuario autenticado (o rol admin)
  - [ ] Los `GET` públicos del catálogo siguen accesibles sin login
  - [ ] Un request sin token/rol adecuado recibe 401/403
  - [ ] El login del T03 emite las credenciales/token usados por esta tarea

#### T07 — Catálogo en Blazor `*(Prioridad: Media)*`
- **Objetivo:** UI pública para navegar y ver productos en `sisstudioWA.Client`, consumiendo la API mediante `IHttpService`.
- **Criterios de aceptación:**
  - [ ] Página `Catalogo` que lista productos vía `GetAsync<ProductoPublicoDTO[]>("api/producto")`
  - [ ] Página `Detalle/{id}` con info del producto y, si es kit, sus componentes (`GetAsync<...>("api/producto/kit/{id}")`)
  - [ ] Las páginas inyectan `IHttpService` (no `HttpClient` ni repositorios)
  - [ ] Si `HttpResp<T>.Error` es true se muestra el `Mensaje`; producto inexistente → estado NotFound
  - [ ] Indicador de `HayStock` visible (sin exponer el número de stock)
  - [ ] Manejo de estado de carga y de error (producto inexistente → NotFound)

#### T08 — Carrito y checkout en la UI `*(Prioridad: Media)*`
- **Objetivo:** Flujo completo de compra: agregar al carrito, revisar y confirmar pedido, usando `IHttpService`.
- **Criterios de aceptación:**
  - [ ] Botón "Agregar al carrito" en detalle de producto
  - [ ] Página `Carrito` con cantidades editables, quitar ítems y total calculado
  - [ ] "Confirmar pedido" crea `Pedido` + `DetallesPedidos` vía API y vacía el carrito
  - [ ] Los `POST/PUT` usan `PostAsync<T, TResp>` y se evalúa `HttpResp<TResp>.Error`
  - [ ] Carrito persiste tras recargar (asociado al usuario o a storage local)

#### T09 — Gestión de kits en el admin `*(Prioridad: Media)*`
- **Objetivo:** Crear y editar kits con sus componentes desde la UI de administración.
- **Criterios de aceptación:**
  - [ ] Formulario de producto con selector `TipoProd = Kit`
  - [ ] Al elegir Kit, se pueden agregar/quitar productos componentes con cantidades
  - [ ] El `POST/PUT` envía el modelo y la API persiste los `KitProducto` (hoy solo `InsertKit` en alta)
  - [ ] Editar un kit reemplaza sus componentes de forma consistente (sin huérfanos)

#### T10 — Controller de Notificaciones de stock `*(Prioridad: Media)*`
- **Objetivo:** API para las alertas de reposición de stock.
- **Criterios de aceptación:**
  - [ ] `NotificacionStockController` con `GET`, `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}`
  - [ ] Endpoint `GET /api/notificacionstock/pendientes` (filtros por `TipoEstadoNotificacion`)
  - [ ] `PUT /{id}/resolver` marca la notificación como atendida
  - [ ] Flujo documentado: qué dispara una notificación (stock bajo / producto sin stock)

#### T11 — Manejo de errores y validaciones en la API `*(Prioridad: Baja)*`
- **Objetivo:** Respuestas HTTP consistentes en todos los endpoints.
- **Criterios de aceptación:**
  - [ ] 400 con mensajes de validación en body para datos inválidos
  - [ ] 404 con mensaje claro cuando el recurso no existe (id inexistente)
  - [ ] 500 no filtra stack trace en producción (middleware de excepciones ya presente)
  - [ ] Documentados en Swagger los códigos de respuesta de `api/producto`

#### T12 — Limpieza del repo `*(Prioridad: Baja)*`
- **Objetivo:** Dejar el repositorio sin archivos ni código muerto.
- **Criterios de aceptación:**
  - [ ] Eliminado `sisstudioWA.Shared/jesus.jpg` del repo (no solo del working tree)
  - [ ] Eliminado código comentado muerto (`OldContextDb`/`OldController` en `ProductoController`, `Exists` en interfaces)
  - [ ] Commit del `Modelado - ...pdf` o agregado al `.gitignore` (definir cuál)
  - [ ] `git status` limpio después de los cambios

### 🔄 En curso

- *(vacío por ahora)*

### ✅ Hecho

#### T13 — Inicialización del repo y solución `*(Prioridad: Alta)*` ✅
- **Objetivo:** Repo git + solución Blazor/ASP.NET all-in-one funcionando. **(Commits `00ff062`, `b3fd3d2`)**

#### T14 — Modelo de datos y migración inicial `*(Prioridad: Alta)*` ✅
- **Objetivo:** 8 entidades con DataAnnotations, enums y migración `Inicio`. **(Commit `dcbfc60`)**

#### T15 — Repositorio genérico `*(Prioridad: Alta)*` ✅
- **Objetivo:** `Repositorio<T>` (Select/SelectById/Insert/Update/Delete) + `ProductoRepositorio` con `InsertKit` y `GetProductosKitById`. **(Commits `dcbfc60`, `32f9db1`)**

#### T16 — CRUD de productos con soporte de kits `*(Prioridad: Alta)*` ✅
- **Objetivo:** `ProductoController` completo: lista pública/admin, por id, crear (alta de kit con componentes), actualizar, eliminar, componentes del kit. **(Commits `dcbfc60`, `32f9db1`)**

#### T17 — Swagger y serialización de enums `*(Prioridad: Media)*` ✅
- **Objetivo:** Swagger en Development + `JsonStringEnumConverter` para enums en JSON/Swagger. **(Commit `dcbfc60`)**

#### T18 — Relaciones FK en el modelo `*(Prioridad: Alta)*` ✅
- **Objetivo:** Definir claves foráneas y navegaciones entre las 8 tablas para que la integridad referencial la garantice la BD.
- **Criterios de aceptación:**
  - [x] FK + navegación en: `Pedido.UsuarioId`, `DetallePedido.PedidoId`, `DetallePedido.ProductoId`, `Carrito.UsuarioId`, `DetalleCarrito.CarritoId`, `DetalleCarrito.ProductoId`, `KitProducto.KitId`, `KitProducto.ProductoId`, `NotificacionStock.UsuarioId`, `NotificacionStock.ProductoId`
  - [x] `KitProducto` = tabla intermedia del M–N: 1 kit tiene muchos productos y 1 producto pertenece a muchos kits (2 relaciones 1–N hacia `Producto` configuradas en `OnModelCreating`: `KitId` Cascade, `ProductoId` Restrict)
  - [x] Migración nueva `20260923170626_Inicio` generada y BD creada sin errores
  - [x] No se rompen los endpoints existentes de `api/producto` (`ProductoRepositorio` adaptado a `KitId`/`ProductoId`)
- **Nota:** se regeneró la migración desde cero porque el esquema anterior estaba desincronizado (columnas `idUsuario`/`idKit` sin FK reales).

---

## Avances hasta el momento

**Estado del repo:** rama `master` sincronizada con `origin/master`, working tree limpio (salvo el PDF de modelado sin trackear). 5 commits:

| Fecha | Commit | Descripción |
|---|---|---|
| 2026-06-09 | `00ff062` | Agregar .gitattributes y .gitignore |
| 2026-06-09 | `b3fd3d2` | Agregar archivos de proyecto (esqueleto Blazor) |
| 2026-08-12 | `dcbfc60` | Dominio: entidades, enums, migración, repositorio y endpoints básicos |
| 2026-08-20 | `32f9db1` | `ProductoRepositorio` + endpoints completos de producto (CRUD + kits) |
| 2026-08-20 | `10727ca` | Prueba de configuración de git |

**Dónde estamos:** arquitectura en capas montada, modelo de datos completo y CRUD de productos funcional con soporte de kits. Falta la capa de Servicios, el resto de controllers, la UI de Blazor y la autenticación.

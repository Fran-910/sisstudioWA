# AGENTS.md

Notas específicas de este repo para no romper el flujo existente. La UI, los mensajes y los comentarios están en español: mantené ese idioma.

## Stack y entrypoint

- .NET 10 (`net10.0`), solución `sisstudioWA.slnx` (no hay `.sln`).
- Blazor WebAssembly **hosteado por** `sisstudioWA.Server`; el Server expone la API y sirve el cliente.
- SQL Server LocalDB `(localdb)\MSSQLLocalDB`, base `sisstudioBD`.
- Flujo de capas: `Client` → `IHttpService` → HTTP → `Server/Controllers` → `Repositorio` → `AppDbContext`.

## Comandos

```bash
dotnet build sisstudioWA.slnx
dotnet run --project sisstudioWA.Server
```

- El Server queda en `http://localhost:5180` (perfil `https`: `https://localhost:7056`).
- Swagger: `/swagger`, solo en Development.
- **No corras `sisstudioWA.Client` solo** para probar la app: su `HttpClient` usa `HostEnvironment.BaseAddress` (su propio origen), no hay proxy al API y las llamadas fallan con 404. Usá siempre el Server.
- No hay proyecto de tests, CI (`.github/workflows` vacío), linter ni formatter. La verificación disponible es `dotnet build`.
- El build hoy termina con 0 errores y ~44 warnings (nullable CS8618/CS8602 y vulnerabilidades NuGet NU1902/NU1903). No persigas warnings ajenos al cambio.

## Base de datos y migraciones

La app **no** migra ni siembra la base al iniciar. En una máquina nueva:

```bash
dotnet ef database update --project sisstudioWA.BD --startup-project sisstudioWA.Server
```

Para una migración nueva:

```bash
dotnet ef migrations add <Nombre> --project sisstudioWA.BD --startup-project sisstudioWA.Server
```

- EF Tools 10.0.12 está instalado globalmente.
- Los flags `--project`/`--startup-project` son obligatorios: `dotnet ef database update` a secas falla con “No project was found” (el README está desactualizado en esto).
- Migraciones en `sisstudioWA.BD/Migrations`; la última es `20261009193450_ImagenesNullable`.
- `Producto.Imagenes` es `List<string>` y EF la persiste como JSON en una única columna `nvarchar(max)`. No asumas una tabla de imágenes.
- `KitProducto` no tiene columna de cantidad: la cantidad se representa repitiendo filas `(KitId, ProductoId)`.

## Quirks del API de productos

- El alta y la edición **no son JSON**: son `multipart/form-data` con una parte `producto` que contiene un string JSON y partes `imagenes` con los archivos.
  - `POST api/producto/crear`
  - `PUT api/producto/{id}` y `PUT api/producto/kit/{id}`
  - `DELETE api/producto/borrar/{id}`
  - `DELETE api/producto/delimg` recibe JSON en el body: `{ "id": 1, "url": ["/uploads/..."] }`
- `GET api/producto/admin` devuelve entidades `Producto`, no `ProductoPrivadoDTO`. Funciona porque el JSON es case-insensitive (`Id` matchea `id`). No renombres los miembros de los DTO (`id`, `tipoProd`, `url`) sin actualizar todas las páginas.
- `ProductoPublicoDTO` hoy **no** incluye `Id` ni `Imagenes`; el endpoint público no las completa. No hay página de catálogo público: `Home.razor` redirige `/` a `/productosadmin`.
- Los enums se serializan como texto (`JsonStringEnumConverter` global y en `TipoProd`). Mantené los nombres de `TipoProd` estables.
- El guardado de imágenes está duplicado en las ramas `Producto` y `Kit` de `ProductoController`; un cambio de uploads suele requerir tocar ambas.
- `HttpService.PostFormDataAsync`/`UpdateFormDataAsync` devuelven `bool` y descartan el mensaje de error del API; si necesitás mostrar ese mensaje, hay que cambiar la capa de servicios.

## Uploads

- Las imágenes se guardan en `sisstudioWA.Server/Storage/Productos/{productoId}/`, no en `wwwroot`.
- Se sirven en `/uploads/...` mediante `UseStaticFiles` configurado en `Server/Program.cs`.
- La ruta sale de `Storage:ProductosPath` en `appsettings.json` (relativa al content root del Server; también acepta absoluta).
- Esa carpeta no está en `.gitignore`: no commitees imágenes generadas al probar.
- El cliente genera previews base64 para la UI, pero al servidor solo envía los archivos reales.

## Limitaciones actuales (no asumas que ya existen)

- **No hay autenticación ni autorización.** `/productosadmin`, el alta, la edición y el borrado son públicos. `UsuarioController` devuelve la entidad `Usuario` completa, incluida `Contraseña`.
- `PedidoController` y `CarritoController` existen, pero `IPedidoRepositorio` e `ICarritoRepositorio` **no están registrados** en `Server/Program.cs`: esos endpoints fallan por DI. Si tocás esa área, registralos.
- `Repositorio<T>.SelectById` lanza excepción cuando no encuentra el registro. Los `if (x != null)` de los controllers son código muerto y un ID inexistente termina en 500, no en 404.
- `Repositorio<T>.Update` copia todas las propiedades por reflexión (excepto `Id`); puede pisar navegaciones. Revisá antes de usarlo con entidades que tengan relaciones.
- El alta de producto inserta, luego guarda imágenes y después hace un `Update`, sin transacción: si falla un paso pueden quedar productos o archivos parciales.
- El alta no valida el DTO en el servidor: las DataAnnotations de la entidad no se aplican al JSON deserializado a mano. Precio/stock negativos, nombre vacío o alta sin imágenes hoy pasan.
- No hay catálogo público ni carrusel de imágenes todavía.
- El `README.md` está desactualizado en varios puntos (nombre de migración, rutas de la API y estado de tareas). Confiá en el código y en el build antes que en el README.

## Convenciones

- No inyectes `HttpClient` ni repositorios en páginas/componentes Blazor: usá `IHttpService`.
- DTOs y enums compartidos van en `sisstudioWA.Shared`; la lógica de acceso a datos en `sisstudioWA.Repositorio`; los controllers deben quedar finos.
- `sisstudioWA.Shared` referencia `Microsoft.AspNetCore.Components.Web` solo por `ImagenSeleccionada` (usa `IBrowserFile`).
- Todos los proyectos tienen `Nullable` e `ImplicitUsings` habilitados; hay warnings de nulabilidad preexistentes.
- Después de cada FEATURE nueva (en modo build), ejecutar las herramientas de prueba Chrome Dev Tools, evaluando su funcionamiento en mobile y pc para asegurar que se vea y funcione correctamente.

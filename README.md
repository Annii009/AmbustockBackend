# 🚑 AmbustockBackend

API REST en **.NET 8** para la gestión de inventario de suministros médicos en ambulancias.

---

## 🚀 Cómo arrancar el backend

El proyecto está dockerizado. Solo necesitas tener **Docker Desktop** instalado.

### Levantar todo (BD + API)

```bash
docker compose up --build
```

Esto arranca automáticamente:
1. **SQL Server** en el puerto `8308`
2. **Init-db** — ejecuta el script SQL de inicialización
3. **Backend** — la API en `http://localhost:5002`

### Parar y limpiar

```bash
# Parar los contenedores
docker compose down

# Si la base de datos se queda pillada, resetear volúmenes
docker compose down -v
docker compose up --build
```

### Sin Docker (desarrollo local)

```bash
dotnet build
dotnet run
```

> Asegúrate de tener SQL Server corriendo y la cadena de conexión configurada en `appsettings.json`.

---

## 🧪 Tests unitarios

El proyecto incluye una suite de **38 tests unitarios** en `AmbuStock.Tests/` que cubre la lógica de negocio de `MaterialService` y `RevisionService`.

### Stack de testing

| Librería | Propósito |
|---|---|
| **xUnit** | Framework de tests |
| **Moq** | Mocking de repositorios e interfaces |
| **FluentAssertions** | Asserts legibles |
| **Coverlet** | Medición de cobertura de código |

### Ejecutar los tests

```bash
dotnet test
```

Resultado esperado:
```
Passed!  - Failed: 0, Passed: 38, Skipped: 0, Total: 38
```

### Generar reporte de cobertura

```bash
# Ejecutar con cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=./coverage/

# Instalar ReportGenerator (solo la primera vez)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Generar HTML
reportgenerator -reports:"AmbuStock.Tests/coverage/coverage.cobertura.xml" -targetdir:"AmbuStock.Tests/coverage/report" -reporttypes:Html

# en mac
~/.dotnet/tools/reportgenerator -reports:"AmbuStock.Tests/coverage/coverage.cobertura.xml" -targetdir:"AmbuStock.Tests/coverage/report" -reporttypes:Html

# Abrir en el navegador
start AmbuStock.Tests/coverage/report/index.html
```

# en mac
open AmbuStock.Tests/coverage/report/index.html

### Resultados de cobertura

Los tests cubren únicamente los Services (la capa de lógica de negocio). El porcentaje global puede parecer bajo porque Coverlet mide todo el ensamblado, pero filtrando por los services testados los resultados son:

| Clase | Cobertura de líneas | Cobertura de ramas |
|---|---|---|
| **MaterialService** | **95.8%** ✅ | **90%** ✅ |
| **RevisionService** | **75.1%** ✅ | **82.5%** ✅ |
| Resto del proyecto | 0% | 0% |

> Los controllers, repositories, DTOs y modelos no tienen tests unitarios porque dependen directamente de la base de datos. Para cubrirlos harían falta tests de integración.

### Técnicas de testing aplicadas

- **Particiones equivalentes** — cada método tiene tests para datos válidos, inválidos y límite
- **Valores límite** — id=0, id negativo, cantidad=0, publicId null/vacío
- **Cobertura de ramas true/false** — cada `if` del código tiene su test para el camino verdadero y el falso
- **Refactorización SOLID** — se extrajo `ICloudinaryService` del `CloudinaryService` concreto aplicando el principio de Inversión de Dependencias (D), lo que permite mockear Cloudinary en los tests sin conectarse al servicio real

### Estructura del proyecto de tests

```
AmbuStock.Tests/
├── AmbuStock.Tests.csproj
├── Helpers/
│   └── TestDataBuilder.cs        ← fábrica de objetos de prueba
└── Services/
    ├── MaterialServiceTests.cs   ← 26 tests
    └── RevisionServiceTests.cs   ← 12 tests
```

---

## 📁 Estructura del proyecto

```
AmbustockBackend/
├── Controllers/     ← endpoints HTTP
├── Services/        ← lógica de negocio (testada)
├── Repositories/    ← acceso a datos (SQL directo)
├── Models/          ← entidades de la BD
├── Dtos/            ← objetos de transferencia
└── SqlServer/       ← scripts de inicialización de BD
```
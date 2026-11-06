-- ==========================================
-- TABLA USERS
-- ==========================================

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(20) NOT NULL DEFAULT 'Estudiante',
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT CK_Users_Rol
        CHECK (Rol IN ('Instructor', 'Estudiante'))
);
GO

-- ==========================================
-- TABLA CURSOS
-- ==========================================

CREATE TABLE Cursos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(MAX) NULL,
    Duracion INT NOT NULL,
    Materiales NVARCHAR(MAX) NULL,
    Cronograma DATETIME2 NOT NULL,
    Estado BIT NOT NULL DEFAULT 1
);
GO

-- ==========================================
-- TABLA ESTUDIANTES
-- ==========================================

CREATE TABLE Estudiantes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Usuario NVARCHAR(100) NULL,
    Progreso INT NOT NULL DEFAULT 0,
    Calificacion INT NOT NULL DEFAULT 0,
    Retroalimentacion NVARCHAR(MAX) NULL,
    Estado BIT NOT NULL DEFAULT 1
);
GO

-- ==========================================
-- TABLA INSCRIPCIONES
-- ==========================================

CREATE TABLE Inscripciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    CursoId INT NOT NULL,
    FechaInscripcion DATETIME2 NOT NULL DEFAULT GETDATE(),
    Estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Inscripciones_Users
        FOREIGN KEY (UsuarioId)
        REFERENCES Users(Id),

    CONSTRAINT FK_Inscripciones_Cursos
        FOREIGN KEY (CursoId)
        REFERENCES Cursos(Id),

    CONSTRAINT UQ_Inscripciones_Usuario_Curso
        UNIQUE (UsuarioId, CursoId)
);
CREATE DATABASE AmbustockDB;
GO

USE AmbustockDB;
GO

-- Tabla: ambulancia
CREATE TABLE ambulancia (
 Id_ambulancia INT PRIMARY KEY IDENTITY(1,1),
 Nombre NVARCHAR(100) NOT NULL,
 Matricula NVARCHAR(20) NOT NULL UNIQUE
);

-- Tabla: zonas
CREATE TABLE zonas (
 ID_zona INT PRIMARY KEY IDENTITY(1,1),
 nombre_zona NVARCHAR(100) NOT NULL,
 Id_ambulancia INT NOT NULL,
 CONSTRAINT FK_zonas_ambulancia FOREIGN KEY (Id_ambulancia) REFERENCES ambulancia(Id_ambulancia)
);

-- Tabla: cajones
CREATE TABLE cajones (
 Id_cajon INT PRIMARY KEY IDENTITY(1,1),
 Nombre_cajon NVARCHAR(100) NOT NULL,
 Id_zona INT NOT NULL,
 CONSTRAINT FK_cajones_zona FOREIGN KEY (Id_zona) REFERENCES zonas(ID_zona)
);

-- Tabla: materiales
CREATE TABLE materiales (
 Id_material INT PRIMARY KEY IDENTITY(1,1),
 nombre_Producto NVARCHAR(200) NOT NULL,
 cantidad INT NOT NULL DEFAULT 0,
 Id_zona INT NOT NULL,
 Id_cajon INT NULL,
 foto_url NVARCHAR(500) NULL,
 foto_public_id NVARCHAR(200) NULL,
 CONSTRAINT FK_materiales_zona FOREIGN KEY (Id_zona) REFERENCES zonas(ID_zona),
 CONSTRAINT FK_materiales_cajon FOREIGN KEY (Id_cajon) REFERENCES cajones(Id_cajon)
);

-- Tabla: servicio
CREATE TABLE servicio (
 Id_servicio INT PRIMARY KEY IDENTITY(1,1),
 fecha_hora DATETIME NOT NULL,
 nombre_servicio NVARCHAR(200),
 Id_responsable INT NULL
);

-- Tabla: responsable
CREATE TABLE responsable (
 Id_responsable INT PRIMARY KEY IDENTITY(1,1),
 Nombre_Responsable NVARCHAR(100) NOT NULL,
 Fecha_Servicio DATETIME,
 Id_servicio INT NULL,
 Id_usuario INT NULL,
 Id_Reposicion INT NULL
);

-- Tabla: usuarios
CREATE TABLE usuarios (
 Id_usuario INT PRIMARY KEY IDENTITY(1,1),
 Nombre_Usuario NVARCHAR(100) NOT NULL,
 Rol NVARCHAR(50),
 email NVARCHAR(255),
 Password NVARCHAR(255) NOT NULL,
 Id_responsable INT NULL,
 Id_Correo INT NULL
);

-- Tabla: Servicio_Ambulancia
CREATE TABLE Servicio_Ambulancia (
 Id_servicioAmbulancia INT PRIMARY KEY IDENTITY(1,1),
 Id_Ambulancia INT NOT NULL,
 Id_Servicio INT NOT NULL,
 CONSTRAINT FK_ServicioAmb_Ambulancia FOREIGN KEY (Id_Ambulancia) REFERENCES ambulancia(Id_ambulancia),
 CONSTRAINT FK_ServicioAmb_Servicio FOREIGN KEY (Id_Servicio) REFERENCES servicio(Id_servicio)
);

-- Tabla: correo
CREATE TABLE correo (
 Id_Correo INT PRIMARY KEY IDENTITY(1,1),
 fecha_alerta DATETIME,
 tipo_problema NVARCHAR(200),
 Id_material INT,
 Id_usuario INT,
 Id_reposicion INT NULL,
 CONSTRAINT FK_correo_material FOREIGN KEY (Id_material) REFERENCES materiales(Id_material),
 CONSTRAINT FK_correo_usuario FOREIGN KEY (Id_usuario) REFERENCES usuarios(Id_usuario)
);

-- Tabla: Reposicion
CREATE TABLE Reposicion (
 id_reposicion INT PRIMARY KEY IDENTITY(1,1),
 Id_Correo INT NOT NULL,
 Nombre_material NVARCHAR(200),
 Cantidad INT,
 Comentarios NVARCHAR(MAX),
 foto_evidencia VARBINARY(MAX),
 CONSTRAINT FK_Reposicion_correo FOREIGN KEY (Id_Correo) REFERENCES correo(Id_Correo)
);

-- Tabla: Detalle_Correo
CREATE TABLE Detalle_Correo (
 Id_detalleCorreo INT PRIMARY KEY IDENTITY(1,1),
 Id_material INT NOT NULL,
 Id_correo INT NOT NULL,
 CONSTRAINT FK_DetalleCorreo_material FOREIGN KEY (Id_material) REFERENCES materiales(Id_material),
 CONSTRAINT FK_DetalleCorreo_correo FOREIGN KEY (Id_correo) REFERENCES correo(Id_Correo)
);

-- Tabla: Revisiones
CREATE TABLE Revisiones (
 Id_revision INT PRIMARY KEY IDENTITY(1,1),
 Id_ambulancia INT NOT NULL,
 Id_servicio INT NOT NULL,
 Nombre_Responsable NVARCHAR(200) NOT NULL,
 Fecha_Revision DATETIME NOT NULL,
 Total_Materiales INT NOT NULL,
 Materiales_Revisados INT NOT NULL,
 Estado NVARCHAR(50) NOT NULL,
 CONSTRAINT FK_Revisiones_ambulancia FOREIGN KEY (Id_ambulancia) REFERENCES ambulancia(Id_ambulancia),
 CONSTRAINT FK_Revisiones_servicio FOREIGN KEY (Id_servicio) REFERENCES servicio(Id_servicio)
);

-- Agregar Foreign Keys adicionales
ALTER TABLE servicio
ADD CONSTRAINT FK_servicio_responsable FOREIGN KEY (Id_responsable) REFERENCES responsable(Id_responsable);

ALTER TABLE responsable
ADD CONSTRAINT FK_responsable_servicio FOREIGN KEY (Id_servicio) REFERENCES servicio(Id_servicio),
 CONSTRAINT FK_responsable_usuario FOREIGN KEY (Id_usuario) REFERENCES usuarios(Id_usuario),
 CONSTRAINT FK_responsable_reposicion FOREIGN KEY (Id_Reposicion) REFERENCES Reposicion(id_reposicion);

ALTER TABLE usuarios
ADD CONSTRAINT FK_usuarios_responsable FOREIGN KEY (Id_responsable) REFERENCES responsable(Id_responsable),
 CONSTRAINT FK_usuarios_correo FOREIGN KEY (Id_Correo) REFERENCES correo(Id_Correo);

ALTER TABLE correo
ADD CONSTRAINT FK_correo_reposicion FOREIGN KEY (Id_reposicion) REFERENCES Reposicion(id_reposicion);

GO

-- 1) AMBULANCIA
INSERT INTO ambulancia (Nombre, Matricula)
VALUES (N'Ambulancia UVI Movil AM50.2-Z', N'2345-XYZ');

-- 2) ZONAS
INSERT INTO zonas (nombre_zona, Id_ambulancia) VALUES
(N'AMPULARIO MEDICO', 1),
(N'AMPULARIO', 1),
(N'COMPRIMIDOS', 1),
(N'MORFICOS', 1),
(N'NEVERA', 1),
(N'BOTIQUIN PEDIATRICO', 1),
(N'BOTIQUIN IMV', 1),
(N'BOTIQUIN RESPIRATORIO ADULTO', 1),
(N'BOTIQUIN SUTURAS', 1),
(N'BOTIQUIN QUEMADOS', 1),
(N'CAJONES', 1),
(N'ESTANTERIAS', 1);

-- 3) CAJONES
INSERT INTO cajones (Nombre_cajon, Id_zona) VALUES
(N'ARMARIO SUEROS', 11),
(N'CAJON 1', 11),
(N'CAJON 2', 11),
(N'CAJON 3 (ADULTO)', 11),
(N'CAJON 4 (PEDIATRICO)', 11),
(N'CAJON 5', 11),
(N'CAJON 6', 11),
(N'CAJON 7', 11),
(N'KIT INTUBACION ADULTO', 11),
(N'CAJON 10', 11),
(N'CAJON 11', 11),
(N'CAJON 12', 11);

INSERT INTO cajones (Nombre_cajon, Id_zona) VALUES
(N'ESTANTERIA 1', 12),
(N'ESTANTERIA 2', 12),
(N'ESTANTERIA 3', 12),
(N'ESTANTERIA 4', 12),
(N'ESTANTERIA 5', 12),
(N'MONITOR', 12),
(N'SONDAS', 12),
(N'KIT DE PARTO', 12);

-- 4) MATERIALES - Zona 1: AMPULARIO MEDICO
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Adolonta (Tramadol Hidrocloruro)', 1, 1, NULL),
(N'Adrenalina (Epinefrina)', 1, 1, NULL),
(N'Anexate', 1, 1, NULL),
(N'Atropina', 2, 1, NULL),
(N'Actocortina 100 mg', 1, 1, NULL),
(N'Benerva (Tiamina)', 1, 1, NULL),
(N'Dormicum (Midazolam)', 1, 1, NULL),
(N'Enantyum (Dexketoprofeno)', 1, 1, NULL),
(N'Naloxona', 2, 1, NULL),
(N'Nolotil (Metamizol)', 1, 1, NULL),
(N'Polaramire (Dexclorferinamina)', 1, 1, NULL),
(N'Primperam (Metoclopramida)', 2, 1, NULL),
(N'Seguril (Furosemida)', 2, 1, NULL),
(N'Trangorex (Amiodarona)', 1, 1, NULL),
(N'Urbason 20mg (Metilprednisolona)', 1, 1, NULL),
(N'Urbason 40mg (Metilprednisolona)', 1, 1, NULL),
(N'Trandate 5 mg (Labetalol hidrocloruro)', 1, 1, NULL),
(N'Solinitrina 5 mg (Nitroglicerina)', 1, 1, NULL),
(N'Akineton (Biperideno)', 1, 1, NULL),
(N'Amchafibrin 500mg (Acido Tranesamico)', 2, 1, NULL),
(N'Paracetamol 1g', 4, 1, NULL),
(N'Valium compr. 5mg', 2, 1, NULL),
(N'Metamizol comp.', 2, 1, NULL),
(N'Clopidogrel 300 mg', 2, 1, NULL),
(N'AAS 500 mg', 10, 1, NULL),
(N'Captopril 25 mg', 10, 1, NULL),
(N'Orfidal 1mg', 18, 1, NULL),
(N'Digoxina 0,25 mg', 1, 1, NULL),
(N'Dogmatil 50mg', 1, 1, NULL),
(N'Dopamina 200mg', 1, 1, NULL),
(N'Haloperidol', 1, 1, NULL),
(N'Adenocor 6mg (Adenosina)', 2, 1, NULL),
(N'Agua oxigenada', 1, 1, NULL),
(N'Alcohol', 1, 1, NULL),
(N'Suero 30ml', 2, 1, NULL),
(N'Povidona (yodo)', 1, 1, NULL),
(N'Clorehixidina', 1, 1, NULL),
(N'Agujas 21G', 6, 1, NULL),
(N'Agujas 25G', 5, 1, NULL),
(N'Agujas 20G', 5, 1, NULL),
(N'Suero fisiologico 500ml', 1, 1, NULL),
(N'Suero fisiologico 100 ml', 1, 1, NULL),
(N'Glucosado 5% 50ml', 2, 1, NULL),
(N'Venda crepe 7x4', 1, 1, NULL),
(N'Venda crepe 5x4', 1, 1, NULL),
(N'Venda crepe 10x10', 1, 1, NULL),
(N'Venda cohesiva 6x4', 1, 1, NULL),
(N'Venda cohesiva 10x4', 1, 1, NULL),
(N'Jeringas 2ml', 4, 1, NULL),
(N'Jeringas 5ml', 4, 1, NULL),
(N'Jeringas 10ml', 2, 1, NULL),
(N'Jeringas 20ml', 3, 1, NULL),
(N'Paracetamol 100ml IV', 1, 1, NULL),
(N'Adrenalina inyectable', 1, 1, NULL),
(N'Aguja 21G Adrenalina', 1, 1, NULL),
(N'Intraosea', 1, 1, NULL),
(N'Llave 3 vias', 3, 1, NULL),
(N'Regulador de flujo', 2, 1, NULL),
(N'Equipo suero 3 vias', 1, 1, NULL),
(N'Equipo suero', 1, 1, NULL),
(N'TRINYSPRAY', 1, 1, NULL),
(N'LIDOCAINA', 1, 1, NULL),
(N'SUPLECAL', 1, 1, NULL),
(N'OMEPRAZOL 40MG', 1, 1, NULL);

-- Zona 2: AMPULARIO
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Actocortina 100mg vial (Hidrocortisona 100mg)', 1, 2, NULL),
(N'Actocortina 500mg vial (Hidrocortisona 500mg)', 1, 2, NULL),
(N'Adenocor (Adenosina 6mg vial)', 6, 2, NULL),
(N'Adrenalina amp (Epinefrina 1mg amp)', 10, 2, NULL),
(N'Akineton (Biperideno clorhidrato 5mg amp)', 4, 2, NULL),
(N'Aleudrina amp (Isoprenalina 0,2mg amp)', 1, 2, NULL),
(N'Amchafibrin 500mg amp (Acido tranxemico 500mg amp)', 4, 2, NULL),
(N'Anexate 0,5mg amp (Flumazenilo 0,5mg amp)', 3, 2, NULL),
(N'Atropina 1mg amp', 6, 2, NULL),
(N'Atrovent 500mcg amp (Bromuro ipatropico 500mcg)', 2, 2, NULL),
(N'Benerva amp (Tiamina 100mg amp)', 3, 2, NULL),
(N'Buscapina (Butilescopolamina bromuro amp)', 1, 2, NULL),
(N'Clexane 80mg amp (Enoxaparina 80mg amp)', 1, 2, NULL),
(N'Cloruro potasico 2 M amp (Cloruropotasico 2 M amp)', 2, 2, NULL),
(N'Diclofenac amp (Diclofenaco sodico 75mg amp)', 3, 2, NULL),
(N'Digoxina amp (Digoxina 0,25mg amp)', 3, 2, NULL),
(N'Dobutamina amp (Dobutamina 250mg amp)', 2, 2, NULL),
(N'Dogmatil amp (Sulpiride 100mg amp)', 3, 2, NULL),
(N'Dopamina 200mg amp (Dopamina 200mg)', 1, 2, NULL),
(N'Dormicun 5mg/3ml amp (Midazolam 5mg/3ml amp)', 2, 2, NULL),
(N'Enantyum amp (Dexketoprofeno amp)', 1, 2, NULL),
(N'Esmolol amp (Esmolol 100mg/10ml amp)', 2, 2, NULL),
(N'Fortecortin amp 4mg', 2, 2, NULL),
(N'Haloperidol 5mg amp', 4, 2, NULL),
(N'Hypnomidate amp (Etomidato 20mg)', 2, 2, NULL),
(N'Inyesprin (Acetil salicilato de lisina 900mg)', 2, 2, NULL),
(N'Kepra amp (Levetiracetam 100mg/ml)', 1, 2, NULL),
(N'Ketolar vial (Ketamina 50mg vial)', 1, 2, NULL),
(N'Lidocaina 5% vial', 2, 2, NULL),
(N'Mepivacaina 2% 5ml', 2, 2, NULL),
(N'Naloxona amp (Naloxona 0,4mg amp)', 3, 2, NULL),
(N'Nolotil amp (Metamizol 2g)', 1, 2, NULL),
(N'Norepinefrina 4ml amp (Norepinefrina amp)', 1, 2, NULL),
(N'Paracetamol supositorio', 1, 2, NULL),
(N'Polaramire amp (Dexclorfenamina 5mg amp)', 3, 2, NULL),
(N'Primperam amp (Metoclopramida 100mg)', 2, 2, NULL),
(N'Pulmicort (Budesonida 0,5mg vial)', 2, 2, NULL),
(N'Rivotril amp (Clonazepam 1mg)', 2, 2, NULL),
(N'Salbuair vial (Salbutamol 5mg/2,5ml)', 2, 2, NULL),
(N'Seguril amp (Furosemida 20mg)', 2, 2, NULL),
(N'Solinitrina fuerte amp (Nitroglicerina 50mg/5ml amp)', 3, 2, NULL),
(N'Solumoderin 1g vial (Metilprednisolona succinato 1g)', 1, 2, NULL),
(N'Stesolid 5mg microenema (Diazepam 5mg microenema)', 1, 2, NULL),
(N'Suplecal 10ml amp (Gluconato calcico 10ml)', 1, 2, NULL),
(N'Tinispray (Nitroglicerina 400mcg spray)', 2, 2, NULL),
(N'Toradol amp (Ketorolaco 50mg vial)', 4, 2, NULL),
(N'Trandate amp (Labetalol 100mg amp)', 1, 2, NULL),
(N'Trangorex (Amiodarona 150mg amp)', 1, 2, NULL),
(N'Tranxilium 20mg amp (Clorazepato dipotasico 20mg vial)', 1, 2, NULL),
(N'Urapidilo amp (Urapidilo 50mg amp)', 2, 2, NULL),
(N'Urbason 20mg amp (Metil prednisolona 20mg amp)', 4, 2, NULL),
(N'Urbason 40mg amp (Metil prednisolona 40mg amp)', 3, 2, NULL),
(N'Valium 10mg amp (Diazepam 10mg amp)', 4, 2, NULL),
(N'Ventolin amp (Salbutamol 500mcg amp)', 1, 2, NULL),
(N'Zantac 50mg amp (Ranitidina 50mg amp)', 1, 2, NULL),
(N'Omeoprazol ampolla', 1, 2, NULL);

-- Zona 3: COMPRIMIDOS
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Cafinitrina (Cafinitrina)', 10, 3, NULL),
(N'Ibuprofeno 600mg (Ibuprofeno 600mg)', 10, 3, NULL),
(N'Paracetamol 1mg (Paracetamol 1g)', 5, 3, NULL),
(N'Enantyum (Dexketoprofeno 25mg)', 3, 3, NULL),
(N'Orfidal (Lorazepam 1mg)', 1, 3, NULL),
(N'Nolotil (Metamizol magnesico, 1 blister)', 1, 3, NULL),
(N'Valium (Diazepam 5mg)', 4, 3, NULL),
(N'Clopidrogel 75 mg (Clopidrogel 100mg)', 4, 3, NULL),
(N'Clopidrogel 300 (Clopidrogel 300mg)', 4, 3, NULL),
(N'Aspirina (A.A.S. 500mg, 1 blister)', 1, 3, NULL),
(N'Captopril 25 mg.', 8, 3, NULL),
(N'Fortocortin 1 mg', 4, 3, NULL),
(N'Adiro', 15, 3, NULL),
(N'Aspirina 100 mg', 6, 3, NULL);

-- Zona 4: MORFICOS
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Adolonta 100mg amp (Tramadol Hidrocloruro)', 3, 4, NULL),
(N'Fentanest (Fentanilo)', 2, 4, NULL),
(N'Morfina (Cloruro morfico)', 2, 4, NULL),
(N'Propofol (Propofol)', 1, 4, NULL),
(N'Dolantina', 2, 4, NULL),
(N'Ventolin 0,5 mg', 1, 4, NULL),
(N'Adrenalina precargada', 1, 4, NULL),
(N'Suero fisiologico 10 ml', 6, 4, NULL),
(N'Agujas verdes', 4, 4, NULL),
(N'Agujas amarillas', 5, 4, NULL),
(N'Agujas naranjas', 8, 4, NULL);

-- Zona 5: NEVERA
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Actrapid -NOVO RAPID- (Insulina)', 1, 5, NULL),
(N'Anectine (Suxametonio 100mg/2ml)', 1, 5, NULL),
(N'Nimbex (Cisatracurio 2mg/ml)', 2, 5, NULL),
(N'Glucagen (Insulina rapida)', 2, 5, NULL),
(N'Rocuronio', 2, 5, NULL),
(N'Agujas insulina', 1, 5, NULL),
(N'Suero fisiologico 500 ml', 1, 5, NULL);

-- Zona 6: BOTIQUIN PEDIATRICO
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Mascarilla laringea 2,5', 1, 6, NULL),
(N'Mascarilla laringea 2', 1, 6, NULL),
(N'Mascarilla laringea 1,5', 1, 6, NULL),
(N'Mascarilla laringea 1', 1, 6, NULL),
(N'Guedel 1', 1, 6, NULL),
(N'Gasa 20x20', 2, 6, NULL),
(N'Talla esteril 75x90 cm', 1, 6, NULL),
(N'Tubo traqueal 3.0', 1, 6, NULL),
(N'Tubo traqueal 3.5', 1, 6, NULL),
(N'Tubo traqueal con balón 4', 1, 6, NULL),
(N'Tubo traqueal con balón 4.5', 1, 6, NULL),
(N'Tubo traqueal con balón 5', 1, 6, NULL),
(N'Jeringa 10 ml', 1, 6, NULL),
(N'Venda gasa 10 cm', 2, 6, NULL),
(N'Pinza maguill', 1, 6, NULL),
(N'Pinza pequeña', 1, 6, NULL),
(N'Gel lubricante', 1, 6, NULL),
(N'Silkospray', 1, 6, NULL),
(N'Guia intubar 2.0', 1, 6, NULL),
(N'Guia intubar 4.8', 1, 6, NULL),
(N'Equipo laringo completo', 1, 6, NULL),
(N'Smart', 2, 6, NULL),
(N'Palometa 21G', 1, 6, NULL),
(N'Palometa 23G', 1, 6, NULL),
(N'Venda cohesiva 10 cm', 1, 6, NULL),
(N'Cateter 22G', 3, 6, NULL),
(N'Cateter 24G', 2, 6, NULL),
(N'Intraosea', 1, 6, NULL),
(N'Aposito cateter', 2, 6, NULL),
(N'Gasas 20x20', 4, 6, NULL),
(N'Clorhexidina 1%', 1, 6, NULL),
(N'Jeringa insulina', 2, 6, NULL),
(N'Jeringa 2 ml', 4, 6, NULL),
(N'Jeringa 5 ml', 2, 6, NULL),
(N'Jeringa 10 ml', 3, 6, NULL),
(N'Jeringa 20 ml', 1, 6, NULL),
(N'Gafas nasales', 1, 6, NULL),
(N'Mascarilla reservorio', 1, 6, NULL),
(N'Mascarilla flujo', 1, 6, NULL),
(N'Mascarilla hudson', 1, 6, NULL);

-- Zona 7: BOTIQUIN IMV
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Vendaje israelí', 1, 7, NULL),
(N'Venda crepe 10x10', 1, 7, NULL),
(N'Tijera ropa', 1, 7, NULL),
(N'Esponja hemostatica', 3, 7, NULL),
(N'Guedel 7', 1, 7, NULL),
(N'Guedel 8', 1, 7, NULL),
(N'Guedel 10', 1, 7, NULL),
(N'Guedel 12', 1, 7, NULL),
(N'Jeringa 10ml', 4, 7, NULL),
(N'Aguja 20G', 5, 7, NULL),
(N'Ketolar 50mg (ketamina)', 1, 7, NULL),
(N'Midazolam 5mg', 4, 7, NULL);

-- Zona 8: BOTIQUIN RESPIRATORIO ADULTO
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Tubo traqueal 6mm', 1, 8, NULL),
(N'Tubo traqueal 6,5 mm', 1, 8, NULL),
(N'Tubo traqueal 7mm', 1, 8, NULL),
(N'Tubo traqueal 7,5mm', 1, 8, NULL),
(N'Tubo traqueal 8mm', 1, 8, NULL),
(N'Tubo traqueal 8,5mm', 1, 8, NULL),
(N'Tubo traqueal 9mm', 1, 8, NULL),
(N'Guia intubación 4,8', 1, 8, NULL),
(N'Talla esteril 75x90', 1, 8, NULL),
(N'Pinza Magil', 1, 8, NULL),
(N'Kit Laringo completo', 1, 8, NULL),
(N'Filtro', 1, 8, NULL),
(N'Lubricante gel', 1, 8, NULL),
(N'Silko spray', 1, 8, NULL),
(N'Ventolin', 1, 8, NULL),
(N'Atrovent', 1, 8, NULL),
(N'Salbutamol para nebulizador', 1, 8, NULL),
(N'Mascarilla reservorio', 2, 8, NULL),
(N'Gafas nasales', 1, 8, NULL),
(N'Mascarilla flujo', 2, 8, NULL),
(N'Mascarilla nebulizador', 3, 8, NULL),
(N'Mascarilla laringea 3', 1, 8, NULL),
(N'Mascarilla laringea 4', 1, 8, NULL),
(N'Mascarilla laringea 5', 1, 8, NULL),
(N'Tijera corte', 1, 8, NULL),
(N'Compresa 10x20', 2, 8, NULL),
(N'Jeringa 20 ml', 2, 8, NULL),
(N'Guedel 9', 1, 8, NULL),
(N'Guedel 10', 1, 8, NULL),
(N'Guedel 12', 1, 8, NULL),
(N'Guantes esteril S', 2, 8, NULL),
(N'Guantes esteril M', 2, 8, NULL),
(N'Guantes esteril L', 2, 8, NULL),
(N'Talla esteril 75x90 (extra)', 1, 8, NULL),
(N'Llave 3 vias', 1, 8, NULL),
(N'Cateter 14G', 2, 8, NULL),
(N'Canula puncion pneunocath', 1, 8, NULL),
(N'Seda 2/0', 1, 8, NULL),
(N'Seda 3/0', 1, 8, NULL),
(N'Pneunovent tubo conexión', 1, 8, NULL),
(N'Pinza Kochez', 1, 8, NULL),
(N'Pinza normal', 1, 8, NULL);

-- Zona 9: BOTIQUIN SUTURAS
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Campo esteril', 3, 9, NULL),
(N'Campo esteril fenestrado', 3, 9, NULL),
(N'Apositos 20x10', 4, 9, NULL),
(N'Apositos 10x10', 2, 9, NULL),
(N'Apositos 5x7', 5, 9, NULL),
(N'Punto sutura adhesivo', 1, 9, NULL),
(N'Aguja 25g', 6, 9, NULL),
(N'Aguja 21g', 6, 9, NULL),
(N'Aguja 20g', 5, 9, NULL),
(N'Jeringa 2ml', 1, 9, NULL),
(N'Kocher', 3, 9, NULL),
(N'Pinza mosquito', 1, 9, NULL),
(N'Jeringa 5ml', 1, 9, NULL),
(N'Jeringa insulina', 2, 9, NULL),
(N'Pinza mosquito curva', 2, 9, NULL),
(N'Tijera curva', 2, 9, NULL),
(N'Tijera', 2, 9, NULL),
(N'Pinza disección', 3, 9, NULL),
(N'Pinza disección con dientes', 1, 9, NULL),
(N'Seda 5/0', 3, 9, NULL),
(N'Seda 4/0', 2, 9, NULL),
(N'Seda 3/0', 3, 9, NULL),
(N'Compresa gasa 10x20', 2, 9, NULL),
(N'Gasas 20x20', 3, 9, NULL),
(N'Tijera corte', 1, 9, NULL),
(N'Bisturí', 1, 9, NULL),
(N'Guantes esteriles M', 2, 9, NULL),
(N'Guantes esteriles S', 2, 9, NULL),
(N'Guantes esteriles L', 2, 9, NULL),
(N'Grapadora', 2, 9, NULL);

-- Zona 10: BOTIQUIN QUEMADOS
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Burnshield hidrogel pequeño', 3, 10, NULL),
(N'Burnshield hidrogel 200X450', 4, 10, NULL),
(N'Burnshield hidrogel cuerpo entero', 1, 10, NULL),
(N'Burnshield 50ml', 1, 10, NULL),
(N'Linitul 5.5x8', 1, 10, NULL),
(N'Furacin', 1, 10, NULL),
(N'Tireja corta ropa', 1, 10, NULL),
(N'Guantes esteriles S', 2, 10, NULL),
(N'Guantes esteriles M', 2, 10, NULL),
(N'Guantes esteriles L', 2, 10, NULL),
(N'Venda crepe 7cm', 1, 10, NULL),
(N'Venda crepe 10cm', 1, 10, NULL);

-- Zona 11: CAJONES - ARMARIO SUEROS
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Gasas 20x20', 14, 11, 1),
(N'Compresas 10x20', 8, 11, 1),
(N'Clorhexidina 10mg/125ml', 1, 11, 1),
(N'Clorhexidina 30 ml', 2, 11, 1),
(N'Alcohol', 1, 11, 1),
(N'Agua oxigenada', 1, 11, 1),
(N'Suero 30 ml', 8, 11, 1),
(N'Betadine', 1, 11, 1),
(N'Vetolin', 1, 11, 1),
(N'Atrovent', 1, 11, 1),
(N'Diazepam rectal 5mg', 1, 11, 1),
(N'Diazepam rectal 10 mg', 1, 11, 1),
(N'Venofusin', 1, 11, 1),
(N'Suero fisiologico 100 ml', 1, 11, 1),
(N'Suero fisiologico 250 ml', 2, 11, 1),
(N'Paracetamol 100 ml', 3, 11, 1),
(N'Suero fisiologico 500 ml', 2, 11, 1),
(N'Lactato de Ringer', 2, 11, 1),
(N'Glucosado 5% 500 ml', 3, 11, 1),
(N'Glucosado 250 ml', 1, 11, 1),
(N'Glucosado 100 ml', 3, 11, 1),
(N'Glucosalino isotonico 0,3 500 ml', 3, 11, 1),
(N'Gelespan 500 ml', 2, 11, 1),
(N'Llondol pediatrico', 1, 11, 1),
(N'Apositos via', 6, 11, 1),
(N'Venda cohesiva 6x4', 1, 11, 1),
(N'Llave 3 vias', 4, 11, 1),
(N'Tapón via', 2, 11, 1),
(N'Llave simple', 3, 11, 1);

-- Zona 11: CAJONES - CAJON 1
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Parches adulto DESA', 1, 11, 2),
(N'Parches pediatrico DESA', 1, 11, 2),
(N'Gel conductor', 1, 11, 2);

-- Zona 11: CAJONES - CAJON 2
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Guedel pediatrico 3.5', 2, 11, 3),
(N'Guedel pediatrico 5', 3, 11, 3),
(N'Guedel pediatrico 5.5', 1, 11, 3),
(N'Guedel pediatrico 6.5', 1, 11, 3),
(N'Guedel adulto 7', 2, 11, 3),
(N'Guedel adulto 8', 1, 11, 3),
(N'Guedel adulto 9', 1, 11, 3),
(N'Guedel adulto 10', 2, 11, 3),
(N'Guedel adulto 12', 3, 11, 3);

-- Zona 11: CAJONES - CAJON 3 (ADULTO)
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Gafas nasales', 1, 11, 4),
(N'Mascarilla traqueo', 1, 11, 4),
(N'Mascarilla nebulizador', 2, 11, 4),
(N'Mascarilla reservorio', 1, 11, 4),
(N'Mascarilla flujo', 2, 11, 4);

-- Zona 11: CAJONES - CAJON 4 (PEDIATRICO)
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Mascarilla flujo', 2, 11, 5),
(N'Gafas nasales', 2, 11, 5),
(N'Mascarilla reservorio', 2, 11, 5),
(N'Mascarilla nebulizador', 2, 11, 5);

-- Zona 11: CAJONES - CAJON 5
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Agua bidestilada 10 ml', 2, 11, 6),
(N'Jeringa 100 ml', 1, 11, 6),
(N'Talla esteril 75x90', 1, 11, 6),
(N'Carbón absorvente', 1, 11, 6),
(N'Bolsas diuresis', 2, 11, 6),
(N'Sonda Foley 12', 1, 11, 6),
(N'Sonda Foley 14', 1, 11, 6),
(N'Sonda Foley 16', 1, 11, 6),
(N'Sonda Foley 18', 1, 11, 6),
(N'Sonda Foley 20', 1, 11, 6),
(N'Guante esteril S', 2, 11, 6),
(N'Guante esteril M', 2, 11, 6),
(N'Guante esteril L', 2, 11, 6),
(N'Jeringa 10 ml', 1, 11, 6),
(N'Sonda gastrica 14', 1, 11, 6),
(N'Sonda gastrica 16', 1, 11, 6);

-- Zona 11: CAJONES - CAJON 6
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Bolsa calor', 3, 11, 7),
(N'Bolsa frio', 4, 11, 7),
(N'Rasuradoras', 2, 11, 7),
(N'Linitul 9x15', 1, 11, 7),
(N'Silvederma', 1, 11, 7),
(N'Neasayomol (picaduras)', 1, 11, 7),
(N'Furacin', 1, 11, 7),
(N'Trombocid', 1, 11, 7),
(N'Diproderm', 1, 11, 7),
(N'Fastum gel', 1, 11, 7),
(N'Reflex', 1, 11, 7),
(N'Nobecutam', 1, 11, 7);

-- Zona 11: CAJONES - CAJON 7
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Filtros', 3, 11, 8),
(N'Silko gel', 1, 11, 8),
(N'Silkospray', 1, 11, 8),
(N'KIT INTUBACION PEDIATRICO', 2, 11, 8),
(N'Pinzas Maguill', 2, 11, 8),
(N'Tubo traqueal 3', 1, 11, 8),
(N'Tubo traqueal 3.5', 1, 11, 8),
(N'Tubo traqueal 4', 1, 11, 8),
(N'Tubo traqueal 4.5', 1, 11, 8),
(N'Tubo traqueal 5', 1, 11, 8),
(N'Tubo traqueal 6', 1, 11, 8),
(N'Fijador 2mm', 1, 11, 8),
(N'Fijador 4.8 mm', 1, 11, 8),
(N'Jeringa 10 ml', 1, 11, 8),
(N'Laringo pediatrico', 1, 11, 8),
(N'Talla esteril 75x90', 1, 11, 8);

-- Zona 11: CAJONES - KIT INTUBACION ADULTO
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Jeringa 10 ml', 1, 11, 9),
(N'Tubo traqueal 6', 1, 11, 9),
(N'Tubo traqueal 7', 1, 11, 9),
(N'Tubo traqueal 7.5', 1, 11, 9),
(N'Tubo traqueal 8', 1, 11, 9),
(N'Tubo traqueal 8.5', 1, 11, 9),
(N'Tubo traqueal 9', 1, 11, 9),
(N'Pinza maguill', 1, 11, 9),
(N'Venda crepe 5 cm', 1, 11, 9),
(N'Fijador 4.8 mm', 1, 11, 9);

-- Zona 11: CAJONES - CAJON 10
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Aguja intravenosa aletas (palomilla) 23G', 3, 11, 10),
(N'Aguja intravenosa aletas (palomilla) 21G', 3, 11, 10),
(N'Abocat 14', 3, 11, 10),
(N'Abocat 16', 3, 11, 10),
(N'Abocat 18', 4, 11, 10),
(N'Abocat 20', 4, 11, 10),
(N'Abocat 22', 3, 11, 10),
(N'Abocat 24', 3, 11, 10);

-- Zona 11: CAJONES - CAJON 11
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Dosi flow', 2, 11, 11),
(N'Equipo corto', 1, 11, 11),
(N'Equipo largo', 1, 11, 11),
(N'Equipo perfusor 50 ml', 1, 11, 11),
(N'Alargadera', 1, 11, 11),
(N'Jeringa 2 ml', 2, 11, 11),
(N'Jeringa 5 ml', 5, 11, 11),
(N'Jeringa 10 ml', 4, 11, 11),
(N'Jeringa 20 ml', 3, 11, 11),
(N'Jeringa insulina', 3, 11, 11),
(N'Agujas verdes', 8, 11, 11),
(N'Agujas amarillas', 2, 11, 11),
(N'Agujas naranjas', 6, 11, 11);

-- Zona 11: CAJONES - CAJON 12
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Suero fisiologico 250ml', 2, 11, 12),
(N'Quitaesmalte', 1, 11, 12);

-- Zona 12: ESTANTERIAS - ESTANTERIA 1
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Quicktrack', 1, 12, 13),
(N'Intraosea adulto', 1, 12, 13),
(N'Intraosea pediatrica', 1, 12, 13),
(N'Pneumovent', 2, 12, 13),
(N'Pleurocath adulto', 1, 12, 13),
(N'Pleurocath infantil', 1, 12, 13),
(N'Trocar toracico', 1, 12, 13),
(N'Vented Hydrogel neumotorax', 1, 12, 13);

-- Zona 12: ESTANTERIAS - ESTANTERIA 2
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Venda algodón grande', 4, 12, 14),
(N'Vendaje triangular', 4, 12, 14),
(N'Manta termica', 4, 12, 14);

-- Zona 12: ESTANTERIAS - ESTANTERIA 3
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Crepe 10x10', 1, 12, 15),
(N'Crepe 7x4', 3, 12, 15),
(N'Crepe 5x4', 3, 12, 15);

-- Zona 12: ESTANTERIAS - ESTANTERIA 4
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Venda cohesiva 10x4', 4, 12, 16),
(N'Venda cohesiva 6x4', 3, 12, 16),
(N'Tensoplast 10x2,5', 4, 12, 16),
(N'Tensoplast 4x2,5', 3, 12, 16),
(N'Venda gasa 10x10', 3, 12, 16),
(N'Venda gasa 5x7', 4, 12, 16);

-- Zona 12: ESTANTERIAS - ESTANTERIA 5
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Omnifux 10x10', 1, 12, 17),
(N'Omnifix 20x10', 1, 12, 17),
(N'Esparadrapo tela 10x10', 2, 12, 17),
(N'Esparadrapo Antialergico', 1, 12, 17),
(N'Esparadrapo sintetico', 1, 12, 17),
(N'Aposito 5x7', 5, 12, 17),
(N'Tiritas caja', 1, 12, 17),
(N'Aposito 20x10 GRANDE', 4, 12, 17),
(N'Aposito 10x10 MEDIANO', 8, 12, 17);

-- Zona 12: ESTANTERIAS - MONITOR
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Electrodos adulto', 1, 12, 18),
(N'Gel', 1, 12, 18),
(N'Capnografo (valvula)', 3, 12, 18),
(N'Electrodos adulto DESA', 1, 12, 18),
(N'Electrodos pediatrico DESA', 1, 12, 18),
(N'Electrodos pediatricos', 1, 12, 18);

-- Zona 12: ESTANTERIAS - SONDAS
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Sonda 14', 1, 12, 19),
(N'Sonda 16', 1, 12, 19),
(N'Sonda 18', 1, 12, 19),
(N'Sonda 8', 1, 12, 19),
(N'Sonda 6', 1, 12, 19),
(N'Canula Yankaouer nº1', 1, 12, 19),
(N'Canula Yankaouer nº3', 1, 12, 19),
(N'Jeringa 50ml', 1, 12, 19),
(N'Alargadera', 1, 12, 19);

-- Zona 12: ESTANTERIAS - KIT DE PARTO
INSERT INTO materiales (nombre_Producto, cantidad, Id_zona, Id_cajon) VALUES
(N'Compresas', 3, 12, 20),
(N'Manta térmica', 1, 12, 20),
(N'Tijera mosquito', 1, 12, 20),
(N'Tijera normal', 2, 12, 20),
(N'Pinzas cordon', 2, 12, 20);

-- 5) SERVICIOS
INSERT INTO servicio (fecha_hora, nombre_servicio)
VALUES
 ('2026-01-01 08:30:00', N'Retén de tarde'),
 ('2026-01-01 12:15:00', N'Fútbol Romareda'),
 ('2026-01-02 03:45:00', N'Baloncesto Príncipe Felipe'),
 ('2026-01-01 06:45:00', N'Retén de Mañana'),
 ('2026-01-01 12:30:00', N'Fútbol Romareda femenino'),
 ('2026-01-02 16:00:00', N'Cabalgata Reyes Magos');

-- 6) RESPONSABLES
INSERT INTO responsable (Nombre_Responsable, Fecha_Servicio)
VALUES
 (N'Administrador Sistema', '2026-01-01 08:00:00'),
 (N'Juan Pérez García', '2026-01-01 08:30:00'),
 (N'Ana Gómez Ruiz', '2026-01-01 12:15:00'),
 (N'Luis Martínez López', '2026-01-02 03:45:00'),
 (N'Carlos Ruiz Hernández', '2026-01-03 09:00:00'),
 (N'Pilar García Prado', '2026-01-03 14:30:00'),
 (N'Armando Jiménez Giménez', '2026-01-04 10:15:00'),
 (N'Pablo Martínez Oriol', '2026-01-04 16:00:00'),
 (N'José Luis Rodríguez Sancho', '2026-01-05 07:45:00'),
 (N'Juan Carlos Piedrafita González', '2026-01-05 11:30:00'),
 (N'Maricarmen Prados Castillo', '2026-01-06 13:00:00'),
 (N'Rosa Triana Legua', '2026-01-06 18:20:00'),
 (N'Alejandra Ruiz Picasso', '2026-01-07 08:50:00');

-- 7) USUARIOS
INSERT INTO usuarios (Nombre_Usuario, Rol, email, Password, Id_responsable)
VALUES
 (N'Administrador Sistema', N'Administrador', N'admin@ambustock.local', N'Admin123!', 1),
 (N'Juan Pérez García', N'Sanitario', N'juan.perez@ambustock.local', N'Super123!', 2),
 (N'Ana Gómez Ruiz', N'Técnico de emergencias', N'ana.gomez@ambustock.local', N'Oper123!', 3),
 (N'Luis Martínez López', N'Técnico de emergencias', N'luis.martinez@ambustock.local', N'Tecnico123!', 4),
 (N'Carlos Ruiz Hernández', N'Sanitario', N'carlos.ruiz@ambustock.local', N'Sanitario123!', 5),
 (N'Pilar García Prado', N'Sanitario', N'pilar.garcia@ambustock.local', N'Sanitario123!', 6),
 (N'Armando Jiménez Giménez', N'Técnico de emergencias', N'armando.jimenez@ambustock.local', N'Sanitario123!', 7),
 (N'Pablo Martínez Oriol', N'Sanitario', N'pablo.martinez@ambustock.local', N'Sanitario123!', 8),
 (N'José Luis Rodríguez Sancho', N'Sanitario', N'joseluis.rodriguez@ambustock.local', N'Sanitario123!', 9),
 (N'Juan Carlos Piedrafita González', N'Sanitario', N'juancarlos.piedrafita@ambustock.local', N'Tecnico123!', 10),
 (N'Maricarmen Prados Castillo', N'Técnico de emergencias', N'maricarmen.prados@ambustock.local', N'Tecnico123!', 11),
 (N'Rosa Triana Legua', N'Técnico de emergencias', N'rosa.triana@ambustock.local', N'Tecnico123!', 12),
 (N'Alejandra Ruiz Picasso', N'Técnico de emergencias', N'alejandra.ruiz@ambustock.local', N'Tecnico123!', 13);

-- 8) ACTUALIZAR responsables con Id_usuario
UPDATE responsable SET Id_usuario = 1 WHERE Id_responsable = 1;
UPDATE responsable SET Id_usuario = 2 WHERE Id_responsable = 2;
UPDATE responsable SET Id_usuario = 3 WHERE Id_responsable = 3;
UPDATE responsable SET Id_usuario = 4 WHERE Id_responsable = 4;
UPDATE responsable SET Id_usuario = 5 WHERE Id_responsable = 5;
UPDATE responsable SET Id_usuario = 6 WHERE Id_responsable = 6;
UPDATE responsable SET Id_usuario = 7 WHERE Id_responsable = 7;
UPDATE responsable SET Id_usuario = 8 WHERE Id_responsable = 8;
UPDATE responsable SET Id_usuario = 9 WHERE Id_responsable = 9;
UPDATE responsable SET Id_usuario = 10 WHERE Id_responsable = 10;
UPDATE responsable SET Id_usuario = 11 WHERE Id_responsable = 11;
UPDATE responsable SET Id_usuario = 12 WHERE Id_responsable = 12;
UPDATE responsable SET Id_usuario = 13 WHERE Id_responsable = 13;

-- 9) SERVICIO_AMBULANCIA
INSERT INTO Servicio_Ambulancia (Id_Ambulancia, Id_Servicio)
VALUES
 (1, 1),
 (1, 2),
 (1, 3);

-- 10) CORREOS
INSERT INTO correo (fecha_alerta, tipo_problema, Id_material, Id_usuario)
VALUES
 ('2026-01-01 09:00:00', N'Nivel bajo de suero fisiológico', 1, 1),
 ('2026-01-01 13:00:00', N'Faltan guantes talla M', 2, 3),
 ('2026-01-02 04:00:00', N'Caducidad próxima de medicación', 3, 2);

-- 11) DETALLE_CORREO
INSERT INTO Detalle_Correo (Id_material, Id_correo)
VALUES
 (1, 1),
 (2, 2),
 (3, 3);

-- 12) REPOSICIONES
INSERT INTO Reposicion (Id_Correo, Nombre_material, Cantidad, Comentarios, foto_evidencia)
VALUES
 (1, N'Suero fisiológico 500ml', 10, N'Repuesto stock mínimo en cajón 1', NULL),
 (2, N'Guantes nitrilo talla M', 50, N'Reposición estándar fin de guardia', NULL),
 (3, N'Adrenalina 1mg/ml', 5, N'Revisión caducidad y reposición', NULL);

-- 13) VINCULAR RESPONSABLE -> USUARIO / REPOSICION
UPDATE responsable SET Id_usuario = 2, Id_Reposicion = 1 WHERE Id_responsable = 1;
UPDATE responsable SET Id_usuario = 1, Id_Reposicion = 2 WHERE Id_responsable = 2;
UPDATE responsable SET Id_usuario = 3, Id_Reposicion = 3 WHERE Id_responsable = 3;

-- 14) VINCULAR USUARIOS -> RESPONSABLE
UPDATE usuarios SET Id_responsable = 1 WHERE Id_usuario = 1;
UPDATE usuarios SET Id_responsable = 2 WHERE Id_usuario = 2;
UPDATE usuarios SET Id_responsable = 3 WHERE Id_usuario = 3;

-- 15) VINCULAR USUARIOS -> CORREO
UPDATE usuarios SET Id_Correo = 1 WHERE Id_usuario = 1;
UPDATE usuarios SET Id_Correo = 2 WHERE Id_usuario = 2;
UPDATE usuarios SET Id_Correo = 3 WHERE Id_usuario = 3;

-- 16) VINCULAR CORREO -> REPOSICION
UPDATE correo SET Id_reposicion = 1 WHERE Id_Correo = 1;
UPDATE correo SET Id_reposicion = 2 WHERE Id_Correo = 2;
UPDATE correo SET Id_reposicion = 3 WHERE Id_Correo = 3;

GO
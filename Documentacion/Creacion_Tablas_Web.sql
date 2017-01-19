--SELECT * FROM PARAMETRO;
--SELECT * FROM ADMINISTRADOR;
--SELECT * FROM CLIENTE;
--SELECT * FROM FILTRO;
--SELECT * FROM CATEGORIA;
--SELECT * FROM FILTRO_CATEGORIA;
--SELECT * FROM ARTICULO;
--SELECT * FROM ARTICULO_CATEGORIA;
--SELECT * FROM FILTRO_ARTICULO;
--SELECT * FROM ESTADO ORDER BY Id;
--SELECT * FROM PEDIDO;
--SELECT * FROM PEDIDO_ARTICULO;
--SELECT * FROM IMAGEN;
--SELECT * FROM PEDIDO_ARTICULO_FILTRO

--USE master;
--GO

--DROP DATABASE ProyectoWeb_Catalogo_Pedidos;
--GO

--CREATE DATABASE ProyectoWeb_Catalogo_Pedidos;
--GO  

USE isamarina;
GO

DROP TABLE dbo.PEDIDO_ARTICULO_FILTRO;
DROP TABLE dbo.PEDIDO_ARTICULO;
ALTER TABLE dbo.CLIENTE   
	DROP CONSTRAINT FK_EnConstruccion_CLIENTE;   
ALTER TABLE dbo.ADMINISTRADOR   
	DROP CONSTRAINT FK_EnConstruccion_ADMINISTRADOR;   
DROP TABLE dbo.PEDIDO;
DROP TABLE dbo.FILTRO_ARTICULO;
DROP TABLE dbo.FILTRO_CATEGORIA;
DROP TABLE dbo.FILTRO;
DROP TABLE dbo.ESTADO;
DROP TABLE dbo.ARTICULO_CATEGORIA;
DROP TABLE dbo.IMAGEN;
DROP TABLE dbo.ARTICULO;
DROP TABLE dbo.CATEGORIA;
DROP TABLE dbo.CLIENTE;
DROP TABLE dbo.ADMINISTRADOR;
DROP TABLE dbo.PARAMETRO;
GO

CREATE TABLE dbo.PARAMETRO
(
	Id INT NOT NULL IDENTITY(1,1),
	IVA NUMERIC(5,2) NOT NULL,
	
	CONSTRAINT PK_PARAMETRO PRIMARY KEY(Id)
);
GO

CREATE TABLE dbo.ADMINISTRADOR
(
	Id INT NOT NULL IDENTITY(1,1),
	Usuario NVARCHAR(20) NOT NULL,
	Contrasenia NVARCHAR(MAX) NOT NULL,
	EnConstruccion INT,
	
	CONSTRAINT PK_ADMINISTRADOR PRIMARY KEY(Id),
	CONSTRAINT UK_Usuario_ADMINISTRADOR UNIQUE(Usuario)
);
GO

CREATE TABLE dbo.CLIENTE
(
	Id INT NOT NULL IDENTITY(1,1),
	Usuario NVARCHAR(20) NOT NULL,
	Contrasenia NVARCHAR(MAX) NOT NULL,
	EnConstruccion INT,
	NombreFantasia NVARCHAR(100) NOT NULL,
	Rut NVARCHAR(50),
	RazonSocial NVARCHAR(100),
	Descuento NUMERIC(5,2) NOT NULL,
	DiasDePago NVARCHAR(50),
	Direccion NVARCHAR(100),
	Telefono NVARCHAR(30),
	NombreContacto NVARCHAR(50),
	TelefonoContacto NVARCHAR(30),
	EmailContacto NVARCHAR(50),
	Imagen NVARCHAR(MAX) NOT NULL,
	Habilitado BIT NOT NULL
	
	CONSTRAINT PK_CLIENTE PRIMARY KEY(Id),
	CONSTRAINT UK_Usuario_CLIENTE UNIQUE(Usuario),
	--CONSTRAINT UK_Rut_CLIENTE UNIQUE(Rut),
	--CONSTRAINT UK_RazonSocial_CLIENTE UNIQUE(RazonSocial),
	CONSTRAINT UK_NombreFantasia_CLIENTE UNIQUE(NombreFantasia)
);
GO

CREATE TABLE dbo.CATEGORIA
(
	Id	INT  NOT NULL IDENTITY(1,1),
	Nombre NVARCHAR(50) NOT NULL,
	Imagen NVARCHAR(MAX) NOT NULL,
	Destacado BIT NOT NULL,

	CONSTRAINT PK_CATEGORIA PRIMARY KEY(Id),
	CONSTRAINT UK_Nombre_CATEGORIA UNIQUE(Nombre)
);
GO

CREATE TABLE dbo.ARTICULO
(
	Id	INT  NOT NULL IDENTITY(1,1),
	Codigo NVARCHAR(20) NOT NULL,
	Nombre NVARCHAR(50) NOT NULL,
	Descripcion NVARCHAR(250) NOT NULL,
	Precio MONEY NOT NULL,
	Stock NUMERIC(6,2) NOT NULL,
	Disponible BIT NOT NULL, --A fin de que pueda dejarlo como no disponible por si no quiere mostrarlo y no se puede eliminar por estar asociado a alg�n pedido.
	Destacado BIT NOT NULL,

	CONSTRAINT PK_PRODUCTO PRIMARY KEY(Id),
	CONSTRAINT UK_Codigo_ARTICULO UNIQUE(Codigo)
);
GO

CREATE TABLE dbo.IMAGEN
(
	Id	INT  NOT NULL IDENTITY(1,1),
	IdArticulo INT NOT NULL,
	Imagen NVARCHAR(MAX) NOT NULL,

	CONSTRAINT PK_IMAGEN PRIMARY KEY(Id),
	CONSTRAINT FK_IdArticulo_IMAGEN FOREIGN KEY (IdArticulo) REFERENCES dbo.ARTICULO (Id)
);
GO	

CREATE TABLE dbo.ARTICULO_CATEGORIA
(
	Id	INT  NOT NULL IDENTITY(1,1),
	IdArticulo INT NOT NULL,
	IdCategoria INT NOT NULL,

	CONSTRAINT PK_ARTICULO_CATEGORIA PRIMARY KEY(Id),
	CONSTRAINT FK_IdProducto_ARTICULO_CATEGORIA FOREIGN KEY (IdArticulo) REFERENCES dbo.ARTICULO (Id),
	CONSTRAINT FK_IdCategoria_ARTICULO_CATEGORIA FOREIGN KEY (IdCategoria) REFERENCES dbo.CATEGORIA (Id)	
);
GO

CREATE TABLE dbo.ESTADO(
	Id INT NOT NULL IDENTITY(1,1),
	Nombre NVARCHAR(30) NOT NULL,
	
	CONSTRAINT PK_ESTADO PRIMARY KEY(Id),
	CONSTRAINT UK_Nombre_ESTADO UNIQUE(Nombre)
);
GO

CREATE TABLE dbo.FILTRO
(
	Id	INT  NOT NULL IDENTITY(1,1),
	Nombre NVARCHAR(50) NOT NULL,
	Color BIT NOT NULL,

	CONSTRAINT PK_FILTRO PRIMARY KEY(Id),
	CONSTRAINT UK_Nombre_FILTRO UNIQUE(Nombre)
);
GO

CREATE TABLE dbo.FILTRO_CATEGORIA
(
	Id	INT  NOT NULL IDENTITY(1,1),
	IdFiltro INT NOT NULL,
	IdCategoria INT NOT NULL,

	CONSTRAINT PK_FILTRO_CATEGORIA PRIMARY KEY(Id),
	CONSTRAINT FK_IdPFiltro_FILTRO_CATEGORIA FOREIGN KEY (IdFiltro) REFERENCES dbo.FILTRO (Id),
	CONSTRAINT FK_IdCategoria_FILTRO_CATEGORIA FOREIGN KEY (IdCategoria) REFERENCES dbo.CATEGORIA (Id)	
);
GO

CREATE TABLE dbo.FILTRO_ARTICULO
(
	Id	INT  NOT NULL IDENTITY(1,1),
	IdFiltro INT NOT NULL,
	IdArticulo INT NOT NULL,

	CONSTRAINT PK_FILTRO_ARTICULO PRIMARY KEY(Id),
	CONSTRAINT FK_IdPFiltro_FILTRO_ARTICULO FOREIGN KEY (IdFiltro) REFERENCES dbo.FILTRO (Id),
	CONSTRAINT FK_IdArticulo_FILTRO_ARTICULO FOREIGN KEY (IdArticulo) REFERENCES dbo.ARTICULO (Id)	
);
GO

CREATE TABLE dbo.PEDIDO
(
	Id	INT  NOT NULL IDENTITY(1,1),
	FechaRealizado DATETIME,
	FechaEntregaSolicitada DATETIME,
	DescuentoCliente NUMERIC (5,2) NOT NULL,
	Iva NUMERIC (5,2) NOT NULL,
	IdCliente INT NOT NULL,
	Comentario NVARCHAR(MAX),
	IdEstado INT NOT NULL,

	CONSTRAINT PK_PEDIDO PRIMARY KEY(Id),
	CONSTRAINT FK_IdCliente_PEDIDO FOREIGN KEY (IdCliente) REFERENCES dbo.CLIENTE (Id),
	CONSTRAINT FK_IdEstadoPedido_PEDIDO FOREIGN KEY (IdEstado) REFERENCES dbo.ESTADO (Id)
);
GO

ALTER TABLE dbo.CLIENTE
	ADD CONSTRAINT FK_EnConstruccion_CLIENTE FOREIGN KEY (EnConstruccion) REFERENCES dbo.PEDIDO (Id);
GO
ALTER TABLE dbo.ADMINISTRADOR
	ADD CONSTRAINT FK_EnConstruccion_ADMINISTRADOR FOREIGN KEY (EnConstruccion) REFERENCES dbo.PEDIDO (Id);
GO

CREATE TABLE dbo.PEDIDO_ARTICULO
(
	Id	INT  NOT NULL IDENTITY(1,1),
	IdPedido INT NOT NULL,
	IdArticulo INT NOT NULL,
	Cantidad INT NOT NULL,
	PrecioUnitario MONEY NOT NULL,

	CONSTRAINT PK_PEDIDO_ARTICULO PRIMARY KEY(Id),
	CONSTRAINT FK_IdPedido_PEDIDO_ARTICULO FOREIGN KEY (IdPedido) REFERENCES dbo.PEDIDO (Id),
	CONSTRAINT FK_IdArticulo_PEDIDO_ARTICULO FOREIGN KEY (IdArticulo) REFERENCES dbo.ARTICULO (Id)
);
GO

CREATE TABLE dbo.PEDIDO_ARTICULO_FILTRO
(
	Id	INT  NOT NULL IDENTITY(1,1),
	IdPedidoArticulo INT NOT NULL,
	IdFiltro INT NOT NULL,

	CONSTRAINT PK_PEDIDO_ARTICULO_FILTRO PRIMARY KEY(Id),
	CONSTRAINT FK_IdPedidoArticulo_PEDIDO_ARTICULO_FILTRO FOREIGN KEY (IdPedidoArticulo) REFERENCES dbo.PEDIDO_ARTICULO (Id),
	CONSTRAINT FK_IdFiltro_PEDIDO_ARTICULO_FILTRO FOREIGN KEY (IdFiltro) REFERENCES dbo.FILTRO (Id)
);
GO
--despues de cambiar el precio de un articulo
CREATE TRIGGER trg_actualizarPrecioEstadoPedidoEnConstruccion
ON dbo.ARTICULO
AFTER UPDATE
AS
BEGIN
	DECLARE @idArticulo INT, @precio INT, @estado BIT
	SELECT @idArticulo = Id, @precio = Precio, @estado = Disponible  FROM inserted
	
	UPDATE dbo.PEDIDO_ARTICULO SET PrecioUnitario = @precio WHERE IdArticulo=@idArticulo 
	and IdPedido in (SELECT IdPedido FROM dbo.PEDIDO P WHERE IdEstado = 5)  --OJO SI CAMBIAS LOS ESTADOS CAMBIA EL NUEMRO
	
	IF @estado = 0
		BEGIN
			DELETE dbo.PEDIDO_ARTICULO WHERE IdArticulo=@idArticulo
			and IdPedido in (SELECT IdPedido FROM dbo.PEDIDO P WHERE IdEstado = 5)  --OJO SI CAMBIAS LOS ESTADOS CAMBIA EL NUEMRO
		END
END
GO

--ANTES DE BORRAR UNA CATEGOR�A, LE ELIMINA TODOS LOS ARTICULOS QUE TIENE ASOCIADOS
CREATE TRIGGER trg_eliminarCategoria
ON dbo.CATEGORIA
INSTEAD OF DELETE
AS
BEGIN
	DECLARE @idCategoria INT, @img INT
	SELECT @idCategoria = Id FROM DELETED
	
	DELETE FROM dbo.ARTICULO_CATEGORIA WHERE IdCategoria = @idCategoria;
	DELETE FROM dbo.CATEGORIA WHERE Id = @idCategoria;
END
GO

--ANTES DE BORRAR UN ARTICULO, LE ELIMINA TODAS LAS CATEGORIAS Y FOTOS QUE TIENE ASOCIADAS
CREATE TRIGGER trg_eliminarArticulo
ON dbo.ARTICULO
INSTEAD OF DELETE
AS
BEGIN
	DECLARE @idArticulo INT
	SELECT @idArticulo = Id FROM DELETED
	
	DELETE FROM dbo.ARTICULO_CATEGORIA WHERE IdArticulo = @idArticulo;
	DELETE FROM dbo.IMAGEN WHERE IdArticulo = @idArticulo;
	DELETE FROM dbo.ARTICULO WHERE Id = @idArticulo;
END
GO


/*
	DATOS DE PRUEBA !!!
*/
INSERT INTO dbo.PARAMETRO VALUES
(22);

INSERT INTO dbo.ADMINISTRADOR VALUES
--Los password de Prueba son: 123
('ADMINISTRADOR1','202cb962ac59075b964b07152d234b70', NULL),
('ADMINISTRADOR2','202cb962ac59075b964b07152d234b70', NULL),
('ADMINISTRADOR3','202cb962ac59075b964b07152d234b70', NULL);

INSERT INTO dbo.CLIENTE VALUES
--Los password de Prueba son: 123
('CLIENTE1','202cb962ac59075b964b07152d234b70', NULL,'NOMBRE FANTASIA CLIENTE 1','1','RAZ. SOC. 1',0,'LUNES y VIERNES','DIRECCION 1','1','CONTACTO CLIENTE 1','1','EMAIL1@EMAIL.COM','CLIENTE1.jpg',1),
('CLIENTE2','202cb962ac59075b964b07152d234b70', NULL,'FATASIA NOMBRE CLIENTE 2','2','RAZ. SOC. 2',10,'MARTES y JUEVES','DIRECCION 2','2','CONTACTO CLIENTE 2','2','EMAIL2@EMAIL.COM','CLIENTE2.jpg',1),
('CLIENTE3','202cb962ac59075b964b07152d234b70', NULL,'CLIENTE 3','3','RAZ. SOC. 3',20,'MIERCOLES','DIRECCION 3','3','CONTACTO CLIENTE 3','3','EMAIL3@EMAIL.COM','CLIENTE3.jpg',1),
('CLIENTE4','202cb962ac59075b964b07152d234b70', NULL,'NOMBRE CLIENTE 4','4','RAZ. SOC. 4',30,'MIERCOLES','DIRECCION 4','4','CONTACTO CLIENTE 4','4','EMAIL4@EMAIL.COM','CLIENTE4.jpg',1),
('CLIENTE5','202cb962ac59075b964b07152d234b70', NULL,'NOMBRE FANTASIA CLIENTE 5','5','RAZ. SOC. 5',40,'MIERCOLES','DIRECCION 5','5','CONTACTO CLIENTE 5','5','EMAIL5@EMAIL.COM','CLIENTE5.jpg',1);

INSERT INTO dbo.CATEGORIA VALUES
('Anillos','CAT1.jpg',1),
('Caravanas','CAT2.jpg',1),
('Collares','CAT3.jpg',1),
('Pulseras','CAT4.jpg',1),
('Otros','CAT5.jpg',1);

INSERT INTO dbo.ARTICULO VALUES
('COD 1','Art�culo 1','Descripci�n Art�culo 1',10,0,1,1),
('COD 2','Art�culo 2','Descripci�n Art�culo 2',20,10,1,1),
('COD 3','Art�culo 3','Descripci�n Art�culo 3',30,20,1,1),
('COD 4','Art�culo 4','Descripci�n Art�culo 4',40,30,1,1),
('COD 5','Art�culo 5','Descripci�n Art�culo 5',50,40,1,1),
('COD 6','Art�culo 6','Descripci�n Art�culo 6',60,50,1,1),
('COD 7','Art�culo 7','Descripci�n Art�culo 7',70,60,1,0),
('COD 8','Art�culo 8','Descripci�n Art�culo 8',80,70,1,0),
('COD 9','Art�culo 9','Descripci�n Art�culo 9',90,80,1,0),
('COD 10','Art�culo 10','Descripci�n Art�culo 10',100,90,1,1),
('COD 11','Art�culo 11','Descripci�n Art�culo 11',10,80,1,1);

INSERT INTO IMAGEN VALUES
(1,'COD1_IMG1.jpg'),
(1,'COD1_IMG2.jpg'),
(1,'COD1_IMG3.jpg'),
(2,'COD2_IMG1.jpg'),
(3,'COD3_IMG1.jpg'),
(3,'COD3_IMG2.jpg'),
(4,'COD4_IMG1.jpg'),
(4,'COD4_IMG2.jpg'),
(5,'COD5_IMG1.jpg'),
(6,'COD6_IMG1.jpg'),
(7,'COD7_IMG1.jpg'),
(8,'COD8_IMG1.jpg'),
(9,'COD9_IMG1.jpg'),
(10,'COD10_IMG1.jpg'),
(10,'COD10_IMG2.jpg'),
(1,'COD1_IMG4.jpg'),
(1,'COD1_IMG5.jpg'),
(11,'COD11_IMG1.jpg'),
(11,'COD11_IMG2.jpg'),
(11,'COD11_IMG3.jpg');

INSERT INTO dbo.ARTICULO_CATEGORIA VALUES
(1,1),
(2,1),
(3,2),
(4,3),
(5,4),
(6,3),
(7,3),
(8,4),
(9,5),
(10,5),
(10,4),
(11,1);

INSERT INTO dbo.ESTADO VALUES
('CONFIRMADO POR CLIENTE'),
('MODIFICADO POR ADMINISTRADOR'),
('REALIZADO'),
('CANCELADO'),
('EN CONSTRUCCION'),
('CONFIRMADO');

INSERT INTO dbo.FILTRO VALUES
('Plata 925',0),
('Enchapado en Oro',0),
('Verde',1),
('Azul',1),
('Amarillo',1),
('Rojo',1);

INSERT INTO dbo.FILTRO_CATEGORIA VALUES
(1,1),
(2,1),
(1,2),
(2,2),
(1,3),
(2,3),
(1,4),
(2,5),
(3,1),
(4,1),
(5,1);

INSERT INTO dbo.FILTRO_ARTICULO VALUES
(1,1),
(2,1),
(1,2),
(2,3),
(2,4),
(4,5),
(5,6),
(1,8),
(1,9),
(2,9),
(1,10),
(2,10),
(3,10),
(4,10),
(5,10),
(1,11);

INSERT INTO dbo.PEDIDO VALUES
('20160215','20160315',0,22,1,'15/02/2016 - Cliente: Primer Comentario de Prueba|20/02/2016 - Administrador: Segundo Comentario de Prueba|10/03/2016 - Cliente:  Tercer Comentario de Prueba',1),
('20160110','20160115',10,22,2,'10/01/2016 - Cliente: Primer Comentario de Prueba',1),
('20160110','20160115',20,22,3,'10/01/2016 - Administrador: Primer Comentario de Prueba',2),
('20160101','20160102',30,22,4,'01/01/2016 - Cliente: Primer Comentario de Prueba|02/01/2016 - Administrador: Segundo Comentario de Prueba',3),
('20160101','20160102',10,22,4,NULL,4),
('20160426','20160520',30,22,4,'26/04/2016 - Cliente: Primer Comentario de Prueba',1),
('20160110','20160115',40,22,5,NULL,1),
('20160101','20160102',5,22,1,'01/01/2016 - Administrador: Primer Comentario de Prueba|01/01/2016 - Administrador: Segundo Comentario de Prueba',4);

INSERT INTO dbo.PEDIDO_ARTICULO VALUES
(1,1,1,10),
(2,1,2,10),
(2,2,3,20),
(2,4,4,40),
(3,10,5,100),
(3,6,6,60),
(4,5,7,50),
(5,4,8,40),
(6,5,9,50),
(6,4,10,40),
(6,3,9,30),
(6,2,8,20),
(7,1,7,10),
(7,2,6,30),
(8,10,5,100),
(8,9,4,90);

--INSERT INTO PEDIDO_ARTICULO_FILTRO VALUES
--(1,1),
--(1,1),
--(2,2);
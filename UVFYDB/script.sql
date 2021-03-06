
CREATE DATABASE UVFY
GO
USE [UVFY]
GO
/****** Object:  Table [dbo].[Albumes]    Script Date: 05/07/2020 2:01:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Albumes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL,
	[Artistas_Id] [int] NOT NULL,
 CONSTRAINT [PK_Albumes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlbumGenero]    Script Date: 05/07/2020 2:01:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlbumGenero](
	[Albumes_Id] [int] NOT NULL,
	[Generos_Id] [int] NOT NULL,
 CONSTRAINT [PK_AlbumGenero] PRIMARY KEY CLUSTERED 
(
	[Albumes_Id] ASC,
	[Generos_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Canciones]    Script Date: 05/07/2020 2:01:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Canciones](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Duracion] [nvarchar](max) NOT NULL,
	[FechaDeLanzamiento] [datetime] NOT NULL,
	[Estado] [smallint] NOT NULL,
	[Artista_Id] [int] NULL,
	[Albums_Id] [int] NULL,
	[Consumidor_Id] [int] NULL,
 CONSTRAINT [PK_Canciones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CancionGenero]    Script Date: 05/07/2020 2:01:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CancionGenero](
	[Canciones_Id] [int] NOT NULL,
	[Generos_Id] [int] NOT NULL,
 CONSTRAINT [PK_CancionGenero] PRIMARY KEY CLUSTERED 
(
	[Canciones_Id] ASC,
	[Generos_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CancionPlaylist]    Script Date: 05/07/2020 2:01:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CancionPlaylist](
	[Cancion_Id] [int] NOT NULL,
	[Playlists_Id] [int] NOT NULL,
 CONSTRAINT [PK_CancionPlaylist] PRIMARY KEY CLUSTERED 
(
	[Cancion_Id] ASC,
	[Playlists_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Generos]    Script Date: 05/07/2020 2:01:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Generos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Generos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Playlists]    Script Date: 05/07/2020 2:01:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Playlists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Consumidor_Id] [int] NOT NULL,
 CONSTRAINT [PK_Playlists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 05/07/2020 2:01:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NombreDeUsuario] [nvarchar](max) NOT NULL,
	[CorreoElectronico] [nvarchar](max) NOT NULL,
	[Contraseña] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios_Artista]    Script Date: 05/07/2020 2:01:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios_Artista](
	[Nombre] [nvarchar](max) NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL,
	[Id] [int] NOT NULL,
 CONSTRAINT [PK_Usuarios_Artista] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios_Consumidor]    Script Date: 05/07/2020 2:01:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios_Consumidor](
	[EstadoDeSuscripcion] [smallint] NOT NULL,
	[FechaDeFinalDeSuscripcion] [datetime] NOT NULL,
	[Id] [int] NOT NULL,
 CONSTRAINT [PK_Usuarios_Consumidor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Albumes]  WITH CHECK ADD  CONSTRAINT [FK_ArtistaAlbum] FOREIGN KEY([Artistas_Id])
REFERENCES [dbo].[Usuarios_Artista] ([Id])
GO
ALTER TABLE [dbo].[Albumes] CHECK CONSTRAINT [FK_ArtistaAlbum]
GO
ALTER TABLE [dbo].[AlbumGenero]  WITH CHECK ADD  CONSTRAINT [FK_AlbumGenero_Album] FOREIGN KEY([Albumes_Id])
REFERENCES [dbo].[Albumes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlbumGenero] CHECK CONSTRAINT [FK_AlbumGenero_Album]
GO
ALTER TABLE [dbo].[AlbumGenero]  WITH CHECK ADD  CONSTRAINT [FK_AlbumGenero_Genero] FOREIGN KEY([Generos_Id])
REFERENCES [dbo].[Generos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlbumGenero] CHECK CONSTRAINT [FK_AlbumGenero_Genero]
GO
ALTER TABLE [dbo].[Canciones]  WITH CHECK ADD  CONSTRAINT [FK_ArtistaCancion] FOREIGN KEY([Artista_Id])
REFERENCES [dbo].[Usuarios_Artista] ([Id])
GO
ALTER TABLE [dbo].[Canciones] CHECK CONSTRAINT [FK_ArtistaCancion]
GO
ALTER TABLE [dbo].[Canciones]  WITH CHECK ADD  CONSTRAINT [FK_CancionAlbum] FOREIGN KEY([Albums_Id])
REFERENCES [dbo].[Albumes] ([Id])
GO
ALTER TABLE [dbo].[Canciones] CHECK CONSTRAINT [FK_CancionAlbum]
GO
ALTER TABLE [dbo].[Canciones]  WITH CHECK ADD  CONSTRAINT [FK_ConsumidorCancion] FOREIGN KEY([Consumidor_Id])
REFERENCES [dbo].[Usuarios_Consumidor] ([Id])
GO
ALTER TABLE [dbo].[Canciones] CHECK CONSTRAINT [FK_ConsumidorCancion]
GO
ALTER TABLE [dbo].[CancionGenero]  WITH CHECK ADD  CONSTRAINT [FK_CancionGenero_Cancion] FOREIGN KEY([Canciones_Id])
REFERENCES [dbo].[Canciones] ([Id])
GO
ALTER TABLE [dbo].[CancionGenero] CHECK CONSTRAINT [FK_CancionGenero_Cancion]
GO
ALTER TABLE [dbo].[CancionGenero]  WITH CHECK ADD  CONSTRAINT [FK_CancionGenero_Genero] FOREIGN KEY([Generos_Id])
REFERENCES [dbo].[Generos] ([Id])
GO
ALTER TABLE [dbo].[CancionGenero] CHECK CONSTRAINT [FK_CancionGenero_Genero]
GO
ALTER TABLE [dbo].[CancionPlaylist]  WITH CHECK ADD  CONSTRAINT [FK_CancionPlaylist_Cancion] FOREIGN KEY([Cancion_Id])
REFERENCES [dbo].[Canciones] ([Id])
GO
ALTER TABLE [dbo].[CancionPlaylist] CHECK CONSTRAINT [FK_CancionPlaylist_Cancion]
GO
ALTER TABLE [dbo].[CancionPlaylist]  WITH CHECK ADD  CONSTRAINT [FK_CancionPlaylist_Playlist] FOREIGN KEY([Playlists_Id])
REFERENCES [dbo].[Playlists] ([Id])
GO
ALTER TABLE [dbo].[CancionPlaylist] CHECK CONSTRAINT [FK_CancionPlaylist_Playlist]
GO
ALTER TABLE [dbo].[Playlists]  WITH CHECK ADD  CONSTRAINT [FK_ConsumidorPlaylist] FOREIGN KEY([Consumidor_Id])
REFERENCES [dbo].[Usuarios_Consumidor] ([Id])
GO
ALTER TABLE [dbo].[Playlists] CHECK CONSTRAINT [FK_ConsumidorPlaylist]
GO
ALTER TABLE [dbo].[Usuarios_Artista]  WITH CHECK ADD  CONSTRAINT [FK_Artista_inherits_Usuario] FOREIGN KEY([Id])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Usuarios_Artista] CHECK CONSTRAINT [FK_Artista_inherits_Usuario]
GO
ALTER TABLE [dbo].[Usuarios_Consumidor]  WITH CHECK ADD  CONSTRAINT [FK_Consumidor_inherits_Usuario] FOREIGN KEY([Id])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Usuarios_Consumidor] CHECK CONSTRAINT [FK_Consumidor_inherits_Usuario]
GO

SET IDENTITY_INSERT [dbo].[Generos] ON 

INSERT [dbo].[Generos] ([Id], [Nombre], [Descripcion]) VALUES (1, N'Pop', N'Non-Stop Pop')
INSERT [dbo].[Generos] ([Id], [Nombre], [Descripcion]) VALUES (2, N'Rock', N'&Roll')
INSERT [dbo].[Generos] ([Id], [Nombre], [Descripcion]) VALUES (3, N'Indie', N'Tan hipster!')
INSERT [dbo].[Generos] ([Id], [Nombre], [Descripcion]) VALUES (4, N'Jazz', N'Con todo el soul')
INSERT [dbo].[Generos] ([Id], [Nombre], [Descripcion]) VALUES (5, N'Clasico', N'Party like it''s 1789!')

GO
﻿using Microsoft.Data.SqlClient;
using System.Data;

namespace ICA.Models
{
    public class RepositorioGenero : RepositorioBase, IRepositorioGenero
    {
        public RepositorioGenero(IConfiguration configuration) : base(configuration)
        {

        }
        public int Alta(Genero entidad)
        {
            if (entidad == null)
            {
                throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula.");
            }

            if (string.IsNullOrWhiteSpace(entidad.Nombre))
            {
                throw new ArgumentException("El campo Nombre no puede estar vacío o ser nulo.", nameof(entidad.Nombre));
            }

            const string sql = @"
            INSERT INTO Genero (Nombre, Descripcion, Estado, TecnicaturaId)
            VALUES (@nombre, @descripcion, @estado, @tecnicaturaId);
            SELECT SCOPE_IDENTITY();";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", entidad.Nombre);
                command.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
                command.Parameters.AddWithValue("@estado", entidad.Estado);
                command.Parameters.AddWithValue("@tecnicaturaId", entidad.TecnicaturaId);

                connection.Open();

                var result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out var newId))
                {
                    entidad.Id = newId;
                    return newId;
                }
                else
                {
                    throw new InvalidOperationException("No se pudo obtener el ID del nuevo registro.");
                }
            }
        }
        public int Baja(int id)
        {
            // Validar el id para asegurarse de que es válido antes de continuar
            if (id <= 0)
            {
                throw new ArgumentException("El identificador debe ser un valor positivo.", nameof(id));
            }

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = @"DELETE FROM Genero WHERE Id = @id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@id", id);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception details for troubleshooting (implement logging as per your requirements)
                // Logger.LogError(ex, "Error al intentar eliminar el género con Id: {Id}", id);

                // Optionally, rethrow the exception or handle it as needed
                throw new ApplicationException("Se produjo un error al intentar eliminar el género.", ex);
            }
        }

        public int Modificacion(Genero entidad)
        {
            // Verificar si la entidad es nula
            if (entidad == null)
            {
                throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula.");
            }

            // Verificar si los campos necesarios están presentes
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
            {
                throw new ArgumentException("El campo Nombre no puede estar vacío o ser nulo.", nameof(entidad.Nombre));
            }

            int rowsAffected = 0;

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    const string sql = "UPDATE Genero SET " +
                                       "Nombre = @nombre, Descripcion = @descripcion, Estado = @estado, TecnicaturaId = @tecnicaturaId " +
                                       "WHERE Id = @id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        // Usar AddWithValue con cuidado: asegúrate de que el tipo de datos sea correcto
                        command.Parameters.AddWithValue("@nombre", entidad.Nombre);
                        command.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
                        command.Parameters.AddWithValue("@estado", entidad.Estado);
                        command.Parameters.AddWithValue("@tecnicaturaId", entidad.TecnicaturaId);
                        command.Parameters.AddWithValue("@id", entidad.Id);

                        command.CommandType = CommandType.Text;

                        connection.Open();

                        // Ejecutar la consulta y obtener el número de filas afectadas
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Manejo de errores específicos de SQL
                // Ejemplo de registro de error:
                // Logger.LogError(ex, "Error al modificar el género: {Message}", ex.Message);
                throw new ApplicationException("Se produjo un error al intentar modificar el género.", ex);
            }
            catch (Exception ex)
            {
                // Manejo de errores generales
                // Ejemplo de registro de error:
                // Logger.LogError(ex, "Error inesperado: {Message}", ex.Message);
                throw new ApplicationException("Se produjo un error inesperado.", ex);
            }

            return rowsAffected;
        }
        public IList<Genero> ObtenerTodos(int Tid)
        {
            var generos = new List<Genero>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    const string sql = @"SELECT g.Id, g.Nombre, g.Descripcion, g.Estado, g.TecnicaturaId, t.Nombre as TecnicaturaNombre
                                         FROM Genero g
                                         INNER JOIN Tecnicatura t ON t.Id = g.TecnicaturaId
                                         WHERE g.TecnicaturaId = @id ";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", Tid);
                        command.CommandType = CommandType.Text;
                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var genero = new Genero
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                    Estado = reader.GetByte(reader.GetOrdinal("Estado")),
                                    TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                    Tecnicatura = new Tecnicatura
                                    {
                                        Nombre = reader.IsDBNull(reader.GetOrdinal("TecnicaturaNombre")) ? null : reader.GetString(reader.GetOrdinal("TecnicaturaNombre"))
                                    }
                                };
                                generos.Add(genero);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Maneja errores específicos de SQL aquí
                // Log del error y/o manejar según la necesidad
                // Por ejemplo: Logger.LogError(ex, "Error al obtener los géneros.");
                throw new ApplicationException("Se produjo un error al intentar obtener los géneros.", ex);
            }
            catch (Exception ex)
            {
                // Maneja errores generales aquí
                // Log del error y/o manejar según la necesidad
                // Por ejemplo: Logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Se produjo un error inesperado.", ex);
            }

            return generos;
        }
        public IList<Genero> ObtenerTodos()
        {
            var generos = new List<Genero>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    const string sql = @"SELECT g.Id, g.Nombre, g.Descripcion, g.Estado, g.TecnicaturaId, t.Nombre as TecnicaturaNombre
                                         FROM Genero g
                                         INNER JOIN Tecnicatura t ON t.Id = g.TecnicaturaId";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var genero = new Genero
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                    Estado = reader.GetByte(reader.GetOrdinal("Estado")),
                                    TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                    Tecnicatura = new Tecnicatura
                                    {
                                        Nombre = reader.IsDBNull(reader.GetOrdinal("TecnicaturaNombre")) ? null : reader.GetString(reader.GetOrdinal("TecnicaturaNombre"))
                                    }
                                };
                                generos.Add(genero);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Maneja errores específicos de SQL aquí
                // Log del error y/o manejar según la necesidad
                // Por ejemplo: Logger.LogError(ex, "Error al obtener los géneros.");
                throw new ApplicationException("Se produjo un error al intentar obtener los géneros.", ex);
            }
            catch (Exception ex)
            {
                // Maneja errores generales aquí
                // Log del error y/o manejar según la necesidad
                // Por ejemplo: Logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Se produjo un error inesperado.", ex);
            }

            return generos;
        }



        

        public Genero ObtenerPorId(int id)
        {
            // Validación del parámetro id
            if (id <= 0)
            {
                throw new ArgumentException("El identificador debe ser un valor positivo.", nameof(id));
            }

            Genero genero = null;
            string query = @"SELECT g.Id, g.Nombre, g.Descripcion, g.Estado, g.TecnicaturaId,
                             t.Nombre AS TecnicaturaNombre
                     FROM Genero g
                     INNER JOIN Tecnicatura t ON t.Id = g.TecnicaturaId
                     WHERE g.Id = @id";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            genero = new Genero
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Estado = reader.GetByte(reader.GetOrdinal("Estado")),
                                TecnicaturaId = reader.GetInt32(reader.GetOrdinal("TecnicaturaId")),
                                Tecnicatura = new Tecnicatura
                                {
                                    Nombre = reader.GetString(reader.GetOrdinal("TecnicaturaNombre"))
                                },

                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Error relacionado con la base de datos
                throw new ApplicationException("Error al acceder a la base de datos para obtener la materia.", ex);
            }
            catch (Exception ex)
            {
                // Error general
                throw new ApplicationException("Se produjo un error inesperado al obtener la materia.", ex);
            }

            return genero;
        }

    }
}

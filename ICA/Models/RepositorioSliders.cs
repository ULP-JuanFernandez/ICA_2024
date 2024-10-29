using Microsoft.Data.SqlClient;
using System.Data;

namespace ICA.Models
{
    public class RepositorioSliders : RepositorioBase, IRepositorioSliders
    {
        public RepositorioSliders(IConfiguration configuration) : base(configuration)
        {

        }
        public int Alta(Slide entidad)
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
            INSERT INTO Slide (Nombre, Descripcion)
            VALUES (@nombre, @descripcion);
            SELECT SCOPE_IDENTITY();";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", entidad.Nombre);
                command.Parameters.AddWithValue("@descripcion", entidad.Descripcion);

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
                    string sql = @"DELETE FROM Slide WHERE Id = @id";

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
        public int Modificacion(Slide entidad)
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
                    const string sql = "UPDATE Slide SET " +
                                       "Nombre = @nombre, Descripcion = @descripcion " +
                                       "WHERE Id = @id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        // Usar AddWithValue con cuidado: asegúrate de que el tipo de datos sea correcto
                        command.Parameters.AddWithValue("@nombre", entidad.Nombre);
                        command.Parameters.AddWithValue("@descripcion", entidad.Descripcion);
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

        public IList<Slide> ObtenerTodos()
        {
            var sliders = new List<Slide>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    const string sql = @"SELECT Id, Nombre, Descripcion, Imagen
                                 FROM Slide";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        connection.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var slide = new Slide
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion")),
                                    Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? null : reader.GetString(reader.GetOrdinal("Imagen"))
                                };
                                sliders.Add(slide);
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

            return sliders;
        }



        public Slide ObtenerPorId(int id)
        {
            Slide entidad = null;
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @$"
					SELECT {nameof(Slide.Id)}, Nombre, Descripcion
                    FROM Slide
					WHERE {nameof(Slide.Id)}=@id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        entidad = new Slide
                        {
                            Id = reader.GetInt32(nameof(Slide.Id)),
                            Nombre = reader.GetString("Nombre"),
                            Descripcion = reader.GetString("Descripcion"),
                        };
                    }
                    connection.Close();
                }
            }
            return entidad;
        }

    }
}

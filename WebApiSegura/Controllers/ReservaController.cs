﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/reserva")]
    public class ReservaController : ApiController
    {

            [HttpGet]
            public IHttpActionResult GetId(int id)
            {
                Reserva reserva = new Reserva();

                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                    {
                        SqlCommand sqlCommand = new SqlCommand(@"SELECT RES_CODIGO, USU_CODIGO, HAB_CODIGO, RES_FECHA_INGRESO, RES_FECHA_SALIDA
                                                                FROM RESERVA WHERE RES_CODIGO = @RES_CODIGO", sqlConnection);

                        sqlCommand.Parameters.AddWithValue("@RES_CODIGO", id);

                        sqlConnection.Open();

                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        while (sqlDataReader.Read())
                        {
                            reserva.RES_CODIGO = sqlDataReader.GetInt32(0);
                            reserva.USU_CODIGO = sqlDataReader.GetInt32(1);
                            reserva.HAB_CODIGO = sqlDataReader.GetInt32(2);
                            reserva.RES_FECHA_INGRESO = sqlDataReader.GetDateTime(3);
                            reserva.RES_FECHA_SALIDA = sqlDataReader.GetDateTime(4);
                        }

                        sqlConnection.Close();
                    }
                }
                catch (Exception e)
                {

                    return InternalServerError(e);
                }

                return Ok(reserva);
            }

            [HttpGet]
            public IHttpActionResult GetAll()
            {
                List<Reserva> reservaciones = new List<Reserva>();

                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                    {
                        SqlCommand sqlCommand = new SqlCommand(@"SELECT RES_CODIGO, USU_CODIGO, HAB_CODIGO, RES_FECHA_INGRESO, RES_FECHA_SALIDA
                                                                FROM RESERVA", sqlConnection);

                        sqlConnection.Open();

                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                        while (sqlDataReader.Read())
                        {
                            Reserva reserva = new Reserva()
                            {
                                RES_CODIGO = sqlDataReader.GetInt32(0),
                                USU_CODIGO = sqlDataReader.GetInt32(1),
                                HAB_CODIGO = sqlDataReader.GetInt32(2),
                                RES_FECHA_INGRESO = sqlDataReader.GetDateTime(3),
                                RES_FECHA_SALIDA = sqlDataReader.GetDateTime(4)

                            };

                        reservaciones.Add(reserva);

                        }

                        sqlConnection.Close();
                    }
                }
                catch (Exception e)
                {

                    return InternalServerError(e);
                }

                return Ok(reservaciones);
            }

            [HttpPost]
            [Route("ingresar")]
            public IHttpActionResult Ingresar(Reserva reserva)
            {
                if (reserva == null)
                    return BadRequest();

                if (RegistrarReserva(reserva))
                    return Ok(reserva);
                else
                    return InternalServerError();
            }

            private bool RegistrarReserva(Reserva reserva)
            {
                bool resultado = false;

                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO RESERVA (USU_CODIGO, HAB_CODIGO, RES_FECHA_INGRESO, RES_FECHA_SALIDA)
                                                        VALUES (@USU_CODIGO, @HAB_CODIGO, @RES_FECHA_INGRESO, @RES_FECHA_SALIDA)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@USU_CODIGO", reserva.USU_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@HAB_CODIGO", reserva.HAB_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@RES_FECHA_INGRESO", reserva.RES_FECHA_INGRESO);
                    sqlCommand.Parameters.AddWithValue("@RES_FECHA_SALIDA", reserva.RES_FECHA_SALIDA);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                        resultado = true;

                    sqlConnection.Close();
                }

                return resultado;
            }


            [HttpPut]
            public IHttpActionResult Put(Reserva reserva)
            {
                if (reserva == null)
                    return BadRequest();

                if (ActualizarReserva(reserva))
                    return Ok(reserva);
                else
                    return InternalServerError();
            }

            private bool ActualizarReserva(Reserva reserva)
            {
                bool resultado = false;

                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE RESERVA SET USU_CODIGO = @USU_CODIGO, HAB_CODIGO = @HAB_CODIGO, 
                                                        RES_FECHA_INGRESO = @RES_FECHA_INGRESO, RES_FECHA_SALIDA = @RES_FECHA_SALIDA 
                                                        WHERE RES_CODIGO = @RES_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@RES_CODIGO", reserva.RES_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@USU_CODIGO", reserva.USU_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@HAB_CODIGO", reserva.HAB_CODIGO);
                    sqlCommand.Parameters.AddWithValue("@RES_FECHA_INGRESO", reserva.RES_FECHA_INGRESO);
                    sqlCommand.Parameters.AddWithValue("@RES_FECHA_SALIDA", reserva.RES_FECHA_SALIDA);


                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                        resultado = true;

                    sqlConnection.Close();
                }

                return resultado;
            }

            [HttpDelete]
            public IHttpActionResult Delete(int id)
            {
                if (id <= 0)
                    return BadRequest();

                if (EliminarReserva(id))
                    return Ok(id);
                else
                    return InternalServerError();
            }

            private bool EliminarReserva(int id)
            {

                bool resultado = false;

                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE RESERVA WHERE RES_CODIGO = @RES_CODIGO", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@RES_CODIGO", id);


                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                        resultado = true;

                    sqlConnection.Close();
                }

                return resultado;
            }
        
    }
}

using RSBotMetrics.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RSBotMetrics.Controllers
{
    public class SkillsController : ApiController
    {
		private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

		// GET api/skills
		public IEnumerable<Skill> Get()
		{
			List<Skill> skills = new List<Skill>();

			SqlConnection conn = new SqlConnection(connectionString);

			try
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand("SELECT * FROM TotalXP", conn);
				SqlDataReader reader = cmd.ExecuteReader();
				while(reader.Read())
				{
					skills.Add(new Skill { id = reader.GetInt32(0), name = reader.GetString(1), xp = reader.GetInt32(2) });
				}
				reader.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return skills;
		}

		// GET api/skills/id
		public Skill Get(int id) {
			Skill skill = new Skill();
			SqlConnection conn = new SqlConnection(connectionString);
			SqlCommand cmd = new SqlCommand("SELECT * FROM TotalXP WHERE id = @skillID", conn);
			cmd.Parameters.Add("@skillID", SqlDbType.Int);
			cmd.Parameters["@skillID"].Value = id;

			try
			{
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				reader.Read();
				skill = new Skill { id = reader.GetInt32(0), name = reader.GetString(1), xp = reader.GetInt32(2) };
				reader.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return skill;
		}

		// POST api/skills/id
		public HttpResponseMessage Post(int id, Skill skill)
		{
			if(ModelState.IsValid)
			{
				SqlConnection conn = new SqlConnection(connectionString);
				SqlCommand cmd = new SqlCommand("UPDATE TotalXP SET xp = xp + @gainedXP WHERE id = @skillID", conn);
				cmd.Parameters.Add("@gainedXP", SqlDbType.Int).Value = skill.xp;
				cmd.Parameters.Add("@skillID", SqlDbType.Int).Value = skill.id;

				try
				{
					conn.Open();
					int rowsUpdated = cmd.ExecuteNonQuery();
					conn.Close();

					if (rowsUpdated > 0)
					{
						return Request.CreateResponse(HttpStatusCode.Accepted, skill);
					}
					else
					{
						return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "No rows updated");
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
					return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Database exception");
				}
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
			}
		}
    }
}

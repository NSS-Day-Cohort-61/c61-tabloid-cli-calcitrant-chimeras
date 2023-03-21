﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TabloidCLI.Models;

namespace TabloidCLI.Repositories
{
    public class JournalRepository : DatabaseConnector, IRepository<Journal>
    {
        public JournalRepository(string connectionString) : base(connectionString) { }

        public List<Journal> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Title, Content, CreateDateTime FROM Journal";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Journal> journals = new List<Journal>();
                    while (reader.Read())
                    {
                        Journal journal = new Journal()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };
                        journals.Add(journal);
                    }

                    reader.Close();
                    return journals;
                }
            }
        }

        public Journal Get(int id) 
        { 
            throw new NotImplementedException();
        }

        public void Insert(Journal journal)
        {
            throw new NotImplementedException();
        }

        public void Update(Journal journal)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP1_BDAvancee.db;
using TP1_BDAvancee.model;

namespace TP1_BDAvancee.controller
{
    internal class ArticleManager
    {
        private List<Article> articles;

        public void addArticle(Article article)
        {
            try
            {
                string query = "INSERT INTO Articles (Code, Name, Description, Brand, Category, Price, imageURL, Valid) VALUES (@Code, @Name, @Description, @Brand, @Category, @Price, @imageURL, @Valid)";
                using (SqlCommand command = new SqlCommand(query, DatabaseConnection.connection))
                {
                    command.Parameters.AddWithValue("@Code", article.code);
                    command.Parameters.AddWithValue("@Name", article.name);
                    command.Parameters.AddWithValue("@Description", article.description);
                    command.Parameters.AddWithValue("@Brand", article.brand);
                    command.Parameters.AddWithValue("@Category", article.category);
                    command.Parameters.AddWithValue("@Price", article.price);
                    command.Parameters.AddWithValue("@imageURL", article.imageURL);
                    command.Parameters.AddWithValue("@Valid", 1);
                    command.ExecuteNonQuery();
                }   
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void deleteArticle(int id)
        {
            try
            {
                string query = "DELETE FROM Articles WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(query, DatabaseConnection.connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void updateArticle(Article article)
        {
            try
            {
                string query = "UPDATE Articles SET Code = @Code, Name = @Name, Description = @Description, Brand = @Brand, Category = @Category, Price = @Price, imageURL = @imageURL, Valid = @Valid WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, DatabaseConnection.connection))
                {
                    command.Parameters.AddWithValue("@Id", article.id);
                    command.Parameters.AddWithValue("@Code", article.code);
                    command.Parameters.AddWithValue("@Name", article.name);
                    command.Parameters.AddWithValue("@Description", article.description);
                    command.Parameters.AddWithValue("@Brand", article.brand);
                    command.Parameters.AddWithValue("@Category", article.category);
                    command.Parameters.AddWithValue("@Price", article.price);
                    command.Parameters.AddWithValue("@imageURL", article.imageURL);
                    command.Parameters.AddWithValue("@Valid", article.valid);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

            public List<Article> getAllArticles()
        {
            try
            {
                string query = "SELECT * FROM Articles";
                using (SqlCommand command = new SqlCommand(query, DatabaseConnection.connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        articles = new List<Article>();
                        while (reader.Read())
                        {
                            articles.Add(new Article(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetDecimal(6), reader.GetString(7), reader.GetBoolean(8)));
                        }
                    }
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return articles;
        }

        public List<Article> getAllValidArticles()
        {
            try
            {
                string query = "SELECT * FROM Articles WHERE Valid = 1";
                using (SqlCommand command = new SqlCommand(query, DatabaseConnection.connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        articles = new List<Article>();
                        while (reader.Read())
                        {
                            articles.Add(new Article(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetDecimal(6), reader.GetString(7), reader.GetBoolean(8)));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return articles;
        }

        public List<Article> getArticlesFiltered(string searchTerm)
        {
            List<Article> filteredArticles = new List<Article>();

            try
            {
                string query = "SELECT * FROM Articles WHERE Name LIKE @SearchTerm AND Valid = 1";

                
                    using (SqlCommand command = new SqlCommand(query, DatabaseConnection.connection))
                    {
                        command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Article article = new Article(
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Code")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetString(reader.GetOrdinal("Description")),
                                    reader.GetString(reader.GetOrdinal("Brand")),
                                    reader.GetString(reader.GetOrdinal("Category")),
                                    reader.GetDecimal(reader.GetOrdinal("Price")),
                                    reader.GetString(reader.GetOrdinal("ImageURL")),
                                    reader.GetBoolean(reader.GetOrdinal("Valid"))
                                );

                                filteredArticles.Add(article);
                            }
                        }
                    }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering articles: {ex.Message}");
            }

            return filteredArticles;
        }

        public int getArticlesCount()
        {
            int count = 0;
            try
            {
                string query = "SELECT COUNT(*) FROM Articles";
                using (SqlCommand command = new SqlCommand(query, DatabaseConnection.connection))
                {
                    count = (int)command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return count;
        }

        public Article getArticleById(int id)
        {
            Article article = new Article();
            try
            {
                string query = "SELECT * FROM Articles WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(query, DatabaseConnection.connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            article.id = reader.GetInt32(0);
                            article.code = reader.GetString(1);
                            article.name = reader.GetString(2);
                            article.description = reader.GetString(3);
                            article.brand = reader.GetString(4);
                            article.category = reader.GetString(5);
                            article.price = reader.GetDecimal(6);
                            article.imageURL = reader.GetString(7);
                            article.valid = reader.GetBoolean(8);

                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return article;
        }

    }
}

using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI.Repositories
{
    public class PostRepository : DatabaseConnector, IRepository<Post>
    {
        public PostRepository(string connectionString) : base(connectionString) { }

        public List<Post> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                              p.id AS PostId,
                                              p.Title AS PostTitle,
                                              p.Url AS PostUrl,
                                              p.PublishDateTime,
                                              a.id AS AuthorId,
                                              a.FirstName,
                                              a.LastName,
                                              a.Bio,
                                              b.id AS BlogId,
                                              b.Title AS BlogTitle,
                                              b.Url AS BlogUrl
                                              FROM Post p
                                              JOIN Author a ON p.AuthorId = a.id
                                              JOIN Blog b ON p.BlogId = b.id";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("PostId")),
                            Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                            Url = reader.GetString(reader.GetOrdinal("PostUrl")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogUrl")),
                            }
                        };
                        posts.Add(post);
                    }

                    reader.Close();

                    return posts;
                }
            };
        }

        public Post Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand()) 
                {
                    //SQL query may need author and blog tags later
                    cmd.CommandText = @"SELECT p.Id AS postId, p.Title AS postTitle, p.URL AS postUrl, p.PublishDateTime AS postPublishDate, 
                                        a.Id AS authorId, a.FirstName AS authorFirstName, a.LastName AS authorLastName, a.Bio AS authorBio,
                                        b.Id AS blogId,b.Title AS blogTitle, b.URL AS blogUrl
                                        FROM Post p
                                        LEFT JOIN Author a ON p.AuthorId = a.Id
                                        LEFT JOIN Blog b ON p.BlogId = b.Id
                                        WHERE p.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    Post post = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) 
                    {   
                        if (post == null)
                        {
                            post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("postId")), 
                                Title = reader.GetString(reader.GetOrdinal("postTitle")),
                                Url = reader.GetString(reader.GetOrdinal("postUrl")),
                                PublishDateTime = reader.GetDateTime(reader.GetOrdinal("postPublishDate")),
                                Author = new Author()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("authorId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("authorFirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("authorLastName")),
                                    Bio = reader.GetString(reader.GetOrdinal("authorBio")),
                                },
                                Blog = new Blog()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("blogId")),
                                    Title = reader.GetString(reader.GetOrdinal("blogTitle")),
                                    Url = reader.GetString(reader.GetOrdinal("blogUrl"))
                                }

                            };
                        }
                    }
                    return post;
                }
            }
        }

        public List<Post> GetByAuthor(int authorId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.id,
                                               p.Title As PostTitle,
                                               p.URL AS PostUrl,
                                               p.PublishDateTime,
                                               p.AuthorId,
                                               p.BlogId,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio,
                                               b.Title AS BlogTitle,
                                               b.URL AS BlogUrl
                                          FROM Post p 
                                               LEFT JOIN Author a on p.AuthorId = a.Id
                                               LEFT JOIN Blog b on p.BlogId = b.Id 
                                         WHERE p.AuthorId = @authorId";
                    cmd.Parameters.AddWithValue("@authorId", authorId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                            Url = reader.GetString(reader.GetOrdinal("PostUrl")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogUrl")),
                            }
                        };
                        posts.Add(post);
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public void Insert(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Post (Title, Url, PublishDateTime, AuthorId, BlogId )
                                                     VALUES (@title, @url, @publishDateTime, @author, @blog )";
                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@url", post.Url);
                    cmd.Parameters.AddWithValue("@publishDateTime", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@author", post.Author.Id);
                    cmd.Parameters.AddWithValue("@blog", post.Blog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Post post)
        {
            using (SqlConnection conn = Connection) 
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Post
                                        SET Title = @title,
                                        URL = @url,
                                        PublishDateTime = @publishDateTime,
                                        AuthorId = @AuthorId,
                                        BlogId = @BlogId
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", post.Id);
                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@url", post.Url);
                    cmd.Parameters.AddWithValue("@publishDateTime", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@AuthorId", post.Author.Id);
                    cmd.Parameters.AddWithValue("@BlogId", post.Blog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertPostTag(int postId, int tagId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO PostTag (PostId, TagId) VALUES (@PostId, @TagId)";
                    cmd.Parameters.AddWithValue("@PostId", postId);
                    cmd.Parameters.AddWithValue("@TagId", tagId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Post WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<PostTag> GetPosttags(int postId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand()) 
                {
                    cmd.CommandText = @"SELECT pt.Id as Id, pt.PostId as PostId, t.Id AS tagId , t.Name as tagName FROM PostTag pt
                                        JOIN Tag t ON pt.TagId = t.Id
                                        WHERE PostId = @id";
                    cmd.Parameters.AddWithValue("@id", postId );

                    List<PostTag> allPostTags = new List<PostTag>();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PostTag postTag = new PostTag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            Tag = new Tag
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("tagId")),
                                Name = reader.GetString(reader.GetOrdinal("tagName"))
                            }
                        };
                        allPostTags.Add(postTag);
                    }
                    reader.Close();
                    return allPostTags;
                }
            }
        }

        public void DeletePosttag(PostTag ptToDelete)
        {
            using (SqlConnection conn = Connection) 
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM PostTag WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", ptToDelete.Id);

                    cmd.ExecuteNonQuery();
                }    
            }
        }
    }
}
